using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ZdravoCorp.Core.Utils.Doctor;
using ZdravoCorp.Core.Utils.Doctor.Repository;
using ZdravoCorp.Core.Utils.Nurse.Repository;

namespace ZdravoCorp.Core.Utils.Nurse
{
    public class NurseService
    {
        public NurseRepository NurseRepository;

        public NurseService() 
        { 
            NurseRepository = new NurseRepository();
        }

        public List<Nurse> Nurses()
        {
            return NurseRepository.Nurses;
        }

        public Nurse? FindByJmbg(string jmbg)
        { 
            return NurseRepository.FindByJmbg(jmbg);
        }

        public void Save()
        {
            NurseRepository.Save();
        }
        public bool ValidatePatientInfo(string jmbg, string name, string lastName, string password, DateTime? birthDate)
        {
            return NurseRepository.ValidatePatientInfo(jmbg, name, lastName, password, birthDate);
        }
        public bool ValidateNewPatient(string jmbg, string name, string lastName, string password, string weight, string height, DateTime? birthDate)
        {
            return NurseRepository.ValidateNewPatient(jmbg, name, lastName, password, weight, height, birthDate);
        }
        public MedicalCard CreateMedicalCard(string weight, string height)
        {
            return NurseRepository.CreateMedicalCard(weight, height);
        }
        public Patient CreatePatient(string jmbg, string name, string lastName, string password, DateTime birthDate, string weight, string height)
        {
            return NurseRepository.CreatePatient(jmbg, name, lastName, password, birthDate, weight, height);
        }
    }
}
