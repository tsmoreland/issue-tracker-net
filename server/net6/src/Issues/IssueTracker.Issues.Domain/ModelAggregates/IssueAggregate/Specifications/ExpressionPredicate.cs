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
using IssueTracker.Issues.Domain.ModelAggregates.Specifications;

namespace IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate.Specifications;

public sealed class ExpressionPredicate : IPredicateSpecification<Issue>
{
    /// <summary>
    /// Parses <paramref name="expression"/> into <see cref="Filter"/> or
    /// </summary>
    /// <param name="expression">string representation of a expression predicate</param>
    /// <exception cref="ArgumentException">
    /// if <paramref name="expression"/> is <see langword="null"/> or empty; of it does
    /// not represent a valid expression.
    /// </exception>
    public ExpressionPredicate(string expression)
    {
        if (expression is not { Length: > 0 })
        {
            throw new ArgumentException("Expression cannot be empty", nameof(expression));
        }

        Expression = expression;
    }

    /// <inheritdoc />
    public Expression<Func<Issue, bool>> Filter =>
        _ => true;

    /// <inheritdoc />
    public string Expression { get; }
}
