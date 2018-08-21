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
    public class DeleteModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public DeleteModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public seg_expira_clave seg_expira_clave { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            seg_expira_clave = await _context.seg_expira_claves.SingleOrDefaultAsync(m => m.IdClave == id);

            if (seg_expira_clave == null)
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

            seg_expira_clave = await _context.seg_expira_claves.FindAsync(id);

            if (seg_expira_clave != null)
            {
                _context.seg_expira_claves.Remove(seg_expira_clave);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
