using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace ZdravoCorp.Core.Utils
{
    public class PatientRepository
    {
        public List<Patient> Patients { get; set; }

        public Dictionary<string, List<DateTime>> PatientAppointmentCreations;
        public Dictionary<string, List<DateTime>> PatientAppointmentEdits;
        public PatientRepository()
        {
            Patients = AllPatients();
            PatientAppointmentCreations = AllPatientAppointmentCreations();
            PatientAppointmentEdits = AllPatientAppointmentEdits();
        }


        public Patient? FindByJmbg(string jmbg)
        {
            return Patients.FirstOrDefault(patient => patient.Jmbg == jmbg);
        }

        public MedicalCondition? FindByName(string medicalCondition,Patient patient)
        {
            return patient.MedicalCard.MedicalHistory.FirstOrDefault(condition => condition.DiagnosisName == medicalCondition);
        }

        public void FindAndDeleteConditionByName(List<MedicalCondition> medicalConditions,string medicalCondition)
        {
            foreach (var condition in medicalConditions)
            {
                if (condition.DiagnosisName == medicalCondition)
                {
                    medicalConditions.Remove(condition);
                    break;
                }
            }
        }

        public void EditMedicalCard(double weight, double height, Patient patient)
        {
            foreach (var pat in Patients.Where(pat => pat.Jmbg == patient.Jmbg))
            {
                pat.MedicalCard.Weight = weight;
                pat.MedicalCard.Height = height;
                break;
            }

            Save();
        }

        public void DeleteMedicalCondition(string medicalCondition, Patient patient)
        {
            foreach (var pat in Patients.Where(pat => pat.Jmbg == patient.Jmbg))
            {
                FindAndDeleteConditionByName(pat.MedicalCard.MedicalHistory,medicalCondition);
                break;
            }

            Save();
        }

        public void AddMedicalCondition(MedicalCondition medicalCondition, Patient patient)
        {
            foreach (var pat in Patients)
            {
                if (pat.Jmbg == patient.Jmbg)
                {
                    pat.MedicalCard.MedicalHistory.Add(medicalCondition);
                    break;
                }

            }
            Save();
        }


        public List<Patient> AllPatients()
        {
            List<Patient> patients = new List<Patient>();
            using (StreamReader r = new StreamReader("../../../Data/patients.json"))
            {
                string json = r.ReadToEnd();
                patients = JsonConvert.DeserializeObject<List<Patient>>(json);
            }
            return patients;
        }

        public Dictionary<string, List<DateTime>> AllPatientAppointmentCreations()
        {
            Dictionary<string, List<DateTime>> patientAppointments = new Dictionary<string, List<DateTime>>();
            using(StreamReader r =new StreamReader("../../../Data/patientAppointmentCreations.json"))
            {
                string json = r.ReadToEnd();
                patientAppointments = JsonConvert.DeserializeObject<Dictionary<string, List<DateTime>>>(json);
            }
            return patientAppointments;
        }
        public Dictionary<string, List<DateTime>> AllPatientAppointmentEdits()
        {
            Dictionary<string, List<DateTime>> patientAppointments = new Dictionary<string, List<DateTime>>();
            using (StreamReader r = new StreamReader("../../../Data/patientAppointmentEdits.json"))
            {
                string json = r.ReadToEnd();
                patientAppointments = JsonConvert.DeserializeObject<Dictionary<string, List<DateTime>>>(json);
            }
            return patientAppointments;
        }
        public bool PatientCreatedTooManyAppointments(string jmbg)
        {
            DateTime startDate = DateTime.Now.AddDays(-30);
            int createdAppointmentLimit = 8;
            if (!PatientAppointmentCreations.ContainsKey(jmbg))
            {
                return false;
            }
            int appointmentsAfterStartDate = 0;

            foreach (DateTime d in PatientAppointmentCreations[jmbg])
            {
                if (d > startDate)
                {
                    appointmentsAfterStartDate++;
                }
            }
            if (appointmentsAfterStartDate >= createdAppointmentLimit)
            {
                return true;
            }
            return false;
        }
        public bool PatientEditedTooManyAppointments(string jmbg)
        {
            DateTime startDate = DateTime.Now.AddDays(-30);
            int editOrDeleteAppointmentLimit = 5;
            if (!PatientAppointmentEdits.ContainsKey(jmbg))
            {
                return false;
            }
            int appointmentsEditsAfterStartDate = 0;
            foreach (DateTime d in PatientAppointmentEdits[jmbg])
            {
                if (d > startDate)
                {
                    appointmentsEditsAfterStartDate++;
                }
            }
            if (appointmentsEditsAfterStartDate >= editOrDeleteAppointmentLimit)
            {
                return true;
            }
            return false;
        }
        public bool IsPatientBlocked(string jmbg)
        {
            return PatientCreatedTooManyAppointments(jmbg) || PatientEditedTooManyAppointments(jmbg);
        }
        public void Save()
        {
            File.WriteAllText("../../../Data/patients.json", JsonConvert.SerializeObject(Patients, Formatting.Indented));
            File.WriteAllText("../../../Data/patientAppointmentCreations.json", JsonConvert.SerializeObject(PatientAppointmentCreations, Formatting.Indented));
            File.WriteAllText("../../../Data/patientAppointmentEdits.json", JsonConvert.SerializeObject(PatientAppointmentEdits, Formatting.Indented));
        }
    }
}
