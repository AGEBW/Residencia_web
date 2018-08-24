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
    public class IndexModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public IndexModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<rh_cat_telefonos> rh_cat_telefono { get; set; }

        public string control { get; set; }
        public int id { get; set; }
        public string ap { get; set; }
        public string am { get; set; }
        public string searchString { get; set; }

        public string search { get; set; }

        [BindProperty]
        public rh_cat_personas rh_cat_persona { get; set; }

        //Método GET
        public async Task<IActionResult> OnGetAsync(int id, string search, string searchString, string control, string ap, string am)
        {
            //Parametros para mostrar la info de la persona a agregar domicilios
            //Parametros para mostrar la info de la persona a agregar domicilios
            if (control != null && ap != null && am != null && searchString != null)
            {

                Microsoft.AspNetCore.Mvc.Razor.Global.control = control;
                Microsoft.AspNetCore.Mvc.Razor.Global.nombre = searchString;
                Microsoft.AspNetCore.Mvc.Razor.Global.ap = ap;
                Microsoft.AspNetCore.Mvc.Razor.Global.am = am;
                Microsoft.AspNetCore.Mvc.Razor.Global.id = id;

            }
            //Parametros para mostrar la info de la persona a agregar domicilios

            this.id = id;
            this.search = search;
            //Esto es para mostrar solo los domicilios de un usuario gracias a su id
            var item = from m in _context.rh_cat_telefonos
                       select m;

            if (!String.IsNullOrEmpty(search))
            {
                item = item.Where(s => s.Principal.Contains(search) || s.Activo.Contains(search) || s.Borrado.Contains(search) || s.CodPais.Contains(search)
                || s.NumTelefono.Contains(search) || s.NumExtension.Contains(search) || s.Referencia.Contains(search)
                || s.UsuarioMod.Contains(search) || s.UsuarioReg.Contains(search));
            }

            item = item.Where(s => s.ClaveReferencia.Equals("" + Microsoft.AspNetCore.Mvc.Razor.Global.id));




            rh_cat_telefono = await item.ToListAsync();

            //Esto es asignarle los valores a la variable rh_cat_personas y poderlo usar en el index para pasar
            //el parametro de id a la siguiente ventana
            if (id == 0)
            {
                return NotFound();
            }

            rh_cat_persona = await _context.rh_cat_personas.SingleOrDefaultAsync(m => m.IdPersona == Microsoft.AspNetCore.Mvc.Razor.Global.id);

            if (rh_cat_persona == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
