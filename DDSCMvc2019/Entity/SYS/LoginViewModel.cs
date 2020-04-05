using Entity.BAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.SYS
{
    public class LoginViewModel
    {
        public string TMId { get; set; }

        public string LoginId { get; set; }

        public string Password { get; set; }

        public string Captcha { get; set; }

        public string Message { get; set; }

        public int Ann_Count { get; set; }

        public List<AnnounceInfoBE> AnnounceInfoList { get; set; }

        public string Ann_Path { get; set; }
        public string Ann_File { get; set; }
        public string Ann_Url { get; set; }

        public string IsNew { get; set; }

        public string OrgId { get; set; }

        public string UserUuid { get; set; }

        public string Pwd3 { get; set; }

        public ChangePwdBE EditChangePwd { get; set; }

        public int PwdLimit { get; set; }
    }
}
