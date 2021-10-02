using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsWebApp.Models
{
    [Index(nameof(INN))]
    [Index(nameof(ClientName))]
    public class Client
    {
        public int Id { get; set; }
        [StringLength(10, MinimumLength = 10), Required, Display(Name = "ИНН"), RegularExpression(@"^[0-9]*$")]
        public string INN { get; set; }
        [StringLength(60, MinimumLength = 7), Required, Display(Name = "Наименование")]
        public string ClientName { get; set; }
        /// <summary>
        /// true - ЮЛ, false - ИП
        /// </summary>
        [Display(Name = "Тип клиента")]
        public bool Type { get; set; }
        [DataType(DataType.DateTime), Display(Name = "Дата добавления")]
        public DateTime AddDate { get; set; }
        [DataType(DataType.DateTime), Display(Name = "Дата последнего обновления")]
        public DateTime UpdateDate { get; set; }

        public virtual ICollection<Founder> Founders { get; set; }

        public Client() 
        {
            Type = true;
        }

        public Client(string data)
        {
            ClientName = data;
        }

        public override string ToString() => $"{ClientName}-{INN}:{Id}";
    }
}
