using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZdravoCorp.Core.Inventory.Transfer
{
    public class TransferRepository
    {
        private string FilePath = "../../../Data/transfer.json";
        private List<Transfer> TransferList;
        public TransferRepository()
        {
            GetAllItems();
        }

        public List<Transfer> GetArrivedItems()
        {
            return (from Transfer transfer in TransferList where transfer.Arrived() select transfer).ToList();
        }
        public void GetAllItems()
        {
            TransferList = JsonConvert.DeserializeObject<List<Transfer>>(File.ReadAllText(FilePath));
        }

        public void DeleteOutdated()
        {
            List<Transfer> newTransferList = new List<Transfer>();
            foreach (Transfer transfer in TransferList)
            {
                if (!transfer.Arrived())
                {
                    newTransferList.Add(transfer);
                }
            }
            TransferList = newTransferList;
            Save();
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(TransferList, Formatting.Indented));
        }

        internal void AddTransfer(int quantity, Equipment.Equipment equipment)
        {
            TransferList.Add(new Transfer(quantity, equipment));
            Save();
        }
    }
}
