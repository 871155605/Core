using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Models
{
    public class GlobalResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public Object Data { get; set; }

        public static GlobalResponse Of() {
            return new GlobalResponse { Code = 1 };
        }
        public static GlobalResponse Of(Object data)
        {
            return new GlobalResponse { Code = 1, Data = data };
        }
        public static GlobalResponse Of(string message)
        {
            return new GlobalResponse { Code = 1, Message = message };
        }
        public static GlobalResponse Of(string message, Object data)
        {
            return new GlobalResponse { Code = 1, Message = message,Data = data };
        }
        public static GlobalResponse Of(int code)
        {
            return new GlobalResponse { Code = code };
        }
        public static GlobalResponse Of(int code,string message)
        {
            return new GlobalResponse { Code = code,Message = message};
        }
        public static GlobalResponse Of(int code, string message,object data)
        {
            return new GlobalResponse { Code = code, Message = message,Data = data };
        }
    }
}
