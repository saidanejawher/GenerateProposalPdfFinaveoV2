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
    /// <typeparam name="T"></typeparam>
    public interface IMethodResult<T>
    {
        /// <summary>
        /// Indicates wether the method executed with errors or not
        /// </summary>
        bool IsExecutionOk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IExecutionError Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        T Result { get; set; }
    }


    [DataContract(Name = "RtoWebApiResponse", Namespace = "")]
    [KnownType(typeof(MethodResult<bool>))]
    [KnownType(typeof(MethodResult<decimal>))]
    [KnownType(typeof(MethodResult<string>))]
    [KnownType(typeof(MethodResult<int>))]
    [KnownType(typeof(MethodResult<double>))]
    [KnownType(typeof(MethodResult<float>))]
    [KnownType(typeof(MethodResult<DateTime>))]
    [KnownType(typeof(MethodResult<bool?>))]
    [KnownType(typeof(MethodResult<decimal?>))]
    [KnownType(typeof(MethodResult<int?>))]
    [KnownType(typeof(MethodResult<double?>))]
    [KnownType(typeof(MethodResult<float?>))]
    [KnownType(typeof(MethodResult<DateTime?>))]
    [KnownType(typeof(MethodResult<ManagementFeesDto>))]
    [KnownType(typeof(MethodResult<ProposalDocumentDto>))]
    [KnownType(typeof(MethodResult<ProposalAttachmentDto>))]
    public abstract class AbstractMethodResult<T> : IMethodResult<T>
    {
        [DataMember]
        public bool IsExecutionOk { get; set; }
        [DataMember]
        public IExecutionError Error { get; set; }
        [DataMember]
        public T Result { get; set; }
    }

    public class MethodResult<T> : AbstractMethodResult<T>
    {
        /// <summary>
        /// dafault constructor
        /// </summary>
        public MethodResult()
        {
            // defaumlt value is set to "true"
            IsExecutionOk = true;
        }
    }
}
