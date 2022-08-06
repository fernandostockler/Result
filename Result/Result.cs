namespace Result;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

/// <summary>
/// Represents the result of a operation: T | Exception
/// </summary>
/// <typeparam name="T">Bound value type</typeparam>
public readonly record struct Result<T>
{
    ResultStatus Status { get; init; }

    [MaybeNull()] T Value { get; init; }

    [MaybeNull()] Exception Exception { get; init; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="value">Value</param>
    public Result(T value)
    {
        Value = value;
        Status = ResultStatus.Success;
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="e"><see cref="Exception"/></param>
    public Result(Exception e)
    {
        Exception = e;
        Status = ResultStatus.Failure;
    }

    /// <summary>
    /// Implicit conversion from T to Result{T}
    /// </summary>
    /// <param name="value">Value</param>
    [Pure]
    public static implicit operator Result<T>(T value) => new(value);

    /// <summary>
    /// If it is a successful result, execute the Success action and return <see langword="true"/>, otherwise return <see langword="false"/>.
    /// </summary>
    /// <param name="Success">The action to be executed in case the operation is successful.</param>
    /// <returns><see langword="true"/> If it is a successful result, otherwise <see langword="false"/>.</returns>
    public bool CaseSuccess(Action<T> Success)
    {
        if (Status is ResultStatus.Success)
        {
            Success(Value!);
            return true;
        }
        else // ResultStatus.Failure
            return false;
    }

    /// <summary>
    /// If it is a successful result, execute the Success function, otherwise execute the defaultValue function.
    /// </summary>
    /// <param name="func"></param>
    /// <param name="defaultValue"></param>
    /// <returns>A <typeparamref name="T"/> value.</returns>
    public R CaseSuccess<R>(Func<T, R> func, Func<Exception, R> defaultValue) => Status is ResultStatus.Success
        ? func(Value!)
        : defaultValue(Exception!);

    /// <summary>
    /// If it is a failed result, execute the Failure action and return <see langword="true"/>, otherwise return <see langword="false"/>.
    /// </summary>
    /// <param name="Failure">The action to be executed in case the operation fail.</param>
    /// <returns><see langword="true"/> If the result is a failure, otherwise <see langword="false"/>.</returns>
    public bool CaseFailure(Action<Exception> Failure)
    {
        if (Status is ResultStatus.Failure)
        {
            Failure(Exception!);
            return true;
        }
        else // ResultStatus.Success
            return false;
    }

    /// <summary>
    /// If it is a failed result, execute a function with the exception and return the result of the function, otherwise return the Value.
    /// </summary>
    /// <param name="func">The function to run in case of failure.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    [Pure]
    public T CaseFailure(Func<Exception, T> func) => Status is ResultStatus.Failure
        ? func(Exception!)
        : Value!;

    /// <summary>
    /// If it is a successful result, it returns the value of the Success function, otherwise it returns the value of the Failure function.
    /// </summary>
    /// <typeparam name="R">The result type: <typeparamref name="T"/> | <see cref="Exception"/>.</typeparam>
    /// <param name="Success">The function to run if it is a successful result.</param>
    /// <param name="Failure">The function to run if it is a failed result.</param>
    /// <returns>A value of type <typeparamref name="R"/>.</returns>
    [Pure]
    public R Match<R>(Func<T, R> Success, Func<Exception, R> Failure)
    => Status is ResultStatus.Success
        ? Success(Value!)
        : Failure(Exception!);

    /// <summary>
    /// If it is a successful result, it executes the Success action, otherwise it executes the Failure action.
    /// </summary>
    /// <param name="Success">The action to run if it is a successful result.</param>
    /// <param name="Failure">The action to run if it is a failed result.</param>
    public void Match(Action<T> Success, Action<Exception> Failure)
    {
        if (Status is ResultStatus.Success)
            Success(Value!);
        else
            Failure(Exception!);
    }

    /// <summary>
    /// Projection from one value to another.
    /// </summary>
    /// <typeparam name="R">Resulting functor value type.</typeparam>
    /// <param name="Map">Projection function.</param>
    /// <returns>Mapped functor</returns>
    [Pure]
    public Result<R> Map<R>(Func<T, R> Map) where R : IComparable<R> => Status is ResultStatus.Success
        ? new Result<R>(Map(Value!))
        : new Result<R>(Exception!);

    /// <summary>
    /// Returns a string representation of a Result{T}.
    /// </summary>
    /// <returns>A value of type <see cref="string"/>.</returns>
    [Pure]
    public override string ToString() => Status is ResultStatus.Success
        ? Value?.ToString() ?? "(null)"
        : Exception?.ToString() ?? "(invalid)";
}