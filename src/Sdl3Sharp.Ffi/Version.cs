using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ffi;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
public readonly struct Version(int major, int minor, int patch) :
	IComparable, IComparable<Version>, IEquatable<Version>, IFormattable, ISpanFormattable, IComparisonOperators<Version, Version, bool>, IEqualityOperators<Version, Version, bool>
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static uint ValidateAndCombineComponents(int major, int minor, int patch)
	{
		if (major is < 0)
		{
			failMajorArgumentOutOfRange();
		}

		if (minor is < 0 or >= 100)
		{
			failMinorArgumentOutOfRange();
		}

		if (patch is < 0 or >= 100)
		{
			failPatchArgumentOutOfRange();
		}

		return unchecked((uint)major * 10_000 + (uint)minor * 100 + (uint)patch);

		[DoesNotReturn]
		static void failMajorArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(major));

		[DoesNotReturn]
		static void failMinorArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(minor));

		[DoesNotReturn]
		static void failPatchArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(patch));
	}

	// libffi uses a C 'unsigned long' (64-bit under 64-bit Linux, 32-bit under 64-bit Windows, 32-bit elsewhere) as packed return type for it's version number,
	// we're just going to use a C# 'uint' (32-bit) and hope versions numbers will never climb high enough to exhaust that
	private readonly uint mValue = ValidateAndCombineComponents(major, minor, patch);

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string DebuggerDisplay => ToString(formatProvider: CultureInfo.InvariantCulture);

	public readonly int Major { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((int)(mValue / 10_000)); }

	public readonly int Minor { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((int)(mValue / 100 % 100)); }

	public readonly int Patch { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => unchecked((int)(mValue % 100)); }	

	public readonly int CompareTo(object? obj)
	{
		return obj switch
		{
			null => 1,
			Version other => CompareTo(other),
			_ => failObjArgumentIsNotVersion()
		};

		[DoesNotReturn]
		static int failObjArgumentIsNotVersion() => throw new ArgumentException(message: $"{nameof(obj)} is not of type {nameof(Version)}", paramName: nameof(obj));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly int CompareTo(Version other) => mValue.CompareTo(other.mValue);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly void Deconstruct(out int major, out int minor, out int patch) { major = Major; minor = Minor; patch = Patch; }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Version other && Equals(other);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(Version other) => mValue == other.mValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly override int GetHashCode() => mValue.GetHashCode();

	public readonly override string ToString() => ToString(format: default, formatProvider: default);

	public readonly string ToString(IFormatProvider? formatProvider) => ToString(format: default, formatProvider);

	public readonly string ToString(string? format) => ToString(format, formatProvider: default);

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
		=> $"{Major.ToString(format, formatProvider)}.{Minor.ToString(format, formatProvider)}{Patch switch { var patch when patch is not 0 => $".{patch.ToString(format, formatProvider)}", _ => string.Empty }}";

	public readonly bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		charsWritten = 0;

		var b = Major.TryFormat(destination, out var tmp, format, provider);
		charsWritten += tmp;

		if (!b) { return false; }

		destination = destination[tmp..];
		
		if (destination.Length is not >= 1) { return false; }

		destination[0] = '.';
		charsWritten++;
		destination = destination[1..];

		b = Minor.TryFormat(destination, out tmp, format, provider);
		charsWritten += tmp;

		if (!b) { return false; }

		destination = destination[tmp..];

		var patch = Patch;
		if (patch is 0) { return true; }

		if (destination.Length is not >= 1) { return false; }

		destination[0] = '.';
		charsWritten++;
		destination = destination[1..];

		b = patch.TryFormat(destination, out tmp, format, provider);
		charsWritten += tmp;

		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator >(Version left, Version right) => left.mValue > right.mValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator >=(Version left, Version right) => left.mValue >= right.mValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator <(Version left, Version right) => left.mValue < right.mValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator <=(Version left, Version right) => left.mValue <= right.mValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(Version left, Version right) => left.mValue == right.mValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(Version left, Version right) => left.mValue != right.mValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static explicit operator Version(System.Version value) => new(value.Major, value.Minor, value.Build switch { < 0 => 0, var build => build });

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static explicit operator System.Version(Version value) => new(value.Major, value.Minor, value.Patch);
}
