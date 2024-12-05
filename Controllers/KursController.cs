using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class KursController:Controller{

        private readonly DataContext _context;
        public KursController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(){
            var kurslar = await _context.Kurslar.Include(x=>x.Ogretmen).ToListAsync();
            return View(kurslar);
        }

        public async Task<IActionResult> Create(){
            ViewBag.Ogretmenler=new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","AdSoyad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursViewModel model){ //kurs ekleme
            if(ModelState.IsValid){
                _context.Kurslar.Add(new Kurs(){KursId=model.KursId,Baslik=model.Baslik,OgretmenId=model.OgretmenId});
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Ogretmenler=new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","AdSoyad");
            return View(model);
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id){
            if(id==null){
                return NotFound();
            }

           // var ogr = await _context.Ogrenciler.FindAsync(id); bu fonk ile sadece id ye gore arama yapabiliriz
            var kurs = await _context.Kurslar
            .Include(x=>x.KursKayitlari)
            .ThenInclude(x=>x.Ogrenci)
            .Select(x=> new KursViewModel{
                KursId=x.KursId,
                Baslik=x.Baslik,
                OgretmenId=x.OgretmenId,
                KursKayitlari=x.KursKayitlari
            })
            .FirstOrDefaultAsync(o=>o.KursId == id); 
            
            if(kurs==null){
                return NotFound();
            }
            ViewBag.Ogretmenler=new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","AdSoyad");
            return View(kurs);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KursViewModel model){ // ogrenci guncelleme
            if(id!=model.KursId){ // route daki id ile modelden gelen id yi karsilastir
                return NotFound();
            }

           if(ModelState.IsValid){
            try
            {
                _context.Update(new Kurs(){KursId=model.KursId,Baslik=model.Baslik,OgretmenId=model.OgretmenId});
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if(!_context.Kurslar.Any(o=>o.KursId == model.KursId)){
                    return NotFound();
                }
                else{
                    throw;
                }
            }
            return RedirectToAction("Index");
           }
            ViewBag.Ogretmenler=new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","AdSoyad");
           return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id){
            if(id==null){
                return NotFound();
            }

           var kurs = await _context.Kurslar.FindAsync(id); 

            if(kurs==null){
                return NotFound();
            }

            return View(kurs);
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id){ //formdaki id ile karsilastirdik
           var kurs = await _context.Kurslar.FindAsync(id); 
            if(kurs==null){
                return NotFound();
            }
            _context.Kurslar.Remove(kurs);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}