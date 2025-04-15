using System.ComponentModel.DataAnnotations;

namespace BatuhanDemiralMVC1.Models
{
    public class Kullanici
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [StringLength(50)]
        public string KullaniciAdi { get; set; }
        
        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi")]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Sifre { get; set; }
        
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
        
        // Blog yazıları ile one-to-many ilişkisi
        public virtual ICollection<BlogYazisi> BlogYazilari { get; set; }
        
        // Yorumlar için navigasyon özelliği
        public virtual ICollection<Yorum> Yorumlar { get; set; }
    }
} 