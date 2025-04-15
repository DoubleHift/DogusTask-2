using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BatuhanDemiralMVC1.ViewModels
{
    public class BlogYazisiViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200)]
        public string Baslik { get; set; }
        
        [Required(ErrorMessage = "İçerik zorunludur")]
        public string Icerik { get; set; }
        
        [Required(ErrorMessage = "Kategori zorunludur")]
        public int KategoriId { get; set; }
        
        // Resim yükleme için
        public IFormFile? ResimDosyasi { get; set; }
        
        // Var olan resmi göstermek için
        public string? MevcutResimYolu { get; set; }
    }
} 