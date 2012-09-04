using Mobilis.Lib;
using System;
using System.Windows;
namespace Mobilis
{
    public class DispatchAdapter : IDispatchOnUIThread
    {
        public void invoke(Action action)
        {
            Deployment.Current.Dispatcher.BeginInvoke(action);
        }
    }
}
