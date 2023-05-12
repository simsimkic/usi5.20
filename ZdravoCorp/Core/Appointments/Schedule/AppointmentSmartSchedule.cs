using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Utils.Doctor;
using ZdravoCorp.Core.Utils;
using ZdravoCorp.Core.Room;
namespace ZdravoCorp.Core.Appointments
{
    using ZdravoCorp.Core.Room;
    public class AppointmentSmartSchedule
    {
        public ScheduleService ScheduleService;
        public string JmbgOfLogged;
        public PatientService PatientService;
        public RoomService RoomService;
        public DoctorService DoctorService;
        public AppointmentSmartSchedule(string jmbgOfLogged)
        {
            ScheduleService = new ScheduleService();
            JmbgOfLogged = jmbgOfLogged;
            PatientService = new PatientService();
            RoomService = new RoomService();
            DoctorService = new DoctorService();
        }
        public List<Appointment> GetAvailableAppointments(Doctor doctor, DateTime date, TimeSlot timeRange)//for single day
        {
            List<Appointment> availableAppointments = new List<Appointment>();

            DateTime startTime = timeRange.Start;
            DateTime endTime = timeRange.End;
            Patient patient = PatientService.FindByJmbg(JmbgOfLogged);

            for (DateTime currTime = startTime; currTime < endTime; currTime = currTime.AddMinutes(15))
            {
                DateTime foundTime = new DateTime(date.Year, date.Month, date.Day, currTime.Hour, currTime.Minute, currTime.Second);
                TimeSlot foundTimeSlot = new TimeSlot(foundTime, foundTime.AddMinutes(15));
                Room room = RoomService.FindFirstAvailableRoom(RoomType.ExaminationRoom, ScheduleService.Appointments(), foundTimeSlot);
                if (ScheduleService.IsAvailable(doctor,foundTimeSlot) && ScheduleService.IsAvailable(patient,foundTimeSlot)
                    && room != null)
                {
                    Appointment appointment = new Appointment(foundTimeSlot,AppointmentType.EXAMINATION,doctor.Jmbg,patient.Jmbg,AppointmentStatus.SCHEDULED,ScheduleService.ScheduleRepository.GenerateAppointmentId(),room.Id);
                    availableAppointments.Add(appointment);
                }
            }

            return availableAppointments;
        }

        public List<Appointment> GetAvailableAppointmentsForDateRange(Doctor doctor, DateTime endDate, TimeSlot timeRange)
        {
            List<Appointment> availableAppointments = new List<Appointment>();
            DateTime today=DateTime.Now;
            int minutes = DateTime.Now.Minute;
            int roundedHours = DateTime.Now.Hour;
            int roundedMinutes = ((int)Math.Ceiling(minutes / 15.0)) * 15;
            if (roundedMinutes == 60)
            {
                roundedMinutes = 0;
                if (roundedHours == 23)
                    roundedHours = 0;
                else
                    roundedHours++;
            }
            today = new DateTime(today.Year, today.Month, today.Day, roundedHours, roundedMinutes, 0);
            DateTime todayStart=new DateTime(today.Year, today.Month, today.Day,timeRange.Start.Hour,timeRange.Start.Minute,timeRange.Start.Second);
            DateTime todayEnd= new DateTime(today.Year, today.Month, today.Day, timeRange.End.Hour, timeRange.End.Minute, timeRange.End.Second);
            TimeSpan differenceStart = today - todayStart;
            TimeSpan differenceEnd = today - todayEnd;

            if (differenceStart.TotalSeconds>0 && differenceEnd.TotalSeconds<0)
            {
                TimeSlot todayTimeSlot = new TimeSlot(today, todayEnd);
                List<Appointment> appointmentsForDay = GetAvailableAppointments(doctor, today, todayTimeSlot);

                availableAppointments.AddRange(appointmentsForDay);
            }else if (differenceStart.TotalSeconds < 0) 
            {
                List<Appointment> appointmentsForDay = GetAvailableAppointments(doctor, today, timeRange);

                availableAppointments.AddRange(appointmentsForDay);
            }
            
            for (DateTime currDate = today.AddDays(1); currDate <= endDate; currDate = currDate.AddDays(1))
            {
                List<Appointment> appointmentsForDay = GetAvailableAppointments(doctor, currDate, timeRange);

                availableAppointments.AddRange(appointmentsForDay);
            }

            return availableAppointments;
        }

        public List<Appointment> GetAvailableAppointmentsDoctorPriority(Doctor doctor, TimeSlot timeRange)
        {
            List<Appointment> availableAppointments = new List<Appointment>();
            DateTime today = DateTime.Now;
            int minutes = DateTime.Now.Minute;
            int roundedHours = DateTime.Now.Hour;
            int roundedMinutes = ((int)Math.Ceiling(minutes / 15.0)) * 15;
            if (roundedMinutes == 60)
            {
                roundedMinutes = 0;
                if (roundedHours == 23)
                    roundedHours = 0;
                else
                    roundedHours++;
            }
            today = new DateTime(today.Year, today.Month, today.Day, roundedHours, roundedMinutes, 0);
            DateTime todayStart = new DateTime(today.Year, today.Month, today.Day, timeRange.Start.Hour, timeRange.Start.Minute, timeRange.Start.Second);
            DateTime todayEnd = new DateTime(today.Year, today.Month, today.Day, timeRange.End.Hour, timeRange.End.Minute, timeRange.End.Second);
            TimeSpan differenceStart = today - todayStart;
            TimeSpan differenceEnd = today - todayEnd;

            if (differenceStart.TotalSeconds > 0 && differenceEnd.TotalSeconds < 0)
            {
                TimeSlot todayTimeSlot = new TimeSlot(today, todayEnd);
                List<Appointment> appointmentsForDay = GetAvailableAppointments(doctor, today, todayTimeSlot);

                availableAppointments.AddRange(appointmentsForDay);
            }
            else if (differenceStart.TotalSeconds < 0)
            {
                List<Appointment> appointmentsForDay = GetAvailableAppointments(doctor, today, timeRange);

                availableAppointments.AddRange(appointmentsForDay);
            }
            
            DateTime currDate = today.AddDays(1);
            while (availableAppointments.Count() < 3)
            {
                List<Appointment> nextAppointments = GetAvailableAppointments(doctor, currDate, timeRange);
                foreach(Appointment appointment in nextAppointments)
                {
                    availableAppointments.Add(appointment);
                }
                currDate = currDate.AddDays(1);
            }
            return availableAppointments;
        }

        public List<Appointment> GetAvailableAppointmentsTimePriority(DateTime endDate, TimeSlot timeRange)
        {
            List<Appointment> availableAppointments = new List<Appointment>();
            DateTime today = DateTime.Now;
            int minutes = DateTime.Now.Minute;
            int roundedHours = DateTime.Now.Hour;
            int roundedMinutes = ((int)Math.Ceiling(minutes / 15.0)) * 15;
            if (roundedMinutes == 60)
            {
                roundedMinutes = 0;
                if (roundedHours == 23)
                    roundedHours = 0;
                else
                    roundedHours++;
            }
            today = new DateTime(today.Year, today.Month, today.Day, roundedHours, roundedMinutes, 0);
            DateTime todayStart = new DateTime(today.Year, today.Month, today.Day, timeRange.Start.Hour, timeRange.Start.Minute, timeRange.Start.Second);
            DateTime todayEnd = new DateTime(today.Year, today.Month, today.Day, timeRange.End.Hour, timeRange.End.Minute, timeRange.End.Second);
            TimeSpan differenceStart = today - todayStart;
            TimeSpan differenceEnd = today - todayEnd;

            if (differenceStart.TotalSeconds > 0 && differenceEnd.TotalSeconds < 0)
            {
                TimeSlot todayTimeSlot = new TimeSlot(today, todayEnd);
                foreach(Doctor doctor in DoctorService.Doctors())
                {
                    List<Appointment> appointmentsForDay = GetAvailableAppointments(doctor, today, todayTimeSlot);

                    availableAppointments.AddRange(appointmentsForDay);
                }
            }
            else if (differenceStart.TotalSeconds < 0) 
            {
                foreach(Doctor doctor in DoctorService.Doctors())
                {
                    List<Appointment> appointmentsForDay = GetAvailableAppointments(doctor, today, timeRange);

                    availableAppointments.AddRange(appointmentsForDay);
                }
            }
            
            if (availableAppointments.Count() == 0)
            {
                foreach(Doctor doctor in DoctorService.Doctors())
                {
                    for (DateTime currDate = today.AddDays(1); currDate <= endDate; currDate = currDate.AddDays(1))
                    {
                        List<Appointment> appointmentsForDay = GetAvailableAppointments(doctor, currDate, timeRange);

                        availableAppointments.AddRange(appointmentsForDay);
                    }
                    if (availableAppointments.Count() > 0)
                        return availableAppointments;
                }               
            }
            return availableAppointments;
        }
    }
}
