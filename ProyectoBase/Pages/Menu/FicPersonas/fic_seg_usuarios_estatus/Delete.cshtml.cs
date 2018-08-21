using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_seg_usuarios_estatus
{
    public class DeleteModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public DeleteModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public seg_usuarios_estatu seg_usuarios_estatu { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            seg_usuarios_estatu = await _context.seg_usuarios_estatus.SingleOrDefaultAsync(m => m.IdCtrlEstatus == id);

            if (seg_usuarios_estatu == null)
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

            seg_usuarios_estatu = await _context.seg_usuarios_estatus.FindAsync(id);

            if (seg_usuarios_estatu != null)
            {
                _context.seg_usuarios_estatus.Remove(seg_usuarios_estatu);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
