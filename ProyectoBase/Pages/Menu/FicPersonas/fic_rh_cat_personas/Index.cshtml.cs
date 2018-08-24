using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.FicPersonas.fic_rh_cat_personas
{
    public class IndexModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public IndexModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        //Se crean la lista
        public SelectList Genres { get; set; }
        public string SearchInstituto { get; set; }
        public string searchString { get; set; }

        public IList<rh_cat_personas> rh_cat_persona { get;set; }

        public async Task OnGetAsync(string searchString, string searchInstituto)
        {
            //Combo para estados
            var institutos = new List<SelectListItem>();
            institutos.Add(new SelectListItem() { Text = "Selecciona un instituto...", Value = string.Empty });
            foreach (var item in _context.cat_institutos)
            {
                institutos.Add(new SelectListItem() { Text = item.DesInstituto, Value = ""+item.IdInstituto});
            }

            ViewData["IdInstituto"] = new SelectList(institutos, "Value", "Text");

            //Query para la busqueda
            IQueryable<string> genreQuery = from m in _context.rh_cat_personas
                                            orderby m.Nombre
                                            select m.Nombre;

            var movies = from m in _context.rh_cat_personas
                         select m;

            var fecha = 0;
            if (searchString != null)
            {
                fecha = int.Parse(searchString);
            }
            //si el campo no esta vacio hara la busqueda
            if (!String.IsNullOrEmpty(searchString))
            {
                if (fecha != 0)
                {
                    movies = movies.Where(s => s.Activo.Contains(searchString) || s.NumControl.Contains(searchString) || s.Nombre.Contains(searchString)
                    || s.ApPaterno.Contains(searchString) || s.ApMaterno.Contains(searchString) || s.RFC.Contains(searchString)
                    || s.CURP.Contains(searchString) || s.Alias.Contains(searchString) ||
                    (s.FechaNac.Day == fecha || s.FechaNac.Month == fecha) || s.FechaNac.Year == fecha);
                }
                else
                {
                    movies = movies.Where(s => s.Activo.Contains(searchString) || s.NumControl.Contains(searchString) || s.Nombre.Contains(searchString)
                    || s.ApPaterno.Contains(searchString) || s.ApMaterno.Contains(searchString) || s.RFC.Contains(searchString)
                    || s.CURP.Contains(searchString) || s.Alias.Contains(searchString));
                }
            }

            if (!String.IsNullOrEmpty(searchInstituto))
            {
                movies = movies.Where(x => ""+x.IdInstituto == searchInstituto);
            }

            //mostrar los datos obtenidos
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            rh_cat_persona = await movies.ToListAsync();
        }
    }
}
