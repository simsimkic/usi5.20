using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Utils
{
    public class MedicalCard
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public List<MedicalCondition> MedicalHistory { get; set; }

        public MedicalCard(double height, double weight, List<MedicalCondition> medicalHistory)
        {
            Height = height;
            Weight = weight;
            MedicalHistory=medicalHistory;
        }

    }
}
