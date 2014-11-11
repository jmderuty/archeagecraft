using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheageCraft.Models
{
    public class Craft
    {
        public int CraftId { get; set; }

        public int LaborCost { get; set; }

        public int ProfessionId { get; set; }

        public virtual Profession Profession { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        
    }

    public class CraftItem
    {
        public int CraftItemId { get; set; }

        public int CraftId { get; set; }
        public virtual Craft Craft { get; set; }

        public int ItemId { get; set;}
        public virtual Item Item { get; set; }

        public int Count { get; set; }
    }
}
