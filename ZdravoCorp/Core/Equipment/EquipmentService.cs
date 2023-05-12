using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ZdravoCorp.Core.Equipment.Repository;
using ZdravoCorp.Core.Inventory;

namespace ZdravoCorp.Core.Equipment
{
    public class EquipmentService
    {
        public EquipmentRepository EquipmentRepository { get; set; }


        public EquipmentService()
        {
            EquipmentRepository = new EquipmentRepository();
        }

        public Equipment FindById(string Id)
        {
            return EquipmentRepository.FindById(Id);
        }

        public void GetAllEquipment()
        {
            EquipmentRepository.GetAllEquipment();
        }

        public bool HasAtribute(string Id, string atribute)
        {
            return FindById(Id).HasAtribute(atribute);
        }

        public void Save()
        {
            EquipmentRepository.Save();
        }

        public void CreateEquipment(Equipment newEquipment)
        {
            EquipmentRepository.CreateEquipment(newEquipment);
            Save();

        }

    }
}
