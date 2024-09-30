namespace NetExtensions.Validation;

/**
 * <doc><summary>The result of an operation was unexpectedly empty [ ]</summary></doc>
*/
#region maybe_public
#if NETXS_ASPUBLIC
public
#endif
#endregion

sealed class ResultWasEmptyException : Exception
{
    public ResultWasEmptyException()
        : base("The resulting sequence was empty and is not accepted")
    { }
    public ResultWasEmptyException(string expression)
        : base($"resulted empty and is not accepted ({expression})")
    { }
    public ResultWasEmptyException(string expression, Exception inner)
        : base($"resulted empty ({expression})", inner)
    { }
}