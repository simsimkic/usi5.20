
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Utils;

namespace ZdravoCorp.Core.Appointments
{
    public enum AppointmentType  {
        OPERATION,
        EXAMINATION
    }

    public enum AppointmentStatus
    {
        SCHEDULED,
        FINISHED,
        CANCELED,
        READY
    }

    public class Appointment
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("JmbgDoctor")]
        public string JmbgDoctor { get; set; }

        [JsonProperty("JmbgPatient")]
        public string JmbgPatient { get; set; }

        [JsonProperty("TimeSlot")]
        public TimeSlot TimeSlot;

        [JsonProperty("Type")]
        public AppointmentType Type;

        [JsonProperty("Status")]
        public AppointmentStatus Status;

        [JsonProperty("Room")]
        public string RoomId;
        //public void Cancel()
        //public void Confirm()

        [JsonConstructor]
        public Appointment(TimeSlot timeSlot, AppointmentType type, string jmbgDoctor, string jmbgPatient, AppointmentStatus status, string id, string roomId)
        {
            TimeSlot = timeSlot;
            Type = type;
            JmbgDoctor = jmbgDoctor;
            JmbgPatient = jmbgPatient;
            Status = status;
            Id = id;
            RoomId = roomId;
        }

        public Appointment(string jmbgdoctor, string jmbgpatient, TimeSlot timeSlot, string id,string roomId)
        {
            JmbgDoctor = jmbgdoctor;
            JmbgPatient = jmbgpatient;
            TimeSlot = timeSlot;
            Type = AppointmentType.EXAMINATION;
            Status = AppointmentStatus.SCHEDULED;
            Id = id;
            RoomId = roomId;
        }

    }
}
