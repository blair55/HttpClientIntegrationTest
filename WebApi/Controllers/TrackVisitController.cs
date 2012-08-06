using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class TrackVisitController : ApiController
    {
        /// <summary>
        /// uri: /api/trackvisit
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            var cookie = new HttpCookie("VisitId")
            {
                Value = Guid.NewGuid().ToString(),
                Expires = DateTime.Now.AddDays(7)
            };

            HttpContext.Current.Response.Cookies.Add(cookie);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}