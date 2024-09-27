global using static NetxsAliases;
global using ExpressionOfAttribute= System.Runtime.CompilerServices.CallerArgumentExpressionAttribute;
global using ConstAttribute = System.Diagnostics.CodeAnalysis.ConstantExpectedAttribute;
global using MethodAttribute = System.Runtime.CompilerServices.MethodImplAttribute;
global using NullAttribute = System.Diagnostics.CodeAnalysis.AllowNullAttribute;
global using NNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;
global using UntraceAttribute = System.Diagnostics.StackTraceHiddenAttribute;
global using HideAttribute = System.Diagnostics.DebuggerHiddenAttribute;
global using Unsafe= System.Runtime.CompilerServices.Unsafe;
global using Marshal= System.Runtime.InteropServices.Marshal;
global using CLMarshal= System.Runtime.InteropServices.CollectionsMarshal;
global using MMarshal = System.Runtime.InteropServices.MemoryMarshal;

using System.Runtime.CompilerServices;
using System.Buffers;

static class NetxsAliases
{
    public const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;
    public const MethodImplOptions FCALL = MethodImplOptions.NoInlining;

    [Method(INLINE)] public static IMemoryOwner<T> GetPooled<T>(uint length, bool skipInit = false) where T : unmanaged
    {
        var rent = MemoryPool<T>.Shared.Rent((int)length);
        if (!skipInit) rent.Memory.Span.Clear();
        return rent;
    }
}