using System;
using System.Collections.Generic;
using System.Data;
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
using ZdravoCorp.Core.Utils.Doctor.Repository;
using ZdravoCorp.Gui.Nurse;


namespace ZdravoCorp.Gui.Doctor.View
{
    using System.Collections;
    using System.Globalization;
    using ZdravoCorp.Core.Inventory;
    using ZdravoCorp.Core.Utils.Doctor;
    using ZdravoCorp.Core.Utils;

    public partial class DoctorWindow : Window
    {
        public Doctor Doctor;
        public DoctorService DoctorService;
        public PatientRepository Patients;
        public PatientService PatientService;
        public ScheduleService ScheduleService;

        public DoctorWindow(Doctor doctor)
        {
            Doctor = doctor;
            ScheduleService = new ScheduleService();
            DoctorService = new DoctorService();
            PatientService = new PatientService();
            InitializeComponent();
            InitializePatientsTable();

        }

        private void doctorWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to log out?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow mw = new MainWindow();
                mw.Show();
            }
            else
            {
                e.Cancel = true;
            }

        }

        public void InitalizeAppointmentsTable(List<Appointment> doctorAppointments)
        {
            DataTable dataTable = CreateAppointmentsTableHeaders();

            foreach (var appointment in doctorAppointments)
            {
                dataTable.Rows.Add(ExtractAppointmentInfo(appointment));
            }

            appGrid.ItemsSource = new DataView(dataTable);
            appGrid.Items.Refresh();
        }

        public void LoadAppointmentsTable()
        {
            ScheduleService.ScheduleRepository.GetAllAppointments();
            InitalizeAppointmentsTable(ScheduleService.GetAppointmentsByDate(Doctor, appointmentDate.SelectedDate.Value));
        }

        public void LoadPatientsTable()
        {
            PatientService = new PatientService();
            InitializePatientsTable();
        }

        public void InitializePatientsTable()
        {
            DataTable dataTable = CreatePatientsTableHeaders();
            List <Patient> patients = PatientService.AllPatients();

            foreach (var patient in patients)
            {
                dataTable.Rows.Add(ExtractPatientInfo(patient));
            }

            patientsGrid.ItemsSource = new DataView(dataTable);
            patientsGrid.Items.Refresh();
        }

        public void EditAppointment()
        {
            Appointment? appointment = ExtractAppointmentFromTable();
            if (appointment != null)
            {
                ScheduleAppointmentWindow editAppointmentWindow = new ScheduleAppointmentWindow(Doctor,appointment);
                editAppointmentWindow.ShowDialog();
                LoadAppointmentsTable();
            }

        }

        public void CancelAppointment()
        {
            Appointment? appointment = ExtractAppointmentFromTable();
            if (appointment != null && appointment.Status != AppointmentStatus.CANCELED)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel this appointment?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (result == MessageBoxResult.Yes)
                {
                    ScheduleService.CancelAppointment(appointment);
                    LoadAppointmentsTable();
                }
            }

        }

        public void ShowPatientMedicalCard(string table,DataGrid grid)
        {
            Patient? patient = ExtractPatientFromTable(table, grid);
            if (patient != null)
            {
                MedicalRecordWindow mrw = new MedicalRecordWindow(patient);
                mrw.ShowDialog();
            }
        }

        public Appointment? ExtractAppointmentFromTable()
        {
            
            DataRowView row = appGrid.SelectedItem as DataRowView;

            if (row != null)
            {
                return ScheduleService.FindAppointmentById(row["Id"].ToString());
            }
            
        
            MessageBox.Show("Please select an appointment! ");
            return null;
        }

        public Patient? ExtractPatientFromTable(string table,DataGrid dg)
        {
            DataRowView row = dg.SelectedItem as DataRowView;

            if (row != null)
            {
                return PatientService.FindByJmbg(row["Patient JMBG"].ToString());
            }

            if (table == "appointment")
            {
                MessageBox.Show("Please select an appointment! ");
            }

            return null;
        }


        public DataTable CreateAppointmentsTableHeaders()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Patient JMBG", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Start", typeof(string));
            dt.Columns.Add("End", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Room", typeof(string));
            return dt;
        }

        public DataTable CreatePatientsTableHeaders()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Patient JMBG", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Last name", typeof(string));
            dt.Columns.Add("Birth date", typeof(string));
            return dt;
        }

        public string[] ExtractAppointmentInfo(Appointment appointment)
        {
           string[] appointmentInfo =
           {
               appointment.JmbgPatient,appointment.TimeSlot.Start.Date.ToString("dd.MM.yyyy"),appointment.TimeSlot.Start.TimeOfDay.ToString(),
               appointment.TimeSlot.End.TimeOfDay.ToString(),appointment.Type.ToString(),appointment.Status.ToString(),appointment.Id,appointment.RoomId
           };
           return appointmentInfo;
        }

        public string[] ExtractPatientInfo(Patient patient)
        {
            string[] patientInfo =
            {
                patient.Jmbg,patient.Name,patient.LastName,patient.BirthDate.Date.ToString("dd.MM.yyyy")
            };
            return patientInfo;
        }

        private bool ValidateAppointments()
        {
            if (ScheduleService.GetAppointmentsByDate(Doctor, appointmentDate.SelectedDate.Value) == null)
            {
                MessageBox.Show("There is no appointments for selected date! ");
            }

            return true;
        }


        private void appointmentDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ScheduleService.GetAppointmentsByDate(Doctor, appointmentDate.SelectedDate.Value).Count == 0)
            {
                MessageBox.Show("There is no appointments for selected date! ");
            }

            LoadAppointmentsTable();
        }


        private void scheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            ScheduleAppointmentWindow scheduleAppointment = new ScheduleAppointmentWindow(Doctor);
            scheduleAppointment.ShowDialog();

            if (appointmentDate.SelectedDate != null)
            {
                LoadAppointmentsTable();
            }

        }

        private void editAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {
            EditAppointment();
        }

        private void cancelAppointmentBtn_Click(object sender, RoutedEventArgs e)
        {
            CancelAppointment();
        }

        private void medicalCardBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowPatientMedicalCard("appointment",appGrid);
            PatientService = new PatientService();
        }

        private void patientSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = patientSearch.Text;
            DataView dataView = (DataView)patientsGrid.ItemsSource;
            DataTable dataTable = dataView.Table;

            DataView dv = new DataView(dataTable);

            dv.RowFilter = string.Format("[Patient JMBG] like '%{0}%' OR Name like '%{0}%' OR [Last name] like '%{0}%' OR [Birth date] like '%{0}%'", filterText);
            patientsGrid.ItemsSource = dv;

        }

        private void PatientsGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Patient? selectedPatient = ExtractPatientFromTable("patient",patientsGrid);
            if (selectedPatient != null)
            {
                bool hasBeenExaminated = ScheduleService.HasPatientBeenExaminatedByDoctor(Doctor, selectedPatient);
                if (hasBeenExaminated)
                {
                    mcPatientTable.Visibility = Visibility.Visible;
                }
                else
                {
                    mcPatientTable.Visibility = Visibility.Hidden;
                }
            }
        }

        private void mcPatientTable_OnClick(object sender, RoutedEventArgs e)
        {
            ShowPatientMedicalCard("patient",patientsGrid);
            LoadPatientsTable();
        }

        public bool IsAppointmentReadyForExamination(Appointment appointment)
        {
            if(!ScheduleService.IsAppointmentScheduled(appointment) || !ScheduleService.IsAppointmentReadyToBegin(appointment))
            {
                return false;
            }
            return true;
        }

        private void startExamination_Click(object sender, RoutedEventArgs e)
        {
            Appointment? slectedAppointment = ExtractAppointmentFromTable();
            if (slectedAppointment != null && IsAppointmentReadyForExamination(slectedAppointment))
            {
                ExaminationWindow ew = new ExaminationWindow(slectedAppointment);
                ew.ShowDialog();
                LoadAppointmentsTable();
            }
        }
    }
}
