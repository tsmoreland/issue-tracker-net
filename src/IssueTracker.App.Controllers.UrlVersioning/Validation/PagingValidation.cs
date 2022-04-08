﻿//
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

using System.Web.Http.ModelBinding;

namespace IssueTracker.App.Controllers.UrlVersioning.Validation
{
    /// <summary>
    /// Paging validation methods
    /// </summary>
    public static class PagingValidation
    {
        /// <summary>
        /// Validates page number and size are non-negative
        /// </summary>
        public static bool ValidatePaging(ModelStateDictionary modelState, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                modelState.AddModelError("pageNumber", "must be greater than or equal to 1");
                return false;
            }
            else if (pageSize < 1)
            {
                modelState.AddModelError("pageSize", "must be greater than or equal to 1");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
