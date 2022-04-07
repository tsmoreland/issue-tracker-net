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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Requests;
using IssueTracker.Data.Abstractions;
using IssueTracker.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace IssueTracker.App.Test;

public sealed class MediatorTest
{
    Mock<IIssueRepository> _repository = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = new Mock<IIssueRepository>();
    }

    [Test]
    public void SendGetAllIssuesRequest_DoesNotThrow_WhenMediatorIsConfigured()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<IIssueRepository>(_repository.Object);
        services.AddIssueServices();
        IServiceProvider provider = services.BuildServiceProvider();
        IMediator mediator = provider.GetRequiredService<IMediator>();

        GetAllIssuesRequest request = new(1, 10);
        Assert.DoesNotThrowAsync(() => mediator.Send(request, CancellationToken.None));
    }

}
