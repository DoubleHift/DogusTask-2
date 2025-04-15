using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BatuhanDemiralMVC1.Models
{
    public class Yorum
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Yorum içeriği zorunludur")]
        public string Icerik { get; set; }
        
        public DateTime YorumTarihi { get; set; } = DateTime.Now;
        
        // BlogYazisi için foreign key
        public int BlogYazisiId { get; set; }
        
        // Kullanıcı (Yorumcu) için foreign key
        public int KullaniciId { get; set; }
        
        // Navigasyon özellikleri
        [ForeignKey("BlogYazisiId")]
        public virtual BlogYazisi BlogYazisi { get; set; }
        
        [ForeignKey("KullaniciId")]
        public virtual Kullanici Kullanici { get; set; }
    }
} 