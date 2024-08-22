using DataLayer.Model_Blog;
using DataLayer.Model_DBContext;
using DataLayer.Model_Kullanicilar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YazarController : ControllerBase
    {
        private readonly Blog_DB _context;

        public YazarController(Blog_DB context)
        {
            _context = context;
        }
        [HttpPost("yazilanlar")]
        public async Task<IActionResult> Yazilanlar([FromBody]int yazar)
        {
            //MVC'den gelen User Ayarlarına Göre Gelsin,Db de kontrol edilsin, kontrolden sonra da mvc ye dönsün
            if(yazar == null)
            {
                return BadRequest();
            }
            var makaleler = await _context.makales
           .Where(m => m.YazarId == yazar)
           .ToListAsync();
            var don = makaleler.SelectMany(x => x.Baslik);
            return Ok(makaleler);
            // var donus = new Makale
            // {

            //     return await _context.makales.Where(m=>m.yazar==yazar.Id)
            //db de ara, makale == yazar.yazarname
            //Baslik = 
            //DB de ara yoksa oluştur
            //kategori = 
            //{
            //    kategori.Id=
            //    kategori.Name=kategori
            //}

            //};
            //return Ok();
        }

        [HttpPost("makale-ekle")]
        public async Task<IActionResult> MakaleYaz(string Baslik,string icerik,string cat, string yaz)
        {
            if (Baslik == null)
            {
                return BadRequest();
            }

            var catid = await _context.kategoris
           .FirstOrDefaultAsync(k => k.Name == cat);

            if (catid == null)
            {
                // Kullanıcı mevcut değilse yeni bir kullanıcı oluştur
                catid = new Kategori
                {
                    Name = cat,
                };

                _context.kategoris.Add(catid);
                await _context.SaveChangesAsync();
            }
            var yazid = await _context.yazars
           .FirstOrDefaultAsync(k => k.Name == yaz);

            if (yazid == null)
            {
                // Kullanıcı mevcut değilse yeni bir kullanıcı oluştur
                yazid = new Yazar
                {
                    Name = yaz,
                };

                _context.yazars.Add(yazid);
                await _context.SaveChangesAsync();
            }
            var kadi = await _context.kategoris.Where(y => y.Name == cat).Select(x => x.Id).ToListAsync();

            var yadi = await _context.yazars.Where(y => y.Name == yaz).Select(x => x.Id).ToListAsync();
            //var kategoraiId = await _context.kategoris.Where(x => x.Name == cat).Select(x => x.Id);
            //mvc den alsın, db ye yazsın
            Makale donus = new Makale()
            {
                Baslik=Baslik,
                Icerik = icerik,
                KategoriId= kadi[0],
                YazarId = yadi[0],
  
            };
            //var yazar = await _context.Yazarlar
            //.Include(y => y.Makaleler) // Makaleleri dahil et
            //.FirstOrDefaultAsync(y => y.YazarId == yazarId);
            //kadi.Maka
            Kategori a = _context.kategoris.FirstOrDefault(x=>x.Name==cat);
            donus.kategori=a;
            Yazar b = _context.yazars.FirstOrDefault(x => x.Name == yaz);
            donus.yazar = b;
            //donus.kategori; 
            _context.makales.Add(donus);
            await _context.SaveChangesAsync();
            return Ok("Kaydoldu");
             

        }
    }
}
