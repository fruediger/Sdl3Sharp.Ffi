using Sdl3Sharp.Ffi.Internal.Interop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ffi;

partial class Type
{
	[DoesNotReturn]
	private static void FailNotSupported([CallerMemberName, ConstantExpected] string? name = default) => throw new NotSupportedException($"{name} is not supported.");

	public static Type Void { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_void())))); } } }

	public static Type UInt8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_uint8())))); } } }

	public static Type SInt8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_sint8())))); } } }

	public static Type UInt16 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_uint16())))); } } }

	public static Type SInt16 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_sint16())))); } } }

	public static Type UInt32 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_uint32())))); } } }

	public static Type SInt32 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_sint32())))); } } }

	public static Type UInt64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_uint64())))); } } }

	public static Type SInt64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_sint64())))); } } }

	public static Type Float { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_float())))); } } }

	public static Type Double { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_double())))); } } }

	public static Type Pointer { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return field ??= new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_pointer())))); } } }

	public static bool HasLongDoubleSupport { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Library.TypeHasLongDoubleSupport; }

	public static Type LongDouble
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				if (field is not null)
				{
					return field;
				}

				if (!HasLongDoubleSupport)
				{
					FailNotSupported();
				}

				return field = new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_longdouble()))));
			}
		}
	}

	public static bool HasComplexSupport { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Library.TypeHasComplexSupport; }

	public static Type ComplexFloat
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				if (field is not null)
				{
					return field;
				}

				if (!HasComplexSupport)
				{
					FailNotSupported();
				}

				return field = new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_complex_float()))));
			}
		}
	}

	public static Type ComplexDouble
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				if (field is not null)
				{
					return field;
				}

				if (!HasComplexSupport)
				{
					FailNotSupported();
				}

				return field = new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_complex_double()))));
			}
		}
	}

	public static bool HasComplexLongDoubleSupport { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Library.TypeHasComplexLongDoubleSupport; }

	public static Type ComplexLongDouble
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			unsafe
			{
				if (field is not null)
				{
					return field;
				}

				if (!HasComplexLongDoubleSupport)
				{
					FailNotSupported();
				}

				return field = new(unchecked((ffi_type*)Unsafe.AsPointer(ref Unsafe.AsRef(in ffi_type_complex_longdouble()))));
			}
		}
	}
}
