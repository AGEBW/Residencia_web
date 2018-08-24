using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_seg_usuarios_grupos
{
    public class CreateModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public CreateModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public seg_usuarios_grupos seg_usuarios_grupo { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
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

            //---------------------------------------------------------------------------------------------------

            return Page();

        }

        [BindProperty]
        public seg_usuarios_grupos grupo { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            
            seg_usuarios_grupo.FechaReg = DateTime.Now;
            //seg_usuarios_grupo.FechaUltMod = DateTime.Now;
            seg_usuarios_grupo.Activo = "S";
            seg_usuarios_grupo.Borrado = "N";
            //seg_usuarios_grupo.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;
            seg_usuarios_grupo.UsuarioReg = Microsoft.AspNetCore.Mvc.Razor.Global.name;

            grupo = await _context.seg_usuarios_grupos.SingleOrDefaultAsync(m => m.IdGrupo == seg_usuarios_grupo.IdGrupo && m.IdUsuario == seg_usuarios_grupo.IdUsuario);
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
            else {
                _context.seg_usuarios_grupos.Add(seg_usuarios_grupo);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToPage("./Index");
        }
    }
}