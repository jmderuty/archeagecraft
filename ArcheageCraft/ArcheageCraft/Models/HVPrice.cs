using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheageCraft.Models
{
    public class Price
    {
        public int PriceId { get; set; }

        public int Value { get; set; }
        public int ItemId { get; set; }

        public string UserName { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

    }
}
