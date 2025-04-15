using System.ComponentModel.DataAnnotations;

namespace BatuhanDemiralMVC1.Models
{
    public class Kategori
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(50)]
        public string Ad { get; set; }
        
        [StringLength(200)]
        public string Aciklama { get; set; }
        
        // Blog yazıları ile many-to-one ilişkisi
        public virtual ICollection<BlogYazisi> BlogYazilari { get; set; }
    }
} 