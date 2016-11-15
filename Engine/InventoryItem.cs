using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace Engine
{
    public class InventoryItem : INotifyPropertyChanged
    {
        private Item _details;
        public Item details
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged("Details");
            }
        }
        private int _quantity;
        public int quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
                OnPropertyChanged("Description");
            }
        }
        public int itemID
        {
            get { return details.id; }
        }
        public float price
        {
            get { return details.price; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public InventoryItem(Item details, int quantity)
        {
            this.details = details;
            this.quantity = quantity;
        }
        public string description
        {
            get { return quantity > 1 ? details.namePlural : details.name; }
        }
    }
}
