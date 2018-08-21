using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_seg_expira_clave
{
    public class EditModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public EditModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public seg_expira_clave seg_expira_clave { get; set; }

        public bool ActualP { get; set; }
        public bool ClaveSys { get; set; }
        public bool ActivoP { get; set; }
        public bool BorradoP { get; set; }
        public string Password { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            seg_expira_clave = await _context.seg_expira_claves.SingleOrDefaultAsync(m => m.IdClave == id);
            Password = seg_expira_clave.Clave.ToString();
            if (seg_expira_clave == null)
            {
                return NotFound();
            }

            if (seg_expira_clave.Activo == "S") {
                ActivoP = true;
            }
            else {
                ActivoP = false; 
            }

            if (seg_expira_clave.Borrado == "S")
            {
                BorradoP = true;
            }
            else
            {
                BorradoP = false;
            }

            if (seg_expira_clave.Actual == "S")
            {
                ActualP = true;
            }
            else
            {
                ActualP = false;
            }

            if (seg_expira_clave.ClaveAutoSys == "S")
            {
                ClaveSys = true;
            }
            else
            {
                ClaveSys = false;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool ActualP, bool ActivoP, bool BorradoP, bool ClaveSys)
        {
            seg_expira_clave.FechaUltMod = DateTime.Now;
            seg_expira_clave.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Actual
            if (ActualP)
            {
                seg_expira_clave.Actual = "S";
            }
            else
            {
                seg_expira_clave.Actual = "N";
            }
            //Activo
            if (ActivoP)
            {
                seg_expira_clave.Activo = "S";
            }
            else
            {
                seg_expira_clave.Activo = "N";
            }
            //Borrado
            if (BorradoP)
            {
                seg_expira_clave.Borrado = "S";
            }
            else
            {
                seg_expira_clave.Borrado = "N";
            }

            //ClaveSys
            if (ClaveSys)
            {
                seg_expira_clave.ClaveAutoSys = "S";
            }
            else
            {
                seg_expira_clave.ClaveAutoSys = "N";
            }

            _context.Attach(seg_expira_clave).State = EntityState.Modified;

            //Si es la contraseña actual entonces tiene que buscar si hay otra para desmarcarlo
            if (seg_expira_clave.Actual == "S")
            {
                var cambiarActual = await _context.seg_expira_claves.SingleOrDefaultAsync
                    (m => m.IdClave != seg_expira_clave.IdClave && m.Actual == "S" && m.IdUsuario == seg_expira_clave.IdUsuario);

                if (cambiarActual != null)
                {
                    cambiarActual.Actual = "N";
                    _context.SaveChanges();
                }//if cambiarPrincipal != null

            }//if principal == S

            //Si quiere quitar la contraseña acutal debemos verificar que no sea la unica clave que contenga actual
            else
            {
                var cambiarActual = await _context.seg_expira_claves.SingleOrDefaultAsync
                    (m => m.IdClave != seg_expira_clave.IdClave && m.Actual == "S" && m.IdUsuario == seg_expira_clave.IdUsuario);
                //Si es nulo significa que va a dejar sin domicilio principal, entonces no lo dejamos desmarcar la casilla
                if (cambiarActual == null)
                {
                    seg_expira_clave.Actual = "S";
                    _context.SaveChanges();
                }//if cambiarPrincipal == null
            }

            //Verificacion de que un usuario no quiera actualizar un dato que ya se elimino
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!seg_expira_claveExists(seg_expira_clave.IdClave))
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

        private bool seg_expira_claveExists(int id)
        {
            return _context.seg_expira_claves.Any(e => e.IdClave == id);
        }
    }
}
