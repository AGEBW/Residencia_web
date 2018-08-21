using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_seg_expira_clave
{
    public class IndexModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public IndexModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        public int SearchUsuarios { get; set; }
    

        public IList<seg_expira_clave> seg_expira_clave { get;set; }
        

        [BindProperty]
        public rh_cat_persona cat_usuarios { get; set; }

        public async Task OnGetAsync(int id, string usuario)
        {
            SearchUsuarios = id;

            if (id != 0 && usuario != null)
            {

                Microsoft.AspNetCore.Mvc.Razor.Global.nombre = usuario;
                Microsoft.AspNetCore.Mvc.Razor.Global.id = id;

            }

            SearchUsuarios = id;

     

            var item = from m in _context.seg_expira_claves
                       select m;

            item = item.Where(s => s.IdUsuario.Equals(Microsoft.AspNetCore.Mvc.Razor.Global.id));

            seg_expira_clave = await item.ToListAsync();


          /*  seg_expira_clave = await _context.seg_expira_claves.ToListAsync();*/
        }
    }
}
