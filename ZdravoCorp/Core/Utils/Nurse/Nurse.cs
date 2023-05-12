using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Utils.Nurse
{
    public class Nurse : Person
    {
        public Nurse(string jmbg, string name, string lastName, string password, DateTime birthDate) : base(jmbg, name, lastName, password, birthDate) { }
    
    }
}
