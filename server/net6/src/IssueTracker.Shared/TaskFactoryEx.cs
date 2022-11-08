//
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
//

namespace IssueTracker.Shared;

internal class TaskFactoryEx
{
    public static Task StartNew(
        object? state,
        Action<object?> action,
        CancellationToken cancellationToken = default,
        TaskCreationOptions taskCreationOptions = TaskCreationOptions.DenyChildAttach) =>
        Task.Factory.StartNew(action, state, cancellationToken, taskCreationOptions, TaskScheduler.Default);

    public static Task StartNew(
        object? state,
        Func<object?, Task> function,
        CancellationToken cancellationToken = default,
        TaskCreationOptions taskCreationOptions = TaskCreationOptions.DenyChildAttach) =>
        Task.Factory.StartNew(function, state, cancellationToken, taskCreationOptions, TaskScheduler.Default).Unwrap();

    public static Task StartNew<T>(
        T state,
        Action<T> action,
        CancellationToken cancellationToken = default,
        TaskCreationOptions taskCreationOptions = TaskCreationOptions.DenyChildAttach) =>
        Task.Factory
            .StartNew(static state =>
                {
                    (T argument, Action<T> action) = ((T, Action<T>))state!;
                    action(argument);
                },
                (state, action),
                cancellationToken,
                taskCreationOptions,
                TaskScheduler.Default);

    public static Task StartNew<T>(
        T state,
        Func<T, Task> function,
        CancellationToken cancellationToken = default,
        TaskCreationOptions taskCreationOptions = TaskCreationOptions.DenyChildAttach) =>
        Task.Factory
            .StartNew(static state =>
            {
                (T argument, Func<T, Task> function) = ((T, Func<T, Task>))state!;
                return function(argument);
            },
            (state, function),
            cancellationToken,
            taskCreationOptions,
            TaskScheduler.Default)
            .Unwrap();

    public static Task<TResult> StartNew<TState, TResult>(
        TState state,
        Func<TState, Task<TResult>> function,
        CancellationToken cancellationToken = default,
        TaskCreationOptions taskCreationOptions = TaskCreationOptions.DenyChildAttach) =>
        Task.Factory
            .StartNew(static state =>
            {
                (TState argument, Func<TState, Task<TResult>> function) = ((TState, Func<TState, Task<TResult>>))state!;
                return function(argument);
            },
            (state, function),
            cancellationToken,
            taskCreationOptions,
            TaskScheduler.Default)
            .Unwrap();

    public static Task<TResult> StartNew<TState, TResult>(
        TState state,
        Func<TState, Task<TResult>> function,
        CancellationToken cancellationToken,
        TaskCreationOptions taskCreationOptions,
        TaskScheduler scheduler) =>
        Task.Factory
            .StartNew(static state =>
            {
                (TState argument, Func<TState, Task<TResult>> function) = ((TState, Func<TState, Task<TResult>>))state!;
                return function(argument);
            },
            (state, function),
            cancellationToken,
            taskCreationOptions,
            scheduler)
            .Unwrap();
}
