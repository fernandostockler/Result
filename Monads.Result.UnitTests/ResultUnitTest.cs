namespace Result.UnitTest;

using FluentAssertions;
using Result;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void VerifySuccessfulResult()
    {
        int value = 0;

        Result<int> successfulResult = 43;

        bool result = successfulResult.CaseSuccess(x => value = x);

        if (result) _ = value.Should().Be(43);

        _ = successfulResult.ToString().Should().Be("43");

        var result2 = successfulResult.CaseFailure(e => e.ToString());

        _ = result2.Should().Be(false);

        var result3 = successfulResult.CaseFailure((e) => 0);

        _ = result3.Should().Be(43);
    }

    [Test]
    public void VerifyFailedResult()
    {
        Result<Exception> failedResult = new(new InvalidOperationException("failedResult"));

        Exception? exception = failedResult.CaseFailure(e => (InvalidOperationException)e);

        _ = exception.Should().BeOfType<InvalidOperationException>();

        _ = exception.Message.Should().Be("failedResult");

        Exception? exception1 = null;

        bool result2 = failedResult.CaseSuccess(x => exception1 = new Exception("failedResult2"));

        _ = exception1.Should().BeNull();

        _ = result2.Should().Be(false);

        var result3 = failedResult.CaseFailure(e => e.ToString());

        _ = result3.Should().Be(true);

        _ = failedResult.ToString().Should().Be("System.InvalidOperationException: failedResult");
    }

    [Test]
    public void VerifyMatchOnSuccess()
    {
        Result<int> successfulResult = 42;

        var result = successfulResult.Match((x) => x, (e) => 0);

        _ = result.Should().Be(42);

        int value = 0;

        void successAction(int x) => value = x * 2;

        void failedAction(Exception e) => value = e.HResult;

        successfulResult.Match(Success: successAction, Failure: failedAction);

        _ = value.Should().Be(42 * 2);
    }

    [Test]
    public void VerifyMatchOnFailure()
    {
        Result<int> failedfulResult = new(new Exception("failedResult"));

        var result = failedfulResult.Match(
            Success: (x) => new Exception("OnSuccess"),
            Failure: (e) => e);

        _ = result.Message.Should().Be("failedResult");

        int value = 0;

        void successAction(int x) => value = x * 2;

        void failedAction(Exception e) => value = e.HResult;

        failedfulResult.Match(Success: successAction, Failure: failedAction);

        _ = value.Should().Be(-2146233088);
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

public readonly record struct Person(string FirstName, string LastName, IEnumerable<string> ToDoList);