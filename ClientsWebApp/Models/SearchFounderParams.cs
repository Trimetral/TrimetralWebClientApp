using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    public class SearchFounderParams
    {
        public string FounderINN { get; set; }
        public string FounderName { get; set; }

    }
}
