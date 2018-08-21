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
using ProyectoBase.Models.FicPersonas;

namespace ProyectoBase.Pages.Menu
{
    public class RegistroModel : PageModel
    {
        private readonly ProyectoBase.Models.FicPersonas.ApplicationDbContext _context;

        public RegistroModel(ProyectoBase.Models.FicPersonas.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public cat_usuarios cat_usuario { get; set; }
        [BindProperty]
        public seg_expira_clave seg_expira_clave { get; set; }
        [BindProperty]
        public rh_cat_persona rh_cat_persona { get; set; }
        [BindProperty]
        public rh_cat_dir_web rh_cat_dir_web { get; set; }
        [BindProperty]
        public rh_cat_telefono rh_cat_telefono { get; set; }

        //AGREGAMOS LAS VARIABLES DEL FORMULARIO

        public string email { get; set; }
        public string confirmaemail { get; set; }
        public string nombre { get; set; }
        public string apPaterno { get; set; }
        public string apMaterno { get; set; }
        public string sexo { get; set; }
        public string usuario { get; set; }
        public string contraseña { get; set; }
        public string confirmarcontraseña { get; set; }
        public string telefono { get; set; }
        public string rutafoto { get; set; }
        public string alias { get; set; }
        public string numerocontrol{ get; set; }

        //FIN DE VARIABLES DE FORMULARIO

        public async Task<IActionResult> OnGetAsync(string email, string confirmaemail,
            string nombre, string apPaterno, string apMaterno, string sexo, string usuario, string contraseña,
            string confirmarcontraseña, string telefono, string rutafoto, string alias,string numerocontrol)
        {
            if (email == null)
            {
                this.email = "";
                this.confirmaemail = "";
                this.nombre = "";
                this.apPaterno = "";
                this.apMaterno = "";
                this.sexo = "";
                this.usuario = "";
                this.contraseña = "";
                this.confirmarcontraseña = "";
                this.telefono = "";
                this.rutafoto = "";
                this.alias = "";
                this.numerocontrol = "";

                //Combo para sexo
                var TipoSexo = new List<SelectListItem>();
                TipoSexo.Add(new SelectListItem() { Text = "Hombre", Value = "H" });
                TipoSexo.Add(new SelectListItem() { Text = "Mujer", Value = "M" });

                ViewData["Sexos"] = new SelectList(TipoSexo, "Value", "Text");
            }
            else {
                //CONEXION A LA BASE DE DATOS

                SqlConnection sqlConnection1 = new SqlConnection("Server=ADAMGE\\AGE12400269;Database=DB_EVA_TEC_TOTAL;Trusted_Connection=True;MultipleActiveResultSets=true");
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                sqlConnection1.Open();

                var fecha = DateTime.Today;

                //insertar personas
                cmd.CommandText = "INSERT INTO rh_cat_personas VALUES(1,'" + numerocontrol + "','" + nombre + "','" + apPaterno + "','" + apMaterno + "','','',GETDATE(),'','" + sexo + "','" + rutafoto + "','" + alias + "',GETDATE(),GETDATE(),'Sistema','Sistema','S','N',5,22,6,28);";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;
                reader = cmd.ExecuteReader();
                sqlConnection1.Close();

                //Obtenemos el id del usuario para insertarle una contraseña
                rh_cat_persona = await _context.rh_cat_personas.SingleOrDefaultAsync(m => m.NumControl.Equals(numerocontrol) && m.Nombre.Equals(nombre) && m.ApPaterno.Equals(apPaterno) && m.ApMaterno.Equals(apMaterno) && m.Sexo.Equals(sexo) && m.RutaFoto.Equals(rutafoto) && m.Alias.Equals(alias));
                 string idpersona = rh_cat_persona.IdPersona+"";
                // aqui tengo el id de la persona   *** rh_cat_persona.IdPersona ***
                sqlConnection1.Open();
                //insertar en dir web
                cmd.CommandText = "INSERT INTO rh_cat_dir_webs VALUES ('','"+confirmaemail+"','S','"+idpersona+"','Correo Inicial',GETDATE(),GETDATE(),'Sistema','Sistema','S','N',9,77);";
                 cmd.CommandType = CommandType.Text;
                 cmd.Connection = sqlConnection1;
                 reader = cmd.ExecuteReader();
                sqlConnection1.Close();

                sqlConnection1.Open();
                //insertar en telefono
                cmd.CommandText = "INSERT INTO rh_cat_telefonos VALUES ('','"+telefono+"','','S','"+idpersona+ "','Telefono Inicial',GETDATE(),GETDATE(),'Sistema','Sistema','S','N',10,53);";
                 cmd.CommandType = CommandType.Text;
                 cmd.Connection = sqlConnection1;
                 reader = cmd.ExecuteReader();
                sqlConnection1.Close();

                sqlConnection1.Open();
                //insertar en usuarios
                cmd.CommandText = "INSERT INTO cat_usuarios VALUES ("+rh_cat_persona.IdPersona+",'"+usuario+ "','S','N',GETDATE(),3,GETDATE(),'Sistema','Sistema','S','N',GETDATE());";
                 cmd.CommandType = CommandType.Text;
                 cmd.Connection = sqlConnection1;
                 reader = cmd.ExecuteReader();
                sqlConnection1.Close();


                cat_usuario = await _context.cat_usuarios.SingleOrDefaultAsync(m => m.Usuario.Equals(usuario));

                 var fechaini = DateTime.Today;
                 var fechafin = (DateTime.Today).AddMonths(6);

                sqlConnection1.Open();
                //insertar en contraseñas
                cmd.CommandText = "INSERT INTO seg_expira_claves VALUES ("+cat_usuario.IdUsuario+",'"+fechaini+"','"+fechafin+"','S','"+confirmarcontraseña+"','N',GETDATE(),'Sistema','Sistema','S','N',GETDATE());";
                 cmd.CommandType = CommandType.Text;
                 cmd.Connection = sqlConnection1;
                 reader = cmd.ExecuteReader();
               
                sqlConnection1.Close();



                sqlConnection1.Open();
                //insertar en contraseñas
                cmd.CommandText = "INSERT INTO seg_usuarios_estatus VALUES (" + cat_usuario.IdUsuario + ",GETDATE(),'S','Registro de sistema',GETDATE(),'Sistema','Sistema','S','N',GETDATE(),4,1);";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;
                reader = cmd.ExecuteReader();

                sqlConnection1.Close();


                //DESCONEXION A LA BASE DE DATOS

                await _context.SaveChangesAsync();
                return RedirectToPage("./Login");
            }
            return Page();
        }
        

    }

}