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
using ZdravoCorp.Core.Equipment;
using ZdravoCorp.Core.Room;
using ZdravoCorp.Core.Inventory;
using System.Security.RightsManagement;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;

namespace ZdravoCorp.Gui.Manager.InventoryWindow
{
    /// <summary>
    /// Interaction logic for InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow : Window
    {

        private InventoryService inventory;
        private DispatcherTimer timer;
        public InventoryWindow(InventoryService inventory)
        {
            InitializeComponent();
            this.inventory = inventory;

            //inventory.CreateEquipment(new Equipment("0007","Gloves",EquipmentType.OperationEquipment,true));

            InitializeRoomTypes();
            InitializeEquipmentTypes();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(20);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            List<InventoryItemData> inventoryItemDatas = inventory.InventoryItemsData();
            itemTable.DataContext = inventoryItemDatas;
            itemAllocationTable.ItemsSource = inventoryItemDatas;
            dynamicEquipmentTable.ItemsSource = inventory.GetDynamicItemsData();
            SortItemTable();
        }
        public void InitializeRoomTypes()
        {
            roomTypes.Items.Add("");
            roomTypes.Items.Add(RoomType.OperationRoom);
            roomTypes.Items.Add(RoomType.ExaminationRoom);
            roomTypes.Items.Add(RoomType.PatientRoom);
            roomTypes.Items.Add(RoomType.WaitingRoom);
            roomTypes.Items.Add(RoomType.Storage);
        }

        public void InitializeEquipmentTypes()
        {
            equipmentTypes.Items.Add("");
            equipmentTypes.Items.Add(EquipmentType.OperationEquipment);
            equipmentTypes.Items.Add(EquipmentType.CorridorEquipment);
            equipmentTypes.Items.Add(EquipmentType.ExaminationEquipment);
            equipmentTypes.Items.Add(EquipmentType.RoomEquipment);
        }

        
        private List<InventoryItem> QuantityFilter(List<InventoryItem> ret)
        {
            if (outOfStock.IsChecked == true)
            {
                return inventory.FindByQuantity(-1, 0, ret);
            }
            else if(mediumStock.IsChecked == true)
            {
                return inventory.FindByQuantity(0, 10, ret);
            }
            else if(largeStock.IsChecked == true)
            {
                return inventory.FindByQuantity(10, int.MaxValue, ret);
            }
            return ret;
        }

        private List<InventoryItem> RoomTypeFilter(List<InventoryItem> ret)
        {
            if (roomTypes.SelectedItem is RoomType selectedRoomType)
            {
                return inventory.FindByRoomType(selectedRoomType, ret);
            }
            return ret;
        }

        private List<InventoryItem> EquipmentTypeFilter(List<InventoryItem> ret)
        {
            if (equipmentTypes.SelectedItem is EquipmentType selectedEquipmentType)
            {
                return inventory.FindByEquipmentType(selectedEquipmentType, ret);
            }
            return ret;
        }

        private List<InventoryItem> OutsideStorageFilter(List<InventoryItem> ret)
        {
            if (outsideStorage.IsChecked == true)
            {
                return inventory.FindOutsideStorage(ret);
            }
            return ret;
        }

        private List<InventoryItem> AtributeFilter(List<InventoryItem> ret)
        {
            if (!string.IsNullOrEmpty(atribute?.Text))
            {
                return inventory.FindByAtributes(atribute.Text, ret);
            }
            return ret;
        }

        private void InventoryWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to log out?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow mw = new MainWindow();
                mw.Show();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void atribute_TextChanged(object sender, TextChangedEventArgs e)
        {
            itemTable.RaiseEvent(new RoutedEventArgs(DataGrid.LoadedEvent));
        }

        private void outsideStorage_Checked(object sender, RoutedEventArgs e)
        {
            itemTable.RaiseEvent(new RoutedEventArgs(DataGrid.LoadedEvent));
        }

        private void outsideStorage_Unchecked(object sender, RoutedEventArgs e)
        {
            itemTable.RaiseEvent(new RoutedEventArgs(DataGrid.LoadedEvent));
        }

        private void equipmentTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            itemTable.RaiseEvent(new RoutedEventArgs(DataGrid.LoadedEvent));
        }

        private void roomTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            itemTable.RaiseEvent(new RoutedEventArgs(DataGrid.LoadedEvent));
        }

        private void outOfStock_Checked(object sender, RoutedEventArgs e)
        {
            itemTable.RaiseEvent(new RoutedEventArgs(DataGrid.LoadedEvent));
        }

        private void mediumStock_Checked(object sender, RoutedEventArgs e)
        {
            itemTable.RaiseEvent(new RoutedEventArgs(DataGrid.LoadedEvent));
        }

        private void largeStock_Checked(object sender, RoutedEventArgs e)
        {
            itemTable.RaiseEvent(new RoutedEventArgs(DataGrid.LoadedEvent));
        }

        private void noneSelected_Checked(object sender, RoutedEventArgs e)
        {
            itemTable.RaiseEvent(new RoutedEventArgs(DataGrid.LoadedEvent));
        }
        private void itemTable_Loaded(object sender, RoutedEventArgs e)
        {
            itemTable.DataContext = inventory.InventoryItemsData();
            SortItemTable();
        }

        private void dynamicEquipmentTable_Loaded(object sender, RoutedEventArgs e)
        {
            dynamicEquipmentTable.ItemsSource = inventory.GetDynamicItemsData();
        }

        private void itemAllocationTable_Loaded(object sender, RoutedEventArgs e)
        {
            itemAllocationTable.ItemsSource = inventory.InventoryItemsData();
        }
        private void SortItemTable()
        {
            List<InventoryItem> ret = inventory.InventoryItems();

            ret = QuantityFilter(ret);
            ret = RoomTypeFilter(ret);
            ret = EquipmentTypeFilter(ret);
            ret = OutsideStorageFilter(ret);
            ret = AtributeFilter(ret);
            itemTable.ItemsSource = inventory.InventoryItemsData(ret);
        }

        
        private void orderDynamicEquipment_Click(object sender, RoutedEventArgs e)
        {
            int ordered;
            if(int.TryParse(orderQuantity.Text, out ordered))
            {
                InventoryItemData selectedItem = dynamicEquipmentTable.SelectedItem as InventoryItemData;
                if(selectedItem != null)
                {
                    inventory.CreateTransfer(ordered, selectedItem);
                    MessageBox.Show("You have ordered "+ ordered+" items with the id: "+ selectedItem.EquipmentId+" to the storage the order will arrive shortly in one day.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    orderQuantity.Text = "";
                    return;
                }

                MessageBox.Show("Select an item in table to order.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Item quantity that needs to be ordered must be a number.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void allocate_Click(object sender, RoutedEventArgs e)
        {

            InventoryItemData selectedItem = itemAllocationTable.SelectedItem as InventoryItemData;
            if (selectedItem != null)
            {
                AllocateItemWindow allocation = new AllocateItemWindow(inventory,selectedItem);
                allocation.Show();
                return;
            }
            MessageBox.Show("Select an item in table to allocate it's stock.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }
    }
}
