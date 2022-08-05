namespace Result.UnitTest;

public readonly record struct Person(string FirstName, string LastName, IEnumerable<string> ToDoList);