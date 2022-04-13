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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Services;
using IssueTracker.App.Shared.Validation;
using IssueTracker.App.Soap.Model.Request;
using IssueTracker.App.Soap.Model.Response;
using IssueTracker.Core.Model;
using IssueTracker.Core.Projections;
using IssueTracker.Core.Requests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.App.Soap
{
    /// <summary>
    /// Summary description for IssueService
    /// </summary>
    [WebService(Namespace = AddressSettings.Namespace)]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class IssueService : WebServiceBase
    {
        private readonly IMediator _mediator;

        public IssueService()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>() ?? throw new InvalidOperationException("Missing dependency: IMediator");
        }

        /// <summary>
        /// Get All Issues 
        /// </summary>
        /// <param name="pageNumber">current page number</param>
        /// <param name="pageSize">number of items to return</param>
        /// <returns>Returns All issues</returns>
        /// <exception cref="NotImplementedException"></exception>
        [WebMethod]
        public IssueSummariesDto GetAllIssues(int pageNumber = 1, int pageSize = 10)
        {
            PagingValidation.ThrowIfPagingIsInvalid(pageNumber, pageSize);

            // very bad practice to use Result but at the time of writing I was having problems getting
            // async/await to work in this web service, this is hopefully a temporary measure
            IAsyncEnumerable<IssueSummaryProjection> projections = _mediator
                .Send(new GetAllIssuesRequest(pageNumber, pageSize), CancellationToken.None).Result;

            List<IssueSummaryDto> dataTransferObjects = IssueSummaryDto.MapFrom(projections).Result;
            return new IssueSummariesDto(dataTransferObjects);
        }

        /// <summary>
        /// Returns <see cref="IssueDto"/> matching <paramref name="id"/>
        /// </summary>
        /// <param name="id">id of the iss</param>
        /// <returns><see cref="IssueDto"/></returns>
        /// <exception cref="KeyNotFoundException">
        /// if no issue matching <paramref name="id"/> is found
        /// </exception>
        [WebMethod]
        public IssueDto GetIssueById(Guid id)
        {
            IssueDto dto = IssueDto.From(_mediator.Send(new FindIssueByIdRequest(id), CancellationToken.None).Result);
            return dto ?? throw new KeyNotFoundException($"no issue matching {id} found");
        }

        /// <summary>
        /// Adds a new issue 
        /// </summary>
        /// <param name="model">the issue to add</param>
        /// <returns>the added model</returns>
        [WebMethod]
        public IssueDto Add(AddIssueDto model)
        {
            Issue issue = _mediator.Send(new CreateIssueRequest(model.ToModel()), CancellationToken.None).Result;
            return IssueDto.From(issue);
        }
    }
}
