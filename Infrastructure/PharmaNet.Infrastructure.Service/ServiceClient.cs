using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PharmaNet.Infrastructure.Service
{
    public class ServiceClient<T>
    {
        public void CallService(Action<T> serviceCall)
        {
            using (ChannelFactory<T> channelFactory = new ChannelFactory<T>())
            {
                channelFactory.Open();
                serviceCall(channelFactory.CreateChannel());
                channelFactory.Close();
            }
        }

        public R CallService<R>(Func<T,R> serviceCall)
        {
            using (ChannelFactory<T> channelFactory = new ChannelFactory<T>())
            {
                channelFactory.Open();
                R result = serviceCall(channelFactory.CreateChannel());
                channelFactory.Close();
                return result;
            }
        }
    }
}
