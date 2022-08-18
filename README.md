# Result 
[![wakatime](https://wakatime.com/badge/github/fernandostockler/Result.svg?logo=github&style=plastic)](https://wakatime.com/badge/github/fernandostockler/Result)

## Descrição
`Result` representa o resultado de uma operação. 
Se a operação falhar, `Result` conterá a exceção ocorrida; 
caso contrário, `Result` conterá o valor resultante da operação.

O método responsável pela operação deve retornar um `Result` com o valor resultante da operação, ou 
em caso de falha, deve retornar um `Result` com uma exceção. O código consumidor deve extrair o 
resultado através de um dos métodos disponíveis.

O resultado da operação pode ser extraido através de vários métodos como: 
`Match()`, `CaseSuccess()`, `CaseFailure()`, `IsSuccess()` e `IsFailure()`.

Note que nenhuma exceção é lançada. As exceções são apenas criadas e devolvidas para o código chamador que
deverá decidir o que fazer com elas.

## Uso
Exemplo de um método responsável pela operação de validação.
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

Exemplo de dois métodos que usam `Result`.

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

## Métodos públicos
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
###  CaseFailure
```C#
T CaseFailure(Func<Exception, T> Failure) => Match(x => x, Failure)
```
    If it is a successful result, it returns the value of the result, otherwise it returns the value of the Failure function.
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