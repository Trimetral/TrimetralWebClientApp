using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    public class ViewModel
    {
        public int Page { get; set; } = 1;
        public int Pages { get; set; } = 1;
        public int Count { get; set; } = 5;
        public SelectList CountVars { get; set; } = new SelectList(new List<int> { 5, 10, 15, 20, 40, 50 });
    }
}
