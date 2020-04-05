using System;

namespace PortalService.Contract.ViewModel
{
    [Serializable]
    public class AaUserModel
    {
        public string UserId { get; set; }
        public string OrgUuid { get; set; }
        public bool IsInternal { get; set; }
    }
}