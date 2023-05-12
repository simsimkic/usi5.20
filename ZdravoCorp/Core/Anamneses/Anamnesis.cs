using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Anamneses
{
    using Newtonsoft.Json;
    using ZdravoCorp.Core.Utils;
    public class Anamnesis
    {
        [JsonProperty("Doctor")]
        public string DoctorJmbg;
        [JsonProperty("Patient")]
        public string PatientJmbg;
        [JsonProperty("Date")]
        public DateTime Date;
        [JsonProperty("Description")]
        public string Description;
        [JsonProperty("Appointment Id")]
        public string AppointmentId;

        public Anamnesis(string doctorJmbg, string patientJmbg, DateTime date, string description, string appointmentId)
        {
            DoctorJmbg = doctorJmbg;
            PatientJmbg = patientJmbg;
            Date = date;
            Description = description;
            AppointmentId = appointmentId;
        }


    }
}
