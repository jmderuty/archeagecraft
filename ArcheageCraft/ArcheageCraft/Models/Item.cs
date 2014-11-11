using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheageCraft.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        public string Name { get; set; }

        public int MerchantCost { get; set; }

        public int VocationBadgeCost { get; set; }

        

        public virtual Profession Profession { get; set; }

        public virtual List<Price> Prices { get; set; }
    }
}
