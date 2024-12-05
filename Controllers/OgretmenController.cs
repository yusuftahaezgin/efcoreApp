using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class OgretmenController : Controller
    {
        private readonly DataContext _context;
        public OgretmenController(DataContext context)
        {
            _context=context;
        }

        public async Task<IActionResult> Index(){
            var ogrenciler = await _context.Ogretmenler.ToListAsync();
            return View(ogrenciler);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogretmen model){ 
            _context.Ogretmenler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id){
            if(id==null){
                return NotFound();
            }

           
            var ogrt = await _context.Ogretmenler.FirstOrDefaultAsync(o=>o.OgretmenId == id); 
            
            if(ogrt==null){
                return NotFound();
            }

            return View(ogrt);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ogretmen model){ // ogrenci guncelleme
            if(id!=model.OgretmenId){ // route daki id ile modelden gelen id yi karsilastir
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
                if(!_context.Ogretmenler.Any(o=>o.OgretmenId == model.OgretmenId)){
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


    }
}