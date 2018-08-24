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
    public class CreateModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public CreateModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public bool TelP { get; set; }

        [BindProperty]
        public rh_cat_personas rh_cat_persona { get; set; }

        public string Control { get; set; }
        public int IdP { get; set; }
        public string Ap { get; set; }
        public string Am { get; set; }
        public string SearchString { get; set; }

        //Método GET
        public async Task<IActionResult> OnGetAsync(int id, string searchString, string control, string ap, string am)
        {
            //Parametros para mostrar la info de la persona a agregar domicilios
            Control = control;
            IdP = id;
            Ap = ap;
            Am = am;
            SearchString = searchString;
            TelP = false;

            if (id == 0)
            {
                return NotFound();
            }

            rh_cat_persona = await _context.rh_cat_personas.SingleOrDefaultAsync(m => m.IdPersona == id);

            if (rh_cat_persona == null)
            {
                return NotFound();
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

        [BindProperty]
        public rh_cat_telefonos rh_cat_telefono { get; set; }

        //Método POST
        public async Task<IActionResult> OnPostAsync(bool TelP)
        {
            rh_cat_telefono.FechaReg = DateTime.Now;
            rh_cat_telefono.FechaUltMod = DateTime.Now;
            rh_cat_telefono.Activo = "S";
            rh_cat_telefono.Borrado = "N";
            rh_cat_telefono.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;
            rh_cat_telefono.UsuarioReg = Microsoft.AspNetCore.Mvc.Razor.Global.name;

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

            _context.rh_cat_telefonos.Add(rh_cat_telefono);

            if (rh_cat_telefono.Principal == "S")
            {
                var cambiarPrincipal = await _context.rh_cat_telefonos.SingleOrDefaultAsync
                    (m => m.IdTelefono != rh_cat_telefono.IdTelefono && m.Principal == "S");
                //Si es nulo significa que aun no registraban algun telefono como principal
                if (cambiarPrincipal != null)
                {
                    cambiarPrincipal.Principal = "N";
                    _context.SaveChanges();
                }//if cambiarPrincipal != null

            }//if principal == S

            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = rh_cat_telefono.IdTelefono });
        }
    }
}