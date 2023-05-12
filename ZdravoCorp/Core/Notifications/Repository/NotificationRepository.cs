using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Appointments;
using ZdravoCorp.Core.Utils.Doctor;

namespace ZdravoCorp.Core.Notifications.Repository
{
    public class NotificationRepository
    {
        public List<Notification> Notifications { get; set; }

        public NotificationRepository() 
        {
            GetAllNotifications();
        }

        public void GetAllNotifications()
        {
            Notifications = JsonConvert.DeserializeObject<List<Notification>>(File.ReadAllText("../../../Data/notifications.json"));
        }

        public string GenerateRandomId()
        {
            Random random = new Random();
            return random.Next(1, 100000).ToString();
        }

        public string GenerateNotificationId()
        {
            while (true)
            {
                string notificationId = GenerateRandomId();

                if (!(FindNotificationById(notificationId) == null))
                {
                    continue;
                }
                return notificationId;
            }
        }

        public void CreateNotification(string jmbg, string message)
        {
            Notifications.Add(new Notification(GenerateNotificationId(), jmbg, message));
            Save();
        }

        public Notification? FindNotificationById(string id)
        {
            return Notifications.FirstOrDefault(notification => notification.Id == id);
        }

        public void DeleteNotification(string id)
        {
            Notifications.Remove(FindNotificationById(id));
            Save();
        }

        public void Save()
        {
            File.WriteAllText("../../../Data/notifications.json", JsonConvert.SerializeObject(Notifications, Formatting.Indented));
        }

    }
}
