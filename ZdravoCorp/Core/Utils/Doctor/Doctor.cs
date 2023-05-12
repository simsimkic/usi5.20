using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Utils.Doctor
{
    using ZdravoCorp.Core.Appointments;

    public class Doctor : Person
    {
        public enum Specialty
        {
            GENERAL,
            CARDIOLOGIST,
            DERMATOLOGIST,
            NEUROLOGIST,
            SURGEON
        }

        [JsonProperty("Specialization")]
        public Specialty Specialization { get; set; }

        [JsonConstructor]
        public Doctor(string jmbg, string name, string lastName, string password, DateTime birthDate, List<DateTime> freeDates, Specialty specialization) : base(jmbg, name, lastName, password, birthDate)
        {
            Specialization = specialization;
        }

        public Doctor(){}
    };

}

