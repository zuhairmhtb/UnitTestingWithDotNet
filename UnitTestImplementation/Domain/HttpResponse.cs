using System.Net;

namespace UnitTestImplementation.Domain
{
    public class HttpResponse
    {
        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get { return StatusCode == HttpStatusCode.OK; } }
    }
}
