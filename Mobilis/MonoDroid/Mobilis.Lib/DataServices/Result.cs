using System;
using System.Collections.Generic;
using System.Text;

namespace Mobilis.Lib.DataServices
{

    /*The generic class can be used to represent the result of any asynchronous operation.
     * The result object either accepts the result (T) or an exception in the constructor,
     * depending on weather the call was successful or not.
     * It also has a method to check if the call was successful or not.*/

    public delegate void ResultCallback<T>(Result<T> result);
    public class Result<T>

    {
        public Exception Error { get; private set; }
        public T Value { get; private set;}
        //public int statusCode { get; set; }

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
