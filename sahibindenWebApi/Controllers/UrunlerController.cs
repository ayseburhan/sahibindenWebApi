using Microsoft.AspNetCore.Mvc;
using sahibindenWebApi.Models;
using System.Linq;

namespace sahibindenWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrunlerController : ControllerBase
    {
        private readonly DBsahibindenContext _context;
        private readonly ILogger<UrunlerController> _logger;

        public UrunlerController(ILogger<UrunlerController> logger, DBsahibindenContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("/AddUrun")]
        public ActionResult<string> AddUrun([FromBody] Urunler yeniUrun)
        {
            _context.Urunlers.Add(yeniUrun);
            _context.SaveChanges();
            return Ok("Ürün başarıyla eklendi.");
        }

        [HttpGet("/GetUrunById")]
        public ActionResult<Urunler> GetUrunById([FromQuery] int id)
        {
            var urun = _context.Urunlers.FirstOrDefault(u => u.Id == id);
            if (urun == null)
            {
                return NotFound("Ürün bulunamadı.");
            }
            return Ok(urun);
        }

        [HttpGet("/GetAllUrunler")]
        public ActionResult<List<Urunler>> GetAllUrunler()
        {
            var urunler = _context.Urunlers.ToList();
            return Ok(urunler);
        }

        [HttpPut("/UpdateUrun")]
        public ActionResult<string> UpdateUrun([FromBody] Urunler guncelUrun)
        {
            var urun = _context.Urunlers.FirstOrDefault(u => u.Id == guncelUrun.Id);
            if (urun == null)
            {
                return NotFound("Ürün bulunamadı.");
            }

            urun.KategoriId = guncelUrun.KategoriId;
            urun.UrunAdi = guncelUrun.UrunAdi;
            urun.UrunAciklama = guncelUrun.UrunAciklama;
            urun.UrunFiyat = guncelUrun.UrunFiyat;
            urun.UrunStok = guncelUrun.UrunStok;
            urun.EklenmeTarihi = guncelUrun.EklenmeTarihi;
            urun.UrunDurum = guncelUrun.UrunDurum;
            urun.UrunGorsel = guncelUrun.UrunGorsel;

            _context.SaveChanges();

            return Ok("Ürün başarıyla güncellendi.");
        }

        [HttpDelete("/DeleteUrun")]
        public ActionResult<string> DeleteUrun([FromQuery] int id)
        {
            var urun = _context.Urunlers.FirstOrDefault(u => u.Id == id);
            if (urun == null)
            {
                return NotFound("Ürün bulunamadı.");
            }

            _context.Urunlers.Remove(urun);
            _context.SaveChanges();
            return Ok("Ürün başarıyla silindi.");
        }
    }
}
