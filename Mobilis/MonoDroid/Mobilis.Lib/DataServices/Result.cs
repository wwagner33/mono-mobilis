using System;
using System.Collections.Generic;
using System.Text;

namespace Mobilis.Lib.DataServices
{
    /* Classe genérica usada para representar o resultado de uma operação asíncrona*/

    // Callback das chamadas ao WS.
    public delegate void ResultCallback<T>(Result<T> result);
    // Callback usado para as chamadas de áudio
    public delegate void AudioCallback(int blockId);
    // Callback usado para o ViewModel informar a View o fim de uma operação qualquer
    public delegate void NotifyView();

    public class Result<T>
    {
        public Exception Error { get; private set; }
        public T Value { get; private set;}
        public Result(T value)
        {
            Value = value;
        }

        public Result(Exception error)
        {
            Error = error;
        }
        public bool hasError()
        {
            return Error != null;
        }
    }
}
