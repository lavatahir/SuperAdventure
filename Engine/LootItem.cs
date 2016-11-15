using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class LootItem
    {
        public Item details { get; set; }
        public int dropPercentage { get; set; }
        public bool isDefaultItem { get; set; }
        public LootItem(Item details, int dropPercentage, bool isDefaultItem)
        {
            this.details = details;
            this.dropPercentage = dropPercentage;
            this.isDefaultItem = isDefaultItem;
        }
    }
}
