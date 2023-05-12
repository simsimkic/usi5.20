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
using ZdravoCorp.Core.Inventory;
using ZdravoCorp.Gui.Nurse;

namespace ZdravoCorp.Gui.Doctor
{
    using ZdravoCorp.Core.Utils;
    using ZdravoCorp.Core.Appointments;

    public partial class ExaminationWindow : Window
    {
        public Appointment Appointment;
        public PatientService PatientService;
        public AnamnesisService AnamnesisService;
        public InventoryService InventoryService;
        public ScheduleService ScheduleService;

        public ExaminationWindow(Appointment appointment)
        {
            
            InitializeComponent();
            Appointment = appointment;
            PatientService = new PatientService();
            AnamnesisService = new AnamnesisService();
            InventoryService = new InventoryService();
            ScheduleService = new ScheduleService();
            InitializeExaminationWindow();
        }

        public void InitializeExaminationWindow()
        {
            Patient patient = PatientService.FindByJmbg(Appointment.JmbgPatient);
            patientName.Text = patient.Name;
            patientLastName.Text = patient.LastName;
        }


        private void medicalCard_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = PatientService.FindByJmbg(Appointment.JmbgPatient);
            MedicalRecordWindow mrw = new MedicalRecordWindow(patient);
            mrw.ShowDialog();
            PatientService = new PatientService();

        }

        public bool ValidateAnamnesis(string anamnesis)
        {
            if (string.IsNullOrEmpty(anamnesis))
            {
                MessageBox.Show("You need to write anamnesis first! ");
                return false;
            }

            return true;

        }

        private void finishExamination_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAnamnesis(doctorAnamnesis.Text))
            {
                Anamnesis anamnesis = AnamnesisService.FindAnamnesisByAppointmentId(Appointment.Id);

                if (anamnesis != null)
                {
                    AnamnesisService.EditAnamnesis(doctorAnamnesis.Text, Appointment.Id);
                }
                else
                {
                    AnamnesisService.CreateAnamnesis(Appointment.JmbgDoctor,Appointment.JmbgPatient,Appointment.TimeSlot.Start,
                        doctorAnamnesis.Text,Appointment.Id);
                }
                Close();
                ScheduleService.FinishAppointment(Appointment);
                EquipmentStatusWindow esw = new EquipmentStatusWindow(Appointment.RoomId);
                esw.ShowDialog();
            }

        }
    }
}
