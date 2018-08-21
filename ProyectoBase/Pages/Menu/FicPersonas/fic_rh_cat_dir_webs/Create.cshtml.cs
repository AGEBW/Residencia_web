using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_rh_cat_dir_webs
{
    public class CreateModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public CreateModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public bool DirWebP { get; set; }

        [BindProperty]
        public rh_cat_personas rh_cat_persona { get; set; }

        public string Control { get; set; }
        public int IdP { get; set; }
        public string Ap { get; set; }
        public string Am { get; set; }
        public string SearchString { get; set; }

        //Método GET
        public async Task<IActionResult> OnGetAsync(int id, string searchString, string control, string ap, string am)
        {

            DirWebP = false;

            //Parametros para mostrar la info de la persona a agregar domicilios
            Control = control;
            IdP = id;
            Ap = ap;
            Am = am;
            SearchString = searchString;
            if (id == 0)
            {
                return NotFound();
            }

            rh_cat_persona = await _context.rh_cat_personas.SingleOrDefaultAsync(m => m.IdPersona == id);

            if (rh_cat_persona == null)
            {
                return NotFound();
            }

            //Combo para tipo dir web
            //Primero hacemos la consulta
            var dir_web = from ocupacion in _context.cat_generales
                      where ocupacion.IdTipoGeneral == 9
                      select new { nom_dir_web = ocupacion.DesGeneral, id_dir_web = ocupacion.IdGeneral };
            //Despues agregamos los datos al combo
            var dirwebReg = new List<SelectListItem>();
            dirwebReg.Add(new SelectListItem() { Text = "Selecciona un tipo de dir web...", Value = string.Empty });
            foreach (var item in dir_web)
            {
                dirwebReg.Add(new SelectListItem() { Text = item.nom_dir_web, Value = "" + item.id_dir_web});
            }

            ViewData["IdTipoDirWeb"] = new SelectList(dirwebReg, "Value", "Text");

            return Page();
        }

        [BindProperty]
        public rh_cat_dir_web rh_cat_dir_web { get; set; }

        public async Task<IActionResult> OnPostAsync(bool DirWebP)
        {
            rh_cat_dir_web.FechaReg = DateTime.Now;
            rh_cat_dir_web.FechaUltMod = DateTime.Now;
            rh_cat_dir_web.Activo = "S";
            rh_cat_dir_web.Borrado = "N";
            rh_cat_dir_web.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;
            rh_cat_dir_web.UsuarioReg = Microsoft.AspNetCore.Mvc.Razor.Global.name;

            //Si marco el checkbox entonces SI es el domicilio principal
            //de lo contrario entonces NO lo es
            if (DirWebP)
            {
                rh_cat_dir_web.Principal = "S";
            }
            else
            {
                rh_cat_dir_web.Principal = "N";
            }

            _context.rh_cat_dir_webs.Add(rh_cat_dir_web);

            if (rh_cat_dir_web.Principal == "S")
            {
                var cambiarPrincipal = await _context.rh_cat_dir_webs.SingleOrDefaultAsync
                    (m => m.IdDirWeb != rh_cat_dir_web.IdDirWeb && m.Principal == "S");
                //Si es nulo significa que aun no registraban ninguna direccion web como principal
                if (cambiarPrincipal != null)
                {
                    cambiarPrincipal.Principal = "N";
                    _context.SaveChanges();
                }//if cambiarPrincipal != null

            }//if principal == S

            await _context.SaveChangesAsync();

            //asp-route-idP="@Model.IdP" asp-route-control="@Model.Control" asp-route-searchString="@Model.SearchString" asp-route-ap="@Model.Ap" asp-route-am="@Model.Am"

            return RedirectToPage("./Details", new { id = rh_cat_dir_web.IdDirWeb, idP = IdP, control = Control, searchString = SearchString, ap = Ap, am = Am });
        }
    }
}