using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Utils;
using ZdravoCorp.Core.Utils.Doctor;

namespace ZdravoCorp.Core.Login
{
    using ZdravoCorp.Core.Utils.Nurse;
    public class Login
    {
        public string Jmbg;
        public string Password;
        public DoctorService DoctorService;
        public NurseService NurseService;
        public PatientService PatientService;

        public Login(string jmbg, string password)
        {
            Jmbg = jmbg;
            Password = password;
            DoctorService = new DoctorService();
            NurseService = new NurseService();
            PatientService = new PatientService();
        }

        public Doctor? CheckDoctorLogin()
        {
            return DoctorService.Doctors().FirstOrDefault(doctor => doctor.Jmbg == Jmbg && doctor.Password == Password);
        }

        public Patient? CheckPatientLogin()
        {
            return PatientService.AllPatients().FirstOrDefault(patient => patient.Jmbg == Jmbg && patient.Password == Password);
        }

        public Nurse? CheckNurseLogin()
        {
            return NurseService.Nurses().FirstOrDefault(nurse => nurse.Jmbg == Jmbg && nurse.Password == Password);
        }

    }
}
