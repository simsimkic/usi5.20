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
using ZdravoCorp.Core.Utils;
using ZdravoCorp.Gui.Doctor;

namespace ZdravoCorp.Gui.Nurse
{
    using ZdravoCorp.Core.Appointments;
    using ZdravoCorp.Core.Utils;
    using ZdravoCorp.Core.Anamneses;
    public partial class MedicalRecordWindow : Window
    {
        public Patient Patient;
        public PatientService PatientService;
        public AnamnesisService AnamnesisService;

        public MedicalRecordWindow(Patient patient)
        {
            Patient = patient;
            PatientService = new PatientService();
            AnamnesisService = new AnamnesisService();
            InitializeComponent();
            InitializePatientInfo();
            InitializeMedicalHistory();
            InitializePatientAnamneses();
            Title = $"{patient.Name} {patient.LastName} - Medical card";
        }
        public DataTable CreateTableHeaders()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Diagnosis name");
            dt.Columns.Add("Diagnosis date");

            return dt;
        }
        public string[] ExtractMedicalConditionInfo(MedicalCondition mc) 
        {
            string[] medicalConditionInfo =
            {
                mc.DiagnosisName,
                mc.DiagnosisDate.ToString("dd.MM.yyyy"),
            };

            return medicalConditionInfo;
        }
        public void LoadMedicalHistory(List<MedicalCondition> medicalConditions)
        {
            DataTable dt = CreateTableHeaders();

            foreach (MedicalCondition mc in medicalConditions)
            {
                dt.Rows.Add(ExtractMedicalConditionInfo(mc));
            }

            medicalHistoryDataGrid.ItemsSource = new DataView(dt);
            medicalHistoryDataGrid.Items.Refresh();
        }

        public void InitializeMedicalHistory()
        {
            LoadMedicalHistory(Patient.MedicalCard.MedicalHistory);
        }

        public void InitializePatientAnamneses()
        {
            List<Anamnesis> anamneses = AnamnesisService.FindAnamnesesByPatientJmbg(Patient.Jmbg);
            LoadAnamneses(anamneses);
        }

        public void LoadAnamneses(List<Anamnesis> anamneses)
        {
            foreach (var anamnesis in anamneses)
            {
                patientAnamneses.Items.Add(anamnesis.Description + " - " +  anamnesis.Date.Date.ToString("dd.MM.yyyy"));
            }
        }

        public void InitializePatientInfo()
        {
            patientName.Text = Patient.Name;
            patientLastName.Text = Patient.LastName;
            patientHeight.Text = Patient.MedicalCard.Height.ToString();
            patientWeight.Text = Patient.MedicalCard.Weight.ToString();
            patientBirthDate.Text = Patient.BirthDate.Date.ToString("dd.MM.yyyy");
            patientJmbg.Text = Patient.Jmbg;
        }


        private void editMedicalCard_Click(object sender, RoutedEventArgs e)
        {
            addDiagnosis.Visibility = Visibility.Visible;
            deleteDiagnosis.Visibility = Visibility.Visible;
            patientHeight.IsEnabled = true;
            patientWeight.IsEnabled = true;
            editMedicalCard.Visibility = Visibility.Hidden;
            editCard.Visibility = Visibility.Visible;

        }

        public MedicalCondition? ExtractConditionFromTable()
        {
            DataRowView row = medicalHistoryDataGrid.SelectedItem as DataRowView;

            if (row != null)
            {
                return PatientService.FindByName(row["Diagnosis name"].ToString(),Patient);
            }

            MessageBox.Show("Please select a condition! ");
            return null;
        }

        public void RefreshMedicalCard()
        {
            PatientService = new PatientService();
            Patient = PatientService.FindByJmbg(Patient.Jmbg);
            InitializeMedicalHistory();
            InitializePatientAnamneses();


        }

        private void addDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            ConditionWindow cw = new ConditionWindow(Patient);
            cw.ShowDialog();
            RefreshMedicalCard();
        }

        private void deleteDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            MedicalCondition? medicalCondition = ExtractConditionFromTable();

            if (medicalCondition != null)
            {
                PatientService.DeleteMedicalCondition(medicalCondition.DiagnosisName,Patient);
                RefreshMedicalCard();
            }
            
        }


        public bool ValidateWidthAndHeight(TextBox text)
        
        {
            double parsedValue;
            if (double.TryParse(text.Text,out parsedValue))
            {   
                if (parsedValue > 0) {return true;}
            }

            if (text == patientWeight)
            {
                MessageBox.Show("Please insert correct weight! ");

            }
            else
            {
                MessageBox.Show("Please insert correct height! ");
            }
            return false;
        }

        private void editCard_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateWidthAndHeight(patientWeight) && ValidateWidthAndHeight(patientHeight))
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to edit this medical card?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (result == MessageBoxResult.Yes)
                {
                    PatientService.EditMedicalCard(Double.Parse(patientWeight.Text), Double.Parse(patientHeight.Text), Patient);
                    RefreshMedicalCard();
                    Close();
                }
            }
        }
    }
}
