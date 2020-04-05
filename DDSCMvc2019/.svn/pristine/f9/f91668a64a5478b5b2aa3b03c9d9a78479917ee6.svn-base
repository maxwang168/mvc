using Entity.SYS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLibrary.ViewModel;

namespace PortalWeb.Models
{
    public class InternalVM : BaseVM
    {
        private List<TodoListBE> g_todoList;
        private List<NotifyRecBE> g_notificationList;
        public List<TodoListBE> TodoList
        {
            get
            {
                if (g_todoList == null)
                {
                    g_todoList = new List<TodoListBE>();
                }
                return g_todoList;
            }
            set
            {
                g_todoList = value;
            }
        }

        public List<NotifyRecBE> NotificationList
        {
            get
            {
                if (g_notificationList == null)
                {
                    g_notificationList = new List<NotifyRecBE>();
                }
                return g_notificationList;
            }
            set
            {
                g_notificationList = value;
            }
        }
    }
}