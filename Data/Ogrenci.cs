using System.ComponentModel.DataAnnotations;

namespace efcoreApp.Data{
    public class Ogrenci{
        // id => primary key
        [Key] // alttaki degisken adina id yazarsak bu key bilgisini eklemeye gerek yok otomatik primary key oldugunu anlar. id yazmazsak degisken adina o zaman primary key yapmak icin bunu yazmaliyiz
        public int OgrenciId { get; set;}

        public string? OgrenciAd { get; set; }
        public string? OgrenciSoyad { get; set; }
        public string AdSoyad { 
            get {
                 return this.OgrenciAd+" "+ this.OgrenciSoyad;
            }
         }
        public string? Eposta { get; set; }
        public string? Telefon { get; set; }

        public ICollection<KursKayit> KursKayitlari { get; set; }= new List<KursKayit>();
        
    }
}