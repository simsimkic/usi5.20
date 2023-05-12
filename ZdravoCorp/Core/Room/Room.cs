using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZdravoCorp.Core.Room
{
    public enum RoomType { OperationRoom, ExaminationRoom, PatientRoom, WaitingRoom, Storage };
    public class Room
    {
        [JsonProperty("Room Id")]
        public string Id { get; set; }
        [JsonProperty("Room Type")]
        public RoomType Type { get; set; }

        public bool HasAtribute(string atribute)
        {
            return Id.ToUpper().Contains(atribute.ToUpper()) || Type.ToString().ToUpper().Contains(atribute.ToUpper());
        }
    }
}
