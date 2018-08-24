using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_seg_expira_clave
{
    public class CreateModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public CreateModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }
        
        public bool ClaveSys { get; set; }

        public IActionResult OnGet()
        {
            ClaveSys = false;
            
            return Page();
        }

        [BindProperty]
        public seg_expira_claves seg_expira_clave { get; set; }

        public async Task<IActionResult> OnPostAsync(bool ClaveSys)
        {

            var passwordExiste = await _context.seg_expira_claves.SingleOrDefaultAsync(m => m.IdUsuario == seg_expira_clave.IdUsuario && m.Clave == seg_expira_clave.Clave);
            if (passwordExiste != null)
            {
                ViewData["Error"] = "La contraseña \"" + seg_expira_clave.Clave+ "\" ya se encuentra registrada, es necesario escribir contraseñas diferentes!";
                return Page();
            }

            seg_expira_clave.FechaReg = DateTime.Now;
            //seg_expira_clave.FechaUltMod = DateTime.Now;
            seg_expira_clave.Activo = "S";
            seg_expira_clave.Borrado = "N";
            seg_expira_clave.Actual = "S";
            //seg_expira_clave.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;
            seg_expira_clave.UsuarioReg = Microsoft.AspNetCore.Mvc.Razor.Global.name;

            if (ClaveSys)
            {
                seg_expira_clave.ClaveAutoSys = "S";
            }
            else
            {
                seg_expira_clave.ClaveAutoSys = "N";
            }

            _context.seg_expira_claves.Add(seg_expira_clave);

            if (seg_expira_clave.Actual == "S")
            {
                var cambiarPrincipal = await _context.seg_expira_claves.SingleOrDefaultAsync
                    (m => m.IdClave != seg_expira_clave.IdClave && m.Actual == "S" && m.IdUsuario == seg_expira_clave.IdUsuario);
                //Si es nulo significa que aun no tenia ninguna contraseña actual
                if (cambiarPrincipal != null)
                {
                    cambiarPrincipal.Actual = "N";
                    _context.SaveChanges();
                }//if cambiarPrincipal != null

            }//if principal == S

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}