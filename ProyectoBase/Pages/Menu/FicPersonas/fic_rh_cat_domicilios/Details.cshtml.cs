using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_rh_cat_domicilios
{
    public class DetailsModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public DetailsModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public rh_cat_domicilios rh_cat_domicilio { get; set; }

        public string Control { get; set; }
        public int IdP { get; set; }
        public string Ap { get; set; }
        public string Am { get; set; }
        public string SearchString { get; set; }

        public string TipoDom { get; set; }

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

            rh_cat_domicilio = await _context.rh_cat_domicilios.SingleOrDefaultAsync(m => m.IdDomicilio == id);

            if (rh_cat_domicilio == null)
            {
                return NotFound();
            }

            //Nombre del tipo de domicilio
            var query = from domicilio in _context.rh_cat_domicilios
                    join generales in _context.cat_generales on domicilio.IdGenDom equals generales.IdGeneral
                    where domicilio.IdDomicilio == rh_cat_domicilio.IdDomicilio
                    select new { nombre = generales.DesGeneral };

            foreach (var item in query)
            {
                TipoDom = item.nombre;
            }

            return Page();
        }
    }
}
