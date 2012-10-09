using System;
using System.Collections.Generic;
using System.Text;

namespace Mobilis.Lib.DataServices
{

    /* Classe genérica usada para representar o resultado de uma operação asíncrona*/
    public delegate void ResultCallback<T>(Result<T> result);
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
