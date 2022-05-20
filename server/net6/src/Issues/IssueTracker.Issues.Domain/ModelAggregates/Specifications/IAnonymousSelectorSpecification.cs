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

using System.Linq.Expressions;
using IssueTracker.Issues.Domain.DataContracts;

namespace IssueTracker.Issues.Domain.ModelAggregates.Specifications;

/// <summary>
/// intend for use in IQueryable where value tuple cannot be used;
/// to accomplish this <see cref="Select"/> should be used against the IQueryable
/// and once converted to IEnumerable <see cref="ToConcrete"/> to
/// translate to final type
/// </summary>
/// <typeparam name="TEntity">Entity to choose elements from</typeparam>
/// <typeparam name="T">final output of the selector</typeparam>
public interface IAnonymousSelectorSpecification<TEntity, out T>
    where TEntity : Entity
{

    public Expression<Func<TEntity, dynamic>> Select { get; }

    public T ToConcrete(dynamic anonysmous);
}
