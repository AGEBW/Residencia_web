using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_rh_cat_dir_webs
{
    public class DetailsModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public DetailsModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        public rh_cat_dir_web rh_cat_dir_web { get; set; }

        public string Control { get; set; }
        public int IdP { get; set; }
        public string Ap { get; set; }
        public string Am { get; set; }
        public string SearchString { get; set; }

        public string TipoDirWeb { get; set; }

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

            rh_cat_dir_web = await _context.rh_cat_dir_webs.SingleOrDefaultAsync(m => m.IdDirWeb == id);

            if (rh_cat_dir_web == null)
            {
                return NotFound();
            }

            //Nombre del tipo de telefono
            var query = from dir_web in _context.rh_cat_dir_webs
                        join generales in _context.cat_generales on dir_web.IdGenDirWeb equals generales.IdGeneral
                        where dir_web.IdDirWeb == rh_cat_dir_web.IdDirWeb
                        select new { nombre = generales.DesGeneral };

            foreach (var item in query)
            {
                TipoDirWeb = item.nombre;
            }

            return Page();
        }
    }
}
