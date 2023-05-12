using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using ZdravoCorp.Core.Appointments;

namespace ZdravoCorp.Core.Utils
{
    

    public class Patient : Person
    {
        public MedicalCard MedicalCard { get; set; }
        public Patient(string jmbg, string name, string lastName, string password, DateTime birthDate, MedicalCard medicalCard)
        {
            Jmbg = jmbg;
            Name = name;
            LastName = lastName;
            Password = password;
            BirthDate = birthDate;
            MedicalCard=medicalCard;
        }


        //public MedicalCard medCard
    }
}
