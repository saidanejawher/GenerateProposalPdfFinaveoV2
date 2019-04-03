using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinaveoV2_PDF.model
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExecutionError
    {
        /// <summary>
        /// 
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Message { get; set; }
    }

    [DataContract]
    [KnownType(typeof(ExecutionError))]
    public class AbstractExecutionError : IExecutionError
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Message { get; set; }
    }


    public class ExecutionError : AbstractExecutionError
    {
        /// <summary>
        /// 
        /// </summary>
        public ExecutionError() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ExecutionError(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
