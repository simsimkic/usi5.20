using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Utils
{
    public class PatientService
    {
        public PatientRepository PatientRepository { get; set; }
        public PatientService()
        {
            PatientRepository = new PatientRepository();
        }
        public PatientService(PatientRepository patientRepository)
        {
            PatientRepository = patientRepository;
        }
        public List<Patient> AllPatients()
        {
            return PatientRepository.AllPatients();
        }
        public Patient? FindByJmbg(string jmbg)
        {
            return PatientRepository.FindByJmbg(jmbg);
        }

        public void EditMedicalCard(double weight, double height, Patient patient)
        {
            PatientRepository.EditMedicalCard(weight,height,patient);
        }

        public void DeleteMedicalCondition(string medicalCondition, Patient patient)
        {
            PatientRepository.DeleteMedicalCondition(medicalCondition,patient);
        }

        public void AddMedicalCondition(MedicalCondition medicalCondition, Patient patient)
        {
            PatientRepository.AddMedicalCondition(medicalCondition,patient);
        }

        public MedicalCondition? FindByName(string medicalCondition, Patient patient)
        {
            return PatientRepository.FindByName(medicalCondition, patient);
        }

        public Dictionary<string, List<DateTime>> AllPatientAppointmentCreations()
        {
            return PatientRepository.AllPatientAppointmentCreations();
        }
        public Dictionary<string, List<DateTime>> AllPatientAppointmentEdits()
        {
            return PatientRepository.AllPatientAppointmentEdits();
        }
        public bool PatientCreatedTooManyAppointments(string jmbg)
        {
            return PatientRepository.PatientCreatedTooManyAppointments(jmbg);
        }
        public bool PatientEditedTooManyAppointments(string jmbg)
        {
            return PatientRepository.PatientEditedTooManyAppointments(jmbg);
        }
        public bool IsPatientBlocked(string jmbg)
        {
            return PatientRepository.IsPatientBlocked(jmbg);
        }
        public void Save()
        {
            PatientRepository.Save();
        }
    }
}
