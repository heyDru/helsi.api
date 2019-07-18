using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models.ServiceReponses
{

    public class ServiceBaseResult<T> where T : struct, IConvertible
    {
        public virtual T OperationStatus { get; protected set; }
        public string Message { get; protected set; }

        public ServiceBaseResult(T operationStatus, string message)
        {
            OperationStatus = operationStatus;
            Message = message;
        }
    }

    public class ServiceBaseResult<T, TO>: ServiceBaseResult<T> where T : struct, IConvertible
    {
        public virtual TO ResultObject { get; protected set; }

        public ServiceBaseResult(T operationStatus, string message, TO resultObject = default(TO)) :base(operationStatus,message)
        {
            ResultObject = resultObject;
        }
    }
}
