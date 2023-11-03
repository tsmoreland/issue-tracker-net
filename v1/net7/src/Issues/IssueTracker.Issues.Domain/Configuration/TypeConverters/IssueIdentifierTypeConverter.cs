//
// Copyright (c) 2023 Terry Moreland
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

using System.ComponentModel;
using System.Globalization;
using IssueTracker.Issues.Domain.ModelAggregates.IssueAggregate;

namespace IssueTracker.Issues.Domain.Configuration.TypeConverters;

public sealed class IssueIdentifierTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || sourceType == typeof(IssueIdentifier) || base.CanConvertFrom(context, sourceType);
    }

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value switch
        {
            string @string => IssueIdentifier.TryConvert(@string, out IssueIdentifier? id) ? id : base.ConvertFrom(context, culture, value),
            IssueIdentifier issueId => issueId.ToString(),
            _ => base.ConvertFrom(context, culture, value)
        };
    }



    /// <inheritdoc />
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return value switch
        {
            string @string when destinationType == typeof(IssueIdentifier) =>
                IssueIdentifier.TryConvert(@string, out IssueIdentifier? id) ? id : base.ConvertTo(context, culture, value, destinationType),
            IssueIdentifier issueId when destinationType == typeof(string) =>
                issueId.ToString(),
            _ => base.ConvertTo(context, culture, value, destinationType),
        };
    }
}
