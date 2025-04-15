using BatuhanDemiralMVC1.Models;
using Microsoft.EntityFrameworkCore;

namespace BatuhanDemiralMVC1.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UygulamaDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<UygulamaDbContext>>()))
            {
                // Örnek veriler eklenip eklenmediğini kontrol et
                if (context.Kullanicilar.Any() || context.Kategoriler.Any() || context.BlogYazilari.Any())
                {
                    return; // Veritabanı zaten verilere sahip
                }

                // Örnek kullanıcılar ekle
                context.Kullanicilar.AddRange(
                    new Kullanici
                    {
                        KullaniciAdi = "admin",
                        Email = "admin@example.com",
                        Sifre = "123456",
                        KayitTarihi = DateTime.Now.AddDays(-30)
                    },
                    new Kullanici
                    {
                        KullaniciAdi = "batuhan",
                        Email = "batuhan@example.com",
                        Sifre = "123456",
                        KayitTarihi = DateTime.Now.AddDays(-25)
                    },
                    new Kullanici
                    {
                        KullaniciAdi = "demirel",
                        Email = "demirel@example.com",
                        Sifre = "123456",
                        KayitTarihi = DateTime.Now.AddDays(-20)
                    }
                );

                context.SaveChanges();

                // Örnek kategoriler ekle
                context.Kategoriler.AddRange(
                    new Kategori
                    {
                        Ad = "Teknoloji",
                        Aciklama = "Teknoloji ile ilgili blog yazıları"
                    },
                    new Kategori
                    {
                        Ad = "Programlama",
                        Aciklama = "Programlama ile ilgili blog yazıları"
                    },
                    new Kategori
                    {
                        Ad = "Web Geliştirme",
                        Aciklama = "Web geliştirme ile ilgili blog yazıları"
                    },
                    new Kategori
                    {
                        Ad = "Mobil",
                        Aciklama = "Mobil uygulama geliştirme ile ilgili blog yazıları"
                    }
                );

                context.SaveChanges();

                // Kullanıcı ve kategori ID'leri al
                var adminId = context.Kullanicilar.First(u => u.KullaniciAdi == "admin").Id;
                var batuhanId = context.Kullanicilar.First(u => u.KullaniciAdi == "batuhan").Id;
                var demirelId = context.Kullanicilar.First(u => u.KullaniciAdi == "demirel").Id;

                var teknolojiId = context.Kategoriler.First(c => c.Ad == "Teknoloji").Id;
                var programlamaId = context.Kategoriler.First(c => c.Ad == "Programlama").Id;
                var webId = context.Kategoriler.First(c => c.Ad == "Web Geliştirme").Id;
                var mobilId = context.Kategoriler.First(c => c.Ad == "Mobil").Id;

                // Örnek blog yazıları ekle
                context.BlogYazilari.AddRange(
                    new BlogYazisi
                    {
                        Baslik = "C# Programlama Dili",
                        Icerik = "C# Microsoft tarafından geliştirilen, modern, nesne yönelimli bir programlama dilidir. C# ile konsol uygulamaları, Windows Forms uygulamaları, web uygulamaları ve mobil uygulamalar geliştirebilirsiniz.",
                        YayinTarihi = DateTime.Now.AddDays(-15),
                        KullaniciId = adminId,
                        KategoriId = programlamaId
                    },
                    new BlogYazisi
                    {
                        Baslik = "ASP.NET Core MVC ile Web Geliştirme",
                        Icerik = "ASP.NET Core MVC, Microsoft'un açık kaynak kodlu, platform bağımsız bir web framework'üdür. Model-View-Controller (MVC) mimarisini kullanarak modern web uygulamaları geliştirmenizi sağlar.",
                        YayinTarihi = DateTime.Now.AddDays(-10),
                        KullaniciId = batuhanId,
                        KategoriId = webId
                    },
                    new BlogYazisi
                    {
                        Baslik = "Entity Framework Core ile Veritabanı İşlemleri",
                        Icerik = "Entity Framework Core, Microsoft'un ORM (Object-Relational Mapping) teknolojisidir. Veritabanı işlemlerini nesne yönelimli bir şekilde yapmanızı sağlar ve SQL kodları yazmadan veritabanı işlemleri gerçekleştirebilirsiniz.",
                        YayinTarihi = DateTime.Now.AddDays(-8),
                        KullaniciId = demirelId,
                        KategoriId = programlamaId
                    },
                    new BlogYazisi
                    {
                        Baslik = "Yapay Zeka ve Makine Öğrenimi",
                        Icerik = "Yapay zeka ve makine öğrenimi, bilgisayar sistemlerinin insan zekasını taklit ederek öğrenme, karar verme ve problem çözme yeteneklerini geliştiren teknolojilerdir.",
                        YayinTarihi = DateTime.Now.AddDays(-5),
                        KullaniciId = adminId,
                        KategoriId = teknolojiId
                    },
                    new BlogYazisi
                    {
                        Baslik = "Xamarin ile Mobil Uygulama Geliştirme",
                        Icerik = "Xamarin, Microsoft'un platform bağımsız mobil uygulama geliştirme teknolojisidir. C# kullanarak iOS, Android ve Windows için tek bir kod tabanı ile uygulama geliştirmenizi sağlar.",
                        YayinTarihi = DateTime.Now.AddDays(-3),
                        KullaniciId = batuhanId,
                        KategoriId = mobilId
                    }
                );

                context.SaveChanges();

                // Blog yazısı ID'lerini al
                var csharpId = context.BlogYazilari.First(b => b.Baslik == "C# Programlama Dili").Id;
                var mvcId = context.BlogYazilari.First(b => b.Baslik == "ASP.NET Core MVC ile Web Geliştirme").Id;
                var efcoreId = context.BlogYazilari.First(b => b.Baslik == "Entity Framework Core ile Veritabanı İşlemleri").Id;

                // Örnek yorumlar ekle
                context.Yorumlar.AddRange(
                    new Yorum
                    {
                        Icerik = "Çok faydalı bir yazı, teşekkürler!",
                        YorumTarihi = DateTime.Now.AddDays(-14),
                        BlogYazisiId = csharpId,
                        KullaniciId = batuhanId
                    },
                    new Yorum
                    {
                        Icerik = "Daha fazla örnek ile desteklerseniz sevinirim.",
                        YorumTarihi = DateTime.Now.AddDays(-13),
                        BlogYazisiId = csharpId,
                        KullaniciId = demirelId
                    },
                    new Yorum
                    {
                        Icerik = "MVC mimarisi gerçekten çok kullanışlı.",
                        YorumTarihi = DateTime.Now.AddDays(-9),
                        BlogYazisiId = mvcId,
                        KullaniciId = adminId
                    },
                    new Yorum
                    {
                        Icerik = "Entity Framework Core ile veritabanı işlemleri çok kolaylaşıyor.",
                        YorumTarihi = DateTime.Now.AddDays(-7),
                        BlogYazisiId = efcoreId,
                        KullaniciId = batuhanId
                    }
                );

                context.SaveChanges();
            }
        }
    }
} 