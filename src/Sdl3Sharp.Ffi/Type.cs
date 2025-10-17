using Sdl3Sharp.Ffi.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ffi;

public sealed partial class Type : IDisposable
{
	[DoesNotReturn]
	private static void FailCouldNotAllocateNativeMemory() => throw new OutOfMemoryException();

	private unsafe ffi_type* mPtr;
	private Type[]? mManagedElements;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe Type(ffi_type* ptr) 
	{
		mPtr = ptr;
		mManagedElements = null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe Type(nuint size, ushort alignment, TypeKind kind)
	{
		mPtr = unchecked((ffi_type*)Ffi.Allocator.Alloc(unchecked((nuint)Unsafe.SizeOf<ffi_type>())));

		if (mPtr is null)
		{
			FailCouldNotAllocateNativeMemory();
		}

		mPtr->size = size;
		mPtr->alignment = alignment;
		mPtr->type = kind;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe void InitializeNativeElementTypes()
	{
		if (mManagedElements!.Length is var length && length is > 0)
		{
			var elementPtr = mPtr!->elements = unchecked((ffi_type**)Ffi.Allocator.Alloc(unchecked((nuint)sizeof(ffi_type*) * ((nuint)length + 1))));

			if (mPtr!->elements is null)
			{
				FailCouldNotAllocateNativeMemory();
			}

			foreach (var element in mManagedElements)
			{
				*elementPtr++ = element!.mPtr;
			}
			*elementPtr = null;
		}
		else
		{
			mPtr!->elements = null;
		}
	}

	public Type(nuint size, ushort alignment, TypeKind kind, params ReadOnlySpan<Type> elements)
		: this(size, alignment, kind)
	{
		try
		{
			mManagedElements = elements.ToArray();

			InitializeNativeElementTypes();
		}
		catch
		{
			Dispose();
			throw;
		}
	}

	public Type(nuint size, ushort alignment, TypeKind kind, IEnumerable<Type> elements) :
		this(size, alignment, kind)
	{
		try
		{
			if (elements is null)
			{
				mManagedElements = [];
			}
			else if (elements is Type[] array)
			{
				mManagedElements = array;
			}
			else if (elements.TryGetNonEnumeratedCount(out var count))
			{
				if (count is 0)
				{
					mManagedElements = [];
				}
				else
				{
					mManagedElements = GC.AllocateUninitializedArray<Type>(count);

					var i = 0;
					foreach (var element in elements)
					{
						mManagedElements[i++] = element;
					}
				}
			}
			else
			{
				mManagedElements = [..elements];
			}

			InitializeNativeElementTypes();
		}
		catch
		{
			Dispose();
			throw;
		}
	}

	public Type(TypeKind kind, params ReadOnlySpan<Type> elements) : 
		this(size: 0, alignment: 0, kind, elements)
	{ }

	public Type(TypeKind kind, IEnumerable<Type> elements) :
		this(size: 0, alignment: 0, kind, elements)
	{ }

	~Type() => DisposeImpl();

	internal unsafe ffi_type* Raw { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mPtr; }

	public nuint Size { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mPtr is not null ? mPtr->size : default; } } }

	public ushort Alignment { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mPtr is not null ? mPtr->alignment : default; } } }

	public TypeKind Kind { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get { unsafe { return mPtr is not null ? mPtr->type : default; } } }

	public IReadOnlyList<Type> Elements { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mManagedElements ?? []; /* none of the predefined types have actual (sub) element types, so this is okay */ }

	internal Type[] ElementsArray { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mManagedElements ?? []; /* see above */ }

	public void Dispose()
	{
		DisposeImpl();
		GC.SuppressFinalize(this); // this is fine, even when called on a predefined Type, as predefined Type don't get modified by a call to DisposeImpl and don't necessarily need to be finalized
	}

	private unsafe void DisposeImpl()
	{
		if (mPtr is not null)
		{
			if (mManagedElements is not null)
			{
				// this means we're responsible for the native memory (of type ffi_type) behind the mPtr pointer

				if (mPtr->elements is not null)
				{
					// this means we're responsible for the native memory backing the mPtr->elements field

					Ffi.Allocator.Free(mPtr->elements);
					mPtr->elements = null;
				}

				Ffi.Allocator.Free(mPtr);				
				mPtr = null; // only in this case will mPtr be set to null, this prevents disposing of predefined Types

				mManagedElements = null;
			}
		}
	}

	public bool IsCompatibleWithClrType(System.Type clrType, bool ignoreIntegralSignedness = false)
	{
		if (clrType is null)
		{
			failClrTypeArgumentNull();
		}

		return Kind switch
		{
			TypeKind.Void => clrType == typeof(void),
			TypeKind.Int => clrType == typeof(int) || (Unsafe.SizeOf<nint>() == sizeof(int) && clrType == typeof(nint)) || (ignoreIntegralSignedness && (clrType == typeof(uint) || (Unsafe.SizeOf<nuint>() == sizeof(int) && clrType == typeof(nuint)))),
			TypeKind.Float => clrType == typeof(float),
			TypeKind.Double => clrType == typeof(double),
			TypeKind.LongDouble => false, // simply return false for now
			TypeKind.UInt8 => clrType == typeof(byte) || clrType == typeof(bool) || (ignoreIntegralSignedness && clrType == typeof(sbyte)),
			TypeKind.SInt8 => clrType == typeof(sbyte) || clrType == typeof(bool) || (ignoreIntegralSignedness && clrType == typeof(byte)),
			TypeKind.UInt16 => clrType == typeof(ushort) || clrType == typeof(char) || (ignoreIntegralSignedness && clrType == typeof(short)),
			TypeKind.SInt16 => clrType == typeof(short) || clrType == typeof(char) || (ignoreIntegralSignedness && clrType == typeof(ushort)),
			TypeKind.UInt32 => clrType == typeof(uint) || (Unsafe.SizeOf<nuint>() == sizeof(uint) && clrType == typeof(nuint)) || (ignoreIntegralSignedness && (clrType == typeof(int) || (Unsafe.SizeOf<nint>() == sizeof(int) && clrType == typeof(nint)))),
			TypeKind.SInt32 => clrType == typeof(int) || (Unsafe.SizeOf<nint>() == sizeof(int) && clrType == typeof(nint)) || (ignoreIntegralSignedness && (clrType == typeof(uint) || (Unsafe.SizeOf<nuint>() == sizeof(int) && clrType == typeof(nuint)))),
			TypeKind.UInt64 => clrType == typeof(ulong) || (Unsafe.SizeOf<nuint>() == sizeof(ulong) && clrType == typeof(nuint)) || (ignoreIntegralSignedness && (clrType == typeof(long) || (Unsafe.SizeOf<nint>() == sizeof(ulong) && clrType == typeof(nint)))),
			TypeKind.SInt64 => clrType == typeof(long) || (Unsafe.SizeOf<nint>() == sizeof(long) && clrType == typeof(nint)) || (ignoreIntegralSignedness && (clrType == typeof(ulong) || (Unsafe.SizeOf<nuint>() == sizeof(long) && clrType == typeof(nuint)))),
			TypeKind.Struct => clrType.IsValueType && unchecked((nuint)Helpers.SizeOfOrDefault(clrType)) <= Size,
			TypeKind.Pointer => clrType == typeof(nint) || clrType == typeof(nuint) || clrType.IsPointer || clrType.IsFunctionPointer || clrType.IsUnmanagedFunctionPointer,
			TypeKind.Complex => false, // simply return false for now
			_ => false
		};

		[DoesNotReturn]
		static void failClrTypeArgumentNull() => throw new ArgumentNullException(nameof(clrType));
	}
}
