using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Threading;
using ZdravoCorp.Core.Appointments;
using ZdravoCorp.Core.Inventory;
using ZdravoCorp.Core.Login;
using ZdravoCorp.Core.Notifications;
using ZdravoCorp.Core.Utils;
using ZdravoCorp.Core.Utils.Doctor;
using ZdravoCorp.Core.Utils.Doctor.Repository;
using ZdravoCorp.Core.Utils.Nurse;
using ZdravoCorp.Gui.Doctor;
using ZdravoCorp.Gui.Doctor.View;
using ZdravoCorp.Gui.Manager.InventoryWindow;
using ZdravoCorp.Gui.Nurse;
using ZdravoCorp.Gui.Patient.View;

namespace ZdravoCorp
{

    public partial class MainWindow : Window
    {
        private InventoryService inventory;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            inventory = new InventoryService();

            UpdateArrivedItems();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        public void ShowNotification(string message)
        {
            MessageBox.Show(message, "Appointment postpone", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void CheckForNotifications(string jmbg)
        {
            NotificationService notificationService = new NotificationService();
            List<string> notificationsToDelete = new List<string>();
            foreach(Notification notification in notificationService.Notifications())
            {
                if(notification.PersonJmbg == jmbg)
                {
                    ShowNotification(notification.Message);
                    notificationsToDelete.Add(notification.Id);
                }
            }
            foreach(string notification in notificationsToDelete)
            {
                notificationService.DeleteNotification(notification);
            }
            
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login(loginJmbg.Text, loginPassword.Password);
            Doctor? doctor = login.CheckDoctorLogin();
            Patient? patient = login.CheckPatientLogin();
            Nurse? nurse = login.CheckNurseLogin();


            if (loginJmbg.Text == "admin" && loginPassword.Password == "admin")
            {
                InventoryWindow inventoryWindow = new InventoryWindow(inventory);
                inventoryWindow.Show();
                Close();
            }
            else if (doctor != null)
            {       
                DoctorWindow doctorWindow = new DoctorWindow(doctor);
                
                doctorWindow.Show();
                CheckForNotifications(doctor.Jmbg);
                Close();
            }
            else if (patient != null)
            {   
                PatientService service=new PatientService();
                PatientWindow patientWindow = new PatientWindow(patient.Jmbg);
                
                if (service.IsPatientBlocked(patient.Jmbg))
                {
                    MessageBox.Show("Patient is blocked!");
                }
                else
                {
                    patientWindow.Show();
                    CheckForNotifications(patient.Jmbg);
                    Close();
                }
            }
            else if (nurse != null)
            {
                NurseWindow nurseWindow = new NurseWindow();
                nurseWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Wrong JMBG or password! ");
                loginJmbg.Text = string.Empty;
                loginPassword.Password = string.Empty;
            }


        }

        private void UpdateArrivedItems()
        {
            inventory.UpdateInnerArrivedEquipment();
            inventory.UpdateArrivedEquipment();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateArrivedItems();
        }
    }
}
