using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IssueTracker.Shared.AspNetCore.ModelBinders;

/// <summary>
/// Model binder which can be used to convert a colleciton of comma separated values into an array of the expected type
/// </summary>
public sealed class ArrayModelBinder : IModelBinder
{
    /// <inheritdoc />
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (!bindingContext.ModelMetadata.IsEnumerableType)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        string value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

        if (string.IsNullOrWhiteSpace(value))
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        Type elementType = bindingContext.ModelType.GetTypeInfo().GetGenericArguments().First();
        TypeConverter converter = TypeDescriptor.GetConverter(elementType);

        object?[] values = value
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(v => converter.ConvertFromString(v.Trim()))
            .ToArray();

        Array? typedValues = Array.CreateInstance(elementType, values.Length);
        values.CopyTo(typedValues, 0);

        bindingContext.Result = ModelBindingResult.Success(typedValues);
        return Task.CompletedTask;
    }
}
