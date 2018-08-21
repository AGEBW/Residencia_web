using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_cat_usuarios
{
    public class IndexModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public IndexModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        public string search { get; set; }
        public IList<cat_usuarios> cat_usuario { get;set; }
        public IList<string> MostrarTodo { get; set; }


        public async Task OnGetAsync(string search)
        {
            var item = from m in _context.cat_usuarios
                       select m;

            if (!String.IsNullOrEmpty(search))
            {
                item = item.Where(s => s.Conectado.Contains(search) || s.Activo.Contains(search) || s.Borrado.Contains(search) 
                || s.Usuario.Contains(search) || s.UsuarioMod.Contains(search) || s.UsuarioReg.Contains(search));
            }

            cat_usuario = await item.ToListAsync();

            ViewData["MostrarTodos"] =
            from usuarios in _context.cat_usuarios
            join personas in _context.rh_cat_personas on usuarios.IdPersona equals personas.IdPersona
            select new { nombre = personas.Nombre + " " + personas.ApPaterno + " " + personas.ApMaterno, usuario = usuarios.Usuario,
                expira = usuarios.Expira, conectado = usuarios.Conectado, intentosMax = usuarios.Numintentos, fechaReg = usuarios.FechaReg,
                fechaUlt = usuarios.FechaUltMod, activo = usuarios.Activo, borrado = usuarios.Borrado };
            
        }
    }
}
