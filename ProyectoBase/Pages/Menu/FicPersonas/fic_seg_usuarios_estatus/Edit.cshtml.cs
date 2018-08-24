using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_seg_usuarios_estatus
{
    public class EditModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public EditModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public seg_usuarios_estatus seg_usuarios_estatu { get; set; }

        
        

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            seg_usuarios_estatu = await _context.seg_usuarios_estatus.SingleOrDefaultAsync(m => m.IdCrtlEstatus == id);

            if (seg_usuarios_estatu == null)
            {
                return NotFound();
            }
            /*
            var estatus = new List<SelectListItem>();
            estatus.Add(new SelectListItem() { Text = "Selecciona un estatus...", Value = string.Empty });
            foreach (var item in _context.cat_estatus)
            {
                estatus.Add(new SelectListItem() { Text = item.DesEstatus, Value = item.IdEstatus +"" });
            }

            ViewData["IdEstatus"] = new SelectList(estatus, "Value", "Text");
            */
            //Combo para estado civil
            //Primero hacemos la consulta
            var estatus = from est in _context.cat_estatus
                              /*   where est.IdEstatus == 3*/
                          select new { id_estatus = est.IdEstatus, des_estatus = est.DesEstatus };
            //Despues agregamos los datos al combo
            var registro = new List<SelectListItem>();
            registro.Add(new SelectListItem() { Text = "Selecciona un estatus...", Value = string.Empty });
            foreach (var item in estatus)
            {
                registro.Add(new SelectListItem() { Text = item.des_estatus + "", Value = "" + item.id_estatus });
            }

            ViewData["IdEstatus"] = new SelectList(registro, "Value", "Text");

            /////////////////////////////////////////////////////////////////////////////////////////////////////////


            var TipoEstatus = from test in _context.cat_tipo_estatus
                                  /*   where est.IdEstatus == 3*/
                              select new { id_testatus = test.IdTipoEstatus, des_testatus = test.DesTipoEstatus };
            //Despues agregamos los datos al combo
            var registro2 = new List<SelectListItem>();
            registro2.Add(new SelectListItem() { Text = "Selecciona un tipo de status...", Value = string.Empty });
            foreach (var item in TipoEstatus)
            {
                registro2.Add(new SelectListItem() { Text = item.des_testatus + "", Value = "" + item.id_testatus });
            }

            ViewData["IdTipoEstatus"] = new SelectList(registro2, "Value", "Text");


            return Page();
        }




        public async Task<IActionResult> OnPostAsync()
        {
            //seg_usuarios_estatu.FechaUltMod = DateTime.Now;
            //seg_usuarios_estatu.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(seg_usuarios_estatu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!seg_usuarios_estatuExists(seg_usuarios_estatu.IdCrtlEstatus))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool seg_usuarios_estatuExists(int id)
        {
            return _context.seg_usuarios_estatus.Any(e => e.IdCrtlEstatus == id);
        }
    }
}
