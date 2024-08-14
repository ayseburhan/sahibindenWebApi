using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sahibindenWebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sahibindenWebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DBsahibindenContext _context;

        public AdminController(DBsahibindenContext Dbcontext)
        {
            _context = Dbcontext;
        }

        // Kullanıcı listesi getirme
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _context.Kullanicis.ToList();
            return Ok(users);
        }
        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            var kullanici = _context.Kullanicis.FirstOrDefault(u => u.Id == id);
            if (kullanici == null)
            {
                return NotFound("User not found.");
            }
            return Ok(kullanici);
        }
        [HttpGet("AdminOnlyEndpoint")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("Bu endpoint sadece admin kullanıcılar içindir.");
        }
        // Kullanıcı ekleme
        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] Kullanici newUser)
        {
            _context.Kullanicis.Add(newUser);
            _context.SaveChanges();
            return Ok(newUser);
        }

        // Kullanıcı güncelleme
        [HttpPut("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] Kullanici updatedUser)
        {
            var user = _context.Kullanicis.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.KullaniciMail = updatedUser.KullaniciMail;
            user.KullaniciSifre = updatedUser.KullaniciSifre;
            user.KullaniciYetki = updatedUser.KullaniciYetki;

            _context.Kullanicis.Update(user);
            _context.SaveChanges();
            return Ok(user);
        }

        // Kullanıcı silme
        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Kullanicis.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Kullanicis.Remove(user);
            _context.SaveChanges();
            return Ok();
        }
    }

}
