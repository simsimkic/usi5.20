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
using ZdravoCorp.Core.Appointments;

namespace ZdravoCorp.Gui.Patient.View
{
    
    using ZdravoCorp.Core.Utils.Doctor;
    public partial class PatientFinishedAppointments : Window
    {
        public string JmbgOfLogged;
        public List<Appointment> PatientAppointments;
        public List<Anamnesis> PatientAnamnesis;
        public ScheduleService ScheduleService;
        public AnamnesisService AnamnesisService;
        public DoctorService DoctorService;
        public PatientFinishedAppointments(string jmbgOfLogged)
        {
            
            JmbgOfLogged = jmbgOfLogged;
            InitializeComponent();
            AnamnesisService = new AnamnesisService();
            ScheduleService = new ScheduleService();
            DoctorService = new DoctorService();
            PatientAppointments = ScheduleService.GetFinishedAppointmentsByPatientJmbg(JmbgOfLogged);
            PatientAnamnesis = AnamnesisService.CreateAmamnesisFromAppointments(PatientAppointments);
            FillListBox();
        }
        public void FillListBox()
        {
            listBox.Items.Clear();
            List<string> strings = new List<string>();
            foreach (Appointment a in PatientAppointments)
            {
                string s = a.JmbgDoctor + " " + a.TimeSlot.Start.ToString("dd/MM/yyyy HH:mm") + " " + a.Status;
                strings.Add(s);
            }
            foreach (string s in strings)
            {
                listBox.Items.Add(s);
            }
            listBox.Items.Refresh();
        }
        private void Appointment_Anamnesis_Click(object sender, RoutedEventArgs e)
        {
            int index = listBox.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Select one of the appointments!");
            }
            else
            {
                MessageBox.Show(AnamnesisService.FindAnamnesisById(PatientAppointments.ElementAt(index).Id).Description);
            }
        }

        private void Sort_By_Date_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointments=ScheduleService.SortAppointmentsByStartTime(PatientAppointments);
            FillListBox();
        }

        private void Sort_By_Doctor_Jmbg_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointments = ScheduleService.SortAppointmentsByDoctorJmbg(PatientAppointments);
            FillListBox();
        }

        private void Sort_By_Doctor_Specialty_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, Doctor.Specialty> doctorSpecialties = new Dictionary<string, Doctor.Specialty>();
            foreach(Appointment appointment in PatientAppointments)
            {
                if (!doctorSpecialties.ContainsKey(appointment.JmbgDoctor))
                {
                    doctorSpecialties[appointment.JmbgDoctor] = DoctorService.FindByJmbg(appointment.JmbgDoctor).Specialization;
                }
            }
            PatientAppointments=ScheduleService.SortAppointmentsByDoctorSpecialty(PatientAppointments, doctorSpecialties);
            FillListBox();
        }

        private void Search_Anamnesis_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != "")
            {
                
                listBox.Items.Clear();
                PatientAppointments = ScheduleService.GetFinishedAppointmentsByPatientJmbg(JmbgOfLogged);
                PatientAnamnesis = AnamnesisService.CreateAmamnesisFromAppointments(PatientAppointments);
                FillListBox();


                PatientAppointments = ScheduleService.FindAppointmentsByAnamnesisDescriptionSubstring(PatientAppointments, textBox.Text);
                if (PatientAppointments.Count() == 0)
                {
                    MessageBox.Show("There are no results");
                }
                PatientAnamnesis = AnamnesisService.CreateAmamnesisFromAppointments(PatientAppointments);
                FillListBox();
            }
            else
            {
                MessageBox.Show("Search can't be empty!");
            }
        }

        private void Go_Back_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();
            PatientAppointments = ScheduleService.GetFinishedAppointmentsByPatientJmbg(JmbgOfLogged);
            PatientAnamnesis = AnamnesisService.CreateAmamnesisFromAppointments(PatientAppointments);
            FillListBox();
        }
    }
}
