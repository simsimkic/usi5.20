using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Room;

namespace ZdravoCorp.Core.Equipment.Repository
{
    public class EquipmentRepository
    {
        public List<Equipment> Equipments { get; set; }
        private string FilePath = "../../../Data/equipment.json";

        public EquipmentRepository()
        {
            GetAllEquipment();
        }

        public Equipment FindById(string Id)
        {
            foreach (Equipment equipment in Equipments)
            {
                if (equipment.Id == Id)
                {
                    return equipment;
                }
            }
            return null;
        }

        public void GetAllEquipment()
        {
            Equipments = JsonConvert.DeserializeObject<List<Equipment>>(File.ReadAllText(FilePath));
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(Equipments, Formatting.Indented));
        }

        public void CreateEquipment(Equipment newEquipment)
        {
            Equipments.Add(newEquipment);
        }

    }
}
