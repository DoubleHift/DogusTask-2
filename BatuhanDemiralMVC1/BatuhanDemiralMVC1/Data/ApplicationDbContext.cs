using BatuhanDemiralMVC1.Models;
using Microsoft.EntityFrameworkCore;

namespace BatuhanDemiralMVC1.Data
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options)
        {
        }
        
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<BlogYazisi> BlogYazilari { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Kullanıcı - BlogYazisi (One-to-Many) ilişkisi
            modelBuilder.Entity<BlogYazisi>()
                .HasOne(b => b.Kullanici)
                .WithMany(u => u.BlogYazilari)
                .HasForeignKey(b => b.KullaniciId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Kategori - BlogYazisi (Many-to-One) ilişkisi
            modelBuilder.Entity<BlogYazisi>()
                .HasOne(b => b.Kategori)
                .WithMany(c => c.BlogYazilari)
                .HasForeignKey(b => b.KategoriId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // BlogYazisi - Yorum (One-to-Many) ilişkisi
            modelBuilder.Entity<Yorum>()
                .HasOne(c => c.BlogYazisi)
                .WithMany(b => b.Yorumlar)
                .HasForeignKey(c => c.BlogYazisiId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Kullanici - Yorum (One-to-Many) ilişkisi
            modelBuilder.Entity<Yorum>()
                .HasOne(c => c.Kullanici)
                .WithMany(u => u.Yorumlar)
                .HasForeignKey(c => c.KullaniciId)
                .OnDelete(DeleteBehavior.NoAction); // BlogYazisi ile Cascade conflict'ini önlemek için
        }
    }
} 