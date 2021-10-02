using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    public class SearchClientParams
    {
        public string ClientINN { get; set; }
        public string ClientName { get; set; }
        public string ClientType { get; set; }
        
    }
}
