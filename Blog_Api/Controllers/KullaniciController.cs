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
    public class KullaniciController : ControllerBase
    {
        private readonly Blog_DB _context;

        public KullaniciController(Blog_DB context)
        {
            _context = context;
        }

        [HttpGet("kategori-ekle")]
        public async Task<IActionResult> KategoriEkle(string name,string kategori)
        {
            // Kullanıcıyı bul
            Kullanici kullanici = _context.kullanicis.FirstOrDefault(x => x.Name == name);
            if (kullanici == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            // Kategoriyi bul
            var kategoriEntity = _context.kategoris.FirstOrDefault(x => x.Name == kategori);
            if (kategoriEntity == null)
            {
                return NotFound("Kategori bulunamadı.");
            }
            // Kullanıcının kategoriId'sini güncelle
            kullanici.katid = kategoriEntity.Id;

            //Kategori a =_context.kategoris.FirstOrDefault(x=>x.Name== kategori);
            if (kullanici.Kategoriler == null)
            {
                kullanici.Kategoriler = new List<Kategori>();
            }
            kullanici.Kategoriler.Add(kategoriEntity);
            //var kategor = await _context.kategoris.Where(k => k.Name == kategori).Select(x => x.Id);

            //Kategori kateg = _context.kategoris.FirstOrDefault(x => x.Id == kategoriEntity.Id);
            //kullanici.Kategoriler = kateg.;

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            return Ok("Kategori başarıyla eklendi.");
            //var kullanici_idsi = _context.kullanicis.Where(X => X.Name == name).Select(a => a.Id);
            //var kullanici_ismi = _context.kullanicis.FirstOrDefault(x => x.Name == name);
            //var kategori_idsi = _context.kategoris.Where(X => X.Name == kategori).Select(a => a.Id).ToList();
            //int id = kategori_idsi.ToList();
            //var b = a.Select(a => a.kategoriId = c.Where(X => X.Id));
            //var kullanicilar = kullaniciListesi.Where(x => x.Name == name).ToList();
            //var kullanici = _context.kullanicis.Where(x => x.katid == kategori_idsi[0]);
            //var kullanici_id_katid = _context.kullanicis.Where(X => X.Name == name).Select(a => a.katid).ToList();
            //if (kullanici_id_katid[0] == null)
            //{
            //    kullanici_id_katid[0]. = kategori_idsi[0]; // Yeni kategoriId değeri
            //    await _context.SaveChangesAsync();
            //}
            //else
            //{

            //}

            //_context.kullanicis.Add(kullanici_ismi.Select(x => x.katname);

            //_context.SaveChanges();


            //Db den ÇEk !!
            //db den kullanıcı katogorisini çek, sonra da buna göre makaleleri getir
            //var kategoriIds = await _context.kullanicis
            //            .Where(k => k.Id == user)
            //            .SelectMany(k => k.Kategoriler)
            //            .Select(c => c.Id)
            //            .ToListAsync();

            //var makaleler = await _context.makales
            //    .Where(m => kategoriIds.Contains(m.KategoriId))
            //    .ToListAsync();

            //return Ok(makaleler);
            //return BadRequest();

        }
        [HttpGet("baslik-goster")]
        public async Task<IActionResult> BaslikGoster(int user)
        {
            var a = _context.kullanicis.Where(X => X.Id == user);
            //Db den ÇEk !!
            //db den kullanıcı katogorisini çek, sonra da buna göre makaleleri getir
            var kategoriIds = await _context.kullanicis
                        .Where(k => k.Id == user)
                        .SelectMany(k => k.Kategoriler)
                        .Select(c => c.Id)
                        .ToListAsync();

            var makaleler = await _context.makales
                .Where(m => kategoriIds.Contains(m.KategoriId))
                .ToListAsync();

            return Ok(makaleler);

            //var don = makaleler.SelectMany(x => x.Baslik);
            //return Ok(don);
            //if (yazar == null)
            //{
            //    return BadRequest();
            //}
            //var donus = new Makale
            //{
            //    //db de ara, makale == yazar.yazarname
            //    //Baslik = 
            //    //DB de ara yoksa oluştur
            //    //kategori = 
            //    //{
            //    //    kategori.Id=
            //    //    kategori.Name=kategori
            //    //}

            //};
            //return Ok(donus);
            //return Ok(a);
        }
        [HttpGet("oku")]
        public async Task<IActionResult> Oku(int id)
        {
            var makale = await _context.makales.FindAsync(id);
            //MVC DEN GELSİN DB DEN DE ÇEKİP MVC YE GÖNDERSİN

            //getirilen makalelerin linkine basılınca gelsin gitsin nasıl olur bilmiyorum !!
            return Ok(makale);
        }
    }
}
