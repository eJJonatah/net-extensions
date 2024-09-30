namespace NetExtensions.AsCommon;
using System.Diagnostics.CodeAnalysis;

/**
 * <doc><summary>Represents data shareable with diffrent platform applications (eg. databases, apis and serialization) being universalized</summary></doc>
*/
#region maybe_public
#if NETXS_ASPUBLIC
public
#endif
#endregion

enum UniversalInformationSets : byte
{
    /**
     * <doc><summary>Commonly related to <see cref="byte"/>-sequences and binary information</summary></doc>
    */ Binary    = 0b0000_0000, /**
     * <doc><summary><see cref="bool"/> type or set of flags bit-sequences</summary></doc>
    */ Logical  = 0b0000_0001, 
    /**
     * <doc><summary>Text types in various encodings (representant <see cref="char"/>)</summary></doc>
    */ Character = 0b0000_0010, /**
     * <doc><summary>Integer non zero or negative numbers (representant <see cref="uint"/>)</summary></doc>
    */ Natural  = 0b0000_0011, 
    /**
     * <doc><summary>Integer negative or zero numbers (representant <see cref="int"/>)</summary></doc>
    */ Integer   = 0b0000_0100, /**
     * <doc><summary>Rational divisible numbers (representant <see cref="float"/>)</summary></doc>
    */ Rational = 0b0000_0101, 
    /**
     * <doc><summary>Date values from a calendar-order (representant <see cref="DateOnly"/>)</summary></doc>
    */ Calendar  = 0b0000_0110, /**
     * <doc><summary>Hour and time values possibly related to calendar (representant <see cref="TimeOnly"/>)</summary></doc>
    */ Horary   = 0b0000_0111, 
    /**
     * <doc><summary>Hash or identification type not related to real value (representant <see cref="Guid"/>)</summary></doc>
    */ Identifier = 0b0000_1000
}

/**
 * <doc><summary>Common bit-size architectures of <see cref="UniversalInformationSets"/></summary></doc>
*/
#region maybe_public
#if NETXS_ASPUBLIC
public
#endif
#endregion

enum ArchitectureOctets : byte
{
    /**
     * <doc><summary>One single abstract bit of information</summary></doc>
    */   X1 = 0b0000_0000, /** 
     * <doc><summary>Two abstract bits of information</summary></doc>
    */   X2 = 0b0001_0000,
    /** 
     * <doc><summary>Four abstract bits of information</summary></doc>
    */   X4 = 0b0010_0000, /** 
     * <doc><summary>A <see cref="byte"/> unit in bit-size</summary></doc>
    */   X8 = 0b0011_0000,
    /** 
     * <doc><summary>A <see cref="ushort"/> unit in bit-size</summary></doc>
    */  X16 = 0b0100_0000, /** 
     * <doc><summary>A <see cref="uint"/> unit in bit-size</summary></doc>
    */  X32 = 0b0101_0000,
    /** 
     * <doc><summary>A <see cref="ulong"/> unit in bit-size</summary></doc>
    */  X64 = 0b0110_0000, /** 
     * <doc><summary>A <see cref="UInt128"/> unit in bit-size</summary></doc>
    */ X128 = 0b0111_0000,
    /** 
     * <doc><summary>A (<see cref="System.Numerics.Vector{T}"/> : <see cref="ulong"/>) unit in bit-size</summary></doc>
    */ X256 = 0b1000_0000, /** 
     * <doc><summary>A (<see cref="System.Numerics.Matrix4x4"/>) unit in bit-size</summary></doc>
    */ X512 = 0b1001_0000,
}

/**
 * <doc><summary>Information about the structure in where an abstract value is stored as a <see cref="UniversalInformationSets"/></summary></doc>
*/
#region maybe_public
#if NETXS_ASPUBLIC
public
#endif
#endregion

readonly struct SetStructureDescription
{
    const byte ARCHITECTURE_MASK = 0b1111_0000;
    const byte INFORMATIONSET_MASK = 0b0000_0111;
    public readonly uint ExcessLength;
    readonly byte architecturAndSet;

#region constructors
    public SetStructureDescription(UniversalInformationSets set, ArchitectureOctets arc, uint excessLength = 0)
    {
        architecturAndSet = (byte)((byte)set | (byte)arc);
        ExcessLength = excessLength;
    }
#endregion
#region properties
    public UniversalInformationSets Set 
        => (UniversalInformationSets)(architecturAndSet & INFORMATIONSET_MASK)
    ;
    public ArchitectureOctets BitsArchitecture 
        => (ArchitectureOctets)(architecturAndSet & ARCHITECTURE_MASK)
    ;
#endregion
#region override object
    public override bool Equals([NotNullWhen(true)] object? obj)
        => typeof(SetStructureDescription).GUID == obj?.GetType().GUID
        && GetHashCode() == obj?.GetHashCode()
    ;
    public override int GetHashCode()
        => HashCode.Combine(architecturAndSet, ExcessLength)
    ;
    public override string ToString()
        => nameof(SetStructureDescription);

    public static bool operator ==(in SetStructureDescription left, in SetStructureDescription right)
        => left.architecturAndSet == right.architecturAndSet
        && left.ExcessLength == right.ExcessLength
    ;
    public static bool operator !=(in SetStructureDescription left, in SetStructureDescription right)
        => !(left == right)
    ;
    #endregion
}