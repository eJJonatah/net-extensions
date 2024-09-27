namespace OnStandart;
using CutDirection = NetExtensions.Standart.TextExtensions.CutDirection;
using CutOptions = NetExtensions.Standart.TextExtensions.CutOptions;
using static NetExtensions.Standart.TextExtensions.EnumAliases;
using NetExtensions.Standart;

[TestFixture] sealed class In_TextExtensions
{
    const string CODE_FILE_PATH = """
        src\std\Standart\TextExtensions.cs
        """;
    const string EXAMPLE_HTML = """
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>HTML 5 Boilerplate</title>
    <link rel="stylesheet" href="style.css">
  </head>this= found! <
  <body>
    <script src=" index.js "></script>
  </body>
</html>
""";
    const string NAME_EXAMPLE = " Net-Extensions ";

    #region Cut_between
    [Test(
        Author = "@ejjonatah",
        Description = $"""
        Invokes {nameof(TextExtensions)}.{nameof(TextExtensions.Cut)} with a series of paremeter sets
        """,
        TestOf = typeof(TextExtensions)
    ),
        TestCase("this=", '<',     EMPTY,    ExpectedResult = " found! "),
        TestCase("this=", '<',     TRIM,     ExpectedResult = "found!"),
        TestCase("SRC=\"", '\"',   ANYCASE,  ExpectedResult = " index.js "),
        TestCase('<', '!',         NOTEMPTY, ExpectedResult = "ResultWasEmptyException"),
        TestCase(default, default, EMPTY,    ExpectedResult = "ArgumentNullException"),

    ] public object Cut_between(object? start, object? end, CutOptions options)
    {
        Debugger.Break();
        try
        {
            return (start, end) switch
            {
                (string s, char s0) => EXAMPLE_HTML.Cut(s, s0, options),
                (char s, string s0) => EXAMPLE_HTML.Cut(s, s0, options),
                (char s, char s0) => EXAMPLE_HTML.Cut(s, s0, options),
                _ => EXAMPLE_HTML.Cut(start?.ToString()!, end?.ToString()!, options),
            };
        }
        catch (Exception e)
        {
            return e.GetType().Name;
        }
    }

    #endregion
    #region Cut_delimited
    [Test(
        Author = "@ejjonatah",
        Description = $"""
        Invokes {nameof(TextExtensions)}.{nameof(TextExtensions.Cut)} with a series of paremeter sets
        """,
        TestOf = typeof(TextExtensions)
    ),
        TestCase('-',  BEFOR, TRIM, ExpectedResult = "Net"),
        TestCase('-',  AFTER, TRIM, ExpectedResult = "Extensions"),
        TestCase('-',  BEFOR, EMPTY, ExpectedResult = " Net"),
        TestCase('-',  AFTER, EMPTY, ExpectedResult = "Extensions "),
        TestCase(' ',  BEFOR, NOTEMPTY, ExpectedResult = "ResultWasEmptyException"),
        TestCase(default, BEFOR, NOTEMPTY, ExpectedResult = "ArgumentNullException"),
        TestCase("Extensions", BEFOR, EMPTY, ExpectedResult = " Net-"),

    ] public object Cut_delimited(object? start, CutDirection direction, CutOptions options)
    {
        Debugger.Break();
        try
        {
            return (start) switch
            {
                (string s) => NAME_EXAMPLE.Cut(direction, s, options),
                (char s) => NAME_EXAMPLE.Cut(direction,s, options),
                _ => NAME_EXAMPLE.Cut(direction, start?.ToString()!, options),
            };
        }
        catch (Exception e)
        {
            return e.GetType().Name;
        }
    }

    #endregion

    [TearDown] public void ReportFailure()
    {
        if (TestContext.CurrentContext.Result.FailCount > 0)
        {
            var msg = GetTestContextString(null);
            TestContext.AddTestAttachment(description: """
                The code file being tested
                """,
                filePath: Path.Combine(ProjectAbsoluteRootPath, CODE_FILE_PATH)
            );
            TestContext.Write(msg);
        }
    }
}
