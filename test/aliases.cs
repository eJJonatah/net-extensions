global using static Aliases;
static class Aliases
{
    public static readonly string ProjectAbsoluteRootPath 
        = Directory.GetParent(Environment.CurrentDirectory)!
        .Parent!
        .Parent!
        .ToString()
    ;
    internal static string? GetTestContextString(TestContext? context = null)
    {
        var test = context?.Test ?? TestContext.CurrentContext.Test;
        var authors = string.Join(", ", test.Properties["Author"]);
        var description = test.Properties["Description"].First();
        var typeId = test.Properties["TestOf"].First();
        var expectedResult = !test.Properties.ContainsKey("ExpectedResult") ? []
            : test.Properties["ExpectedResult"].ToArray();
        return $"""
------ {test.ClassName}::{test.MethodName} -----
    Author: {authors}
     Brief: {description}
    OfType: {typeId}
    Return: {(expectedResult is [] ? "<none>" : expectedResult.First()?.ToString() ?? "<null>")}
---------------------------------------------
""";
    }
}