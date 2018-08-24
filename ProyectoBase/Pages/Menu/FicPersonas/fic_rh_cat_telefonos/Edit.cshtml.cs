using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_rh_cat_telefonos
{
    public class EditModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public EditModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public bool TelP { get; set; }
        public bool ActivoP { get; set; }
        public bool BorradoP { get; set; }

        [BindProperty]
        public rh_cat_telefonos rh_cat_telefono { get; set; }

        public string Control { get; set; }
        public int IdP { get; set; }
        public string Ap { get; set; }
        public string Am { get; set; }
        public string SearchString { get; set; }

        //Método GET
        public async Task<IActionResult> OnGetAsync(int id, int idP, string searchString, string control, string ap, string am)
        {
            //Parametros para mostrar la info de la persona a agregar domicilios
            Control = control;
            IdP = idP;
            Ap = ap;
            Am = am;
            SearchString = searchString;
            if (id == 0)
            {
                return NotFound();
            }

            rh_cat_telefono = await _context.rh_cat_telefonos.SingleOrDefaultAsync(m => m.IdTelefono == id);

            if (rh_cat_telefono == null)
            {
                return NotFound();
            }

            //Telefono Principal
            if (rh_cat_telefono.Principal == "S")
            {
                TelP = true;
            }
            else
            {
                TelP = false;
            }

            //Activo
            if (rh_cat_telefono.Activo == "S")
            {
                ActivoP = true;
            }
            else
            {
                ActivoP = false;
            }

            //Borrado
            if (rh_cat_telefono.Borrado == "S")
            {
                BorradoP = true;
            }
            else
            {
                BorradoP = false;
            }

            //Combo para tipo telefono
            //Primero hacemos la consulta
            var tel = from ocupacion in _context.cat_generales
                      where ocupacion.IdTipoGeneral == 10
                      select new { nom_tel = ocupacion.DesGeneral, id_tel = ocupacion.IdGeneral };
            //Despues agregamos los datos al combo
            var telefonoReg = new List<SelectListItem>();
            telefonoReg.Add(new SelectListItem() { Text = "Selecciona un tipo de telefono...", Value = string.Empty });
            foreach (var item in tel)
            {
                telefonoReg.Add(new SelectListItem() { Text = item.nom_tel, Value = "" + item.id_tel });
            }

            ViewData["IdTipoTel"] = new SelectList(telefonoReg, "Value", "Text");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool TelP, bool ActivoP, bool BorradoP)
        {
            rh_cat_telefono.FechaUltMod = DateTime.Now;
            rh_cat_telefono.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Si marco el checkbox entonces SI es el domicilio principal
            //de lo contrario entonces NO lo es
            if (TelP)
            {
                rh_cat_telefono.Principal = "S";
            }
            else
            {
                rh_cat_telefono.Principal = "N";
            }
            //Activo
            if (ActivoP)
            {
                rh_cat_telefono.Activo = "S";
            }
            else
            {
                rh_cat_telefono.Activo = "N";
            }
            //Borrado
            if (BorradoP)
            {
                rh_cat_telefono.Borrado = "S";
            }
            else
            {
                rh_cat_telefono.Borrado = "N";
            }

            _context.Attach(rh_cat_telefono).State = EntityState.Modified;

            //Si es el telefono principal entonces tiene que buscar si hay otro para desmarcarlo
            if (rh_cat_telefono.Principal == "S")
            {
                var cambiarPrincipal = await _context.rh_cat_telefonos.SingleOrDefaultAsync
                    (m => m.IdTelefono != rh_cat_telefono.IdTelefono && m.Principal == "S");

                if (cambiarPrincipal != null)
                {
                    cambiarPrincipal.Principal = "N";
                    _context.SaveChanges();
                }//if cambiarPrincipal != null

            }//if principal == S

            //Si quiere quitar el telefono principal primero debemos de ver que no sea el ultimo
            else
            {
                var cambiarPrincipal = await _context.rh_cat_telefonos.SingleOrDefaultAsync
                    (m => m.IdTelefono != rh_cat_telefono.IdTelefono && m.Principal == "S");
                //Si es nulo significa que va a dejar sin telefono principal, entonces no lo dejamos desmarcar la casilla
                if (cambiarPrincipal == null)
                {
                    rh_cat_telefono.Principal = "S";
                    _context.SaveChanges();
                }//if cambiarPrincipal == null
            }

            //Intenta actualizar y verificar que no se haya eliminado mientras otro usuario modificaba/eliminaba el mismo registro
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!rh_cat_telefonoExists(rh_cat_telefono.IdTelefono))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = rh_cat_telefono.IdTelefono });
        }

        private bool rh_cat_telefonoExists(int id)
        {
            return _context.rh_cat_telefonos.Any(e => e.IdTelefono == id);
        }
    }
}
