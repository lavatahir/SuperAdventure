using Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Player player;
        //private Monster currentMonster;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";
        public SuperAdventure()
        {
            InitializeComponent();
            
            if (File.Exists(PLAYER_DATA_FILE_NAME))
            {
                player = Player.CreatePlayerFromXMLString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            }
            else
            {
                player = Player.createDefaultPlayer();
            }
            
            //player = Player.createDefaultPlayer();
            setUp();
            /*
            lblHitPoints.DataBindings.Add("Text", player, "currentHitPoints");
            lblGold.DataBindings.Add("Text", player, "gold");
            lblExpierence.DataBindings.Add("Text", player, "expPoints");
            lblLevel.DataBindings.Add("Text", player, "level");
            
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;
            dgvInventory.DataSource = player.inventory;
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Description"
            });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity"
            });
            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;
            dgvQuests.DataSource = player.quests;
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "name"
            });
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Done?",
                DataPropertyName = "isCompleted"
            });

            cboWeapons.DataSource = player.weapons;
            cboWeapons.DisplayMember = "Name";
            cboWeapons.ValueMember = "id";
            if(player.currentWeapon != null)
            {
                cboWeapons.SelectedItem = player.currentWeapon;
            }
            cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;

            cboPotions.DataSource = player.potions;
            cboPotions.DisplayMember = "Name";
            cboPotions.ValueMember = "id";

            player.PropertyChanged += player_OnPropertyChanged;
            player.onMessage += displayMessage;
            player.moveTo(player.currentLocation);
            */
        }
        private void setUp()
        {
            lblHitPoints.DataBindings.Add("Text", player, "currentHitPoints");
            lblGold.DataBindings.Add("Text", player, "gold");
            lblExpierence.DataBindings.Add("Text", player, "expPoints");
            lblLevel.DataBindings.Add("Text", player, "level");

            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;
            dgvInventory.DataSource = player.inventory;
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Description"
            });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity"
            });
            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;
            dgvQuests.DataSource = player.quests;
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "name"
            });
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Done?",
                DataPropertyName = "isCompleted"
            });

            cboWeapons.DataSource = player.weapons;
            cboWeapons.DisplayMember = "Name";
            cboWeapons.ValueMember = "id";
            if (player.currentWeapon != null)
            {
                cboWeapons.SelectedItem = player.currentWeapon;
            }
            cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;

            cboPotions.DataSource = player.potions;
            cboPotions.DisplayMember = "Name";
            cboPotions.ValueMember = "id";

            player.PropertyChanged += player_OnPropertyChanged;
            player.onMessage += displayMessage;
            player.moveTo(player.currentLocation);
        }
        private void displayMessage(object sender, MessageEventArgs messageEventArgs)
        {
            rtbMessages.Text += messageEventArgs.message + Environment.NewLine;
            if (messageEventArgs.addExtraNewLine)
            {
                rtbMessages.Text += Environment.NewLine;
            }
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }
        private void player_OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChanged)
        {
            if(propertyChanged.PropertyName == "weapons")
            {
                cboWeapons.DataSource = player.weapons;
                if (!player.weapons.Any())
                {
                    cboWeapons.Visible = false;
                    btnUseWeapon.Visible = false;
                }
            }
            if(propertyChanged.PropertyName == "potions")
            {
                cboPotions.DataSource = player.potions;
                if (!player.potions.Any())
                {
                    cboPotions.Visible = false;
                    btnUsePotion.Visible = false;
                }
            }
            if(propertyChanged.PropertyName == "currentLocation")
            {
                btnNorth.Visible = (player.currentLocation.locationNorth != null);
                btnSouth.Visible = (player.currentLocation.locationSouth != null);
                btnEast.Visible = (player.currentLocation.locationEast != null);
                btnWest.Visible = (player.currentLocation.locationWest != null);

                btnTrade.Visible = (player.currentLocation.vendorWorkingHere != null);

                rtbLocation.Text = player.currentLocation.name + Environment.NewLine;
                rtbLocation.Text += player.currentLocation.description + Environment.NewLine;
                if(player.currentLocation.monsterLivingHere == null)
                {
                    cboWeapons.Visible = false;
                    cboPotions.Visible = false;
                    btnUseWeapon.Visible = false;
                    btnUsePotion.Visible = false;
                }
                else
                {
                    cboWeapons.Visible = player.weapons.Any();
                    cboPotions.Visible = player.potions.Any();
                    btnUseWeapon.Visible = player.weapons.Any();
                    btnUsePotion.Visible = player.potions.Any();
                }
            }
        }
        private void btnNorth_Click(object sender, EventArgs e)
        {
            player.moveNorth();
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            player.moveSouth();
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            player.moveEast();
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            player.moveWest();
        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            player.currentWeapon = (Weapon)cboWeapons.SelectedItem;
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;
            player.useWeapon(currentWeapon);
            //monsterTurn();        
        }
        private void btnTrade_Click(object sender, EventArgs e)
        {
            TradingScreen tradingScreen = new TradingScreen(player);
            tradingScreen.StartPosition = FormStartPosition.CenterParent;
            tradingScreen.ShowDialog(this);
        }
        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            HealingPotion hp = (HealingPotion)cboPotions.SelectedItem;
            player.usePotion(hp);
        }
        private void updateLabels()
        {
            lblHitPoints.Text = player.currentHitPoints.ToString();
            lblGold.Text = player.gold.ToString();
            lblExpierence.Text = player.expPoints.ToString();
            lblLevel.Text = player.level.ToString();
        }
        private void updateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";
            dgvInventory.Rows.Clear();
            foreach (InventoryItem ii in player.inventory)
            {
                if (ii.quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { ii.details.name, ii.quantity.ToString() });
                }
            }
        }
        private void updateQuestListUI()
        {
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Completed";
            dgvInventory.Rows.Clear();

            foreach (PlayerQuest pq in player.quests)
            {
                dgvInventory.Rows.Add(new[] { pq.details.name, pq.isCompleted.ToString() });
            }
        }
        private void updateWeaponListUI()
        {
            List<Weapon> weapons = new List<Weapon>();
            foreach (InventoryItem ii in player.inventory)
            {
                if (ii.details is Weapon)
                {
                    if (ii.quantity > 0)
                    {
                        weapons.Add((Weapon)ii.details);
                    }
                }
            }
            if (weapons.Count == 0)
            {
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.SelectedIndexChanged -= cboWeapons_SelectedIndexChanged;
                cboWeapons.DataSource = weapons;
                cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";             
                if(player.currentWeapon != null)
                {
                    cboWeapons.SelectedItem = player.currentWeapon;
                }
                else
                {
                    cboWeapons.SelectedIndex = 0;
                }
            }
        }
        private void updatePotionListUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();
            foreach (InventoryItem ii in player.inventory)
            {
                if (ii.details is HealingPotion)
                {
                    if (ii.quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)ii.details);
                    }
                }
            }
            if (healingPotions.Count == 0)
            {
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";
                cboPotions.SelectedIndex = 0;
            }
        }
        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }
        private void SuperAdventure_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, player.toXMLString());
        }
        private void mnuItmNew_Click(object sender, EventArgs e)
        {
            clearBindings();
            this.player = Player.createDefaultPlayer();
            setUp();
            //Application.Restart();
        }
        private void clearBindings()
        {
            lblHitPoints.DataBindings.Clear();
            lblGold.DataBindings.Clear();
            lblExpierence.DataBindings.Clear();
            lblLevel.DataBindings.Clear();
            rtbMessages.Clear();
            
            dgvInventory.DataSource = null;
            dgvInventory.Rows.Clear();
            dgvInventory.Columns.Clear();

            dgvQuests.DataSource = null;
            dgvQuests.Rows.Clear();
            dgvQuests.Columns.Clear();
        }
        private void mnuItmOpen_Click(object sender, EventArgs e)
        {       
            openFD.Title = "Open the PlayerData File";
            openFD.InitialDirectory = Environment.CurrentDirectory;
            openFD.FileName = "";
            openFD.Filter = "XML|*.xml";
            openFD.ShowDialog();
            clearBindings();
            player = Player.CreatePlayerFromXMLString(File.ReadAllText(openFD.FileName));
            setUp();
        }

        private void mnuItmSave_Click(object sender, EventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, player.toXMLString());
            rtbMessages.Text += "\nFile has been saved.\n";
        }

        private void mnuItmClose_Click(object sender, EventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, player.toXMLString());
            Application.Exit();
        }
    }
}