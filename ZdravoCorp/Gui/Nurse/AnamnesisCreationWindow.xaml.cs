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
using ZdravoCorp.Core.Anamneses;

namespace ZdravoCorp.Gui.Nurse
{
    /// <summary>
    /// Interaction logic for AnamnesisCreationWindow.xaml
    /// </summary>
    public partial class AnamnesisCreationWindow : Window
    {
        public string DoctorJmbg;
        public string PatientJmbg;
        public string AppointmentId;
        public AnamnesisService AnamnesisService;
        public AnamnesisCreationWindow(string doctorJmbg, string patientJmbg, string appointmentId)
        {
            InitializeComponent();
            DoctorJmbg = doctorJmbg;
            PatientJmbg = patientJmbg;
            AppointmentId = appointmentId;
            AnamnesisService = new AnamnesisService();
        }

        private void CreateAnamnesisButton_Click(object sender, RoutedEventArgs e)
        {
            AnamnesisService.CreateAnamnesis(DoctorJmbg, PatientJmbg, DateTime.Now, "Symptoms: " + SymptomsTextBox.Text + " | ", AppointmentId);
            this.Close();
        }
    }
}
