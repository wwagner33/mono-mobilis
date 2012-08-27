using System;

namespace Mobilis.Lib
{
    public interface IDispatchOnUIThread
    {
        void invoke(Action action);
    }
}