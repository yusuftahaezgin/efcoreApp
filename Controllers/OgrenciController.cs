using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class OgrenciController:Controller
    {   
        private readonly DataContext _context;
        public OgrenciController(DataContext context)
        {
            _context=context;
        }

        public async Task<IActionResult> Index(){ // ogrencileri listeleme
            var ogrenciler = await _context.Ogrenciler.ToListAsync();
            return View(ogrenciler);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogrenci model){ //ogrenci ekleme
            _context.Ogrenciler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id){
            if(id==null){
                return NotFound();
            }

           // var ogr = await _context.Ogrenciler.FindAsync(id); bu fonk ile sadece id ye gore arama yapabiliriz
            var ogr = await _context.Ogrenciler
            .Include(x=>x.KursKayitlari)
            .ThenInclude(x=>x.Kurs)
            .FirstOrDefaultAsync(o=>o.OgrenciId == id); 
            
            if(ogr==null){
                return NotFound();
            }

            return View(ogr);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ogrenci model){ // ogrenci guncelleme
            if(id!=model.OgrenciId){ // route daki id ile modelden gelen id yi karsilastir
                return NotFound();
            }

           if(ModelState.IsValid){
            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!_context.Ogrenciler.Any(o=>o.OgrenciId == model.OgrenciId)){
                    return NotFound();
                }
                else{
                    throw;
                }
            }
            return RedirectToAction("Index");
           }
           return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id){
            if(id==null){
                return NotFound();
            }

           var ogr = await _context.Ogrenciler.FindAsync(id); 

            if(ogr==null){
                return NotFound();
            }

            return View(ogr);
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id){ //formdaki id ile karsilastirdik
           var ogrenci = await _context.Ogrenciler.FindAsync(id); 
            if(ogrenci==null){
                return NotFound();
            }
            _context.Ogrenciler.Remove(ogrenci);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }

}