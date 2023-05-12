using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Utils
{
    public class MedicalCondition
    {
        public string DiagnosisName { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public MedicalCondition(string diagnosisName, DateTime diagnosisDate)
        {
            DiagnosisName = diagnosisName;
            DiagnosisDate = diagnosisDate;
        }

    }
}
