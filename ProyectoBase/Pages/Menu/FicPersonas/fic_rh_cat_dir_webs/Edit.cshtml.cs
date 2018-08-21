using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_rh_cat_dir_webs
{
    public class EditModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public EditModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public rh_cat_dir_web rh_cat_dir_web { get; set; }

        public string Control { get; set; }
        public int IdP { get; set; }
        public string Ap { get; set; }
        public string Am { get; set; }
        public string SearchString { get; set; }

        public bool DirWebP { get; set; }
        public bool ActivoP { get; set; }
        public bool BorradoP { get; set; }

        //Método GET
        public async Task<IActionResult> OnGetAsync(int id, int idP, string searchString, string control, string ap, string am)
        {
            //Parametros para mostrar la info de la persona a agregar domicilios
            Control = control;
            IdP = idP;
            Ap = ap;
            Am = am;
            SearchString = searchString;
            if (id == 0)
            {
                return NotFound();
            }

            rh_cat_dir_web = await _context.rh_cat_dir_webs.SingleOrDefaultAsync(m => m.IdDirWeb == id);

            if (rh_cat_dir_web == null)
            {
                return NotFound();
            }

            //Domicilio Principal
            if (rh_cat_dir_web.Principal == "S")
            {
                DirWebP = true;
            }
            else
            {
                DirWebP = false;
            }

            //Activo
            if (rh_cat_dir_web.Activo == "S")
            {
                ActivoP = true;
            }
            else
            {
                ActivoP = false;
            }

            //Borrado
            if (rh_cat_dir_web.Borrado == "S")
            {
                BorradoP = true;
            }
            else
            {
                BorradoP = false;
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
                dirwebReg.Add(new SelectListItem() { Text = item.nom_dir_web, Value = "" + item.id_dir_web });
            }

            ViewData["IdTipoDirWeb"] = new SelectList(dirwebReg, "Value", "Text");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool DirWebP, bool ActivoP, bool BorradoP)
        {
           
            rh_cat_dir_web.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;
            rh_cat_dir_web.FechaUltMod = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return Page();
            }

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
            //Activo
            if (ActivoP)
            {
                rh_cat_dir_web.Activo = "S";
            }
            else
            {
                rh_cat_dir_web.Activo = "N";
            }
            //Borrado
            if (BorradoP)
            {
                rh_cat_dir_web.Borrado = "S";
            }
            else
            {
                rh_cat_dir_web.Borrado = "N";
            }

            _context.Attach(rh_cat_dir_web).State = EntityState.Modified;

            //Si es el domicilio principal entonces tiene que buscar si hay otro para desmarcarlo
            if (rh_cat_dir_web.Principal == "S")
            {
                var cambiarPrincipal = await _context.rh_cat_dir_webs.SingleOrDefaultAsync
                    (m => m.IdDirWeb != rh_cat_dir_web.IdDirWeb && m.Principal == "S");

                if (cambiarPrincipal != null)
                {
                    cambiarPrincipal.Principal = "N";
                    _context.SaveChanges();
                }//if cambiarPrincipal != null

            }//if principal == S

            //Si quiere quitar el domicilio principal primero debemos de ver que no sea el ultimo
            else
            {
                var cambiarPrincipal = await _context.rh_cat_dir_webs.SingleOrDefaultAsync
                    (m => m.IdDirWeb != rh_cat_dir_web.IdDirWeb && m.Principal == "S");
                //Si es nulo significa que va a dejar sin domicilio principal, entonces no lo dejamos desmarcar la casilla
                if (cambiarPrincipal == null)
                {
                    rh_cat_dir_web.Principal = "S";
                    _context.SaveChanges();
                }//if cambiarPrincipal == null
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!rh_cat_dir_webExists(rh_cat_dir_web.IdDirWeb))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = rh_cat_dir_web.IdDirWeb });
        }

        private bool rh_cat_dir_webExists(int id)
        {
            return _context.rh_cat_dir_webs.Any(e => e.IdDirWeb == id);
        }
    }
}
