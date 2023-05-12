using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ZdravoCorp.Core.Utils;
using ZdravoCorp.Core.Utils.Doctor;
using ZdravoCorp.Core.Utils.Doctor.Repository;
using System.Windows.Controls;
using System.Windows;
using System.Collections;



namespace ZdravoCorp.Core.Appointments
{
    using System.Data;
    using System.Windows.Documents;
    using ZdravoCorp.Core.Room;
    using static ZdravoCorp.Core.Utils.Doctor.Doctor;

    public class Schedule
    {
        public List<Appointment> Appointments { get; set; }

        public Schedule()
        {
            GetAllAppointments();
            foreach (Appointment appointment in Appointments)
            {

                if (appointment.Status == AppointmentStatus.SCHEDULED && DateTime.Now > appointment.TimeSlot.End)
                {
                    appointment.Status = AppointmentStatus.CANCELED;
                }
            }
            Save();
        }

        public Schedule(List<Appointment> appointments)
        {
            Appointments = appointments;
        }

        public void CreateAppointment(string doctorJmbg, string patientJmbg, TimeSlot appointmentDuration,AppointmentType type,string roomId)
        {
            Appointments.Add(new Appointment(appointmentDuration,type, doctorJmbg, patientJmbg,AppointmentStatus.SCHEDULED, GenerateAppointmentId(), roomId));
            Save();
        }
        
        public void CreateAppointment(string doctorJmbg, string patientJmbg, TimeSlot appointmentDuration,string roomId)
        {
            Appointments.Add(new Appointment(appointmentDuration, AppointmentType.EXAMINATION, doctorJmbg, patientJmbg, AppointmentStatus.SCHEDULED,GenerateAppointmentId(), roomId));
            Save();
        }

        public void EditAppointment(string id,string newId, string patientJmbg, TimeSlot appointmentDuration, AppointmentType type, AppointmentStatus status,string roomId)
        {
            Appointment appointment = FindAppointmentById(id);
            appointment.Id = newId;
            appointment.JmbgPatient = patientJmbg;
            appointment.TimeSlot = appointmentDuration;
            appointment.Type = type;
            appointment.Status = status;
            appointment.RoomId = roomId;
            Save();
        }


        public void CancelAppointment(Appointment appointmentForCancel)
        {
            foreach (var appointment in Appointments.Where(appointment => appointmentForCancel.Id == appointment.Id))
            {
                appointment.Status = AppointmentStatus.CANCELED;
            }
            Save();
        }

        public void FinishAppointment(Appointment appointmentForFinish)
        {
            foreach (var appointment in Appointments.Where(appointment => appointmentForFinish.Id == appointment.Id))
            {
                appointment.Status = AppointmentStatus.FINISHED;
            }
            Save();
        }

        public void ReadyForAppointment(Appointment readyAppointment)
        {
            foreach (var appointment in Appointments.Where(appointment => readyAppointment.Id == appointment.Id))
            {
                appointment.Status = AppointmentStatus.READY;
            }
            Save();
        }

        public bool IsAppointmentInNext15Minutes(Appointment appointment)
        {
            if(appointment.TimeSlot.Start <  DateTime.Now || appointment.TimeSlot.Start > DateTime.Now.AddMinutes(15))
            {
                return false;
            }
            return true;
        }

        public string GenerateRandomId()
        {
            Random random = new Random();
            return random.Next(1, 100000).ToString();
        }

        public string GenerateAppointmentId()
        {
            while (true)
            {
                string appointmentId = GenerateRandomId();

                if (!(FindAppointmentById(appointmentId) == null))
                {
                    continue;
                }
                return appointmentId;
            }
        }

        public bool HasPatientBeenExaminatedByDoctor(Doctor doctor, Patient patient)
        {
            return Appointments.Any(appointment => appointment.JmbgDoctor == doctor.Jmbg && appointment.JmbgPatient == patient.Jmbg && appointment.Status == AppointmentStatus.FINISHED);
        }

        public bool IsAvailable(Doctor doctor, TimeSlot duration)
        {
            List<Appointment>? doctorAppointments = GetAppointmentsByDoctorJmbg(doctor.Jmbg);
            foreach (Appointment appointment in doctorAppointments)
            {
                if (appointment.TimeSlot.OverlapsWith(duration) && (appointment.Status != AppointmentStatus.CANCELED))
                {
                    return false;
                }
            }
            return true;
        }
        public List<Appointment> SortAppointmentsByStartTime(List<Appointment> appointments)
        {
            var sortedAppointments = appointments.OrderBy(a => a.TimeSlot.Start).ToList();
            return sortedAppointments;
        }
        public List<Appointment> SortAppointmentsByDoctorJmbg(List<Appointment> appointments)
        {
            var sortedAppointments = appointments.OrderBy(a => a.JmbgDoctor).ToList();
            return sortedAppointments;

        }
        public List<Appointment> SortAppointmentsByDoctorSpecialty(List<Appointment> appointments, Dictionary<string, Doctor.Specialty> doctorSpecialties)
        {
            Dictionary<string, Doctor.Specialty> appointmentDoctorSpecialties = new Dictionary<string, Doctor.Specialty>();
            foreach (Appointment appointment in appointments)
            {
                if (!appointmentDoctorSpecialties.ContainsKey(appointment.JmbgDoctor))
                {
                    appointmentDoctorSpecialties[appointment.JmbgDoctor] = doctorSpecialties[appointment.JmbgDoctor];
                }
            }

            List<Appointment> sortedAppointments = appointments.OrderBy(appointment => appointmentDoctorSpecialties[appointment.JmbgDoctor]).ToList();

            return sortedAppointments;
        }
        public bool IsAvailable(Patient patient, TimeSlot duration)
        {
            List<Appointment>? patientAppointments = GetAppointmentsByPatientJmbg(patient.Jmbg);
            foreach (Appointment appointment in patientAppointments)
            {
                if (appointment.TimeSlot.OverlapsWith(duration) && (appointment.Status != AppointmentStatus.CANCELED))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsInThreeDayRange(DateTime startDate, Appointment appointment)
        {
            DateTime endDate = startDate.AddDays(3);
            return (appointment.TimeSlot.Start == startDate) ||
                   (appointment.TimeSlot.Start >= startDate && appointment.TimeSlot.Start <= endDate);
        }

        public Appointment? FindAppointment(string doctorJmbg, TimeSlot timeSlot, string patientJmbg)
        {
            foreach (Appointment appointment in Appointments)
            {
                if (appointment.JmbgDoctor == doctorJmbg && appointment.JmbgPatient == patientJmbg && appointment.TimeSlot.Start == timeSlot.Start && appointment.TimeSlot.End == timeSlot.End)
                {
                    return appointment;
                }
            }
            return null;
        }

        public List<Appointment>? FindAllApointmetsWithDifferentId(string id)
        {
            return Appointments.Where(appointment => appointment.Id != id).ToList();
        }

        public Appointment? FindAppointmentById(string id)
        {
            return Appointments.FirstOrDefault(appointment => appointment.Id == id);
        }

        public List<Appointment>? GetAppointmentsByDate(Doctor doctor, DateTime startDate)
        {
            return GetAppointmentsByDoctorJmbg(doctor.Jmbg).Where(appointment => IsInThreeDayRange(startDate, appointment)).ToList();
        }

        public List<Appointment>? GetAppointmentsByDoctorJmbg(string doctorJmbg)
        {
            return Appointments.Where(appointment => appointment.JmbgDoctor == doctorJmbg).ToList();
        }

        public List<Appointment>? GetAppointmentsByPatientJmbg(string patientJmbg)
        {
            return Appointments.Where(appointment => (appointment.JmbgPatient == patientJmbg)).ToList();
        }
        public List<Appointment>? GetFinishedAppointmentsByPatientJmbg(string patientJmbg)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (Appointment appointment in Appointments)
            {
                if(appointment.JmbgPatient==patientJmbg && appointment.Status == AppointmentStatus.FINISHED)
                {
                    appointments.Add(appointment);
                }
            }
            return appointments;
        }

        public (TimeSlot, string) IsDoctorAvailableForUrgentAppointment(Doctor doc, DateTime start, int duration, AppointmentType appointmentType)
        {
            RoomService roomService = new RoomService();
            DateTime temp = start;
            RoomType rt = appointmentType == AppointmentType.OPERATION ? RoomType.OperationRoom : RoomType.ExaminationRoom;
            while(temp < start.AddHours(2))
            {
                TimeSlot ts = new TimeSlot(temp, temp.AddMinutes(duration));
                Room room = roomService.FindFirstAvailableRoom(rt, Appointments, ts);
                if (IsAvailable(doc, ts))
                {
                    return (ts, room.Id);
                }
                temp = temp.AddMinutes(1);
            }
            return (null, null);
        }

        public TimeSlot FindSlotForUrgentAppointmentWithin2Hours(Doctor.Specialty specialty, string patientJmbg,
            AppointmentType appointmentType, DateTime start, int duration)
        {
            DoctorService doctorService = new DoctorService();
            foreach (Doctor doc in doctorService.Doctors())
            {
                if (doc.Specialization == specialty)
                {
                    (TimeSlot ts, string room) = IsDoctorAvailableForUrgentAppointment((Doctor)doc, start, duration, appointmentType);

                    if (ts != null) {
                        CreateAppointment(doc.Jmbg, patientJmbg, ts, appointmentType, room);
                        return ts;
                    }
                }
            }
            return null;
        }
        public bool IsDoctorOnAppointmentCorrectSpecialty(Doctor.Specialty specialty, Appointment app)
        {
            DoctorService doctorService = new DoctorService();
            return doctorService.FindByJmbg(app.JmbgDoctor).Specialization == specialty;
        }

        public bool IsAppointmentDurationCompatible(Appointment app, int duration) 
        {
            return (app.TimeSlot.End - app.TimeSlot.Start).Minutes >= duration;
        }

        public int CalculateMinMinutesUntilDoctorAvailable(Appointment app, Patient patient)
        {
            DateTime tempTime = DateTime.Now.AddHours(2);
            DoctorService doctorService = new DoctorService();

            while (true)
            {
                Doctor doc = FindNextAvailableDoctorForAppointmentPostpone(doctorService, app, tempTime, patient);
                if (doc != null)
                {
                    TimeSpan difference = tempTime - DateTime.Now;
                    return (int)difference.TotalMinutes;
                }
                tempTime = tempTime.AddMinutes(1);
            }
        }
        public bool HasAvailableRoomToPostpone(RoomType roomType, TimeSlot timeSlot)
        {
            RoomService roomService = new RoomService();
            return roomService.FindFirstAvailableRoom(roomType, Appointments, timeSlot) != null;
        }
        public Doctor FindNextAvailableDoctorForAppointmentPostpone(DoctorService doctorService, Appointment app, DateTime tempTime, Patient patient)
        {
            RoomService roomService = new RoomService();
            TimeSlot ts = GetPostponedAppointmentTimeSlot(tempTime, app);
            foreach (Doctor doc in doctorService.Doctors())
            {
                if (!IsDoctorOnAppointmentCorrectSpecialty(doc.Specialization, app))
                {
                    continue;
                }
                if (!IsAvailable(doc, ts))
                {
                    continue;
                }
                if (!IsAvailable(patient, ts)){
                    continue;
                }
                if(!HasAvailableRoomToPostpone(roomService.FindById(app.RoomId).Type, ts))
                {
                    continue;
                }
                return doc;
            }
            return null;
        }

        public TimeSlot GetPostponedAppointmentTimeSlot(DateTime tempTime, Appointment app)
        {
            return new TimeSlot(tempTime, tempTime.AddMinutes((app.TimeSlot.End - app.TimeSlot.Start).Minutes));
        }

        public TimeSlot GetTimeSlotForPostponedAppointment(Appointment app)
        {
            DoctorService doctorService = new DoctorService();
            Doctor doc = doctorService.FindByJmbg(app.JmbgDoctor);
            DateTime tempTime = app.TimeSlot.Start;
            while (true)
            {
                TimeSlot ts = new TimeSlot(tempTime, tempTime.AddMinutes((app.TimeSlot.End - app.TimeSlot.Start).Minutes));
                if (IsAvailable(doc, ts))
                {
                    return ts;
                }
                tempTime = tempTime.AddMinutes(1);
            }
        }

        public void PostponeAppointment(Appointment app)
        {
            RoomService roomService = new RoomService();
            CancelAppointment(app);
            TimeSlot postponeTimeSlot = GetTimeSlotForPostponedAppointment(app);
            
            Room room = roomService.FindFirstAvailableRoom(roomService.FindById(app.RoomId).Type, Appointments, postponeTimeSlot);
            CreateAppointment(app.JmbgDoctor, app.JmbgPatient, postponeTimeSlot, app.Type, room.Id);
        }

        public bool IsAppointmentValidToPostpone(Appointment appointment, Doctor.Specialty specialty, int duration)
        {
            if (appointment.Status != AppointmentStatus.SCHEDULED)
            {
                return false;
            }

            if (!IsDoctorOnAppointmentCorrectSpecialty(specialty, appointment))
            {
                return false;
            }

            if (appointment.TimeSlot.Start < DateTime.Now || appointment.TimeSlot.Start > DateTime.Now.AddHours(2))
            {
                return false;
            }

            if (!IsAppointmentDurationCompatible(appointment, duration))
            {
                return false;
            }

            return true;
        }

        public List<Appointment> GetValidAppointmentsToPostpone(Doctor.Specialty specialty, int duration)
        {
            List<Appointment> validAppointments = new List<Appointment>();

            foreach (Appointment appointment in Appointments)
            {
                if (IsAppointmentValidToPostpone(appointment, specialty, duration))
                {
                    validAppointments.Add(appointment);
                }
            }

            return validAppointments;
        }

        private List<KeyValuePair<Appointment, int>> SortAppointmentsByPostponeMinutes(List<Appointment> appointments, Patient patient)
        {
            List<KeyValuePair<Appointment, int>> sortedAppointments = new List<KeyValuePair<Appointment, int>>();

            foreach (Appointment appointment in appointments)
            {
                int minutesToPostpone = CalculateMinMinutesUntilDoctorAvailable(appointment, patient);
                sortedAppointments.Add(new KeyValuePair<Appointment, int>(appointment, minutesToPostpone));
            }

            sortedAppointments.Sort((x, y) => x.Value.CompareTo(y.Value));

            return sortedAppointments;
        }

        private void FilterAppointmentsForPostponeThreshold(List<KeyValuePair<Appointment, int>> sortedAppointments)
        {
            if (sortedAppointments.Count > 5)
            {
                sortedAppointments.RemoveRange(0, sortedAppointments.Count - 5);
            }
        }

        public List<KeyValuePair<Appointment, int>> FindAppointmentsToPostpone(Doctor.Specialty specialty, int duration, Patient patient)
        {

            List<Appointment> validAppointments = GetValidAppointmentsToPostpone(specialty, duration);
            List<KeyValuePair<Appointment, int>> sortedAppointments = SortAppointmentsByPostponeMinutes(validAppointments, patient);
            FilterAppointmentsForPostponeThreshold(sortedAppointments);

            return sortedAppointments;
        }
       
        public void GetAllAppointments()
        {
            Appointments = JsonConvert.DeserializeObject<List<Appointment>>(File.ReadAllText("../../../Data/appointments.json"));
        }

        public void Save()
        {
            File.WriteAllText("../../../Data/appointments.json", JsonConvert.SerializeObject(Appointments, Formatting.Indented));
        }
    }
}
