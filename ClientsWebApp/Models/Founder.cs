using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    public class Founder
    {
        public int Id { get; set; }
        [StringLength(12, MinimumLength = 12), Required, Display(Name = "ИНН Учредителя"), RegularExpression(@"^[0-9]*$")]
        public string INN { get; set; }
        [StringLength(60, MinimumLength = 5), Required, Display(Name = "ФИО")]
        public string NameSurname { get; set; }
        [DataType(DataType.DateTime), Display(Name = "Дата добавления")]
        public DateTime AddDate { get; set; }
        [DataType(DataType.DateTime), Display(Name = "Дата последнего обновления")]
        public DateTime UpdateDate { get; set; }

        public Founder() { }

        public Founder(string data)
        {
            NameSurname = data;
        }

        public override string ToString() => $"{NameSurname}-{INN}:{Id}";

    }
}
