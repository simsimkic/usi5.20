using System;
using System.Collections.Generic;
using System.Drawing;
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
using ZdravoCorp.Core.Inventory;
using ZdravoCorp.Core.Room;

namespace ZdravoCorp.Gui.Manager
{
    /// <summary>
    /// Interaction logic for AllocateItemWindow.xaml
    /// </summary>
    public partial class AllocateItemWindow : Window
    {
        private InventoryService inventory;
        private InventoryItemData item;
        private int availableQuantity;
        bool success = false;

        public AllocateItemWindow(InventoryService inventory,InventoryItemData item)
        {
            InitializeComponent();
            this.inventory = inventory;
            this.item = item;
            InitializeRoomIds();
            if (item.EquipmentDinamic)
            {
                Delivery_Date.Visibility = Visibility.Hidden;
                Delivery_Time.Visibility = Visibility.Hidden;
                TimeLabel.Visibility = Visibility.Hidden;
                DateLabel.Visibility = Visibility.Hidden;
            }
            availableQuantity = inventory.OccupiedQuantity(item);
            maxQuantity.Text = availableQuantity.ToString();
        }
        
        private void InitializeRoomIds()
        {
            foreach(Room room in inventory.GetRooms())
            {
                if(room.Id != item.RoomId)
                {
                    deliveryRoomId.Items.Add(room.Id);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int allocated;
            if (!int.TryParse(Quantity.Text, out allocated))
            {
                MessageBox.Show("Quantity needs to be a number in order to be allocated.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (allocated > availableQuantity)
            {
                MessageBox.Show("Quantity needs to be lesser or equal to the availableq quantity in order to be allocated.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (deliveryRoomId.SelectedItem == null)
            {
                MessageBox.Show("Room to be delivered to needs to be selected in order to be allocated to.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (item.EquipmentDinamic)
            {
                inventory.CreateInnerTransfer(allocated, item, (string)deliveryRoomId.SelectedItem);
                inventory.UpdateInnerArrivedEquipment();
                success = true;
                Close();
                return;
            }

            DateTime deliveryDate;

            if (!ItemsSelected()) { return; }
            

            int hour = Convert.ToInt32(((ComboBoxItem)hourComboBox.SelectedItem).Content);
            int minute = Convert.ToInt32(((ComboBoxItem)minuteComboBox.SelectedItem).Content);
            string am_pm = ((ComboBoxItem)ampmComboBox.SelectedItem).Content.ToString();


            string formattedDate = string.Format("{0:d/M/yyyy} {1}:{2} {3}", Delivery_Date.SelectedDate.Value, hour, minute, am_pm);

            if (!DateTime.TryParse(formattedDate, out deliveryDate))
            {
                MessageBox.Show("Delivery date didn't parse well:" + formattedDate, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (deliveryDate < DateTime.Now)
            {
                MessageBox.Show("Delivery date needs to be in future.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            inventory.CreateInnerTransfer(allocated, item, (string)deliveryRoomId.SelectedItem, deliveryDate);
            success = true;
            Close();
        }

        private bool ItemsSelected()
        {
            if (!Delivery_Date.SelectedDate.HasValue)
            {
                MessageBox.Show("Item delivery date needs to be selected in order to be allocated.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (hourComboBox.SelectedItem == null || minuteComboBox.SelectedItem == null || ampmComboBox.SelectedItem == null)
            {
                MessageBox.Show("Item delivery time needs to be selected in order to be allocated.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (success)
            {
                MessageBox.Show("Item transfer is now underway.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Do you want to quit the allocation of this item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
