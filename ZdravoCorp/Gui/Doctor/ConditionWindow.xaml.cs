using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZdravoCorp.Gui.Doctor
{
    using System.Text.RegularExpressions;
    using ZdravoCorp.Core.Utils;

    public partial class ConditionWindow : Window
    {
        public Patient Patient;
        PatientService PatientService;

        public ConditionWindow(Patient patient)
        {
            Patient = patient;
            PatientService = new PatientService();
            InitializeComponent();
        }

        public bool ValidateConditionDate()
        {
            if (conditionDate.SelectedDate == null || conditionDate.SelectedDate < Patient.BirthDate.Date)
            {
                MessageBox.Show("Please insert correct condition date! ");
                return false;
            }

            return true;
        }

        public bool ValidateConditionName()
        {
            Regex regex = new Regex("^[a-zA-Z ]+$");
            if (string.IsNullOrEmpty(conditionName.Text) || !regex.IsMatch(conditionName.Text) 
                                                         || PatientService.FindByName(conditionName.Text,Patient) != null)

            {
                MessageBox.Show("Please insert correct condition! ");
                return false;
            }

            return true;
        }


        public bool ValidateCondition()
        {
            if (ValidateConditionDate() && ValidateConditionName())
            {
                return true;
            }

            return false;
        }

        private void addCondition_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateCondition())
            {
                MedicalCondition medicalCondition =
                    new MedicalCondition(conditionName.Text, conditionDate.SelectedDate.Value);

                PatientService.AddMedicalCondition(medicalCondition,Patient);
                Close();
            }
        }
    }
}
