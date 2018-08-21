using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.Razor
{
    public static class Global
    {
        
    public static bool Login { get; set; }


        public static bool usuario_incorrecto { get; set; }
        public static bool contraseña_incorrecta { get; set; }
        public static bool contraseña_no_actual { get; set; }
        public static bool contraseña_expiro { get; set; }

        public static bool contraseña_sistema{ get; set; }
        public static bool contraseñas_vacias { get; set; }
        public static bool contraseñas_no_coinciden { get; set; }
        public static bool contraseñas_coinciden { get; set; }
        public static bool contraseña_igual_anterior { get; set; }
        public static bool bloqueado_por_intentos { get; set; }

        public static string name { get; set; }
        public static int intentos { get; set; }
        public static string name_intento { get; set; }

        public static bool intentos_superados { get; set; }
        public static bool estatus_no_activo { get; set; }

        public static string nombre { get; set; }
        public static string ap { get; set; }
        public static string am { get; set; }
        public static string control { get; set; }
        public static int id { get; set; }






    }
}

