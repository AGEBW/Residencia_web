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
using ProyectoBase.Models.;

namespace ProyectoBase.Pages.Menu
{
    public class LoginModel : PageModel
    {
        private readonly ProyectoBase.Models.ApplicationDbContext _context;

        public LoginModel(ProyectoBase.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public cat_usuarios cat_usuarios { get; set; }
        public seg_expira_clave seg_expira_clave { get; set; }
        public seg_usuarios_estatu seg_usuario_estatus { get; set; }

        public string Usuario { get; set; }
        public string Contraseña { get; set; }

        public string Contraseña_vieja { get; set; }
        public string Contraseña_nueva { get; set; }



        
        public async Task<IActionResult> OnGetAsync(string FicPaUsuario, string FicPaContraseña, string FicPaContraseña_vieja, string FicPaContraseña_nueva)
        {

            Microsoft.AspNetCore.Mvc.Razor.Global.contraseña_no_actual = false;
            Microsoft.AspNetCore.Mvc.Razor.Global.contraseña_incorrecta = false;
            Microsoft.AspNetCore.Mvc.Razor.Global.usuario_incorrecto = false;
            Microsoft.AspNetCore.Mvc.Razor.Global.contraseña_expiro = false;
            Microsoft.AspNetCore.Mvc.Razor.Global.contraseñas_vacias = false;
            Microsoft.AspNetCore.Mvc.Razor.Global.contraseñas_coinciden = false;
            Microsoft.AspNetCore.Mvc.Razor.Global.contraseñas_no_coinciden = false;
            Microsoft.AspNetCore.Mvc.Razor.Global.contraseña_igual_anterior = false;
            Microsoft.AspNetCore.Mvc.Razor.Global.estatus_no_activo = false;
            Microsoft.AspNetCore.Mvc.Razor.Global.bloqueado_por_intentos = false;
            Microsoft.AspNetCore.Mvc.Razor.Global.contraseña_sistema = false;

            cat_usuarios = await _context.cat_usuarios.SingleOrDefaultAsync(m => m.Usuario == FicPaUsuario);

            int idusuario = 0;

            if (cat_usuarios==null) {}
            else {idusuario = cat_usuarios.IdUsuario; }

            seg_expira_clave = await _context.seg_expira_claves.SingleOrDefaultAsync(m => m.IdUsuario == idusuario && m.Clave == FicPaContraseña);

            seg_usuario_estatus = await _context.seg_usuarios_estatus.SingleOrDefaultAsync(m => m.IdUsuario == idusuario && m.Actual == "S");

            if (FicPaContraseña_vieja != FicPaContraseña_nueva) {
                Microsoft.AspNetCore.Mvc.Razor.Global.contraseñas_no_coinciden = true;
            }
            else { 
            if (FicPaContraseña_vieja == FicPaContraseña_nueva && FicPaContraseña_vieja != null)
            {
                
                if (FicPaContraseña_vieja == FicPaContraseña && FicPaContraseña_nueva == FicPaContraseña)
                {
                    Microsoft.AspNetCore.Mvc.Razor.Global.contraseña_igual_anterior = true;
                }
                else
                {
                    Microsoft.AspNetCore.Mvc.Razor.Global.contraseñas_coinciden = true;

                        //update
                        
                        seg_expira_clave.Actual = "N";
                        seg_expira_clave.FechaUltMod = DateTime.Now;
                        seg_expira_clave.UsuarioMod = "Sistema";

                        _context.Attach(seg_expira_clave).State = EntityState.Modified;
                        _context.SaveChanges();

                        //Insertar seg_usuario_estatus Nuevo
                        SqlConnection sqlConnection1 = new SqlConnection("Server=(local); Database=Proyecto_DAE; Trusted_Connection=True; MultipleActiveResultSets=true");
                        SqlCommand cmd = new SqlCommand();

                        SqlDataReader reader;

                        sqlConnection1.Open();

                        var fechafin = (DateTime.Today).AddMonths(6);
                        var fecha = DateTime.Today;
                        cmd.CommandText = "INSERT INTO seg_expira_claves VALUES (" + idusuario + ",GETDATE(),GETDATE()+180,'S','" + FicPaContraseña_nueva + "','N',GETDATE(),'Sistema','Sistema','S','N',GETDATE());";
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
                    }
                }
            

            //Validaciones
            if (FicPaUsuario==null) { return Page(); }
            else { 

                    if (cat_usuarios==null) {

                          Microsoft.AspNetCore.Mvc.Razor.Global.usuario_incorrecto = true;
                          return Page(); }

                         else {



                    if (seg_usuario_estatus.IdEstatus == 1 && seg_usuario_estatus.IdTipoEstatus == 4)
                    {
                        if (seg_expira_clave == null)
                        {
                            if (cat_usuarios.Numintentos == Microsoft.AspNetCore.Mvc.Razor.Global.intentos)
                            {

                                seg_usuario_estatus.FechaEstatus = DateTime.Now;
                                seg_usuario_estatus.Actual = "N";
                                seg_usuario_estatus.FechaUltMod = DateTime.Now;
                                seg_usuario_estatus.UsuarioMod = "Sistema";

                                _context.Attach(seg_usuario_estatus).State = EntityState.Modified;
                                _context.SaveChanges();

                                //Insertar seg_usuario_estatus Nuevo
                                SqlConnection sqlConnection1 = new SqlConnection("Server=(local); Database=Proyecto_DAE; Trusted_Connection=True; MultipleActiveResultSets=true");
                                SqlCommand cmd = new SqlCommand();

                                SqlDataReader reader;

                                sqlConnection1.Open();

                                var fecha = DateTime.Today;
                                cmd.CommandText = "INSERT INTO seg_usuarios_estatus VALUES (" + idusuario + ",'" + fecha + "','S','Intentos Alcanzados','" + fecha + "','Sistema','Sistema','S','N','" + fecha + "',4,4);";
                                cmd.CommandType = CommandType.Text;
                                cmd.Connection = sqlConnection1;

                                reader = cmd.ExecuteReader();
                                // Data is accessible through the DataReader object here.
                                while (reader.Read())
                                {
                                    Console.WriteLine(String.Format("{0}", reader[0]));
                                }

                                sqlConnection1.Close();

                                //Estatus Nuevo 
                                Microsoft.AspNetCore.Mvc.Razor.Global.intentos = 0;
                                Microsoft.AspNetCore.Mvc.Razor.Global.intentos_superados = true;
                                return Page();

                            }
                            else
                            {

                                Microsoft.AspNetCore.Mvc.Razor.Global.intentos++;
                                Microsoft.AspNetCore.Mvc.Razor.Global.contraseña_incorrecta = true;
                                return Page();
                            }
                        }

                        else
                        {
                            
                            if (seg_expira_clave.Actual == "N")
                            {
                                Microsoft.AspNetCore.Mvc.Razor.Global.contraseña_no_actual = true;
                                return Page();
                            }

                            else
                            {
                                if (seg_expira_clave.ClaveAutoSys == "S")
                                {

                                    Microsoft.AspNetCore.Mvc.Razor.Global.contraseña_sistema = true;
                                    return Page();
                                }
                                else { 
                                if (seg_expira_clave.FechaExpiraFin < DateTime.Now)
                                {
                                    Microsoft.AspNetCore.Mvc.Razor.Global.contraseña_expiro = true;
                                    return Page();
                                }

                                else
                                {
                                    Microsoft.AspNetCore.Mvc.Razor.Global.intentos = 0;
                                    Microsoft.AspNetCore.Mvc.Razor.Global.Login = true;
                                    Microsoft.AspNetCore.Mvc.Razor.Global.name = FicPaUsuario;
                                    return RedirectToPage("./Index");

                                }
                                }
                            }
                        }

                    }

                    else
                    {

                        if (seg_usuario_estatus.IdEstatus == 4 && seg_usuario_estatus.IdTipoEstatus == 4)
                        {

                            Microsoft.AspNetCore.Mvc.Razor.Global.bloqueado_por_intentos = true;
                            return Page();
                        }
                        else
                        {
                            Microsoft.AspNetCore.Mvc.Razor.Global.estatus_no_activo = true;
                            return Page();
                        }

                    }
                }
            }
        }

    }

}

