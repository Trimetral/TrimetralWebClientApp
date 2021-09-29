using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    public class EditClientModel
    {
        public Client Client { get; set; }
        public IEnumerable<Founder> Founders { get; set; }
    }
}
