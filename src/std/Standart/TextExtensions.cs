namespace NetExtensions.Standart;

using NetExtensions.Validation;
using System.Buffers;
using static TextExtensions.EnumAliases;
using Chars = ReadOnlySpan<char>;

/**
 * <doc><summary>Group of methods to use and work with representation of text mainly: <see cref="string"/>, (<see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/> of <see cref="char"/>) and UTF-8 variations</summary></doc>
*/
#region maybe_public
#if NETXS_ASPUBLIC
public
#endif
#endregion

static class TextExtensions 
{
    /** 
     * <doc><summary>Additional parameters to <see cref="TextExtensions"/>.Cut's methods</summary></doc>
    */ [Flags] public enum CutOptions : byte
    {
        /**
         * <doc><summary>Indicates that the result can be left empty</summary></doc>
        */ Empty    = 0b0000_0000,
        /**
         * <doc><summary>Indicates that the result cannot be empty</summary></doc>
        */ NotEmpty = 0b0000_0001,
        /**
         * <doc><summary>Indicates that the result should be trimmed at the end</summary></doc>
        */ Trim     = 0b0000_0010,
        /**
         * <doc><summary>Indicates that the seeking mechanism shouldn't cosiderate case sensitivity</summary></doc>
        */ AnyCase  = 0b0000_0100,
    }
    /** 
     * <doc><summary>Provides the direction of slicing in a <see cref="TextExtensions"/>.Cut method</summary></doc>
    */ public enum CutDirection : byte
    {
        /**
         * <doc><summary>Slice content after the delimiter</summary></doc>
        */ After, /**
         * <doc><summary>Slice content before the delimiter</summary></doc>
        */ Before
    }
    /**
     * <doc><summary>Sintatical sugar aliases for <see cref="CutOptions"/> and <see cref="CutDirection"/> enum values</summary></doc>
    */ public static class EnumAliases
    {
        /**
         * <inheritdoc cref="CutOptions.NotEmpty"/>
        */ public const CutOptions NOTEMPTY = CutOptions.NotEmpty;
        /**
         * <inheritdoc cref="CutOptions.AnyCase"/>
        */ public const CutOptions ANYCASE  = CutOptions.AnyCase;
        /**
         * <inheritdoc cref="CutOptions.Empty"/>
        */ public const CutOptions EMPTY    = CutOptions.Empty;
        /**
         * <inheritdoc cref="CutOptions.Trim"/>
        */ public const CutOptions TRIM     = CutOptions.Trim;

        /** 
         * <inheritdoc cref="CutDirection.After"/>
        */ public const CutDirection AFTER = CutDirection.After;
        /** 
         * <inheritdoc cref="CutDirection.After"/>
        */ public const CutDirection BEFOR = CutDirection.Before;
    }

    /**
     * <doc>
     * <summary>Seeks <see langword="this"/> <see cref="string"/> to find <paramref name="between"/> a delimiter <paramref name="and"/> other</summary>
     * <param name="source">The given <see langword="this"/> <see cref="string"/></param>
     * <param name="between">The start delimiter (must not be <see langword="null"/> or empty)</param>
     * <param name="and">The end delimiter (must not be <see langword="null"/> or empty)</param>
     * <param name="options">Additional parameters</param>
     * <returns><see langword="new"/> Instace of <see cref="string"/> containing the sliced content</returns>
     * <exception cref="ArgumentNullException"/>
     * <inheritdoc cref="Cut(Chars, Chars, Chars, CutOptions)"/>
     * </doc>
    */ public static string Cut(this string source, string between, string and, CutOptions options = new())
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(between, nameof(source));
        ArgumentNullException.ThrowIfNull(and, nameof(source));

        return new(Cut(source.AsSpan(), between.AsSpan(), and.AsSpan(), options));
    }
    /** 
     * <inheritdoc cref="Cut"/>
    */ public static string Cut(this string source, Chars between, Chars and, CutOptions options = new())
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        return new(Cut(source.AsSpan(), between, and, options));
    }
    /** 
     * <inheritdoc cref="Cut"/>
    */ public static string Cut(this string source, Chars between, char and, CutOptions options = new())
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        return new(Cut(source.AsSpan(), between, and, options));
    }
    /** 
     * <inheritdoc cref="Cut"/>
    */ public static string Cut(this string source, char between, Chars and, CutOptions options = new())
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        return new(Cut(source.AsSpan(), between, and, options));
    }
    /** 
     * <inheritdoc cref="Cut"/>
    */ public static string Cut(this string source, char between, char and, CutOptions options = new())
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        return new(Cut(source.AsSpan(), between, and, options));
    }
    /**
     * <doc>
     * <summary>Seeks <see langword="this"/> <see cref="string"/> to find a <paramref name="delimiter"/>'s position</summary>
     * <param name="source">The given <see langword="this"/> <see cref="string"/></param>
     * <param name="direction">Which part of the <see cref="string"/> to consider related to the <paramref name="delimiter"/> position</param>
     * <param name="delimiter">The delimiter point (must not be <see langword="null"/> or empty)</param>
     * <param name="options">Additional parameters</param>
     * <returns><see langword="new"/> Instace of <see cref="string"/> containing the sliced content</returns>
     * <exception cref="ArgumentNullException"/>
     * <inheritdoc cref="Cut(Chars, CutDirection, Chars, CutOptions)"/>
     * </doc>
    */ public static string Cut(this string source, CutDirection direction, string delimiter, CutOptions options = new())
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(delimiter, nameof(source));

        return new(Cut(source.AsSpan(), direction, delimiter.AsSpan(), options));
    }
    /** 
     * <inheritdoc cref="Cut(string, CutDirection, string, CutOptions)"/>
    */ public static string Cut(this string source, CutDirection direction, char delimiter, CutOptions options = new())
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(delimiter, nameof(source));

        return new(Cut(source.AsSpan(), direction, delimiter, options));
    }

    /**
     * <doc>
     * <summary>Seeks <see langword="this"/> <see cref="Chars"/> to find <paramref name="between"/> a delimiter <paramref name="and"/> other</summary>
     * <param name="source">The given <see langword="this"/> <see cref="Chars"/></param>
     * <param name="between">The start delimiter (must not be <see langword="null"/> or empty)</param>
     * <param name="and">The end delimiter (must not be <see langword="null"/> or empty)</param>
     * <param name="options">Additional parameters</param>
     * <returns><see cref="Chars.Slice(int)"/> of the found content</returns>
     * <inheritdoc cref="Argument.Span_MustNotBe_Empty"/>
     * <inheritdoc cref="Thrower.Index_MustNotBe_Negative"/>
     * <inheritdoc cref="Thrower.Span_MustNotBe_Empty"/>
     * </doc>
    */ public static Chars Cut(this Chars source, Chars between, Chars and, CutOptions options = new())
    {
        if (options.HasFlag(ANYCASE)) return cutInvariant(source, between, and, options);

        Argument.Span_MustNotBe_Empty(between);
        Argument.Span_MustNotBe_Empty(and);

        int indexof_start, indexof_limit;

        indexof_start = source.IndexOf(between);
        indexof_limit = source[(indexof_start is -1 ? 0 : (indexof_start + between.Length))..].IndexOf(and);

        Thrower.Index_MustNotBe_Negative(indexof_start);
        Thrower.Index_MustNotBe_Negative(indexof_limit -= and.Length-1);
        indexof_start += between.Length;
        bool shouldTrim = options.HasFlag(TRIM);
        Chars result;

        if (options.HasFlag(NOTEMPTY))
        {
            Thrower.Span_MustNotBe_Empty(
                result = source.Slice(indexof_start, indexof_limit));
            if (shouldTrim) Thrower.Span_MustNotBe_Empty(
                result = result.Trim());
            return result;
        }

        result = source.Slice(indexof_start, indexof_limit);
        return !shouldTrim ? result : result.Trim();
    }
    /** 
     * <inheritdoc cref="Cut(Chars, Chars, Chars, CutOptions)"/>
    */ public static Chars Cut(this Chars source, Chars between, char and, CutOptions options = new())
    {
        if (options.HasFlag(ANYCASE)) return cutInvariant(source, between, and, options);

        Argument.Span_MustNotBe_Empty(between);

        int indexof_start, indexof_limit;

        indexof_start = source.IndexOf(between);
        indexof_limit = source[(indexof_start is -1 ? 0 : (indexof_start + between.Length))..].IndexOf(and);

        Thrower.Index_MustNotBe_Negative(indexof_start);
        Thrower.Index_MustNotBe_Negative(indexof_limit);
        indexof_start += between.Length;
        bool shouldTrim = options.HasFlag(TRIM);
        Chars result;

        if (options.HasFlag(NOTEMPTY))
        {
            Thrower.Span_MustNotBe_Empty(
                result = source.Slice(indexof_start, indexof_limit));
            if (shouldTrim) Thrower.Span_MustNotBe_Empty(
                result = result.Trim());
            return result;
        }

        result = source.Slice(indexof_start, indexof_limit);
        return !shouldTrim ? result : result.Trim();
    }
    /** 
     * <inheritdoc cref="Cut(Chars, Chars, Chars, CutOptions)"/>
    */ public static Chars Cut(this Chars source, char between, Chars and, CutOptions options = new())
    {
        if (options.HasFlag(ANYCASE)) return cutInvariant(source, between, and, options);

        Argument.Span_MustNotBe_Empty(and);

        int indexof_start, indexof_limit;

        indexof_start = source.IndexOf(between);
        indexof_limit = source[(indexof_start is -1 ? 0 : (indexof_start + 1))..].IndexOf(and);

        Thrower.Index_MustNotBe_Negative(indexof_start);
        Thrower.Index_MustNotBe_Negative(indexof_limit -= and.Length - 1);
        indexof_start += 1;
        bool shouldTrim = options.HasFlag(TRIM);
        Chars result;

        if (options.HasFlag(NOTEMPTY))
        {
            Thrower.Span_MustNotBe_Empty(
                result = source.Slice(indexof_start, indexof_limit));
            if (shouldTrim) Thrower.Span_MustNotBe_Empty(
                result = result.Trim());
            return result;
        }

        result = source.Slice(indexof_start, indexof_limit);
        return !shouldTrim ? result : result.Trim();
    }
    /** 
     * <inheritdoc cref="Cut(Chars, Chars, Chars, CutOptions)"/>
    */ public static Chars Cut(this Chars source, char between, char and, CutOptions options = new())
    {
        if (options.HasFlag(ANYCASE)) return cutInvariant(source, between, and, options);

        int indexof_start, indexof_limit;

        indexof_start = source.IndexOf(between);
        indexof_limit = source[(indexof_start is -1 ? 0 : (indexof_start + 1))..].IndexOf(and);

        Thrower.Index_MustNotBe_Negative(indexof_start);
        Thrower.Index_MustNotBe_Negative(indexof_limit);
        indexof_start += 1;
        bool shouldTrim = options.HasFlag(TRIM);
        Chars result;

        if (options.HasFlag(NOTEMPTY))
        {
            Thrower.Span_MustNotBe_Empty(
                result = source.Slice(indexof_start, indexof_limit));
            if (shouldTrim) Thrower.Span_MustNotBe_Empty(
                result = result.Trim());
            return result;
        }

        result = source.Slice(indexof_start, indexof_limit);
        return !shouldTrim ? result : result.Trim();
    }
    /**
     * <doc>
     * <summary>Seeks <see langword="this"/> <see cref="Chars"/> to find a <paramref name="delimiter"/>'s position</summary>
     * <param name="direction">Which part of the <see cref="Chars"/> to consider related to the <paramref name="delimiter"/> position</param>
     * <param name="delimiter">The delimiter point (must not be <see langword="null"/> or empty)</param>
     * <inheritdoc cref="Cut(Chars, Chars, Chars, CutOptions)"/>
     * </doc>
    */ public static Chars Cut(this Chars source, CutDirection direction, Chars delimiter, CutOptions options = new())
    {
        if (options.HasFlag(ANYCASE)) return cutInvariant(source, direction, delimiter, options);
        Argument.Span_MustNotBe_Empty(delimiter);

        int indexof_delimiter;

        indexof_delimiter = source.IndexOf(delimiter);
        Thrower.Index_MustNotBe_Negative(indexof_delimiter);

        Chars result;
        if (direction is BEFOR)
        {
            if (options.HasFlag(NOTEMPTY)) Thrower.Span_MustNotBe_Empty(result = source[..indexof_delimiter]);
            else result = source[..indexof_delimiter];
        }
        else
        {
            indexof_delimiter += delimiter.Length;
            if (options.HasFlag(NOTEMPTY)) Thrower.Span_MustNotBe_Empty(result = source[indexof_delimiter..]);
            else result = source[indexof_delimiter..];
        }
        if (options.HasFlag(TRIM)) if (options.HasFlag(NOTEMPTY)) result = result.Trim();
            else Thrower.Span_MustNotBe_Empty(result = result.Trim());

        return result;
    }
    /** 
     * <inheritdoc cref="Cut(Chars, CutDirection, Chars, CutOptions)"/>
    */ public static Chars Cut(this Chars source, CutDirection direction, char delimiter, CutOptions options = new())
    {
        if (options.HasFlag(ANYCASE)) return cutInvariant(source, direction, delimiter, options);

        int indexof_delimiter;

        indexof_delimiter = source.IndexOf(delimiter);
        Thrower.Index_MustNotBe_Negative(indexof_delimiter);

        Chars result;
        if (direction is BEFOR)
        {
            if (options.HasFlag(NOTEMPTY)) Thrower.Span_MustNotBe_Empty(result = source[..indexof_delimiter]);
            else result = source[..indexof_delimiter];
        }
        else
        {
            indexof_delimiter += 1;
            if (options.HasFlag(NOTEMPTY)) Thrower.Span_MustNotBe_Empty(result = source[indexof_delimiter..]);
            else result = source[indexof_delimiter..];
        }
        if (options.HasFlag(TRIM)) if (options.HasFlag(NOTEMPTY)) result = result.Trim();
            else Thrower.Span_MustNotBe_Empty(result = result.Trim());

        return result;
    }

    #region private helpers
    static Chars cutInvariant(this Chars source, Chars between, Chars and, CutOptions options = new())
    {
        Argument.Span_MustNotBe_Empty(between);
        Argument.Span_MustNotBe_Empty(and);
        
        IMemoryOwner<char>? ownership = null;
        
        var minimumlength = (uint)(source.Length + between.Length + and.Length);
        bool canStackalloc = minimumlength < 2048;
        scoped Span<char> buffer = canStackalloc ? stackalloc char[(int)minimumlength]
            : (ownership = GetPooled<char>(minimumlength, skipInit: true)).Memory.Span;

        using var autoDispose = ownership;

        scoped Span<char> 
            sourceLwr  = buffer[..source.Length], 
            betweenLwr = buffer.Slice(source.Length, between.Length), 
            andLwr     = buffer[^and.Length..]
        ;

        source.ToLowerInvariant(sourceLwr);
        between.ToLowerInvariant(betweenLwr);
        and.ToLowerInvariant(andLwr);

        int indexof_start, indexof_limit;

        indexof_start = sourceLwr.IndexOf(betweenLwr);
        indexof_limit = sourceLwr[(indexof_start is -1 ? 0 : (indexof_start + betweenLwr.Length))..].IndexOf(andLwr);

        Thrower.Index_MustNotBe_Negative(indexof_start);
        Thrower.Index_MustNotBe_Negative(indexof_limit -= and.Length - 1);
        indexof_start += between.Length;
        bool shouldTrim = options.HasFlag(TRIM);
        Chars result;

        if (options.HasFlag(NOTEMPTY))
        {
            Thrower.Span_MustNotBe_Empty(
                result = source.Slice(indexof_start, indexof_limit));
            if (shouldTrim) Thrower.Span_MustNotBe_Empty(
                result = result.Trim());
            return result;
        }

        result = source.Slice(indexof_start, indexof_limit);
        return !shouldTrim ? result : result.Trim();
        
    }
    static Chars cutInvariant(this Chars source, Chars between, char and, CutOptions options = new())
    {
        Argument.Span_MustNotBe_Empty(between);
        IMemoryOwner<char>? ownership = null;

        var minimumlength = (uint)(source.Length + between.Length);
        bool canStackalloc = minimumlength < 2048;
        scoped Span<char> buffer = canStackalloc ? stackalloc char[(int)minimumlength]
            : (ownership = GetPooled<char>(minimumlength, skipInit: true)).Memory.Span;

        using var autoDispose = ownership;

        scoped Span<char> 
            sourceLwr  = buffer[..source.Length], 
            betweenLwr = buffer.Slice(source.Length, between.Length)
        ;

        source.ToLowerInvariant(sourceLwr);
        between.ToLowerInvariant(betweenLwr);

        int indexof_start, indexof_limit;

        indexof_start = sourceLwr.IndexOf(betweenLwr);
        indexof_limit = sourceLwr[(indexof_start is -1 ? 0 : (indexof_start + betweenLwr.Length))..].IndexOf(char.ToLowerInvariant(and));

        Thrower.Index_MustNotBe_Negative(indexof_start);
        Thrower.Index_MustNotBe_Negative(indexof_limit);
        indexof_start += between.Length;
        bool shouldTrim = options.HasFlag(TRIM);
        Chars result;

        if (options.HasFlag(NOTEMPTY))
        {
            Thrower.Span_MustNotBe_Empty(
                result = source.Slice(indexof_start, indexof_limit));
            if (shouldTrim) Thrower.Span_MustNotBe_Empty(
                result = result.Trim());
            return result;
        }

        result = source.Slice(indexof_start, indexof_limit);
        return !shouldTrim ? result : result.Trim();
        
    }
    static Chars cutInvariant(this Chars source, char between, Chars and, CutOptions options = new())
    {
        Argument.Span_MustNotBe_Empty(and);
        
        IMemoryOwner<char>? ownership = null;
        
        var minimumlength = (uint)(source.Length + and.Length);
        bool canStackalloc = minimumlength < 2048;
        scoped Span<char> buffer = canStackalloc ? stackalloc char[(int)minimumlength]
            : (ownership = GetPooled<char>(minimumlength, skipInit: true)).Memory.Span;

        using var autoDispose = ownership;

        scoped Span<char> 
            sourceLwr  = buffer[..source.Length],
            andLwr = buffer[source.Length..]
        ;

        source.ToLowerInvariant(sourceLwr);
        and.ToLowerInvariant(andLwr);

        int indexof_start, indexof_limit;

        indexof_start = sourceLwr.IndexOf(char.ToLowerInvariant(between));
        indexof_limit = sourceLwr[(indexof_start is -1 ? 0 : (indexof_start + 1))..].IndexOf(andLwr);

        Thrower.Index_MustNotBe_Negative(indexof_start);
        Thrower.Index_MustNotBe_Negative(indexof_limit -= and.Length - 1);
        indexof_start += 1;
        bool shouldTrim = options.HasFlag(TRIM);
        Chars result;

        if (options.HasFlag(NOTEMPTY))
        {
            Thrower.Span_MustNotBe_Empty(
                result = source.Slice(indexof_start, indexof_limit));
            if (shouldTrim) Thrower.Span_MustNotBe_Empty(
                result = result.Trim());
            return result;
        }

        result = source.Slice(indexof_start, indexof_limit);
        return !shouldTrim ? result : result.Trim();
        
    }
    static Chars cutInvariant(this Chars source, char between, char and, CutOptions options = new())
    {
        
        IMemoryOwner<char>? ownership = null;
        
        var minimumlength = (uint)source.Length;
        bool canStackalloc = minimumlength < 2048;
        scoped Span<char> buffer = canStackalloc ? stackalloc char[(int)minimumlength]
            : (ownership = GetPooled<char>(minimumlength, skipInit: true)).Memory.Span;

        using var autoDispose = ownership;

        source.ToLowerInvariant(buffer);

        int indexof_start, indexof_limit;

        indexof_start = buffer.IndexOf(char.ToLowerInvariant(between));
        indexof_limit = buffer[(indexof_start is -1 ? 0 : (indexof_start + 1))..].IndexOf(char.ToLowerInvariant(and));

        Thrower.Index_MustNotBe_Negative(indexof_start);
        Thrower.Index_MustNotBe_Negative(indexof_limit);
        indexof_start += 1;
        bool shouldTrim = options.HasFlag(TRIM);
        Chars result;

        if (options.HasFlag(NOTEMPTY))
        {
            Thrower.Span_MustNotBe_Empty(
                result = source.Slice(indexof_start, indexof_limit));
            if (shouldTrim) Thrower.Span_MustNotBe_Empty(
                result = result.Trim());
            return result;
        }

        result = source.Slice(indexof_start, indexof_limit);
        return !shouldTrim ? result : result.Trim();
        
    }
    static Chars cutInvariant(this Chars source, CutDirection direction, Chars delimiter, CutOptions options = new())
    {
        Argument.Span_MustNotBe_Empty(delimiter);

        IMemoryOwner<char>? ownership = null;

        var minimumlength = (uint)(source.Length + delimiter.Length);
        var canStackalloc = minimumlength < 2048;
        scoped Span<char> buffer = canStackalloc ? stackalloc char[(int)minimumlength]
            : (ownership = GetPooled<char>(minimumlength, skipInit: true)).Memory.Span;

        using var autoDispose = ownership;

        scoped Span<char>
            sourceLwr = buffer[..source.Length],
            delimiterLwr = buffer[source.Length..]
        ;

        source.ToLowerInvariant(sourceLwr);
        delimiter.ToLowerInvariant(delimiterLwr);

        int indexof_delimiter;

        indexof_delimiter = sourceLwr.IndexOf(delimiterLwr);
        Thrower.Index_MustNotBe_Negative(indexof_delimiter);
        Chars result;

        if (direction is BEFOR)
        {
            indexof_delimiter += delimiterLwr.Length;
            if (options.HasFlag(NOTEMPTY)) Thrower.Span_MustNotBe_Empty(result = source[..indexof_delimiter]);
            else result = source[..indexof_delimiter];
        }
        else
        {
            indexof_delimiter += 1;
            if (options.HasFlag(NOTEMPTY)) Thrower.Span_MustNotBe_Empty(result = source[indexof_delimiter..]);
            else result = source[indexof_delimiter..];
        }
        if (options.HasFlag(TRIM)) 
            if (options.HasFlag(NOTEMPTY)) result = result.Trim();
            else Thrower.Span_MustNotBe_Empty(result = result.Trim());

        return result;
    }
    static Chars cutInvariant(this Chars source, CutDirection direction, char delimiter, CutOptions options = new())
    {
        IMemoryOwner<char>? ownership = null;

        var minimumlength = (uint)source.Length;
        var canStackalloc = minimumlength < 2048;
        scoped Span<char> buffer = canStackalloc ? stackalloc char[(int)minimumlength]
            : (ownership = GetPooled<char>(minimumlength, skipInit: true)).Memory.Span;

        using var autoDispose = ownership;

        source.ToLowerInvariant(buffer);

        int indexof_delimiter;

        indexof_delimiter = buffer.IndexOf(char.ToLowerInvariant(delimiter));
        Thrower.Index_MustNotBe_Negative(indexof_delimiter);
        Chars result;

        if (direction is BEFOR)
        {indexof_delimiter += 1;
            if (options.HasFlag(NOTEMPTY)) Thrower.Span_MustNotBe_Empty(result = source[..indexof_delimiter]);
            else result = source[..indexof_delimiter];
        }
        else
        {
            indexof_delimiter += 1;
            if (options.HasFlag(NOTEMPTY)) Thrower.Span_MustNotBe_Empty(result = source[indexof_delimiter..]);
            else result = source[indexof_delimiter..];
        }
        if (options.HasFlag(TRIM)) 
            if (options.HasFlag(NOTEMPTY)) result = result.Trim();
            else Thrower.Span_MustNotBe_Empty(result = result.Trim());

        return result;
    }
    #endregion
}