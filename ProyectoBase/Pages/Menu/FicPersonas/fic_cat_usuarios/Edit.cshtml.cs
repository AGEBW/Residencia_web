using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_cat_usuarios
{
    public class EditModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public EditModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public cat_usuarios cat_usuario { get; set; }

        public bool ExpiraP { get; set; }
        public bool ConectadoP { get; set; }
        public bool ActivoP { get; set; }
        public bool BorradoP { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            cat_usuario = await _context.cat_usuarios.SingleOrDefaultAsync(m => m.IdUsuario == id);

            if (cat_usuario == null)
            {
                return NotFound();
            }

            if (cat_usuario.Expira == "S")
            {
                ExpiraP = true;
            }
            else {
                ExpiraP = false;
            }

            //Si selecciono de expirar o no
            if (cat_usuario.Activo == "S")
            {
                ActivoP = true;
            }
            else
            {
                ActivoP = false;
            }

            //Si selecciono de expirar o no
            if (cat_usuario.Borrado == "S")
            {
                BorradoP = true;
            }
            else
            {
                BorradoP = false;
            }

            //Si selecciono de expirar o no
            if (cat_usuario.Conectado == "S")
            {
                ConectadoP = true;
            }
            else
            {
                ConectadoP = false;
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool ExpiraP,bool ActivoP,bool BorradoP,bool ConectadoP)
        {
            cat_usuario.FechaUltMod = DateTime.Now;
            cat_usuario.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;

            //Si selecciono de expirar o no
            if (ExpiraP)
            {
                cat_usuario.Expira = "S";
            }
            else
            {
                cat_usuario.Expira = "N";
            }

            //Si selecciono de expirar o no
            if (ActivoP)
            {
                cat_usuario.Activo = "S";
            }
            else
            {
                cat_usuario.Activo = "N";
            }

            //Si selecciono de expirar o no
            if (BorradoP)
            {
                cat_usuario.Borrado = "S";
            }
            else
            {
                cat_usuario.Borrado = "N";
            }

            //Si selecciono de expirar o no
            if (ConectadoP)
            {
                cat_usuario.Conectado = "S";
            }
            else
            {
                cat_usuario.Conectado = "N";
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(cat_usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cat_usuarioExists(cat_usuario.IdUsuario))
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

        private bool cat_usuarioExists(int id)
        {
            return _context.cat_usuarios.Any(e => e.IdUsuario == id);
        }
    }
}
