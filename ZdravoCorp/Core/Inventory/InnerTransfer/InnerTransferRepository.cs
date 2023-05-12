using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZdravoCorp.Core.Inventory.InnerTransfer
{
    public class InnerTransferRepository
    {
        private string FilePath = "../../../Data/innerTransfer.json";
        private List<InnerTransfer> innerTransferList;
        public InnerTransferRepository()
        {
            GetAllItems();
        }

        public List<InnerTransfer> GetArrivedItems()
        {
            return (from InnerTransfer transfer in innerTransferList where transfer.Arrived() select transfer).ToList();
        }
        public void GetAllItems()
        {
            innerTransferList = JsonConvert.DeserializeObject<List<InnerTransfer>>(File.ReadAllText(FilePath));
        }

        public void DeleteOutdated()
        {
            List<InnerTransfer> newInnerTransferList = new List<InnerTransfer>();
            foreach (InnerTransfer transfer in innerTransferList)
            {
                if (!transfer.Arrived())
                {
                    newInnerTransferList.Add(transfer);
                }
            }
            innerTransferList = newInnerTransferList;
            Save();
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(innerTransferList, Formatting.Indented));
        }

        public void AddTransfer(int quantity, InventoryItemData item, string deliveryRoomId, DateTime dateTime)
        {
            innerTransferList.Add(new InnerTransfer(quantity, item, deliveryRoomId, dateTime));
            Save();
        }

        public void AddTransfer(int quantity, InventoryItemData item, string deliveryRoomId)
        {
            innerTransferList.Add(new InnerTransfer(quantity, item, deliveryRoomId));
            Save();
        }

        public List<InnerTransfer> GetInnerTransfers()
        {
            return innerTransferList;
        }
    }
}
