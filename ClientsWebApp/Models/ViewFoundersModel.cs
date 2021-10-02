using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    public class ViewFoundersModel : ViewModel
    {
        public IList<Founder> Founders { get; set; }
        public SearchFounderParams Search { get; set; }

    }
}
