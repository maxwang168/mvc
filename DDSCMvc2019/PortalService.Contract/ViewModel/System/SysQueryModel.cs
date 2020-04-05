using System;

namespace PortalService.Contract.ViewModel.System
{
    [Serializable]
    public class SysQueryModel
    {
        public bool IsUpdate { get; set; }

        public string QueryData { get; set; }
    }
}