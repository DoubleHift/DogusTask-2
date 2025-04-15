using System.Security.Claims;
using BatuhanDemiralMVC1.Data;
using BatuhanDemiralMVC1.Models;
using BatuhanDemiralMVC1.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BatuhanDemiralMVC1.Controllers
{
    public class HesapController : Controller
    {
        private readonly BlogApiVeSorgular _blogApi;

        public HesapController(BlogApiVeSorgular blogApi)
        {
            _blogApi = blogApi;
        }
        
        // Kayıt Sayfası
        [HttpGet]
        public IActionResult Kayit()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kayit(KayitViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kullanici = await _blogApi.KullaniciAdinaGoreKullaniciGetirAsync(model.KullaniciAdi);
                
                if (kullanici != null)
                {
                    ModelState.AddModelError("KullaniciAdi", "Bu kullanıcı adı zaten kullanılıyor.");
                    return View(model);
                }
                
                kullanici = new Kullanici
                {
                    KullaniciAdi = model.KullaniciAdi,
                    Email = model.Email,
                    Sifre = model.Sifre,
                    KayitTarihi = DateTime.Now
                };
                
                await _blogApi.KullaniciEkleAsync(kullanici);
                
                // Otomatik giriş yap
                await KullaniciGirisYapAsync(kullanici);
                
                return RedirectToAction("Index", "Home");
            }
            
            return View(model);
        }
        
        // Giriş Sayfası
        [HttpGet]
        public IActionResult Giris(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Giris(GirisViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var kullanici = await _blogApi.KullaniciAdinaGoreKullaniciGetirAsync(model.KullaniciAdi);
                
                if (kullanici != null && kullanici.Sifre == model.Sifre)
                {
                    await KullaniciGirisYapAsync(kullanici);
                    
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    
                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
            }
            
            return View(model);
        }
        
        // Çıkış İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Cikis()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        
        // Erişim Engellendi
        [HttpGet]
        public IActionResult ErisimEngellendi()
        {
            return View();
        }
        
        // Kullanıcı giriş yardımcı methodu
        private async Task KullaniciGirisYapAsync(Kullanici kullanici)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
                new Claim(ClaimTypes.Name, kullanici.KullaniciAdi)
            };
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };
            
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
} 