using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Models
{
    public class ClientResponse<T>
    {
        public static readonly string DefaultErrorMsg = "Sorry, your request could not be completed.";
        public static readonly string DefaultSuccessMsg = "Request completed succesfully.";
        public static readonly int DefaultSuccessCode = 0;
        public ClientResponse()
        {
            SetDefaults();
        }
        public ClientResponse(HttpStatusCode statusCode)
        {
            SetDefaults((int)statusCode);
        }

        public int Code { get; set; }
        public string Title { get; set; }
        public List<string> Messages { get; set; }
        public Exception? SystemException { get; set; }
        public string StackTrace { get; set; }
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        private void SetDefaults(int code = 0)
        {
            Code = code;
            Title = (code == 0) ? DefaultSuccessMsg : DefaultErrorMsg;
            Messages = new List<string>();
            IsSuccess = code == 0;
            SystemException = null;
        }

        public void CopyResponse<TData>(ClientResponse<TData> clientResponse)
        {
            Code = clientResponse.Code;
            Title = clientResponse.Title;
            Messages = clientResponse.Messages;
            IsSuccess = clientResponse.IsSuccess;
            SystemException = clientResponse.SystemException;
            StackTrace = clientResponse.StackTrace;
            StatusCode = clientResponse.StatusCode;
        }
    }
}
