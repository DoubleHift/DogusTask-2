using System.Diagnostics;
using BatuhanDemiralMVC1.Data;
using BatuhanDemiralMVC1.Models;
using Microsoft.AspNetCore.Mvc;

namespace BatuhanDemiralMVC1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogApiVeSorgular _blogApi;

        public HomeController(ILogger<HomeController> logger, BlogApiVeSorgular blogApi)
        {
            _logger = logger;
            _blogApi = blogApi;
        }

        public async Task<IActionResult> Index(int? kategoriId = null)
        {
            List<BlogYazisi> blogYazilari;

            if (kategoriId.HasValue)
            {
                blogYazilari = await _blogApi.KategoriyeGoreBlogYazilariniGetirAsync(kategoriId.Value);
                ViewBag.AktifKategoriId = kategoriId;
            }
            else
            {
                blogYazilari = await _blogApi.TumBlogYazilariniGetirAsync();
            }

            // Tüm kategorileri ViewBag'e ekle
            ViewBag.Kategoriler = await _blogApi.TumKategorileriGetirAsync();

            return View(blogYazilari);
        }

        public IActionResult Gizlilik()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Hata()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
