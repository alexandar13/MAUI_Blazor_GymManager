using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface INavigationService
    {
        Func<string, Task>? NavigateToMauiPageAction { get; set; }
        Task NavigateToMauiPage(string pageRoute);
    }
}
