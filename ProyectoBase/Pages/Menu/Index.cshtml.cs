using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProyectoBase.Pages.Menu
{
    public class IndexModel : PageModel
    {
        public void OnGetAsync(int?id)
        {
           

            if (id==1) {
                Microsoft.AspNetCore.Mvc.Razor.Global.Login = false;
                Microsoft.AspNetCore.Mvc.Razor.Global.contraseñas_coinciden = false;
            }
            
        }

    }
}
