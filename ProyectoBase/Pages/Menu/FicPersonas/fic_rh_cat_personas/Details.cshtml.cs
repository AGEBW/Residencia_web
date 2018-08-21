using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.FicPersonas.fic_rh_cat_personas
{
    public class DetailsModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public DetailsModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        public string Instituto { get; set; }
        public string EstadoCivil { get; set; }
        public string Ocupacion { get; set; }

        public rh_cat_persona rh_cat_persona { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            rh_cat_persona = await _context.rh_cat_personas.SingleOrDefaultAsync(m => m.IdPersona == id);
            
            //Nombre de la institucion
            var query = from persona in _context.rh_cat_personas
            join instituto in _context.cat_institutos on persona.IdInstituto equals instituto.IdInstituto
                        where persona.IdPersona == rh_cat_persona.IdPersona
            select new { nombre = instituto.DesInstituto};

            foreach (var item in query) {
                Instituto = item.nombre;
            }

            //Nombre de la ocupación
            query = from persona in _context.rh_cat_personas
                        join generales in _context.cat_generales on persona.IdGenOcupacion equals generales.IdGeneral
                        where persona.IdPersona == rh_cat_persona.IdPersona
                        select new { nombre = generales.DesGeneral};

            foreach (var item in query)
            {
                Ocupacion = item.nombre;
            }

            //Nombre del estado civil
            query = from persona in _context.rh_cat_personas
                    join generales in _context.cat_generales on persona.IdGenEstadoCivil equals generales.IdGeneral
                    where persona.IdPersona == rh_cat_persona.IdPersona
                    select new { nombre = generales.DesGeneral };

            foreach (var item in query)
            {
                EstadoCivil = item.nombre;
            }

            if (rh_cat_persona == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
