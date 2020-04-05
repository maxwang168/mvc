using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Service
{
    public class WCFService<T> : Service<T>
    {

        private ChannelFactory<T> _channelFactory = null;

        public WCFService(string pi_sEndPointConfigName)
        {
            _channelFactory = new ChannelFactory<T>(pi_sEndPointConfigName);
        }


        public override R Use<R>(Func<T, R> codeBlock)
        {
            IClientChannel proxy = (IClientChannel)_channelFactory.CreateChannel();

            bool success = false;
            R returnValue = default(R);
            try
            {
                returnValue = codeBlock((T)proxy);
                proxy.Close();
                success = true;
                return returnValue;
            }
            finally
            {
                if (!success)
                {
                    proxy.Abort();
                }
            }
        }


        public override void Use(Action<T> codeBlock)
        {
            IClientChannel proxy = (IClientChannel)_channelFactory.CreateChannel();
            bool success = false;
            try
            {
                codeBlock((T)proxy);
                proxy.Close();
                success = true;
            }
            finally
            {
                if (!success)
                {
                    proxy.Abort();
                }
            }
        }


    }
}
