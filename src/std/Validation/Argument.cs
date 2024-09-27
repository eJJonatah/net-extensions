namespace NetExtensions.Validation;

using System.Diagnostics;


/**
 * <doc><summary>Cuttoff methods to validate input arguments and throw invalid entries</summary></doc>
*/
#region maybe_public
#if NETXS_ASPUBLIC
public
#endif
#endregion

static class Argument 
{
    /**
     * <doc>
     * <summary>
     * Ensures that the argument <see cref="Stream"/> is readable
     * </summary>
     * <param name="s">The input argument of <see cref="Stream"/></param>
     * <param name="paramName">Captures the argument name</param>
     * <exception cref="ArgumentNullException"><see cref="Stream"/> <paramref name="s"/> was <see langword="null"/></exception>
     * <exception cref="ArgumentException"><see cref="Stream"/> was in fact not readable when required</exception>
     * </doc>
    */ [Untrace, Hide] public static void Stream_MustBe_Readable([NNull, Null] Stream s, [ExpressionOf(nameof(s))] string paramName = null!)
    {
        ArgumentNullException.ThrowIfNull(s, paramName);
        if (s.CanRead is false) throw new ArgumentException($"Stream must be readable ({paramName})'", paramName);
    }
    /**
     * <doc>
     * <summary>
     * Ensures that the argument <see cref="Stream"/> is writable
     * </summary>
     * <param name="s">The input argument of <see cref="Stream"/></param>
     * <param name="paramName">Captures the argument name</param>
     * <exception cref="ArgumentNullException"><see cref="Stream"/> <paramref name="s"/> was <see langword="null"/></exception>
     * <exception cref="ArgumentException"><see cref="Stream"/> was in fact not writable when required</exception>
     * </doc>
    */ [Untrace, Hide] public static void Stream_MustBe_Writable([NNull, Null] Stream s, [ExpressionOf(nameof(s))] string paramName = null!)
    {
        ArgumentNullException.ThrowIfNull(s, paramName);
        if (s.CanWrite is false) throw new ArgumentException($"Stream must be writable ({paramName})", paramName);
    }
    /**
     * <doc>
     * <summary>
     * Ensures that the argument <see cref="IEnumerable{T}"/> yields at least one value
     * </summary>
     * <typeparam name="TElement">Sub-type of the <see cref="IEnumerable{T}"/> (unused)</typeparam>
     * <param name="enmrtr">The input argument of <see cref="IEnumerable{T}"/></param>
     * 
     * <param name="paramName">Captures the argument's name</param>
     * <exception cref="ArgumentNullException"><see cref="IEnumerable{T}"/> <paramref name="enmrtr"/> was <see langword="null"/></exception>
     * <exception cref="ArgumentException">The <see cref="IEnumerable{T}"/> was in fact not filled when required</exception>
     * </doc>
    */ [Untrace, Hide] public static void Enumerable_MustBeAtLeast_One<TElement>([NNull, Null] IEnumerator<TElement> enmrtr, [ExpressionOf(nameof(enmrtr))] string paramName = null!)
    {
        ArgumentNullException.ThrowIfNull(enmrtr, paramName);
        if (!enmrtr.MoveNext()) throw new ArgumentException($"Must provide at least one element ({paramName})", paramName);
    }
    /**
     * <doc>
     * <summary>
     * Ensures that the argument <see cref="ReadOnlySpan{T}"/> is not empty
     * </summary>
     * <typeparam name="TElement">Sub-type of the <see cref="ReadOnlySpan{T}"/> (unused)</typeparam>
     * <param name="span">The input argument of <see cref="ReadOnlySpan{T}"/></param>
     * <param name="paramName">Captures the argument's name</param>
     * <exception cref="ArgumentException">The <see cref="ReadOnlySpan{T}"/> was in fact not filled when required</exception>
     * </doc>
    */ [Untrace, Hide] public static void Span_MustNotBe_Empty<TElement>(ReadOnlySpan<TElement> span, [ExpressionOf(nameof(span))] string paramName = null!)
    {
        if (span is []) throw new ArgumentException($"Must provide at least one element ({paramName})", paramName);
    }
    /**
     * <inheritdoc cref="Span_MustNotBe_Empty{TElement}(ReadOnlySpan{TElement}, string)"/>
    */ [Untrace, Hide] public static void Span_MustNotBe_Empty<TElement>(Span<TElement> span, [ExpressionOf(nameof(span))] string paramName = null!)
    {
        if (span is []) throw new ArgumentException($"Must provide at least one element ({paramName})", paramName);
    }
    /**
     * <doc>
     * <summary>
     * Ensures that the argument <see cref="string"/> is not only spaces
     * </summary>
     * <param name="str">The input argument <see cref="string"/></param>
     * <param name="allowEmpty">If <see cref="string.Empty"/> should also throw</param>
     * <param name="expression">Captures the argument's name</param>
     * <exception cref="ArgumentNullException"><see cref="string"/> <paramref name="str"/> was <see langword="null"/></exception>
     * <exception cref="ArgumentException">
     *  In fact the <see cref="string"/> was only empty spaces or (<paramref name="allowEmpty"/> was <see langword="true"/> and <paramref name="str"/> was <see cref="string.Empty"/>)
     * </exception>
     * </doc>
    */ [Untrace, Hide] public static void String_MustNotBe_OnlySpaces([NNull, Null] string str, bool allowEmpty = true, [ExpressionOf(nameof(str))] string expression = null!)
    {
        ArgumentNullException.ThrowIfNull(str, expression);
        if (!allowEmpty) Enumerable_MustBeAtLeast_One(str, expression);
        if (string.IsNullOrWhiteSpace(str)) throw new ArgumentException(
            paramName: expression, 
            message: allowEmpty
                ? $"String must not be space chars only ({expression})"
                : $"String must not be empty or space chars only ({expression})"
        );
    }
    /**
     * <doc>
     * <summary>
     * Ensures that the argument <see cref="IEnumerable{T}"/> has alteast a specified length
     * </summary>
     * <param name="minimum">The constant minimum length required </param>
     * <param name="collection">The input argument <see cref="IEnumerable{T}"/></param>
     * <param name="expression">Captures the argument's name</param>
     * <exception cref="ArgumentNullException"><see cref="IEnumerable{T}"/> <paramref name="collection"/> was <see langword="null"/></exception>
     * <exception cref="ArgumentException">
     *  In fact <paramref name="collection"/> doesn't have the <paramref name="minimum"/> required length
     * </exception>
     * </doc>
    */ [Untrace, Hide] public static void Collection_MininumLength<TElements>([Const] uint minimum, [NNull, Null] IEnumerable<TElements> collection, [ExpressionOf(nameof(collection))] string expression = null!)
    {
        ArgumentNullException.ThrowIfNull(collection, expression);
        Debug.Assert(collection.TryGetNonEnumeratedCount(out _));
        if (collection.Count() < minimum) throw new ArgumentException($"Minimum required length of ({minimum}) ({expression})", expression);
    }
    /**
     * <doc>
     * <summary>
     * Ensures that the argument <typeparamref name="TStruct"/> is not a <see langword="default"/> instance
     * </summary>
     * <typeparam name="TStruct">The type of <paramref name="structure"/></typeparam>
     * <param name="paramName">Captures the argument's name</param>
     * <exception cref="ArgumentException">The <paramref name="structure"/> argument was in fact, a <see langword="default"/> instance</exception>
     * </doc>
    */ [Untrace, Hide] public static void Struct_MustNotBe_Default<TStruct>(in TStruct structure, [ExpressionOf(nameof(structure))] string paramName = null!) where TStruct : struct 
    {
        var defaultHash = default(TStruct).GetHashCode();
        Debug.Assert(defaultHash == default(TStruct).GetHashCode(), $"""
            the default instance of '{typeof(TStruct).Name}' produces different hashcodes beign unpredictable
            """);

        if (structure.GetHashCode() == defaultHash) throw new ArgumentException($"Structure must not be default ({paramName})", paramName);
    }
}