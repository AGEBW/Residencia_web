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
    public class DeleteModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public DeleteModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public rh_cat_domicilios rh_cat_domicilio { get; set; }

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
            if (id == null)
            {
                return NotFound();
            }

            rh_cat_domicilio = await _context.rh_cat_domicilios.SingleOrDefaultAsync(m => m.IdDomicilio == id);

            if (rh_cat_domicilio == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            rh_cat_domicilio = await _context.rh_cat_domicilios.FindAsync(id);

            if (rh_cat_domicilio != null)
            {
                _context.rh_cat_domicilios.Remove(rh_cat_domicilio);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index",new {id = rh_cat_domicilio.ClaveReferencia});
        }
    }
}
