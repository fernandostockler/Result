namespace Result.UnitTest;

using FluentAssertions;
using Result;

public class ResultTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void VerifyCaseSuccessMethod()
    {
        Result<int> successfulResult = 43;

        int value = 0;

        if (successfulResult.CaseSuccess(x => value = x))
            _ = value.Should().Be(43);

        Exception? exception = null;

        Result<Exception> failedResult = new(new InvalidOperationException("failedResult"));

        if (!failedResult.CaseSuccess(x => exception = new Exception("failedResult2")))
            _ = exception.Should().BeNull();
    }

    [Test]
    public void VerifyCaseFailureWithActionMethod()
    {
        Result<int> successfulResult = 43;

        int value = 0;

        Exception? exception = null;

        if (!successfulResult.CaseFailure(e => exception = new Exception("failedResult2")))
        {
            _ = exception.Should().BeNull();
            _ = value.Should().Be(0);
            _ = successfulResult.ToString().Should().Be("43");
        }

        Result<Exception> failedResult = new(new InvalidOperationException("failedResult"));

        if (failedResult.CaseFailure(e => value = 42))
            _ = value.Should().Be(42);
    }

    [Test]
    public void VerifyCaseFailureWithFunctionMethod()
    {
        Result<int> successfulResult = 43;

        int failureCode = successfulResult.CaseFailure(e => 42);

        _ = failureCode.Should().Be(43);

        _ = successfulResult.ToString().Should().Be("43");

        Result<Exception> failedResult = new(new InvalidOperationException("failedResult"));

        var result = failedResult.CaseFailure(e => e);

        _ = result.Should().BeOfType<InvalidOperationException>();
        _ = result.Message.Should().Be("failedResult");
        _ = failedResult.ToString().Should().Be("System.InvalidOperationException: failedResult");
    }

    [Test]
    public void VerifyMatchWithAction()
    {
        int value = 0;

        void successAction(int x) => value = x * 2;

        void failedAction(Exception e) => value = 66;

        Result<int> successfulResult = 42;

        successfulResult.Match(Success: successAction, Failure: failedAction);

        _ = value.Should().Be(42 * 2);

        value = 0;

        Result<int> failedfulResult = new(new Exception("failedResult"));

        failedfulResult.Match(Success: successAction, Failure: failedAction);

        _ = value.Should().Be(66);
    }

    [Test]
    public void VerifyMatchWithFunction()
    {
        Result<int> successfulResult = 42;

        int result = successfulResult.Match((x) => x * 3, (e) => 66);

        _ = result.Should().Be(42 * 3);

        Result<int> failedfulResult = new(new InvalidOperationException("failedResult"));

        Exception? exception = failedfulResult.Match(
            Success: (x) => new Exception("OnSuccess"),
            Failure: (e) => e);

        _ = exception.Should().BeOfType<InvalidOperationException>();
        _ = exception.Message.Should().Be("failedResult");
    }

    [Test]
    public void VerifyMapOnSuccess()
    {
        Result<string> successfulResult = new("hello");

        var result = successfulResult.Map((s) => s.Length);

        string resultToString = result.ToString();

        _ = resultToString.Should().Be("5");

        int value = 0;

        bool result2 = result.CaseSuccess(x => value = x);

        _ = result2.Should().Be(true);

        _ = value.Should().Be(5);

        Person JoeSmith = new("Joe", "Smith",
            ToDoList: new List<string>()
            {
                "Caminhar",
                "Correr",
                "Descançar"
            });
    }

    [Test]
    public void VerifyMapOnFailure()
    {
        Result<Person> failedfulResult = new(new InvalidOperationException("invalid"));

        var result = failedfulResult.Map(p => p.ToDoList.First());

        string resultToString = result.ToString();

        _ = resultToString.Should().Be("System.InvalidOperationException: invalid");

        string value = string.Empty;

        string result2 = result.CaseFailure(e => value = e.Message);

        _ = result2.Should().Be("invalid");

        _ = value.Should().Be("invalid");
    }

    [Test]
    public void VerifyToString()
    {
        var test = new Result<string>(null as string);

        _ = test.ToString().Should().Be("(null)");

        var test2 = new Result<string>(null as Exception);

        _ = test2.ToString().Should().Be("(invalid)");
    }
}