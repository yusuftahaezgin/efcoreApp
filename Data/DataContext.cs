using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Data{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }
        public DbSet<Kurs> Kurslar =>Set<Kurs>(); // nullable oldugu icin uyari verir uyariyi engellemek icin set ettik basta

        public DbSet<Ogrenci> Ogrenciler =>Set<Ogrenci>();

        public DbSet<KursKayit> KursKayitlar =>Set<KursKayit>();
        public DbSet<Ogretmen> Ogretmenler =>Set<Ogretmen>();
    }
}