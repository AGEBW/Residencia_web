using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_rh_cat_telefonos
{
    public class DetailsModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public DetailsModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public rh_cat_telefonos rh_cat_telefono { get; set; }

        public string Control { get; set; }
        public int IdP { get; set; }
        public string Ap { get; set; }
        public string Am { get; set; }
        public string SearchString { get; set; }

        public string TipoTel { get; set; }


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

            //Nombre del tipo de telefono
            var query = from telefono in _context.rh_cat_telefonos
                        join generales in _context.cat_generales on telefono.IdGenTelefono equals generales.IdGeneral
                        where telefono.IdTelefono == rh_cat_telefono.IdTelefono
                        select new { nombre = generales.DesGeneral };

            foreach (var item in query)
            {
                TipoTel = item.nombre;
            }

            return Page();
        }
    }
}
