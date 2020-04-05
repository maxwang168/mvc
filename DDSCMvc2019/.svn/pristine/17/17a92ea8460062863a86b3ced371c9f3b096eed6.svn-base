using System.ServiceModel;
using Entity.COM;

namespace PortalService.Contract
{
    [ServiceContract]
    public interface IComService
    {
        #region Common

        [OperationContract]
        CustIDBE CustIDQuery(string p_id);

        [OperationContract]
        string generateSN(string p_sn_key, string p_sn_date);
        #endregion
    }
}
