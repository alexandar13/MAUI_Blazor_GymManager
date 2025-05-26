using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NavigationService : INavigationService
    {
        public Func<string, Task>? NavigateToMauiPageAction { get; set; }

        public async Task NavigateToMauiPage(string pageRoute)
        {
            if (NavigateToMauiPageAction != null)
                await NavigateToMauiPageAction.Invoke(pageRoute);
        }
    }
}
