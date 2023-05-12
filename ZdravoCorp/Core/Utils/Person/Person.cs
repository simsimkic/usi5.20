using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Utils
{
    public class Person
    {
        [JsonProperty("Jmbg")]
        public string Jmbg { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("BirthDate")]
        public DateTime BirthDate { get; set; }
        [JsonConstructor]
        public Person(string jmbg, string name, string lastName, string password, DateTime birthDate)
        {
            Jmbg = jmbg;
            Name = name;
            LastName = lastName;
            Password = password;
            BirthDate = birthDate;
        }
        public Person() { }
    }
}
