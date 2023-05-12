using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Utils.Doctor.Repository
{
    public class DoctorRepository
    {
        public List<Doctor> Doctors { get; set; }

        public DoctorRepository()
        {
           GetAllDoctors();
        }
        public Doctor? FindByJmbg(string jmbg)
        {
            return Doctors.FirstOrDefault(doctor => doctor.Jmbg == jmbg);
        }

        public void GetAllDoctors()
        {
            Doctors = JsonConvert.DeserializeObject<List<Doctor>>(File.ReadAllText("../../../Data/doctors.json"));
        }

        public void Save()
        {
            File.WriteAllText("../../../Data/doctors.json", JsonConvert.SerializeObject(Doctors,Formatting.Indented));
        }



    }
}
