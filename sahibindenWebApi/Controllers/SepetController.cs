using Microsoft.AspNetCore.Mvc;
using sahibindenWebApi.Models;
using System.Linq;

namespace sahibindenWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SepetController : ControllerBase
    {
        private readonly DBsahibindenContext _context;
        private readonly ILogger<SepetController> _logger;

        public SepetController(ILogger<SepetController> logger, DBsahibindenContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("/AddSepet")]
        public ActionResult<string> AddSepet([FromBody] Sepet yeniSepet)
        {
            _context.Sepets.Add(yeniSepet);
            _context.SaveChanges();
            return Ok("Sepet öğesi başarıyla eklendi.");
        }

        [HttpGet("/GetSepetByKullaniciId")]
        public ActionResult<List<Sepet>> GetSepetByKullaniciId([FromQuery] int kullaniciId)
        {
            var sepetler = _context.Sepets.Where(s => s.KullaniciId == kullaniciId).ToList();
            if (sepetler == null || sepetler.Count == 0)
            {
                return NotFound("Sepet öğesi bulunamadı.");
            }
            return Ok(sepetler);
        }


        [HttpDelete("/DeleteSepet")]
        public ActionResult<string> DeleteSepet([FromQuery] int id)
        {
            var sepet = _context.Sepets.FirstOrDefault(s => s.Id == id);
            if (sepet == null)
            {
                return NotFound("Sepet öğesi bulunamadı.");
            }

            _context.Sepets.Remove(sepet);
            _context.SaveChanges();
            return Ok("Sepet öğesi başarıyla silindi.");
        }
    }
}
