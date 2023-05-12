using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Equipment;
using System.IO;
using Newtonsoft.Json;
using ZdravoCorp.Core.Appointments;


namespace ZdravoCorp.Core.Room.Repository
{
    public class RoomRepository
    {
        public List<Room> Rooms { get; set; }
        private string FilePath = "../../../Data/room.json";

        public RoomRepository()
        {
            GetAllRoom();
        }

        public Room FindById(string Id)
        {
            foreach (Room room in Rooms)
            {
                if (room.Id == Id)
                {
                    return room;
                }
            }
            return null;
        }

        public List<Room>? FindByType(RoomType roomType)
        {
            List<Room> rooms = new List<Room>();
            foreach (var room in Rooms)
            {
                if (room.Type == roomType)
                {
                    rooms.Add(room);
                }
            }

            return rooms;
        }

        public bool IsAvailable(string roomId, List<Appointment> appointments,TimeSlot duration)
        {
            foreach (var appointment in appointments)
            {
                if (appointment.RoomId == roomId && appointment.Status == AppointmentStatus.SCHEDULED && appointment.TimeSlot.OverlapsWith(duration))
                {
                    return false;
                }
            }

            return true;
        }

        public Room? FindFirstAvailableRoom(RoomType roomType, List<Appointment> appointments, TimeSlot duration)
        {
            List<Room>? rooms = FindByType(roomType);
            if (rooms.Count != 0)
            {
                foreach (var room in rooms)
                {
                    if (IsAvailable(room.Id, appointments, duration))
                    {
                        return room;
                    }
                }
            }
            
            return null;
        }

        public void GetAllRoom()
        {
            Rooms = JsonConvert.DeserializeObject<List<Room>>(File.ReadAllText(FilePath));
        }

        public void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(Rooms, Formatting.Indented));
        }
    }
}
