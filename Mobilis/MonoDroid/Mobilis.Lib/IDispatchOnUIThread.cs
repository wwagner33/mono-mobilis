using System;

namespace Mobilis.Lib
{
    // Interface que exibe operação de acesso a thread principal
    public interface IDispatchOnUIThread
    {
        void invoke(Action action);
    }
}