using System.Net;

namespace Store.App.Crosscutting.Commom.ViewModel
{
    public class RequestResponseVM
    {
        public string TextResponse { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    }
}
