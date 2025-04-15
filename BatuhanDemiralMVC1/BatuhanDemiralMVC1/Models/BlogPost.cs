using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BatuhanDemiralMVC1.Models
{
    public class BlogYazisi
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200)]
        public string Baslik { get; set; }
        
        [Required(ErrorMessage = "İçerik zorunludur")]
        public string Icerik { get; set; }
        
        public DateTime YayinTarihi { get; set; } = DateTime.Now;
        
        // İsteğe bağlı resim yolu
        [StringLength(255)]
        public string? ResimYolu { get; set; }
        
        // Kullanıcı (Yazar) için foreign key
        public int KullaniciId { get; set; }
        
        // Kategori için foreign key
        public int KategoriId { get; set; }
        
        // Navigasyon özellikleri
        [ForeignKey("KullaniciId")]
        public virtual Kullanici Kullanici { get; set; }
        
        [ForeignKey("KategoriId")]
        public virtual Kategori Kategori { get; set; }
        
        // Yorumlar için navigasyon özelliği
        public virtual ICollection<Yorum> Yorumlar { get; set; }
    }
} 