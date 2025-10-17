using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ffi;

[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Abi
{
	private readonly uint mValue;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private Abi(uint value) => mValue = value;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe void GetStructOffsetsImpl(Type.ffi_type* structType, Span<nuint> offsets)
	{
		Status status;

		fixed (nuint* offsetsPtr = offsets)
		{
			status = ffi_get_struct_offsets(this, structType!, offsetsPtr);
		}

		if (status is not Status.Ok)
		{
			failStatusNotOk(status);
		}

		[DoesNotReturn]
		static void failStatusNotOk(Status status) => throw new StatusException(status);
	}

	public nuint[] GetStructOffsets(Type structType)
	{
		unsafe
		{
			if (structType is null || (structType.Raw is var structTypePtr && structTypePtr is null))
			{
				failStructTypeArgumentNull();
			}

			var result = GC.AllocateUninitializedArray<nuint>(structType.ElementsArray.Length);

			GetStructOffsetsImpl(structTypePtr, result);

			return result;
		}

		[DoesNotReturn]
		static void failStructTypeArgumentNull() => throw new ArgumentNullException(nameof(structType));
	}

	public void GetStructOffsets(Type structType, Span<nuint> offsets)
	{
		unsafe
		{
			if (structType is null || (structType.Raw is var structTypePtr && structTypePtr is null))
			{
				failStructTypeArgumentNull();
			}

			if (offsets.Length < structType.ElementsArray.Length)
			{
				failOffsetsArgumentToShort();
			}

			GetStructOffsetsImpl(structTypePtr, offsets);
		}

		[DoesNotReturn]
		static void failStructTypeArgumentNull() => throw new ArgumentNullException(nameof(structType));

		[DoesNotReturn]
		static void failOffsetsArgumentToShort() => throw new ArgumentException(message: $"{nameof(offsets)} is short the the number of elements in {nameof(structType)}", paramName: nameof(offsets));
	}
}
