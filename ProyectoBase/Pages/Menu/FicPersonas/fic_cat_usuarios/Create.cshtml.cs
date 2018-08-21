using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_cat_usuarios
{
    public class CreateModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public CreateModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        public bool ExpiraP { get; set; }

        public IActionResult OnGetAsync(int id)
        {
            ExpiraP = false;
            ViewData["Error"] = "";
            ViewData["IdPersona"] = new SelectList(_context.rh_cat_personas, "IdPersona", "Nombre");

            return Page();
        }

        [BindProperty]
        public cat_usuarios cat_usuario { get; set; }
        [BindProperty]
        public seg_usuarios_estatu estatu { get; set; }

        public async Task<IActionResult> OnPostAsync(bool ExpiraP)
        {
            var usuarioExiste = await _context.cat_usuarios.SingleOrDefaultAsync(m => m.Usuario == cat_usuario.Usuario);
            //Signfica que si encontro coincidencias, entonces le mandamos un mensaje de que ya existe
            if (usuarioExiste != null)
            {
                ViewData["IdPersona"] = new SelectList(_context.rh_cat_personas, "IdPersona", "Nombre");
                ViewData["Error"] = "El usuario \"" + cat_usuario.Usuario + "\" ya se encuentra registrado!";
                return Page();
            }

            cat_usuario.Conectado = "N";
            cat_usuario.FechaAlta = DateTime.Now;
            cat_usuario.FechaReg = DateTime.Now;
            cat_usuario.FechaUltMod = DateTime.Now;
            cat_usuario.Activo = "S";
            cat_usuario.Borrado = "N";
            cat_usuario.UsuarioMod = Microsoft.AspNetCore.Mvc.Razor.Global.name;
            cat_usuario.UsuarioReg = Microsoft.AspNetCore.Mvc.Razor.Global.name;

            //Si selecciono de expirar o no
            if (ExpiraP)
            {
                cat_usuario.Expira = "S";
            }
            else
            {
                cat_usuario.Expira = "N";
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.cat_usuarios.Add(cat_usuario);
            await _context.SaveChangesAsync();
            
            //Una ve que se crea el usuario, se le asigna el estatus activo por defecto
            estatu.IdUsuario = cat_usuario.IdUsuario;
            estatu.FechaEstatus = DateTime.Now;
            estatu.Actual = "S";
            estatu.Observacion = "Creación del usuario";
            estatu.FechaUltMod = DateTime.Now;
            estatu.UsuarioReg = "Admin";
            estatu.UsuarioMod = "Admin";
            estatu.Activo = "S";
            estatu.Borrado = "N";
            estatu.FechaReg = DateTime.Now;
            estatu.IdTipoEstatus = 4;
            estatu.IdEstatus = 1;

            var result = _context.Add(estatu);
            _context.SaveChanges(); // Saving Data in database  


            return RedirectToPage("./Index");
        }





        /*
        public ActionResult OnPost()
        {
            var estatu = seg_usuarios_estatu;
            

            estatu.IdUsuario = 2;
            estatu.FechaEstatus = DateTime.Now;
            estatu.Actual = "1";
            estatu.Observacion = "hola";
            estatu.FechaUltMod = DateTime.Now;
            estatu.UsuarioReg = "gul";
            estatu.UsuarioMod = "gul2";
            estatu.Activo = "1";
            estatu.Borrado = "0";
            estatu.FechaReg = DateTime.Now;
            estatu.IdTipoEstatus = 4;
            estatu.IdEstatus = 1;

            var result = _context.Add(estatu);
            _context.SaveChanges(); // Saving Data in database  

            return RedirectToPage("./Index");
        }*/












        /* public async Task<IActionResult> Create([Bind("IdUsuario,FechaEstatus,Actual,Observacion,FechaUltMod,UsuarioReg,UsuarioMod,Activo,Borrado,FechaReg,IdTipoEstatus,IdEstatus")] seg_usuarios_estatu estatus)
         {

             var a= new SelectList(_context.rh_cat_personas, "IdPersona", "Nombre");

             _context.Add(estatus);
             await _context.SaveChangesAsync();
             return RedirectToPage("./Index");

         }*/

    }
}