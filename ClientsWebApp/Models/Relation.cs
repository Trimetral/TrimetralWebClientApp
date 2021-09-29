using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    public class Relation
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int FounderId { get; set; }
    }
}
