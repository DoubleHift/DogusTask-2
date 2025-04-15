using System.ComponentModel.DataAnnotations;

namespace BatuhanDemiralMVC1.ViewModels
{
    public class GirisViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string KullaniciAdi { get; set; }
        
        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }
        
        public bool BeniHatirla { get; set; }
    }
} 