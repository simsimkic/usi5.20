using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Utils.Doctor.Repository;

namespace ZdravoCorp.Core.Utils.Doctor
{
    public class DoctorService
    {
        public DoctorRepository DoctorRepository;

        public DoctorService()
        {
            DoctorRepository = new DoctorRepository();
        }

        public List<Doctor> Doctors()
        {
            return DoctorRepository.Doctors;
        }

        public Doctor? FindByJmbg(string jmbg)
        {
            return DoctorRepository.FindByJmbg(jmbg);
        }

        public void Save()
        {
            DoctorRepository.Save();
        }

    }
}
