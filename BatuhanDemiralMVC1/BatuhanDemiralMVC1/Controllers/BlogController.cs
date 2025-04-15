using System.Security.Claims;
using BatuhanDemiralMVC1.Data;
using BatuhanDemiralMVC1.Models;
using BatuhanDemiralMVC1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BatuhanDemiralMVC1.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogApiVeSorgular _blogApi;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BlogController(BlogApiVeSorgular blogApi, IWebHostEnvironment hostEnvironment)
        {
            _blogApi = blogApi;
            _hostEnvironment = hostEnvironment;
        }
        
        // Blog detay sayfası
        [HttpGet]
        public async Task<IActionResult> Detay(int id)
        {
            var blogYazisi = await _blogApi.BlogYazisiniGetirAsync(id);
            
            if (blogYazisi == null)
            {
                return NotFound();
            }
            
            return View(blogYazisi);
        }
        
        // Yeni blog yazısı ekleme sayfası
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Olustur()
        {
            ViewBag.Kategoriler = await _blogApi.TumKategorileriGetirAsync();
            return View();
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Olustur(BlogYazisiViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı ID al
                var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                
                var blogYazisi = new BlogYazisi
                {
                    Baslik = model.Baslik,
                    Icerik = model.Icerik,
                    KategoriId = model.KategoriId,
                    KullaniciId = kullaniciId,
                    YayinTarihi = DateTime.Now
                };
                
                // Eğer resim yüklenmişse
                if (model.ResimDosyasi != null && model.ResimDosyasi.Length > 0)
                {
                    string dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(model.ResimDosyasi.FileName);
                    string klasorYolu = Path.Combine(_hostEnvironment.WebRootPath, "img", "blog");
                    
                    // Klasör yoksa oluştur
                    if (!Directory.Exists(klasorYolu))
                    {
                        Directory.CreateDirectory(klasorYolu);
                    }
                    
                    string dosyaYolu = Path.Combine(klasorYolu, dosyaAdi);
                    
                    using (var fileStream = new FileStream(dosyaYolu, FileMode.Create))
                    {
                        await model.ResimDosyasi.CopyToAsync(fileStream);
                    }
                    
                    blogYazisi.ResimYolu = "/img/blog/" + dosyaAdi;
                }
                
                await _blogApi.BlogYazisiEkleAsync(blogYazisi);
                
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.Kategoriler = await _blogApi.TumKategorileriGetirAsync();
            return View(model);
        }
        
        // Blog yazısı düzenleme sayfası
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Duzenle(int id)
        {
            var blogYazisi = await _blogApi.BlogYazisiniGetirAsync(id);
            
            if (blogYazisi == null)
            {
                return NotFound();
            }
            
            // Yalnızca yazarın kendisi düzenleyebilir
            var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (blogYazisi.KullaniciId != kullaniciId)
            {
                return Forbid();
            }
            
            var model = new BlogYazisiViewModel
            {
                Id = blogYazisi.Id,
                Baslik = blogYazisi.Baslik,
                Icerik = blogYazisi.Icerik,
                KategoriId = blogYazisi.KategoriId,
                MevcutResimYolu = blogYazisi.ResimYolu
            };
            
            ViewBag.Kategoriler = await _blogApi.TumKategorileriGetirAsync();
            return View(model);
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(BlogYazisiViewModel model)
        {
            if (ModelState.IsValid)
            {
                var blogYazisi = await _blogApi.BlogYazisiniGetirAsync(model.Id);
                
                if (blogYazisi == null)
                {
                    return NotFound();
                }
                
                // Yalnızca yazarın kendisi düzenleyebilir
                var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (blogYazisi.KullaniciId != kullaniciId)
                {
                    return Forbid();
                }
                
                blogYazisi.Baslik = model.Baslik;
                blogYazisi.Icerik = model.Icerik;
                blogYazisi.KategoriId = model.KategoriId;
                
                // Eğer yeni resim yüklenmişse
                if (model.ResimDosyasi != null && model.ResimDosyasi.Length > 0)
                {
                    // Eski resmi sil
                    if (!string.IsNullOrEmpty(blogYazisi.ResimYolu))
                    {
                        string eskiResimYolu = Path.Combine(_hostEnvironment.WebRootPath, blogYazisi.ResimYolu.TrimStart('/'));
                        if (System.IO.File.Exists(eskiResimYolu))
                        {
                            System.IO.File.Delete(eskiResimYolu);
                        }
                    }
                    
                    // Yeni resim ekle
                    string dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(model.ResimDosyasi.FileName);
                    string klasorYolu = Path.Combine(_hostEnvironment.WebRootPath, "img", "blog");
                    
                    // Klasör yoksa oluştur
                    if (!Directory.Exists(klasorYolu))
                    {
                        Directory.CreateDirectory(klasorYolu);
                    }
                    
                    string dosyaYolu = Path.Combine(klasorYolu, dosyaAdi);
                    
                    using (var fileStream = new FileStream(dosyaYolu, FileMode.Create))
                    {
                        await model.ResimDosyasi.CopyToAsync(fileStream);
                    }
                    
                    blogYazisi.ResimYolu = "/img/blog/" + dosyaAdi;
                }
                
                await _blogApi.BlogYazisiGuncelleAsync(blogYazisi);
                
                return RedirectToAction("Detay", new { id = blogYazisi.Id });
            }
            
            ViewBag.Kategoriler = await _blogApi.TumKategorileriGetirAsync();
            return View(model);
        }
        
        // Blog yazısı silme işlemi
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            var blogYazisi = await _blogApi.BlogYazisiniGetirAsync(id);
            
            if (blogYazisi == null)
            {
                return NotFound();
            }
            
            // Yalnızca yazarın kendisi silebilir
            var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (blogYazisi.KullaniciId != kullaniciId)
            {
                return Forbid();
            }
            
            // Resmi sil
            if (!string.IsNullOrEmpty(blogYazisi.ResimYolu))
            {
                string resimYolu = Path.Combine(_hostEnvironment.WebRootPath, blogYazisi.ResimYolu.TrimStart('/'));
                if (System.IO.File.Exists(resimYolu))
                {
                    System.IO.File.Delete(resimYolu);
                }
            }
            
            await _blogApi.BlogYazisiSilAsync(id);
            
            return RedirectToAction("Index", "Home");
        }
        
        // Yorum ekleme işlemi
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YorumEkle(int blogYazisiId, string yorumIcerik)
        {
            if (string.IsNullOrWhiteSpace(yorumIcerik))
            {
                return RedirectToAction("Detay", new { id = blogYazisiId });
            }
            
            var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var yorum = new Yorum
            {
                BlogYazisiId = blogYazisiId,
                KullaniciId = kullaniciId,
                Icerik = yorumIcerik,
                YorumTarihi = DateTime.Now
            };
            
            await _blogApi.YorumEkleAsync(yorum);
            
            return RedirectToAction("Detay", new { id = blogYazisiId });
        }
    }
} 