using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    public class AddClientModel
    {
        public Client Client { get; set; }
        public IList<Founder> Founders { get; set; }
        public SelectList ExistingFounders { get; set; }
    }
}
