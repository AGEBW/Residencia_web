﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_seg_expira_clave
{
    public class DetailsModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public DetailsModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public seg_expira_claves seg_expira_clave { get; set; }

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
    }
}
