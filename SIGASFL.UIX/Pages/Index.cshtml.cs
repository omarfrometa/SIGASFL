using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SIGASFL.UIX.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            Console.WriteLine($"Peticion Recibida: {DateTime.Now}");
        }
    }
}
