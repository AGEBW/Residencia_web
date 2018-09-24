using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoBase.Models;



namespace ProyectoBase.Pages.Menu
{
    public class OlvidoContraModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public OlvidoContraModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        
        public string correo { get; set; }
        public string usuario { get; set; }
        [BindProperty]
        public cat_usuarios Usuario { get; set; }
        public string error { get; set; }
        string tUsuario=" ";




        public async Task<IActionResult> OnGetAsync(string correo,string usuario)
        {
            ViewData["Error"] = "";

            if (correo == null && usuario == null) {  }
            else
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("calabazos2018dae@gmail.com");
                

                try
                {
                  

                    SqlConnection connection2 = new SqlConnection("Server=ADAMGE\\AGE12400269;Database=DB_EVA_TEC_TOTAL;Trusted_Connection=True;MultipleActiveResultSets=true");
                    string commandtext2 = "SELECT DirWeb  FROM rh_cat_personas  WHERE UsuarioReg='" + usuario + "';";
                    SqlCommand command2 = new SqlCommand(commandtext2, connection2);
                    connection2.Open();
                    tUsuario = (string)command2.ExecuteScalar();
                    connection2.Close();

                 
                }
                catch ( Exception ex )
                {
                    Microsoft.AspNetCore.Mvc.Razor.Global.correo_valido = true;
                }

                try
                {
                    SqlConnection connection = new SqlConnection("Server=ADAMGE\\AGE12400269;Database=DB_EVA_TEC_TOTAL;Trusted_Connection=True;MultipleActiveResultSets=true");
                    string commandtext = "SELECT DirWeb  FROM rh_cat_dir_web  WHERE DirWeb='" + correo + "' AND ClaveReferencia=';" + Usuario.IdUsuario + "';";
                    SqlCommand command = new SqlCommand(commandtext, connection);
                    connection.Open();
                    string tDirweb = (string)command.ExecuteScalar();
                    connection.Close();

                    //SqlConnection conexion = new SqlConnection("Server=ADAMGE\\AGE12400269;Database=DB_EVA_TEC_TOTAL;Trusted_Connection=True;MultipleActiveResultSets=true");
                    //string commandotxt = "SELECT DirWeb  FROM rh_cat_dir_web  WHERE ClaveReferencia='" + Usuario.IdUsuario + "';";
                    //SqlCommand commando = new SqlCommand(commandotxt, conexion);
                    //conexion.Open();
                    //string tDi = (string)commando.ExecuteScalar();
                    //conexion.Close();


                    if (tDirweb.Equals(correo) == false)
                    {
                        ViewData["Error"] = "El usuario y el correo no coinciden.";
                        return Page();
                    }

                }
                catch( Exception ex)
                {
                    Microsoft.AspNetCore.Mvc.Razor.Global.usuario_incorrecto = true;
                }
                


                //genera codigo aleatorio

                int longitud = 6;
                const string alfabeto = "qwertyuiopasdfghjklzxcvbnmABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                StringBuilder token = new StringBuilder();
                Random rnd = new Random();

                for (int i = 0; i < longitud; i++)
                {
                    int indice = rnd.Next(alfabeto.Length);
                    token.Append(alfabeto[indice]);
                }

                //termina de generar codigo
                Usuario = await _context.cat_usuarios.SingleOrDefaultAsync(m => m.Usuario.Equals(usuario));
                if (Usuario == null || Usuario.Equals( tUsuario)==false)
                {
                    ViewData["Error"] = "El usuario no se encuentra registrado.";
                    return Page();
                }
                else
                {
                   
                      
                  
                    SqlConnection sqlConnection2 = new SqlConnection("Server=ADAMGE\\AGE12400269;Database=DB_EVA_TEC_TOTAL;Trusted_Connection=True;MultipleActiveResultSets=true");
                    SqlCommand cmd2 = new SqlCommand();
                    SqlDataReader reader2;
                    sqlConnection2.Open();

                    var fechaini = DateTime.Today;
                    var fechafin = (DateTime.Today).AddMonths(6);

                    

                    cmd2.CommandText = "Update seg_expira_claves set Actual='N' WHERE IdUsuario="+Usuario.IdUsuario+";";
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = sqlConnection2;

                    reader2 = cmd2.ExecuteReader();
                    // Data is accessible through the DataReader object here.
                    while (reader2.Read())
                    {
                        Console.WriteLine(String.Format("{0}", reader2[0]));
                    }

                    sqlConnection2.Close();

                    SqlConnection sqlConnection1 = new SqlConnection("Server=ADAMGE\\AGE12400269;Database=DB_EVA_TEC_TOTAL;Trusted_Connection=True;MultipleActiveResultSets=true");
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;
                    sqlConnection1.Open();

                    cmd.CommandText = "INSERT INTO seg_expira_claves VALUES(" + Usuario.IdUsuario + ", GETDATE(), GETDATE()+180, 'S', '" + token + "','S',GETDATE(),'Sistema','Sistema','S','N',GETDATE());";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;

                    reader = cmd.ExecuteReader();
                    // Data is accessible through the DataReader object here.
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("{0}", reader[0]));
                    }

                    sqlConnection1.Close();
                }

                //termina el insertar en la bd


                mail.To.Add(correo);
                mail.Subject = "RECUPERACION DE CONTRASEÑA";
                mail.Body = "Su contraseña temporal sera: " + token + " favor de modificar a una contraseña personal";

                SmtpClient smtp = new SmtpClient();

                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;

                smtp.Credentials = new NetworkCredential("calabazos2018dae@gmail.com", "calabazos18");
                smtp.EnableSsl = true;
                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    throw new Exception("No se ha podido enviar el email", ex.InnerException);
                }
                finally
                {
                    smtp.Dispose();
                }

                return Redirect("./Login");
            }//else 

            return Page();



        }





       

    }

}













