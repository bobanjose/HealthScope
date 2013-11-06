using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthScope.Configuration
{
    public class Request
    {
        public Request()
        {
            Parameters=new NameValueCollection();
        }

        public NameValueCollection Parameters { get; private set; }
    }

    public class Response
    {
        public Response()
        {
            Data = new NameValueCollection();
        }

        public bool Passed { get; set; }
        
        public NameValueCollection Data { get; private set; }
    }

    public class CurrentContext
    {
        public CurrentContext()
        {
            Response = new Response();
            Request = new Request();
        }

        public Response Response { get; private set; }

        public Request Request{ get; private set; }
    }
}
