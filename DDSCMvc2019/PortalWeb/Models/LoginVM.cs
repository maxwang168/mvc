using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalWeb.Models
{
    public class LoginVM
    {
        #region Login

        public string TMId { get; set; }

        public string LoginId { get; set; }

        public string Password { get; set; }

        public string Captcha { get; set; }

        public string ValidateCode { get; set; }

        public string Message { get; set; }

        public List<SelectListItem> DeptList { get; set; }

        public string SelectedDeptId { get; set; }

        public string IsInternet { get; set; }

        public string LoginType { get; set; }

        public int RetryMax { get; set; }

        public string MailDNS { get; set; }

        #endregion
    }
}