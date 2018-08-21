using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.FicPersonas.fic_rh_cat_personas
{
    public class EditModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public EditModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        public bool ActivoP { get; set; }
        public bool BorradoP { get; set; }

        [BindProperty]
        public rh_cat_persona rh_cat_persona { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Combo para tipo persona
            var TipoPersona = new List<SelectListItem>();
            TipoPersona.Add(new SelectListItem() { Text = "Fisica", Value = "Fisica" });
            TipoPersona.Add(new SelectListItem() { Text = "Moral", Value = "Moral" });

            ViewData["TipoPersona"] = new SelectList(TipoPersona, "Value", "Text");

            //Combo para institutos
            //Primero hacemos la consulta
            var institutos = from instituto in _context.cat_institutos
                             select new { nom_instituto = instituto.DesInstituto, id_instituto = instituto.IdInstituto };
            //Despues agregamos los datos al combo
            var datos = new List<SelectListItem>();
            datos.Add(new SelectListItem() { Text = "Selecciona un instituto...", Value = string.Empty });
            foreach (var item in institutos)
            {
                datos.Add(new SelectListItem() { Text = item.nom_instituto, Value = "" + item.id_instituto });
            }

            ViewData["Institutos"] = new SelectList(datos, "Value", "Text");

            //Combo para ocupacion
            //Primero hacemos la consulta
            var ocupaciones = from ocupacion in _context.cat_generales
                              where ocupacion.IdTipoGeneral == 5
                              select new { nom_ocupaciones = ocupacion.DesGeneral, id_ocupacion = ocupacion.IdGeneral };
            //Despues agregamos los datos al combo
            var registros = new List<SelectListItem>();
            registros.Add(new SelectListItem() { Text = "Selecciona una ocupación...", Value = string.Empty });
            foreach (var item in ocupaciones)
            {
                registros.Add(new SelectListItem() { Text = item.nom_ocupaciones, Value = "" + item.id_ocupacion });
            }

            ViewData["IdOcupacion"] = new SelectList(registros, "Value", "Text");

            //Combo para estado civil
            //Primero hacemos la consulta
            var estados_civiles = from estado in _context.cat_generales
                                  where estado.IdTipoGeneral == 6
                                  select new { nom_estado_civil = estado.DesGeneral, id_estado_civil = estado.IdGeneral };
            //Despues agregamos los datos al combo
            var registro = new List<SelectListItem>();
            registro.Add(new SelectListItem() { Text = "Selecciona un estado civi...", Value = string.Empty });
            foreach (var item in estados_civiles)
            {
                registro.Add(new SelectListItem() { Text = item.nom_estado_civil, Value = "" + item.id_estado_civil });
            }

            ViewData["IdEstadoCivil"] = new SelectList(registro, "Value", "Text");

            //Combo para imagenes
            var Imagenes = new List<SelectListItem>();
            Imagenes.Add(new SelectListItem() { Text = "Selecciona una imagen...", Value = string.Empty });
            Imagenes.Add(new SelectListItem() { Text = "pablo.jpg", Value = "~/Images/personas/pablo.jpg" });
            Imagenes.Add(new SelectListItem() { Text = "kevin.jpg", Value = "~/Images/personas/kevin.jpg" });
            Imagenes.Add(new SelectListItem() { Text = "julio.jpg", Value = "~/Images/personas/julio.jpg" });
            Imagenes.Add(new SelectListItem() { Text = "nomar.jpg", Value = "~/Images/personas/nomar.jpg" });
            Imagenes.Add(new SelectListItem() { Text = "daniel.jpg", Value = "~/Images/personas/daniel.jpg" });
            Imagenes.Add(new SelectListItem() { Text = "Francisco.jpg", Value = "~/Images/personas/Francisco.jpg" });

            ViewData["RutaImagen"] = new SelectList(Imagenes, "Value", "Text");

            rh_cat_persona = await _context.rh_cat_personas.SingleOrDefaultAsync(m => m.IdPersona == id);

            if (rh_cat_persona == null)
            {
                return NotFound();
            }

            //Combo para sexo
            var TipoSexo = new List<SelectListItem>();
            TipoSexo.Add(new SelectListItem() { Text = "Hombre", Value = "H" });
            TipoSexo.Add(new SelectListItem() { Text = "Mujer", Value = "M" });

            ViewData["Sexos"] = new SelectList(TipoSexo, "Value", "Text");

            //Activo
            if (rh_cat_persona.Activo == "S")
            {
                ActivoP = true;
            }
            else
            {
                ActivoP = false;
            }

            //Borrado
            if (rh_cat_persona.Borrado == "S")
            {
                BorradoP = true;
            }
            else
            {
                BorradoP = false;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool ActivoP, bool BorradoP)
        {
            rh_cat_persona.FechaUltMod = DateTime.Now;
            rh_cat_persona.UsuarioReg = Microsoft.AspNetCore.Mvc.Razor.Global.name;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Activo
            if (ActivoP)
            {
                rh_cat_persona.Activo = "S";
            }
            else
            {
                rh_cat_persona.Activo = "N";
            }
            //Borrado
            if (BorradoP)
            {
                rh_cat_persona.Borrado = "S";
            }
            else
            {
                rh_cat_persona.Borrado = "N";
            }

            _context.Attach(rh_cat_persona).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!rh_cat_personaExists(rh_cat_persona.IdPersona))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool rh_cat_personaExists(int id)
        {
            return _context.rh_cat_personas.Any(e => e.IdPersona == id);
        }
    }
}
