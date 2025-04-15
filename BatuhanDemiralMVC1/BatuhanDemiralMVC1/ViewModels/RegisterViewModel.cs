using System.ComponentModel.DataAnnotations;

namespace BatuhanDemiralMVC1.ViewModels
{
    public class KayitViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [StringLength(50)]
        public string KullaniciAdi { get; set; }
        
        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi")]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }
        
        [Required(ErrorMessage = "Şifre tekrarı zorunludur")]
        [DataType(DataType.Password)]
        [Compare("Sifre", ErrorMessage = "Şifreler eşleşmiyor")]
        public string SifreTekrari { get; set; }
    }
} 