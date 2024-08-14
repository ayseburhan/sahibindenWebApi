using Microsoft.AspNetCore.Mvc;
using sahibindenWebApi.Models;
using System.Linq;

namespace sahibindenWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavoriController : ControllerBase
    {
        private readonly DBsahibindenContext _context;
        private readonly ILogger<FavoriController> _logger;

        public FavoriController(ILogger<FavoriController> logger, DBsahibindenContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("/AddFavori")]
        public ActionResult<string> AddFavori([FromBody] Favori yeniFavori)
        {
            _context.Favoris.Add(yeniFavori);
            _context.SaveChanges();
            return Ok("Favori başarıyla eklendi.");
        }

        [HttpGet("/GetFavoriByKullaniciId")]
        public ActionResult<List<Favori>> GetFavoriByKullaniciId([FromQuery] int kullaniciId)
        {
            var favoriler = _context.Favoris.Where(f => f.KullaniciId == kullaniciId).ToList();
            if (favoriler == null || favoriler.Count == 0)
            {
                return NotFound("Favori bulunamadı.");
            }
            return Ok(favoriler);
        }

        [HttpPut("/UpdateFavori")]
        public ActionResult<string> UpdateFavori([FromBody] Favori guncelFavori)
        {
            var favori = _context.Favoris.FirstOrDefault(f => f.Id == guncelFavori.Id);
            if (favori == null)
            {
                return NotFound("Favori bulunamadı.");
            }

            favori.KullaniciId = guncelFavori.KullaniciId;
            favori.UrunId = guncelFavori.UrunId;
            _context.SaveChanges();

            return Ok("Favori başarıyla güncellendi.");
        }

        [HttpDelete("/DeleteFavori")]
        public ActionResult<string> DeleteFavori([FromQuery] int id)
        {
            var favori = _context.Favoris.FirstOrDefault(f => f.Id == id);
            if (favori == null)
            {
                return NotFound("Favori bulunamadı.");
            }

            _context.Favoris.Remove(favori);
            _context.SaveChanges();
            return Ok("Favori başarıyla silindi.");
        }
    }
}
