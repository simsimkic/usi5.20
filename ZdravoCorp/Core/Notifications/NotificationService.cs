using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Notifications.Repository;
using ZdravoCorp.Core.Utils.Doctor;
using ZdravoCorp.Core.Utils.Doctor.Repository;

namespace ZdravoCorp.Core.Notifications
{
    public class NotificationService
    {
        public NotificationRepository NotificationRepository;

        public NotificationService()
        {
            NotificationRepository = new NotificationRepository();
        }
        
        public List<Notification> Notifications()
        {
            return NotificationRepository.Notifications;
        }
        public void CreateNotification(string jmbg, string message)
        {
            NotificationRepository.CreateNotification(jmbg, message);
        }
        public void DeleteNotification(string id)
        {
            NotificationRepository.DeleteNotification(id);
        }
        public void Save()
        {
            NotificationRepository.Save();
        }
    }
}
