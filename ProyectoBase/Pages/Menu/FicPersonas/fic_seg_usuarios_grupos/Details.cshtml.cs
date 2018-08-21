using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_seg_usuarios_grupos
{
    public class DetailsModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public DetailsModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        public seg_usuarios_grupo seg_usuarios_grupo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            seg_usuarios_grupo = await _context.seg_usuarios_grupos.SingleOrDefaultAsync(m => m.IdsegGrupo == id);

            if (seg_usuarios_grupo == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
