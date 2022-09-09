using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace UnitTestImplementation.Code
{
    public class HttpResponse
    {
        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get { return StatusCode == HttpStatusCode.OK; } }
    }
}
