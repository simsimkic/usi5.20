using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Utils;

namespace ZdravoCorp.Core.Anamneses
{
    using Newtonsoft.Json;
    using ZdravoCorp.Core.Appointments;
    using ZdravoCorp.Core.Anamneses;

    public class AnamnesisService
    {
        public AnamnesisRepository AnamnesisRepository { get; set;}

        public AnamnesisService()
        {
            AnamnesisRepository = new AnamnesisRepository();
        }

        [JsonProperty("Doctor")]
        public string DoctorJmbg;
        [JsonProperty("Patient")]
        public string PatientJmbg;
        [JsonProperty("Date")]
        public DateTime Date;
        [JsonProperty("Description")]
        public string Description;
        [JsonProperty("Appointment Id")]
        public int AppointmentId;

        public void CreateAnamnesis(string doctorJmbg, string patientJmbg, DateTime date, string description,
            string appointmentId)
        {
            AnamnesisRepository.CreateAnamnesis(doctorJmbg, patientJmbg, date, description, appointmentId);
        }

        public Anamnesis? FindAnamnesisByAppointmentId(string appointmentId)
        {
            return AnamnesisRepository.FindAnamnesisByAppointmentId(appointmentId);
        }

        public List<Anamnesis>? FindAnamnesesByPatientJmbg(string patientJmbg)
        {
            return AnamnesisRepository.FindAnamnesesByPatientJmbg(patientJmbg);
        }

        public void EditAnamnesis(string newDescription, string appointmentId)
        {
            AnamnesisRepository.EditAnamnesis(newDescription, appointmentId);
        }

        public Anamnesis? FindAnamnesisById(string id)
        {
            return AnamnesisRepository.FindAnamnesisById(id);

        }

        public List<Anamnesis> GetAllAnamneses()
        {
            return AnamnesisRepository.Anamneses;
        }
        public List<Anamnesis> CreateAmamnesisFromAppointments(List<Appointment> appointments)
        {
            return AnamnesisRepository.CreateAmamnesisFromAppointments(appointments);
        }
        public void Save()
        {
            AnamnesisRepository.Save();
        }
    }
}
