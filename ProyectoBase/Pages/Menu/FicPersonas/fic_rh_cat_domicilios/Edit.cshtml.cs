using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_rh_cat_domicilios
{
    public class EditModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public EditModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public string Control { get; set; }
        public int IdP { get; set; }
        public string Ap { get; set; }
        public string Am { get; set; }
        public string SearchString { get; set; }

        public bool DomicilioP { get; set; }
        public bool ActivoP { get; set; }
        public bool BorradoP { get; set; }

        [BindProperty]
        public rh_cat_domicilios rh_cat_domicilio { get; set; }

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

            rh_cat_domicilio = await _context.rh_cat_domicilios.SingleOrDefaultAsync(m => m.IdDomicilio == id);

            if (rh_cat_domicilio == null)
            {
                return NotFound();
            }

            //Combo para paises
            var paises = new List<SelectListItem>();
            paises.Add(new SelectListItem() { Text = "Selecciona un pais...", Value = string.Empty });
            foreach (var item in _context.cat_paises)
            {
                paises.Add(new SelectListItem() { Text = item.DesPais, Value = item.DesPais });
            }

            ViewData["IdPais"] = new SelectList(paises, "Value", "Text");

            //Combo para estados
            var estados = new List<SelectListItem>();
            estados.Add(new SelectListItem() { Text = "Selecciona un estado...", Value = string.Empty });
            foreach (var item in _context.cat_estados)
            {
                estados.Add(new SelectListItem() { Text = item.DesEstado, Value = item.DesEstado });
            }

            ViewData["IdEstado"] = new SelectList(estados, "Value", "Text");

            //Combo para municipios
            var municipios = new List<SelectListItem>();
            municipios.Add(new SelectListItem() { Text = "Selecciona un municipio...", Value = string.Empty });
            foreach (var item in _context.cat_municipios)
            {
                municipios.Add(new SelectListItem() { Text = item.DesMunicipio, Value = item.DesMunicipio });
            }

            ViewData["IdMunicipio"] = new SelectList(municipios, "Value", "Text");

            //Combo para localidades
            var localidades = new List<SelectListItem>();
            localidades.Add(new SelectListItem() { Text = "Selecciona una localidad...", Value = string.Empty });
            foreach (var item in _context.cat_localidades)
            {
                localidades.Add(new SelectListItem() { Text = item.DesLocalidad, Value = item.DesLocalidad });
            }

            ViewData["IdLocalidad"] = new SelectList(localidades, "Value", "Text");

            //Combo para estados
            var colonias = new List<SelectListItem>();
            colonias.Add(new SelectListItem() { Text = "Selecciona una colonia...", Value = string.Empty });
            foreach (var item in _context.cat_colonias)
            {
                colonias.Add(new SelectListItem() { Text = item.DesColonia, Value = item.DesColonia });
            }

            ViewData["IdColonia"] = new SelectList(colonias, "Value", "Text");

            //Combo para tipo de domicilio
            //Primero hacemos la consulta
            var dom = from ocupacion in _context.cat_generales
                      where ocupacion.IdTipoGeneral == 8
                      select new { nom_dom = ocupacion.DesGeneral, id_dom = ocupacion.IdGeneral };
            //Despues agregamos los datos al combo
            var domicilioReg = new List<SelectListItem>();
            domicilioReg.Add(new SelectListItem() { Text = "Selecciona un tipo de domicilio...", Value = string.Empty });
            foreach (var item in dom)
            {
                domicilioReg.Add(new SelectListItem() { Text = item.nom_dom, Value = "" + item.id_dom });
            }

            ViewData["IdTipoDom"] = new SelectList(domicilioReg, "Value", "Text");

            //Combo para Tipo domicilio(fiscal, moral)
            var tipdom = new List<SelectListItem>();
            tipdom.Add(new SelectListItem() { Text = "Selecciona un tipo de domicilio...", Value = string.Empty });
            tipdom.Add(new SelectListItem() { Text = "Fiscal", Value = "F" });
            tipdom.Add(new SelectListItem() { Text = "Moral", Value = "M" });

            ViewData["TipoDom"] = new SelectList(tipdom, "Value", "Text");

            //Para activar o desactivar los checkbox

            //Domicilio Principal
            if (rh_cat_domicilio.Principal == "S")
            {
                DomicilioP = true;
            }
            else {
                DomicilioP = false;
            }

            //Activo
            if (rh_cat_domicilio.Activo == "S")
            {
                ActivoP = true;
            }
            else
            {
                ActivoP = false;
            }

            //Borrado
            if (rh_cat_domicilio.Borrado == "S")
            {
                BorradoP = true;
            }
            else
            {
                BorradoP = false;
            }

            return Page();
        }
        
        //MÉTODO POST
        public async Task<IActionResult> OnPostAsync(bool DomicilioP, bool ActivoP, bool BorradoP)
        {
            rh_cat_domicilio.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name; 
            rh_cat_domicilio.FechaUltMod = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Si marco el checkbox entonces SI es el domicilio principal
            //de lo contrario entonces NO lo es
            if (DomicilioP)
            {
                rh_cat_domicilio.Principal = "S";
            }
            else
            {
                rh_cat_domicilio.Principal = "N";
            }
            //Activo
            if (ActivoP)
            {
                rh_cat_domicilio.Activo = "S";
            }
            else
            {
                rh_cat_domicilio.Activo = "N";
            }
            //Borrado
            if (BorradoP)
            {
                rh_cat_domicilio.Borrado = "S";
            }
            else
            {
                rh_cat_domicilio.Borrado = "N";
            }

            _context.Attach(rh_cat_domicilio).State = EntityState.Modified;

            //Si es el domicilio principal entonces tiene que buscar si hay otro para desmarcarlo
            if (rh_cat_domicilio.Principal == "S")
            {
                var cambiarPrincipal = await _context.rh_cat_domicilios.SingleOrDefaultAsync
                    (m => m.IdDomicilio != rh_cat_domicilio.IdDomicilio && m.Principal == "S");

                if (cambiarPrincipal != null)
                {
                    cambiarPrincipal.Principal = "N";
                    _context.SaveChanges();
                }//if cambiarPrincipal != null

            }//if principal == S

            //Si quiere quitar el domicilio principal primero debemos de ver que no sea el ultimo
            else {
                var cambiarPrincipal = await _context.rh_cat_domicilios.SingleOrDefaultAsync
                    (m => m.IdDomicilio != rh_cat_domicilio.IdDomicilio && m.Principal == "S");
                //Si es nulo significa que va a dejar sin domicilio principal, entonces no lo dejamos desmarcar la casilla
                if (cambiarPrincipal == null)
                {
                    rh_cat_domicilio.Principal = "S";
                    _context.SaveChanges();
                }//if cambiarPrincipal == null
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!rh_cat_domicilioExists(rh_cat_domicilio.IdDomicilio))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = rh_cat_domicilio.IdDomicilio });
        }

        private bool rh_cat_domicilioExists(int id)
        {
            return _context.rh_cat_domicilios.Any(e => e.IdDomicilio == id);
        }
    }
}
