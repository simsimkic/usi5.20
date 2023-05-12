using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ZdravoCorp.Core.Anamneses
{
    using Newtonsoft.Json;
    using ZdravoCorp.Core.Appointments;
    using ZdravoCorp.Core.Anamneses;

    public class AnamnesisRepository
    {
        public List<Anamnesis> Anamneses;

        public AnamnesisRepository()
        {
            GetAllAnamneses();
        }

        public void CreateAnamnesis(string doctorJmbg, string patientJmbg, DateTime date, string description,
            string appointmentId)
        {
            Anamneses.Add(new Anamnesis(doctorJmbg, patientJmbg, date, description, appointmentId));
            Save();
        }

        public void EditAnamnesis(string newDescription, string appointmentId)
        {
            Anamnesis anamnesis = FindAnamnesisByAppointmentId(appointmentId);
            anamnesis.Description += newDescription;
            Save();
        }

        public Anamnesis? FindAnamnesisByAppointmentId(string appointmentId)
        {
            return Anamneses.FirstOrDefault(anamnesis => anamnesis.AppointmentId == appointmentId);
        }

        public List<Anamnesis>? FindAnamnesesByPatientJmbg(string patientJmbg)
        {
            return Anamneses.Where(anamnesis => anamnesis.PatientJmbg == patientJmbg).ToList();
        }
        public Anamnesis? FindAnamnesisById(string id)
        {
            return Anamneses.FirstOrDefault(anamnesis => anamnesis.AppointmentId == id);
        }
        public void GetAllAnamneses()
        {
            Anamneses = JsonConvert.DeserializeObject<List<Anamnesis>>(File.ReadAllText("../../../Data/anamneses.json"));
        }
        public List<Anamnesis> CreateAmamnesisFromAppointments(List<Appointment> appointments)
        {
            List<Anamnesis> retList = new List<Anamnesis>();
            foreach(Appointment appointment in appointments)
            {
                retList.Add(FindAnamnesisById(appointment.Id));
            }
            return retList;
        }
        public void Save()
        {
            File.WriteAllText("../../../Data/anamneses.json", JsonConvert.SerializeObject(Anamneses, Formatting.Indented));
        }
    }
}
