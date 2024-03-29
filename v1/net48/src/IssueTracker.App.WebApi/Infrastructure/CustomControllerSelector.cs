﻿// 
// Copyright (c) 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace IssueTracker.App.WebApi.Infrastructure
{
    public sealed class CustomControllerSelector : DefaultHttpControllerSelector
    {
        /// <inheritdoc />
        public CustomControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        /// <inheritdoc />
        public override string GetControllerName(HttpRequestMessage request)
        {
            try
            {
                string controllerName = base.GetControllerName(request);
                return controllerName;
            }
            catch (Exception)
            {
                return "Issues";
            }
        }

        /// <inheritdoc />
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            try
            {
                return base.SelectController(request);
            }
            catch (HttpResponseException ex) when (ex.Response.StatusCode == HttpStatusCode.NotFound)
            {
                IDictionary<string, object> routeValues = request.GetRouteData().Values;
                routeValues["controller"] = "ErrorApi";
                routeValues["action"] = nameof(Controllers.ErrorApiController.EndpointNotFound);
                return base.SelectController(request);
            }
        }
    }
}
