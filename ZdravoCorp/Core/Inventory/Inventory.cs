using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Equipment;
using ZdravoCorp.Core.Equipment.Repository;
using ZdravoCorp.Core.Room.Repository;
using ZdravoCorp.Core.Room;

namespace ZdravoCorp.Core.Inventory
{
    public class Inventory
    {
        private string FilePath = "../../../Data/inventory.json";
        public List<InventoryItem> Items { get; set; }
        public EquipmentService EquipmentService { get; set; }
        public RoomService RoomService { get; set; }

        public Inventory()
        {
            GetAllItems();
            EquipmentService = new EquipmentService();
            RoomService = new RoomService();

        }
        public void AddItem(InventoryItem item)
        {
            Items.Add(item);
            Save();
        }

        public InventoryItem GetInventoryItem(string equipmentId, string roomId)
        {
            foreach(InventoryItem item in Items)
            {
                if(item.RoomId == roomId && item.EquipmentId == equipmentId)
                {
                    return item;
                }
            }
            return null;
        }

        public Equipment.Equipment GetItemEquipment(InventoryItem item)
        {
            return EquipmentService.FindById(item.EquipmentId);
        }

        public Equipment.Equipment GetItemEquipment(string id)
        {
            return EquipmentService.FindById(id);
        }

        public Room.Room GetItemRoom(InventoryItem item)
        {
            return RoomService.FindById(item.RoomId);
        }

        public List<InventoryItem> FindByEquipmentType(EquipmentType type, List<InventoryItem> items) 
        {
            return (from InventoryItem item in items
                    where GetItemEquipment(item).Type == type
                    select item).ToList();
        }

        public List<InventoryItem> FindByRoomType(RoomType room, List<InventoryItem> items) {
            return (from InventoryItem item in items
                    where GetItemRoom(item).Type == room
                    select item).ToList();
        }

        public List<InventoryItem> FindByQuantity(int low, int high, List<InventoryItem> items) {
            return (from InventoryItem item in items
                    where high >= item.Quantity && item.Quantity > low
                    select item).ToList();
        }

        public List<InventoryItem> FindOutsideStorage(List<InventoryItem> items) {
            return (from InventoryItem item in items
                    where GetItemRoom(item).Type != RoomType.Storage
                    select item).ToList();
        }

        public List<InventoryItem> FindByAtributes(string atribute, List<InventoryItem> items)
        {
            return (from InventoryItem item in items
                    where RoomService.HasAtribute(item.RoomId, atribute) || EquipmentService.HasAtribute(item.EquipmentId, atribute) || item.Quantity.ToString().ToUpper().Contains(atribute.ToUpper())
                    select item).ToList();
        }

        public List<InventoryItemData> GetInventoryItemsData(List<InventoryItem> items){
            List<InventoryItemData> ret = new List<InventoryItemData>();
            foreach(InventoryItem item in items){
                ret.Add(GetInventoryItemData(item));
            }
            return ret;
        }

        public InventoryItemData GetInventoryItemData(InventoryItem item){
            InventoryItemData ret = new InventoryItemData(item.Quantity, GetItemEquipment(item), GetItemRoom(item));
            return ret;
        }

        public void GetAllItems()
        {
            Items = JsonConvert.DeserializeObject<List<InventoryItem>>(File.ReadAllText(FilePath));
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(Items, Formatting.Indented));
        }

        public void CreateEquipment(Equipment.Equipment newEquipment)
        {
            EquipmentService.CreateEquipment(newEquipment);
        }

        
    }
}