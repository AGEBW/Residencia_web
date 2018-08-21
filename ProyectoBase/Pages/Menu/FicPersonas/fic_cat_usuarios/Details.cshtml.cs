using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_cat_usuarios
{
    public class DetailsModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public DetailsModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public cat_usuarios cat_usuario { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            cat_usuario = await _context.cat_usuarios.SingleOrDefaultAsync(m => m.IdUsuario == id);

            if (cat_usuario == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
