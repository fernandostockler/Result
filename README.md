# Result 
<!--[![wakatime](https://wakatime.com/badge/github/fernandostockler/Result.svg?logo=github&style=plastic)](https://wakatime.com/badge/github/fernandostockler/Result)-->

## Description
`Result` represents the result of an operation. If the operation fails, Result will contain the exception that occurred;
otherwise, `Result` will contain the value resulting from the operation.

The method responsible for the operation must return a `Result` with the value resulting from the operation, 
or in case of failure, it must return a `Result` with an exception. 
The consumer code must extract the result through one of the available methods.

The result of the operation can be extracted through several methods such as: 
`Match()`, `ValueOrDefault`, `CaseSuccess()`, `CaseFailure()`, `IsSuccess()` and `IsFailure()`.

Note that no exceptions are thrown. 
Exceptions are just created and returned to the calling code which must decide what to do with them.

## Use
Example of a method responsible for the validation operation.
```C#
public static Result<string> TryValidate(string? s, IFormatProvider? provider = default)
{
    if (s is null)
        return new Result<string>(new ArgumentNullException(nameof(s)));

    if (s.Length > MaxLength)
        return new Result<string>(new ArgumentException(
            message: $"{nameof(Name)} is invalid. The value '{s}' must be at most {MaxLength} characters and currently has {s.Length} characters.",
            paramName: nameof(s)));

    return new Result<string>(s);
}
```
<br>

Example of two methods that use `Result`.

```C#
public static Result<Name> TryParse(string? s, IFormatProvider? provider = default) =>
    TryValidate(s, provider).Match(
        Success: x => new Result<Name>(new Name(x)),
        Failure: e => new Result<Name>(e));

public static bool IsValid(string? s, IFormatProvider? provider = null) =>
    TryValidate(s, provider).Match(
        Success: x => true,
        Failure: e => false);
```
<br>

## Public Methods
### Match
```C#
R Match<R>(Func<T, R> Success, Func<Exception, R> Failure)
```
    If it is a successful result, it returns the value of the Success function, 
    otherwise it returns the value of the Failure function.

```C#
Match(Action<T> Success, Action<Exception> Failure)
```
    If it is a successful result, it executes the Success action, otherwise it executes the Failure action.

### CaseSuccess
```C#
R CaseSuccess<R>(Func<T, R> Success, Func<Exception, R> defaultValue)
```
    If it is a successful result, it returns the value of the Success function, 
    otherwise it returns the value of the defaultValue function.
```C#
bool CaseSuccess(Action<T> Success)
```
    If it is a successful result, it executes the Success action, otherwise it returns false.
###  ValueOrDefault
```C#
T ValueOrDefault(Func<Exception, T> Failure) => Match(value => value, Failure)
```
    If it is a successful result, it returns the value of the result, otherwise it returns the value of the Failure function.
### CaseFailure
```C#
bool CaseFailure(Action<Exception> Failure)
```
    If it is a failure result, it executes the Failure action and return true, otherwise it returns false.
### IsSuccess
```C#
bool IsSuccess()
```
    If it is a successful result, it returns true, otherwise it returns false.
### IsFailure
```C#
bool IsFailure()
```
    If it is a failure result, it returns true, otherwise it returns false.

## References
C# functional language extensions  - [louthy/language-ext](https://github.com/louthy/language-ext)

Don't throw exceptions in C#. Do this instead - [Nick Chapsas](https://www.youtube.com/watch?v=a1ye9eGTB98&t=761s&ab_channel=NickChapsas)