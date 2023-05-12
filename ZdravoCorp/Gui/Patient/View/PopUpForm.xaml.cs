using System;
using System.Collections.Generic;
using System.Globalization;
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
using ZdravoCorp.Core.Appointments;
using ZdravoCorp.Core.Utils;
using ZdravoCorp.Core.Utils.Doctor;
using ZdravoCorp.Core.Utils.Doctor.Repository;

namespace ZdravoCorp.Gui.Patient.View
{
    using ZdravoCorp.Core.Utils.Doctor;
    
    public partial class PopUpForm : Window
    {
        public Appointment SelectedAppointment;
        public string JmbgOfLogged;
        public DoctorService DoctorService;
        public PatientService PatientService;
        public ScheduleService ScheduleService;
        public PopUpForm(Appointment selectedAppointment,string jmbgOfLogged)
        {
            InitializeComponent();
            SelectedAppointment = selectedAppointment;
            JmbgOfLogged= jmbgOfLogged;
            DoctorService=new DoctorService();
            PatientService = new PatientService();
            ScheduleService=new ScheduleService();
        }
        public bool ValidFieldsInput()
        {
            string timeString = comboBoxTime.Text;
            DateTime time;
            string docJmbg;
            if (doctorJmbg.SelectedIndex==-1)
            {
                MessageBox.Show("Jmbg field can't be empty!");
                return false;
            }
            bool found = false;
            foreach (var d in DoctorService.DoctorRepository.Doctors)
            {
                if (d.Jmbg == doctorJmbg.Text)
                {
                    found = true;
                    docJmbg = d.Jmbg;
                    break;
                }
            }
            if (!found)
            {
                MessageBox.Show("Doctor not found!");
                return false;
            }
            else if (datePicker.SelectedDate == null || datePicker.SelectedDate.Value.Date <= DateTime.Now)
            {
                MessageBox.Show("Date is not selected or isn't in the future!");
                return false;
            }
            else if (!DateTime.TryParseExact(timeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
            {
                MessageBox.Show("Wrong time input!");
                return false;
            }
            return true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidFieldsInput())
            {
                Appointment oldAppointment = ScheduleService.ScheduleRepository.FindAppointment(SelectedAppointment.JmbgDoctor, SelectedAppointment.TimeSlot, SelectedAppointment.JmbgPatient);
                DateTime selectedDate = datePicker.SelectedDate.Value.Date;
                string sTime = comboBoxTime.Text;
                TimeSpan time = TimeSpan.Parse(sTime);
                DateTime selectedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, time.Hours, time.Minutes, time.Seconds);
                TimeSlot timeSlot = new TimeSlot(selectedDateTime, selectedDateTime.AddMinutes(15));

                Doctor docWithJmbg = DoctorService.FindByJmbg(doctorJmbg.Text);

                if (SelectedAppointment.JmbgDoctor==doctorJmbg.Text && SelectedAppointment.TimeSlot.Start==timeSlot.Start && SelectedAppointment.TimeSlot.End == timeSlot.End)
                {
                    MessageBox.Show("You didn't change any parameter!");
                }

                else if (ScheduleService.IsAvailable(docWithJmbg, timeSlot))
                {
                    
                    if (!PatientService.PatientRepository.PatientAppointmentEdits.ContainsKey(JmbgOfLogged))
                    {
                        PatientService.PatientRepository.PatientAppointmentEdits[JmbgOfLogged] = new List<DateTime>();
                    }
                    PatientService.PatientRepository.PatientAppointmentEdits[JmbgOfLogged].Add(DateTime.Now);
                    PatientService.Save();

                    oldAppointment.JmbgDoctor = doctorJmbg.Text;
                    oldAppointment.JmbgPatient = JmbgOfLogged;
                    oldAppointment.TimeSlot=timeSlot;

                    MessageBox.Show("Successfuly edited appointment!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Doctor is not available at that time!");
                }
                ScheduleService.Save();
            }
        }
    }
}
