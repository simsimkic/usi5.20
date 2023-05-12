using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Equipment;
using ZdravoCorp.Core.Inventory.InnerTransfer;
using ZdravoCorp.Core.Room;

namespace ZdravoCorp.Core.Inventory
{
    public class InventoryService
    {
        public Inventory InventoryRepository { get; set; }
        public Transfer.TransferRepository TransferRepository { get; set; }

        public InnerTransfer.InnerTransferRepository InnerTransferRepository { get; set; }
        public InventoryService()
        {
            InventoryRepository = new Inventory();
            TransferRepository = new Transfer.TransferRepository();
            InnerTransferRepository = new InnerTransfer.InnerTransferRepository();
        }

        public List<Room.Room> GetRooms()
        {
            return InventoryRepository.RoomService.GetRooms();
        }
        public Equipment.Equipment GetItemEquipment(InventoryItem item)
        {
            return InventoryRepository.GetItemEquipment(item);
        }

        public Equipment.Equipment GetItemEquipment(string id)
        {
            return InventoryRepository.GetItemEquipment(id);
        }

        public Room.Room GetItemRoom(InventoryItem item)
        {
            return InventoryRepository.GetItemRoom(item);
        }

        public InventoryItem GetInventoryItem(string equipmentId, string roomId)
        {
            return InventoryRepository.GetInventoryItem(equipmentId, roomId);
        }

        public List<InventoryItem> InventoryItems()
        {
            return InventoryRepository.Items;
        }

        public List<InventoryItemData> InventoryItemsData()
        {
            return InventoryRepository.GetInventoryItemsData(InventoryItems());
        }

        public List<InventoryItemData> InventoryItemsData(List<InventoryItem> items)
        {
            return InventoryRepository.GetInventoryItemsData(items);
        }

        public List<InventoryItemData> GetDynamicItemsData()
        {
            return (from InventoryItemData item in InventoryItemsData()
                    where item.EquipmentDinamic && item.Quantity < 5
                    select item).ToList();
        }

        public List<InventoryItemData>? GetDynamicEquipmentByRoomId(string roomId)
        {
            return (from inventoryItem in InventoryItemsData()
                    where inventoryItem.EquipmentDinamic && inventoryItem.RoomId == roomId && inventoryItem.Quantity > 0
                    select inventoryItem).ToList();
        }


        public void GetAllItems()
        {
            InventoryRepository.GetAllItems();
        }

        public void AddItem(InventoryItem item) {
            InventoryRepository.AddItem(item);
        }

        public void CreateEquipment(Equipment.Equipment newEquipment)
        {
            if (GetItemEquipment(newEquipment.Id) == null)
            {
                InventoryRepository.CreateEquipment(newEquipment);
                foreach (Room.Room room in GetRooms())
                {

                    AddItem(new InventoryItem(room, newEquipment));
                }

                GetAllItems();
            }
        }

        public List<InventoryItem> FindByEquipmentType(EquipmentType type, List<InventoryItem> items)
        {
            return InventoryRepository.FindByEquipmentType(type, items);
        }

        public List<InventoryItem> FindByRoomType(RoomType room, List<InventoryItem> items)
        {
            return InventoryRepository.FindByRoomType(room,items);

        }

        public List<InventoryItem> FindByQuantity(int low, int high, List<InventoryItem> items)
        {
            return InventoryRepository.FindByQuantity(low, high, items);

        }

        public List<InventoryItem> FindOutsideStorage(List<InventoryItem> items)
        {
            return InventoryRepository.FindOutsideStorage(items);

        }

        public List<InventoryItem> FindByAtributes(string atribute, List<InventoryItem> items)
        {
            return InventoryRepository.FindByAtributes(atribute, items);
        }

        private bool ChangeItemQuantity(string EquipmentId, string RoomId, int Quantity)
        {
            InventoryItem item = GetInventoryItem(EquipmentId, RoomId);
            if (item != null && item.Quantity + Quantity >= 0)
            {
                item.Quantity += Quantity;
                return true;
            }
            return false;
        }
        public void UpdateArrivedEquipment()
        {
            List<Transfer.Transfer> arrived = TransferRepository.GetArrivedItems();
            TransferRepository.DeleteOutdated();

            foreach (Transfer.Transfer transfer in arrived)
            {
                ChangeItemQuantity(transfer.EquipmentId, "00", transfer.QuantityOrdered);
            }
            Save();
        }

        public void CreateTransfer(int quantity, InventoryItemData item)
        {

            TransferRepository.AddTransfer(quantity, InventoryRepository.EquipmentService.FindById(item.EquipmentId));
        }

        public void UpdateInnerArrivedEquipment()
        {
            List<InnerTransfer.InnerTransfer> arrived = InnerTransferRepository.GetArrivedItems();
            InnerTransferRepository.DeleteOutdated();

            foreach (InnerTransfer.InnerTransfer transfer in arrived)
            {
                if(ChangeItemQuantity(transfer.EquipmentId, transfer.RoomId, -transfer.QuantityTransfered))
                {
                    ChangeItemQuantity(transfer.EquipmentId, transfer.DeliveryRoomId, transfer.QuantityTransfered);
                }
            }
            Save();
        }

        public void CreateInnerTransfer(int quantity, InventoryItemData item, string DestinationRoomId)
        {
            InnerTransferRepository.AddTransfer(quantity, item, DestinationRoomId);
        }

        public void CreateInnerTransfer(int quantity, InventoryItemData item, string DestinationRoomId, DateTime dateTime)
        {
            InnerTransferRepository.AddTransfer(quantity, item, DestinationRoomId, dateTime);
        }

        public int OccupiedQuantity(InventoryItemData item)
        {
            int quantity = item.Quantity;
            foreach (InnerTransfer.InnerTransfer transfer in InnerTransferRepository.GetInnerTransfers())
            {
                if (transfer.IsItems(item))
                {
                    quantity -= transfer.QuantityTransfered;
                }
            }
            return quantity;
        }
        public bool UseDynamicEquipment(string EquipmentId, string RoomId, int Quantity)
        {
            bool ret = ChangeItemQuantity(EquipmentId, RoomId, -Quantity);
            Save();
            return ret;
        }
        public void Save()
        {
            InventoryRepository.Save();
        }
    }
}
