using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;

namespace ProyectoBase.Pages.Menu.FicPersonas.fic_cat_usuarios
{
    public class DeleteModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public DeleteModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public cat_usuarios cat_usuario { get; set; }
        [BindProperty]
        public seg_expira_claves claves { get; set; }
        [BindProperty]
        public seg_usuarios_estatus estatus { get; set; }
        //[BindProperty]
       // public seg_usuarios_grupos grupos { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            cat_usuario = await _context.cat_usuarios.SingleOrDefaultAsync(m => m.IdUsuario == id);

            if (cat_usuario == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            cat_usuario = await _context.cat_usuarios.FindAsync(id);

            //Insertar seg_usuario_estatus Nuevo
            SqlConnection sqlConnection1 = new SqlConnection("Server=(local); Database=Proyecto_DAE; Trusted_Connection=True; MultipleActiveResultSets=true");
            SqlCommand cmd = new SqlCommand();

            SqlDataReader reader;

            //Contraseñas
            sqlConnection1.Open();
            
            cmd.CommandText = "delete seg_expira_claves where IdUsuario = "+cat_usuario.IdUsuario+";";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            reader = cmd.ExecuteReader();

            sqlConnection1.Close();

            //Estatus
            sqlConnection1.Open();

            cmd.CommandText = "delete seg_usuarios_estatus where IdUsuario = " + cat_usuario.IdUsuario + ";";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            reader = cmd.ExecuteReader();

            sqlConnection1.Close();

            //Grupos
            sqlConnection1.Open();

            cmd.CommandText = "delete seg_usuarios_grupos where IdUsuario = " + cat_usuario.IdUsuario + ";";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            reader = cmd.ExecuteReader();

            sqlConnection1.Close();

            if (cat_usuario != null)
            {
                //Eliminamos el usuario
                _context.cat_usuarios.Remove(cat_usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
