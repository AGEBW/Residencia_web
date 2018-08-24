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
    public class CreateModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public CreateModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            //Combo para estado civil
            //Primero hacemos la consulta
            var estatus = from est in _context.cat_estatus
                                where est.IdTipoEstatus == 4 //pablo descomento esto 
                                  select new { id_estatus = est.IdEstatus, des_estatus = est.DesEstatus };
            //Despues agregamos los datos al combo
            var registro = new List<SelectListItem>();
            registro.Add(new SelectListItem() { Text = "Selecciona un estatus...", Value = string.Empty });
            foreach (var item in estatus)
            {
                registro.Add(new SelectListItem() { Text = item.des_estatus +"" , Value = "" + item.id_estatus });
            }

            ViewData["IdEstatus"] = new SelectList(registro, "Value", "Text");

            /////////////////////////////////////////////////////////////////////////////////////////////////////////

            return Page();
        }

        [BindProperty]
        public seg_usuarios_estatus seg_usuarios_estatu { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            seg_usuarios_estatu.FechaReg = DateTime.Now;
            //seg_usuarios_estatu.FechaUltMod = DateTime.Now;
            seg_usuarios_estatu.Activo = "S";
            seg_usuarios_estatu.Borrado = "N";
            //seg_usuarios_estatu.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;
            seg_usuarios_estatu.UsuarioReg = Microsoft.AspNetCore.Mvc.Razor.Global.name;
            seg_usuarios_estatu.Actual = "S";
            seg_usuarios_estatu.FechaEstatus = DateTime.Now;
            seg_usuarios_estatu.IdTipoEstatus = 4;

            _context.seg_usuarios_estatus.Add(seg_usuarios_estatu);
            await _context.SaveChangesAsync();

            var cambiarPrincipal = await _context.seg_usuarios_estatus.SingleOrDefaultAsync
                (m => m.IdCrtlEstatus != seg_usuarios_estatu.IdCrtlEstatus && m.Actual == "S" && m.IdUsuario == seg_usuarios_estatu.IdUsuario);
            //Si es nulo significa que aun no registraban ningun domicilio como principal
            if (cambiarPrincipal != null)
            {
                cambiarPrincipal.Actual = "N";
                _context.SaveChanges();
            }//if cambiarPrincipal != null

            return RedirectToPage("./Index");
        }
    }
}