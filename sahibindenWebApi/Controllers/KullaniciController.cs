using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sahibindenWebApi.Models;
using sahibindenWebApi.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace sahibindenWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciController : ControllerBase
    {
        private readonly DBsahibindenContext _context;
        private readonly ILogger<KullaniciController> _logger;
        private readonly IConfiguration _configuration;

        public KullaniciController(ILogger<KullaniciController> logger, DBsahibindenContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] KullaniciDto kullanici)
        {
            var kullaniciCheck = _context.Kullanicis.FirstOrDefault(a => a.KullaniciMail == kullanici.KullaniciMail && a.KullaniciSifre == kullanici.KullaniciSifre);
            if (kullaniciCheck != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("KullaniciId", kullaniciCheck.KullaniciId.ToString()),
                    new Claim("Mail", kullaniciCheck.KullaniciMail),
                    new Claim(ClaimTypes.Role, kullaniciCheck.KullaniciYetki == 1 ? "Admin" : "Kullanici")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: signIn
                );
                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                HttpContext.Response.Cookies.Append("token", tokenValue, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60)
                });
                return Ok(new { Token = tokenValue, kullanici = kullaniciCheck });
            }
            return Unauthorized("Invalid credentials.");
        }

        [HttpGet("Users")]
        public ActionResult<List<Kullanici>> GetUsers()
        {
            var kullanicilar = _context.Kullanicis.ToList();
            return Ok(kullanicilar);
        }

        [HttpPost("Register")]
        public ActionResult<string> Register([FromBody] Kullanici yeniKullanici)
        {
            var mevcutKullaniciMail = _context.Kullanicis.FirstOrDefault(a => a.KullaniciMail == yeniKullanici.KullaniciMail);
            if (mevcutKullaniciMail != null)
            {
                return Conflict("Bu e-posta ile kayýtlý bir kullanýcý zaten var.");
            }

            _context.Kullanicis.Add(yeniKullanici);
            _context.SaveChanges();
            return Ok("Kayýt baþarýlý.");
        }

        [HttpGet("User/{id}")]
        public IActionResult GetKullaniciById(int id)
        {
            var kullanici = _context.Kullanicis.FirstOrDefault(k => k.KullaniciId == id);

            if (kullanici == null)
            {
                return NotFound("Kullanýcý bulunamadý.");
            }

            return Ok(kullanici);
        }

        [HttpDelete("DeleteAccount")]
        public ActionResult<string> DeleteAccount([FromQuery] string email, [FromQuery] string sifre)
        {
            var kullaniciCheck = _context.Kullanicis.FirstOrDefault(a => a.KullaniciMail == email && a.KullaniciSifre == sifre);
            if (kullaniciCheck == null)
            {
                return NotFound("Kullanýcý bulunamadý veya þifre yanlýþ.");
            }
            _context.Kullanicis.Remove(kullaniciCheck);
            _context.SaveChanges();
            return Ok("Hesap baþarýyla silindi.");
        }

        [HttpPost("ResetPassword")]
        public ActionResult<string> ResetPassword([FromQuery] string email, [FromQuery] string yeniSifre)
        {
            var kullaniciCheck = _context.Kullanicis.FirstOrDefault(a => a.KullaniciMail == email);
            if (kullaniciCheck == null)
            {
                return NotFound("Kullanýcý bulunamadý.");
            }

            kullaniciCheck.KullaniciSifre = yeniSifre;
            _context.SaveChanges();
            return Ok("Þifre baþarýyla sýfýrlandý.");
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var kullanici = _context.Kullanicis.SingleOrDefault(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);

                if (kullanici == null)
                {
                    return Unauthorized("Invalid refresh token");
                }

                var newJwtToken = GenerateJwtToken(kullanici);
                var newRefreshToken = GenerateRefreshToken();

                // Refresh token'ý güncelle
                kullanici.RefreshToken = newRefreshToken;
                kullanici.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);  
                _context.Kullanicis.Update(kullanici);
                _context.SaveChanges();

                return Ok(new
                {
                    Token = newJwtToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refreshing token");
                return StatusCode(500, "Internal server error");
            }
        }

        private string GenerateJwtToken(Kullanici kullanici)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("KullaniciId", kullanici.KullaniciId.ToString()),
                new Claim("Mail", kullanici.KullaniciMail),
                new Claim(ClaimTypes.Role, kullanici.KullaniciYetki == 1 ? "Admin" : "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),  // Örneðin, 60 dakika geçerlilik süresi
                signingCredentials: signIn
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
