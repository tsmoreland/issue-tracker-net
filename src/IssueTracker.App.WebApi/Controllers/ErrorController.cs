//
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpPutAttribute = System.Web.Http.HttpPutAttribute;
using HttpDeleteAttribute = System.Web.Http.HttpDeleteAttribute;
using HttpHeadAttribute = System.Web.Http.HttpHeadAttribute;
using HttpOptionsAttribute = System.Web.Http.HttpOptionsAttribute;
using AcceptVerbsAttribute = System.Web.Http.AcceptVerbsAttribute;

namespace IssueTracker.WebApi.App.Controllers
{
    public sealed class ErrorController : Controller
    {

        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        [HttpHead]
        [HttpOptions]
        [AcceptVerbs("PATCH")]
        [ApiExplorerSettings(IgnoreApi=true)]
        public void EndpointNotFound()
        {
            //used by api controller
            //HttpRequestMessage requestMessage = (HttpRequestMessage)HttpContext.Items["MS_HttpRequestMessage"];

            HttpResponseBase response = HttpContext.Response;
            // TODO: content-type should follow accept type if possible
            response.ContentType = "application/json";
            response.StatusCode = 404;

        }
    }
}
