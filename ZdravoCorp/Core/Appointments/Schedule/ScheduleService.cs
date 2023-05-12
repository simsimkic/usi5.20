using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Utils.Doctor;
using ZdravoCorp.Core.Utils;
using ZdravoCorp.Core.Anamneses;

namespace ZdravoCorp.Core.Appointments
{
    public class ScheduleService
    {
        public Schedule ScheduleRepository { get; set; }

        public ScheduleService()
        {
            ScheduleRepository = new Schedule();
        }

        public List<Appointment> Appointments()
        {
            return ScheduleRepository.Appointments;
        }

        public void CreateAppointment(string doctorJmbg, string patientJmbg, TimeSlot appointmentDuration, AppointmentType type, string roomId)
        {
            ScheduleRepository.CreateAppointment(doctorJmbg, patientJmbg, appointmentDuration, type, roomId);
        }

        public void CreateAppointment(string doctorJmbg, string patientJmbg, TimeSlot appointmentDuration, string roomId)
        {
            ScheduleRepository.CreateAppointment(doctorJmbg, patientJmbg, appointmentDuration, roomId);
        }

        public void EditAppointment(string id, string newId, string patientJmbg, TimeSlot appointmentDuration, AppointmentType type, AppointmentStatus status,string roomId)
        {
           ScheduleRepository.EditAppointment(id,newId,patientJmbg,appointmentDuration,type,status,roomId);
        }



        public bool IsAppointmentReadyToBegin(Appointment appointment)
        {
            TimeSpan timeUntilAppointment = appointment.TimeSlot.Start - DateTime.Now;

            if (timeUntilAppointment.TotalMinutes <= 15 && timeUntilAppointment.TotalMinutes >= 0)
            {
                return true;
            }

            MessageBox.Show("You cant begin examination at this time! ");
            return false;
        }


        public bool IsAppointmentScheduled(Appointment appointment)
        {
            if (appointment.Status == AppointmentStatus.READY)
            {
                return true;
            }

            MessageBox.Show("You cant begin examination unitl appointment is ready! ");
            return false;
        }

        public void CancelAppointment(Appointment appointmentForCancel)
        {
            ScheduleRepository.CancelAppointment(appointmentForCancel);
        }

        public void FinishAppointment(Appointment appointmentForFinish)
        {
            ScheduleRepository.FinishAppointment(appointmentForFinish);
        }

        public void ReadyForAppointment(Appointment readyAppointment)
        {
            ScheduleRepository.ReadyForAppointment(readyAppointment);
        }

        public bool IsAppointmentInNext15Minutes(Appointment appointment)
        {
            return ScheduleRepository.IsAppointmentInNext15Minutes(appointment);
        }

        public bool HasPatientBeenExaminatedByDoctor(Doctor doctor, Patient patient)
        {
            return ScheduleRepository.HasPatientBeenExaminatedByDoctor(doctor, patient);
        }

        public bool IsAvailable(Doctor doctor, TimeSlot duration)
        {
            return ScheduleRepository.IsAvailable(doctor, duration);
        }

        public bool IsAvailable(Patient patient, TimeSlot duration)
        {
            return ScheduleRepository.IsAvailable(patient, duration);
        }

        public List<Appointment>? FindAllApointmetsWithDifferentId(string id)
        {
            return ScheduleRepository.FindAllApointmetsWithDifferentId(id);
        }

        public Appointment? FindAppointmentById(string id)
        {
            return ScheduleRepository.FindAppointmentById(id);
        }

        public List<Appointment>? GetAppointmentsByDate(Doctor doctor, DateTime startDate)
        {
            return ScheduleRepository.GetAppointmentsByDate(doctor,startDate);
        }

        public List<Appointment>? GetAppointmentsByDoctorJmbg(string doctorJmbg,bool isScheduled)
        {
            return ScheduleRepository.GetAppointmentsByDoctorJmbg(doctorJmbg);
        }
        public List<Appointment> SortAppointmentsByStartTime(List<Appointment> appointments)
        {
            return ScheduleRepository.SortAppointmentsByStartTime(appointments);
        }
        public List<Appointment> SortAppointmentsByDoctorJmbg(List<Appointment> appointments)
        {
            return ScheduleRepository.SortAppointmentsByDoctorJmbg(appointments);
        }
        public List<Appointment> SortAppointmentsByDoctorSpecialty(List<Appointment> appointments, Dictionary<string, Doctor.Specialty> doctorSpecialties)
        {
            return ScheduleRepository.SortAppointmentsByDoctorSpecialty(appointments,doctorSpecialties);
        }
        public List<Appointment> FindAppointmentsByAnamnesisDescriptionSubstring(List<Appointment> appointments, string searchString)
        {
            AnamnesisService anamnesisService=new AnamnesisService();
            List<Appointment> matchingAppointments = new List<Appointment>();
            foreach (Appointment appointment in appointments)
            {
                Anamnesis anamnesis = anamnesisService.FindAnamnesisById(appointment.Id);
                
                if (anamnesis.Description.ToUpper().Contains(searchString.ToUpper()))
                {
                    matchingAppointments.Add(appointment);
                }
            }
            return matchingAppointments;
        }
        public List<Appointment>? GetAppointmentsByPatientJmbg(string patientJmbg)
        {
            return ScheduleRepository.GetAppointmentsByPatientJmbg(patientJmbg);
        }
        public List<Appointment>? GetFinishedAppointmentsByPatientJmbg(string patientJmbg)
        {
            return ScheduleRepository.GetFinishedAppointmentsByPatientJmbg(patientJmbg);
        }

        public TimeSlot FindSlotForUrgentAppointmentWithin2Hours(Doctor.Specialty specialty, string patientJmbg,
            AppointmentType appointmentType, DateTime start, int duration)
        {
            return ScheduleRepository.FindSlotForUrgentAppointmentWithin2Hours(specialty, patientJmbg,
                appointmentType, start, duration);
        }

        public List<KeyValuePair<Appointment, int>> FindAppointmentsToPostpone(Doctor.Specialty specialty, int duration, Patient patient)
        {
            return ScheduleRepository.FindAppointmentsToPostpone(specialty, duration, patient);
        }
        public void PostponeAppointment(Appointment app)
        {
            ScheduleRepository.PostponeAppointment(app);
        }
        public void Save()
        {
            ScheduleRepository.Save();
        }



    }
}
