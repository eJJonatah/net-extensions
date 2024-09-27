namespace NetExtensions.Validation;

/**
 * <doc><summary>Cuttoff methods specializing in breaking the code flow and throwing in specific situations</summary></doc>
*/
#region maybe_public
#if NETXS_ASPUBLIC
public
#endif
#endregion

static class Thrower 
{
    /**
     * <doc>
     * <summary>Breaks the code flow if the result of an index scan was inexistent</summary>
     * <param name="index">The resulting value of and index search/scan</param>
     * <param name="expression">captures the expression that gave value to the index</param>
     * <exception cref="EntryPointNotFoundException">The index position was inexistent (<see langword="-1"/>)</exception>
     * </doc>
    */ [Untrace, Hide] public static void Index_MustNotBe_Negative(int index, [ExpressionOf(nameof(index))] string expression = null!)
    {
        if (index is < 0) throw new EntryPointNotFoundException($"index was not found ({expression})");
    }
    /** 
     * <inheritdoc cref="Index_MustNotBe_Negative"/>
    */ [Untrace, Hide] public static void Index_MustNotBe_Negative(long index, [ExpressionOf(nameof(index))] string expression = null!)
    {
        if (index is < 0) throw new EntryPointNotFoundException($"index was not found ({expression})");
    }
    /**
     * <doc>
     * <summary>Breaks the code flow if the result of span slicing was empty -> <see cref="[ ]"/></summary>
     * <typeparam name="TElement">The element type of the span (unused)</typeparam>
     * <param name="span">The slice call result</param>
     * <param name="expression">captures the expression that gave value to the span</param>
     * <exception cref="ResultWasEmptyException">The result span was in fact empty</exception>
     * </doc>
    */ [Untrace, Hide] public static void Span_MustNotBe_Empty<TElement>(ReadOnlySpan<TElement> span, [ExpressionOf(nameof(span))] string expression = null!)
    {
        if (span is []) throw new ResultWasEmptyException(expression);
    }
    /**
     * <doc>
     * <summary>Breaks the code flow if the moving span runs out of space for <typeparamref name="TStructure"/></summary>
     * <typeparam name="TStructure">The structure type</typeparam>
     * <param name="span">The slice call result</param>
     * <param name="useMarshal">Switches when to use <br/> <see cref="Unsafe"/> or<br/> <see cref="Marshal"/> <br/> to compute the <typeparamref name="TStructure"/> size</param>
     * <param name="expression">captures the expression that gave value to the span</param>
     * <exception cref="InternalBufferOverflowException">The moving span in fact, could not fit the structure</exception>
     * </doc>
    */ [Untrace, Hide] public static void Span_MustFit_Structure<TStructure>(ReadOnlySpan<byte> span, bool useMarshal = false, [ExpressionOf(nameof(span))] string expression = null!) where TStructure : unmanaged
    {
        if (span is [] || span.Length >= (useMarshal ? Marshal.SizeOf<TStructure>() : Unsafe.SizeOf<TStructure>()))
            throw new InternalBufferOverflowException($"""
Moving span cannot fit structure of type "{typeof(TStructure).Name}" ({expression})
""");
    }
    /**
     * <inheritdoc cref="Span_MustFit_Structure{TStructure}(ReadOnlySpan{byte}, bool, string)"/>
    */ [Untrace, Hide] public static void Span_MustFit_Structure<TStructure>(Span<byte> span, bool useMarshal = false, [ExpressionOf(nameof(span))] string expression = null!) where TStructure : unmanaged
    {
        if (span is [] || span.Length >= (useMarshal ? Marshal.SizeOf<TStructure>() : Unsafe.SizeOf<TStructure>()))
            throw new InternalBufferOverflowException($"""
Moving span cannot fit structure of type "{typeof(TStructure).Name}" ({expression})
""");
    }
}