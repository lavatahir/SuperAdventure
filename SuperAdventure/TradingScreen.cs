using System;
using Engine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperAdventure
{
    public partial class TradingScreen : Form
    {
        private Player currentPlayer;
        public TradingScreen(Player player)
        {
            currentPlayer = player;
            InitializeComponent();
            DataGridViewCellStyle rightAlignedCellStyle = new DataGridViewCellStyle();
            rightAlignedCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvMyItems.RowHeadersVisible = false;
            dgvMyItems.AutoGenerateColumns = false;

            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "itemID",
                Visible = false
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                DataPropertyName = "description",
                Width = 100
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Qty",
                DataPropertyName = "quantity",
                Width = 30,
                DefaultCellStyle = rightAlignedCellStyle
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Price",
                DataPropertyName = "price",
                Width = 35,
                DefaultCellStyle = rightAlignedCellStyle
            });
            dgvMyItems.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Sell 1",
                DataPropertyName = "itemID",
                Width = 50,
                UseColumnTextForButtonValue = true
            });
            dgvMyItems.DataSource = currentPlayer.inventory;
            dgvMyItems.CellClick += dgvMyItems_CellClick;
            dgvVendorItems.RowHeadersVisible = false;
            dgvVendorItems.AutoGenerateColumns = false;
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "itemID",
                Visible = false
            });
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                DataPropertyName = "description",
                Width = 100
            });
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Price",
                DataPropertyName = "price",
                Width = 35,
                DefaultCellStyle = rightAlignedCellStyle
            });
            dgvVendorItems.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Buy 1",
                DataPropertyName = "itemID",
                Width = 50,
                UseColumnTextForButtonValue = true
            });
            dgvVendorItems.DataSource = currentPlayer.currentLocation.vendorWorkingHere.inventory;
            dgvVendorItems.CellClick += dgvVendorItems_CellClick;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void dgvMyItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 4)
            {
                var itemID = dgvMyItems.Rows[e.RowIndex].Cells[0].Value;
                Item itemBeingSold = World.itemByID(Convert.ToInt32(itemID));
                if(itemBeingSold.price == World.UNSELLABLE_ITEM_PRICE)
                {
                    MessageBox.Show("This item is not sellable");
                }
                else
                {
                    currentPlayer.removeItemFromInventory(itemBeingSold);
                    currentPlayer.currentLocation.vendorWorkingHere.addItemtoInventory(itemBeingSold);
                    currentPlayer.gold += Convert.ToInt32(Math.Round(itemBeingSold.price));
                }
            }
        }
        private void dgvVendorItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 3)
            {
                var itemID = dgvVendorItems.Rows[e.RowIndex].Cells[0].Value;
                Item itemBeingBought = World.itemByID(Convert.ToInt32(itemID));
                if(currentPlayer.gold >= itemBeingBought.price)
                {
                    currentPlayer.addItemtoInventory(itemBeingBought);
                    currentPlayer.gold -= Convert.ToInt32(Math.Round(itemBeingBought.price));
                }
                else
                {
                    MessageBox.Show("you do not have enough gold to buy this item");
                }

            }
        }
    }
}
