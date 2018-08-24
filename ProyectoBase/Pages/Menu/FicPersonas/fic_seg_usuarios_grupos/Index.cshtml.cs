using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_seg_usuarios_grupos
{
    public class IndexModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public IndexModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }


        public int SearchUsuarios { get; set; }

        public IList<seg_usuarios_grupos> seg_usuarios_grupo { get;set; }


        [BindProperty]
        public rh_cat_personas cat_usuarios { get; set; }

        public async Task OnGetAsync(int id, string usuario)
        {
            SearchUsuarios = id;

            if (id != 0 && usuario != null)
            {

                Microsoft.AspNetCore.Mvc.Razor.Global.nombre = usuario;
                Microsoft.AspNetCore.Mvc.Razor.Global.id = id;

            }

            var item = from m in _context.seg_usuarios_grupos
                       select m;

            item = item.Where(s => s.IdUsuario.Equals(Microsoft.AspNetCore.Mvc.Razor.Global.id));

            seg_usuarios_grupo = await item.ToListAsync();


           
        }



        /*
        public async Task OnGetAsync()
        {
            seg_usuarios_grupo = await _context.seg_usuarios_grupos.ToListAsync();
        }*/









    }
}
