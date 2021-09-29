using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    public class ViewClientsModel
    {
        public IList<Client> Clients { get; set; }
        public IEnumerable<Founder> Founders { get; set; }
    }
}
