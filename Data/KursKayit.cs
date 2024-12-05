using System.ComponentModel.DataAnnotations;

namespace efcoreApp.Data{
    public class KursKayit{

        [Key]
        public int KayitId { get; set; }
        public int OgrenciId { get; set; }
        public Ogrenci Ogrenci { get; set; }=null!; // iliskili iki tabloyu join etmek icin gerekli (navigation properties)
        public int KursId { get; set; }
        public Kurs Kurs { get; set; }=null!; // iliskili iki tabloyu join etmek icin gerekli (navigation properties)

        public DateTime KayitTarihi { get; set; }
    }
}