using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public string namePlural { get; set; }
        public float price { get; set; }
        public Item(int id, string name, string namePlural, float price)
        {
            this.id = id;
            this.name = name;
            this.namePlural = namePlural;
            this.price = price;
        }
    }
}
