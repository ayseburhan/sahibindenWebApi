using Microsoft.AspNetCore.Mvc;
using sahibindenWebApi.Models;
using System.Linq;

namespace sahibindenWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KategoriController : ControllerBase
    {
        private readonly DBsahibindenContext _context;
        private readonly ILogger<KategoriController> _logger;

        public KategoriController(ILogger<KategoriController> logger, DBsahibindenContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("/AddKategori")]
        public ActionResult<string> AddKategori([FromBody] Kategori yeniKategori)
        {
            _context.Kategoris.Add(yeniKategori);
            _context.SaveChanges();
            return Ok("Kategori başarıyla eklendi.");
        }


        [HttpGet("/GetAllKategoris")]
        public ActionResult<List<Kategori>> GetAllKategoris()
        {
            var kategoriler = _context.Kategoris.ToList();
            return Ok(kategoriler);
        }

        [HttpPut("/UpdateKategori")]
        public ActionResult<string> UpdateKategori([FromBody] Kategori guncelKategori)
        {
            var kategori = _context.Kategoris.FirstOrDefault(k => k.Id == guncelKategori.Id);
            if (kategori == null)
            {
                return NotFound("Kategori bulunamadı.");
            }

            kategori.KategoriAdi = guncelKategori.KategoriAdi;
            _context.SaveChanges();

            return Ok("Kategori başarıyla güncellendi.");
        }

        [HttpDelete("/DeleteKategori")]
        public ActionResult<string> DeleteKategori([FromQuery] int id)
        {
            var kategori = _context.Kategoris.FirstOrDefault(k => k.Id == id);
            if (kategori == null)
            {
                return NotFound("Kategori bulunamadı.");
            }

            _context.Kategoris.Remove(kategori);
            _context.SaveChanges();
            return Ok("Kategori başarıyla silindi.");
        }
    }
}
