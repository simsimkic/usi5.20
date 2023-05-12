using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Core.Appointments;
using ZdravoCorp.Core.Equipment;
using ZdravoCorp.Core.Inventory;

namespace ZdravoCorp.Gui.Doctor
{
    public partial class EquipmentStatusWindow : Window
    {
        public string RoomId;
        public InventoryService InventoryService;
        public EquipmentService EquipmentService;

        public EquipmentStatusWindow(string roomId)
        {   
            RoomId = roomId;
            InventoryService = new InventoryService();
            EquipmentService = new EquipmentService();
            InitializeComponent();
            InitializeDynamicEquipment();
        }

        private void equipmentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("You finished appointment successfully!");
        }


        public void InitializeDynamicEquipment()
        {
            List<InventoryItemData> dynamicEquipment = InventoryService.GetDynamicEquipmentByRoomId(RoomId);
            
            foreach (var inventoryItem in dynamicEquipment)
            {
                equipmentBox.Items.Add(inventoryItem.EquipmentName + " - " + inventoryItem.EquipmentId);
            }

        }

        public void LoadEquipmentQuantity(InventoryItem inventoryItem)
        {
            equipmentQuantity.Text = inventoryItem.Quantity.ToString();
            spentQuantity.Text = "0";
        }

        public string GetEquipmentIdFromBox()
        {
            return equipmentBox.SelectedItem.ToString().Split("-")[1].Trim();
        }

        public bool IsSpentQuantityValid()
        {
            if (!spentQuantity.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please insert only numbers for quantity! ");
                return false;
            }
            return true;
        }

        private void EquipmentBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshEquipmentData();
        }

        public void RefreshEquipmentData()
        {
            string equipmentId = GetEquipmentIdFromBox();
            InventoryItem inventoryItem = InventoryService.GetInventoryItem(equipmentId, RoomId);
            LoadEquipmentQuantity(inventoryItem);
        }

        private void spendEquipment_Click(object sender, RoutedEventArgs e)
        {
            if (IsSpentQuantityValid())
            {
                string equipmentId = GetEquipmentIdFromBox();
                if (InventoryService.UseDynamicEquipment(equipmentId,RoomId, int.Parse(spentQuantity.Text)))
                {
                    MessageBox.Show($"You spent {spentQuantity.Text} of item : {equipmentBox.SelectedItem.ToString().Split("-")[0].ToLower()}");
                }
                else
                {
                    MessageBox.Show("There is not enough equipment quantity to spend! ");
                }

                RefreshEquipmentData();
            }
        }

    }
}
