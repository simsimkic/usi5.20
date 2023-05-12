using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Appointments;
using ZdravoCorp.Core.Room.Repository;

namespace ZdravoCorp.Core.Room
{
    public class RoomService
    {
        public RoomRepository RoomRepository { get; set; }

        public RoomService()
        {
            RoomRepository = new RoomRepository();
        }

        public Room FindById(string Id)
        {
            return RoomRepository.FindById(Id);
        }

        public List<Room>? FindByType(RoomType roomType)
        {
            return RoomRepository.FindByType(roomType);
        }

        public bool IsAvailable(string roomId, List<Appointment> appointments, TimeSlot duration)
        {
            return RoomRepository.IsAvailable(roomId, appointments, duration);
        }

        public Room FindFirstAvailableRoom(RoomType roomType, List<Appointment> appointments, TimeSlot duration)
        {
            return RoomRepository.FindFirstAvailableRoom(roomType, appointments, duration);
        }

        public void GetAllRoom()
        {
            RoomRepository.GetAllRoom();
        }
        public List<Room> GetRooms()
        {
            return RoomRepository.Rooms;
        }
        public bool HasAtribute(string Id, string atribute)
        {
            return FindById(Id).HasAtribute(atribute);
        }

        public void Save()
        {
            RoomRepository.Save();
        }
    }
}
