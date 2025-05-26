using MAUI_Blazor_GymManager.PagesXaml.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Blazor_GymManager.PagesXaml
{
    public static class Routes
    {
        //Ovde registrovati stranice
        public static Dictionary<string, Func<Page>> route_page = new()
        {
            {"/scanner", () => new Scanner() }
        };

        public static Page getPageByRoute(string route)
        {
            return route_page.TryGetValue(route, out var newPage)
                ? newPage()
                : throw new ArgumentException($"Page for route '{route}' not found.");
        }
    }
}
