using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Engine
{
    public class Vendor : INotifyPropertyChanged
    {
        public string name { get; set; }
        public BindingList<InventoryItem> inventory { get; private set; }

        public Vendor(string name)
        {
            this.name = name;
            inventory = new BindingList<InventoryItem>();
        }
        public void addItemtoInventory(Item item, int quantity = 1)
        {
            InventoryItem i = inventory.SingleOrDefault(ii => ii.details.id == item.id);
            if(i == null)
            {
                inventory.Add(new InventoryItem(item, quantity));
            }
            else
            {
                i.quantity += quantity;
            }
            OnPropertyChanged("inventory");
        }
        public void removeItemFromInventory(Item item, int quantity = 1)
        {
            InventoryItem i = inventory.SingleOrDefault(ii => ii.details.id == item.id);
            if(i == null)
            {
                //nothing to do
            }
            else
            {
                i.quantity -= quantity;
                if(i.quantity == 0)
                {
                    inventory.Remove(i);
                }
            }
            OnPropertyChanged("inventory");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        
    }
}
