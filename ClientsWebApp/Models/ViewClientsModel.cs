using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    public class ViewClientsModel : ViewModel
    {
        public IList<Client> Clients { get; set; }
        public SearchClientParams Search { get; set; }
        public SelectList Types { get; set; } = new SelectList(new List<string> { "ИП", "ЮЛ" });
    }
}
