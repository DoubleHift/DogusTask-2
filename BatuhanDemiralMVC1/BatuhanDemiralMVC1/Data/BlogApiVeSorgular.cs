using BatuhanDemiralMVC1.Models;
using Microsoft.EntityFrameworkCore;

namespace BatuhanDemiralMVC1.Data
{
    public class BlogApiVeSorgular
    {
        private readonly UygulamaDbContext _db;

        public BlogApiVeSorgular(UygulamaDbContext db)
        {
            _db = db;
        }

        // API SORGULARI
        
        // Tüm blog yazılarını getir
        public async Task<List<BlogYazisi>> TumBlogYazilariniGetirAsync()
        {
            return await _db.BlogYazilari
                .Include(b => b.Kullanici)
                .Include(b => b.Kategori)
                .OrderByDescending(b => b.YayinTarihi)
                .ToListAsync();
        }
        
        // Blog yazısını ID'ye göre getir
        public async Task<BlogYazisi> BlogYazisiniGetirAsync(int id)
        {
            return await _db.BlogYazilari
                .Include(b => b.Kullanici)
                .Include(b => b.Kategori)
                .Include(b => b.Yorumlar)
                    .ThenInclude(c => c.Kullanici)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        
        // Kategoriye göre blog yazılarını getir
        public async Task<List<BlogYazisi>> KategoriyeGoreBlogYazilariniGetirAsync(int kategoriId)
        {
            return await _db.BlogYazilari
                .Include(b => b.Kullanici)
                .Include(b => b.Kategori)
                .Where(b => b.KategoriId == kategoriId)
                .OrderByDescending(b => b.YayinTarihi)
                .ToListAsync();
        }
        
        // Kullanıcıya göre blog yazılarını getir
        public async Task<List<BlogYazisi>> KullaniciyaGoreBlogYazilariniGetirAsync(int kullaniciId)
        {
            return await _db.BlogYazilari
                .Include(b => b.Kategori)
                .Where(b => b.KullaniciId == kullaniciId)
                .OrderByDescending(b => b.YayinTarihi)
                .ToListAsync();
        }
        
        // Blog yazısı ekle
        public async Task<int> BlogYazisiEkleAsync(BlogYazisi blogYazisi)
        {
            _db.BlogYazilari.Add(blogYazisi);
            await _db.SaveChangesAsync();
            return blogYazisi.Id;
        }
        
        // Blog yazısı güncelle
        public async Task<bool> BlogYazisiGuncelleAsync(BlogYazisi blogYazisi)
        {
            _db.BlogYazilari.Update(blogYazisi);
            var etkilenenSatirlar = await _db.SaveChangesAsync();
            return etkilenenSatirlar > 0;
        }
        
        // Blog yazısı sil
        public async Task<bool> BlogYazisiSilAsync(int id)
        {
            var blogYazisi = await _db.BlogYazilari.FindAsync(id);
            if (blogYazisi == null)
                return false;
                
            _db.BlogYazilari.Remove(blogYazisi);
            var etkilenenSatirlar = await _db.SaveChangesAsync();
            return etkilenenSatirlar > 0;
        }
        
        // Tüm kategorileri getir
        public async Task<List<Kategori>> TumKategorileriGetirAsync()
        {
            return await _db.Kategoriler.ToListAsync();
        }
        
        // Yorum ekle
        public async Task<int> YorumEkleAsync(Yorum yorum)
        {
            _db.Yorumlar.Add(yorum);
            await _db.SaveChangesAsync();
            return yorum.Id;
        }
        
        // Kullanıcı ekleme
        public async Task<int> KullaniciEkleAsync(Kullanici kullanici)
        {
            _db.Kullanicilar.Add(kullanici);
            await _db.SaveChangesAsync();
            return kullanici.Id;
        }
        
        // Kullanıcı adına göre kullanıcı getir
        public async Task<Kullanici> KullaniciAdinaGoreKullaniciGetirAsync(string kullaniciAdi)
        {
            return await _db.Kullanicilar
                .FirstOrDefaultAsync(u => u.KullaniciAdi == kullaniciAdi);
        }
        
        // SQL SORGULARI
        
        /*
        -- Tüm blog yazılarını getir
        SELECT b.*, k.KullaniciAdi, kat.Ad as KategoriAdi
        FROM BlogYazilari b
        INNER JOIN Kullanicilar k ON b.KullaniciId = k.Id
        INNER JOIN Kategoriler kat ON b.KategoriId = kat.Id
        ORDER BY b.YayinTarihi DESC
        
        -- Kategoriye göre blog yazılarını getir
        SELECT b.*, k.KullaniciAdi, kat.Ad as KategoriAdi
        FROM BlogYazilari b
        INNER JOIN Kullanicilar k ON b.KullaniciId = k.Id
        INNER JOIN Kategoriler kat ON b.KategoriId = kat.Id
        WHERE b.KategoriId = @kategoriId
        ORDER BY b.YayinTarihi DESC
        
        -- Kullanıcıya göre blog yazılarını getir
        SELECT b.*, kat.Ad as KategoriAdi
        FROM BlogYazilari b
        INNER JOIN Kategoriler kat ON b.KategoriId = kat.Id
        WHERE b.KullaniciId = @kullaniciId
        ORDER BY b.YayinTarihi DESC
        
        -- Blog yazısını ID'ye göre getir (yorumlarla birlikte)
        SELECT b.*, k.KullaniciAdi, kat.Ad as KategoriAdi,
               y.Id as YorumId, y.Icerik as YorumIcerik, y.YorumTarihi,
               yk.KullaniciAdi as YorumYazari
        FROM BlogYazilari b
        INNER JOIN Kullanicilar k ON b.KullaniciId = k.Id
        INNER JOIN Kategoriler kat ON b.KategoriId = kat.Id
        LEFT JOIN Yorumlar y ON y.BlogYazisiId = b.Id
        LEFT JOIN Kullanicilar yk ON y.KullaniciId = yk.Id
        WHERE b.Id = @blogId
        ORDER BY y.YorumTarihi DESC
        
        -- Kullanıcı adı ve şifreye göre kullanıcı sorgulama (giriş için)
        SELECT * FROM Kullanicilar 
        WHERE KullaniciAdi = @kullaniciAdi AND Sifre = @sifre
        */
    }
} 