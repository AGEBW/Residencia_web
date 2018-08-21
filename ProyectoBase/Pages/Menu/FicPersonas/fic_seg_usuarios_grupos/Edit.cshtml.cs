using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_seg_usuarios_grupos
{
    public class EditModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public EditModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public seg_usuarios_grupo seg_usuarios_grupo { get; set; }

        public bool ActivoP { get; set; }
        public bool BorradoP { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            seg_usuarios_grupo = await _context.seg_usuarios_grupos.SingleOrDefaultAsync(m => m.IdsegGrupo == id);

            if (seg_usuarios_grupo.Activo == "S") {
                ActivoP = true;
            }
            else
            {
                ActivoP = false;
            }

            if (seg_usuarios_grupo.Borrado == "S")
            {
                BorradoP = true;
            }
            else {
                BorradoP = false;
            }

            //Combo para tipo de domicilio general
            //Primero hacemos la consulta
            var dom = from ocupacion in _context.cat_generales
                      where ocupacion.IdTipoGeneral == 4
                      select new { nom_dom = ocupacion.DesGeneral, id_dom = ocupacion.IdGeneral };
            //Despues agregamos los datos al combo
            var domicilioReg = new List<SelectListItem>();
            domicilioReg.Add(new SelectListItem() { Text = "Selecciona un grupo...", Value = string.Empty });
            foreach (var item in dom)
            {
                domicilioReg.Add(new SelectListItem() { Text = item.nom_dom, Value = "" + item.id_dom });
            }

            ViewData["IdGrupos"] = new SelectList(domicilioReg, "Value", "Text");

            if (seg_usuarios_grupo == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool AcivoP, bool BorradoP)
        {
            seg_usuarios_grupo.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;
            seg_usuarios_grupo.FechaUltMod = DateTime.Now;
            if (ActivoP)
            {
                seg_usuarios_grupo.Activo = "S";
            }
            else {
                seg_usuarios_grupo.Activo = "N";
            }

            if (BorradoP)
            {
                seg_usuarios_grupo.Borrado = "S";
            }
            else {
                seg_usuarios_grupo.Borrado = "N";                   
            }

            var grupo = await _context.seg_usuarios_grupos.SingleOrDefaultAsync(m => m.IdGrupo == seg_usuarios_grupo.IdGrupo && m.IdUsuario == seg_usuarios_grupo.IdUsuario);
            if (grupo != null)
            {
                //Combo para tipo de domicilio general
                //Primero hacemos la consulta
                var dom = from ocupacion in _context.cat_generales
                          where ocupacion.IdTipoGeneral == 4
                          select new { nom_dom = ocupacion.DesGeneral, id_dom = ocupacion.IdGeneral };
                //Despues agregamos los datos al combo
                var domicilioReg = new List<SelectListItem>();
                domicilioReg.Add(new SelectListItem() { Text = "Selecciona un grupo...", Value = string.Empty });

                foreach (var item in dom)
                {

                    domicilioReg.Add(new SelectListItem() { Text = item.nom_dom, Value = "" + item.id_dom });
                }

                ViewData["IdGrupos"] = new SelectList(domicilioReg, "Value", "Text");
                ViewData["Error"] = "Ya se encuentra registrado en ese grupo";
                return Page();
            }
            else
            {
             

                _context.Attach(seg_usuarios_grupo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!seg_usuarios_grupoExists(seg_usuarios_grupo.IdsegGrupo))
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

        private bool seg_usuarios_grupoExists(int id)
        {
            return _context.seg_usuarios_grupos.Any(e => e.IdTipoGrupo == id);
        }
    }
}
