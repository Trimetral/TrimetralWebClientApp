using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    public class EditFounderModel
    {
        public Founder Founder { get; set; }
        public IEnumerable<Client> Clients { get; set; }
    }
}
