using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ffi;

public sealed partial class CallInterface : IDisposable
{	
	private unsafe ffi_cif* mPtr;
	private Type? mManagedReturnType;
	private Type[]? mManagedArgumentTypes;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe CallInterface(Type returnType)
	{
		if (returnType is null)
		{
			failReturnTypeArgumentNull();
		}

		mManagedReturnType = returnType;

		mPtr = unchecked((ffi_cif*)Ffi.Allocator.Alloc(unchecked((nuint)Unsafe.SizeOf<ffi_cif>())));

		if (mPtr is null)
		{
			FailCouldNotAllocateNativeMemory();
		}				

		[DoesNotReturn]
		static void failReturnTypeArgumentNull() => throw new ArgumentNullException(nameof(returnType));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe void InitializeNativeArgumentTypes(out Type.ffi_type** argtypes)
	{
		if (mManagedArgumentTypes!.Length is var length && length is > 0)
		{
			var argtypePtr = argtypes = unchecked((Type.ffi_type**)Ffi.Allocator.Alloc(unchecked((nuint)sizeof(Type.ffi_type*) * ((nuint)length + 1))));

			if (argtypes is null)
			{
				FailCouldNotAllocateNativeMemory();
			}

			foreach (var argtype in mManagedArgumentTypes)
			{
				*argtypePtr++ = argtype!.Raw;
			}
			*argtypePtr = null;
		}
		else
		{
			argtypes = null;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private CallInterface(Type returnType, ReadOnlySpan<Type> argumentTypes)
		: this(returnType)
	{
		try
		{
			if (argumentTypes.IsEmpty)
			{
				mManagedArgumentTypes = [];
			}
			else
			{
				mManagedArgumentTypes = argumentTypes.ToArray();
			}
		}
		catch
		{
			Dispose();
			throw;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private CallInterface(Type returnType, IEnumerable<Type> argumentTypes)
		: this(returnType)
	{
		try
		{
			if (argumentTypes is null)
			{
				mManagedArgumentTypes = [];
			}
			else if (argumentTypes is Type[] array)
			{
				mManagedArgumentTypes = array;
			}
			else if (argumentTypes.TryGetNonEnumeratedCount(out var count))
			{
				if (count is 0)
				{
					mManagedArgumentTypes = [];
				}
				else
				{
					mManagedArgumentTypes = GC.AllocateUninitializedArray<Type>(count);

					var i = 0;
					foreach (var type in argumentTypes)
					{
						mManagedArgumentTypes[i++] = type;
					}
				}
			}
			else
			{
				mManagedArgumentTypes = [..argumentTypes];
			}
		}
		catch
		{
			Dispose();
			throw;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe void InitializeCif(Abi abi, Type returnType)
	{
		try
		{
			InitializeNativeArgumentTypes(out var argtypes);

			var status = ffi_prep_cif(mPtr!, abi, unchecked((uint)mManagedArgumentTypes!.Length), returnType!.Raw, argtypes);

			if (status is not Status.Ok)
			{
				FailStatusNotOk(status);
			}
		}
		catch
		{
			Dispose();
			throw;
		}
	}

	public CallInterface(Abi abi, Type returnType, params ReadOnlySpan<Type> argumentTypes) :
		this(returnType, argumentTypes)
		=> InitializeCif(abi, returnType);

	public CallInterface(Abi abi, Type returnType, IEnumerable<Type> argumentTypes) :
		this(returnType, argumentTypes)
		=> InitializeCif(abi, returnType);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private unsafe void InitializeCifVar(Abi abi, int fixedArgumentsCount, Type returnType)
	{
		try
		{
			var length = mManagedArgumentTypes!.Length;

			if (fixedArgumentsCount is < 0 || fixedArgumentsCount > length)
			{
				failFixedArgumentsCountArgumentOutOfRange();
			}

			InitializeNativeArgumentTypes(out var argtypes);

			var status = ffi_prep_cif_var(mPtr!, abi, unchecked((uint)fixedArgumentsCount), unchecked((uint)length), returnType!.Raw, argtypes);

			if (status is not Status.Ok)
			{
				FailStatusNotOk(status);
			}
		}
		catch
		{
			Dispose();
			throw;
		}

		[DoesNotReturn]
		static void failFixedArgumentsCountArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(fixedArgumentsCount));
	}

	public CallInterface(Abi abi, int fixedArgumentsCount, Type returnType, params ReadOnlySpan<Type> argumentTypes) :
		this(returnType, argumentTypes)
		=> InitializeCifVar(abi, fixedArgumentsCount, returnType);

	public CallInterface(Abi abi, int fixedArgumentsCount, Type returnType, IEnumerable<Type> argumentTypes) :
		this(returnType, argumentTypes)
		=> InitializeCifVar(abi, fixedArgumentsCount, returnType);

	~CallInterface() => DisposeImpl();

	public Abi Abi { get { unsafe { return mPtr is not null ? mPtr->abi : default; } } }

	public IReadOnlyList<Type> ArgumentTypes { get => mManagedArgumentTypes ?? []; }

	public Type ReturnType { get => mManagedReturnType ?? Type.Void; }

	public uint Bytes { get { unsafe { return mPtr is not null ? mPtr->bytes : default; } } }

	public uint Flags { get { unsafe { return mPtr is not null ? mPtr->flags : default; } } }
	
	[DoesNotReturn]
	private static void FailCouldNotAllocateNativeMemory() => throw new OutOfMemoryException();	

	[DoesNotReturn]
	private static void FailStatusNotOk(Status status) => throw new StatusException(status);

	private unsafe void InvokeImpl(IntPtr functionPointer, void* rvalue, params ReadOnlySpan<object> arguments)
	{
		unsafe
		{
			if (arguments.Length != mManagedArgumentTypes?.Length)
			{
				failArgumentsLengthMismatch();
			}

			var gcHandles = stackalloc GCHandle[arguments.Length];
			var gcHandle = gcHandles;

			try
			{
				var argvalues = stackalloc void*[arguments.Length + 1];
				var argvalue = argvalues;

				var i = 0;
				foreach (var argument in arguments)
				{
					if (argument is null)
					{
						failArgumentNull(i);
					}

					var argumentType = argument.GetType();
					if (!mManagedArgumentTypes[i].IsCompatibleWithClrType(argumentType, ignoreIntegralSignedness: true))
					{
						failArgumentTypeIncompatible(i, argumentType);
					}

					i++;

					var argHandle = GCHandle.Alloc(argument, GCHandleType.Pinned);
					*gcHandle++ = argHandle;
					*argvalue++ = unchecked((void*)argHandle.AddrOfPinnedObject());
				}
				*argvalue = null;

				ffi_call(mPtr, unchecked((void*)functionPointer), rvalue, argvalues);
			}
			finally
			{
				while (gcHandle > gcHandles)
				{
					var handle = *--gcHandle;
					if (handle.IsAllocated)
					{
						handle.Free();
					}
				}
			}
		}

		[DoesNotReturn]
		static void failArgumentsLengthMismatch() => throw new ArgumentException(message: $"The number of {nameof(arguments)} given doesn't match the length of {nameof(ArgumentTypes)}", paramName: nameof(arguments));

		[DoesNotReturn]
		static void failArgumentNull(int index) => throw new ArgumentNullException(message: $"The argument at index {index} is null", paramName: default);

		[DoesNotReturn]
		static void failArgumentTypeIncompatible(int index, System.Type type) => throw new ArgumentException(message: $"The argument type at index {index} \"{type}\" is not compatible with the corresponding argument type in {nameof(ArgumentTypes)}", paramName: default);
	}	

	public void Invoke(IntPtr functionPointer, params ReadOnlySpan<object> arguments)
	{
		unsafe
		{
			if (mManagedReturnType?.IsCompatibleWithClrType(typeof(void)) is not true)
			{
				failReturnTypeIncompatible();
			}

			Unsafe.SkipInit(out ffi_arg rvalue);

			InvokeImpl(functionPointer, &rvalue, arguments);
		}

		[DoesNotReturn]
		static void failReturnTypeIncompatible() => throw new InvalidOperationException("The return type is not compatible with void");
	}

	public void Invoke<TResult>(IntPtr functionPointer, out TResult result, params ReadOnlySpan<object> arguments)
		where TResult : unmanaged
	{
		unsafe
		{
			if (mManagedReturnType?.IsCompatibleWithClrType(typeof(TResult), ignoreIntegralSignedness: true) is not true)
			{
				failReturnTypeIncompatible();
			}

			if (Unsafe.SizeOf<TResult>() >= Unsafe.SizeOf<ffi_arg>())
			{
				fixed (TResult* rvalue = &result)
				{
					InvokeImpl(functionPointer, rvalue, arguments);
				}
			}
			else
			{
				Unsafe.SkipInit(out ffi_arg rvalue);

				InvokeImpl(functionPointer, &rvalue, arguments);

				result = *(TResult*)&rvalue;
			}
		}		

		[DoesNotReturn]
		static void failReturnTypeIncompatible() => throw new ArgumentException(message: $"The return type \"{typeof(TResult)}\" is not compatible with {nameof(ReturnType)}", paramName: nameof(result));
	}

	public void Dispose()
	{
		DisposeImpl();
		GC.SuppressFinalize(this);
	}

	private unsafe void DisposeImpl()
	{
		if (mPtr is not null)
		{
			// we're responsible for the native memory (of type ffi_cif) behind the mPtr pointer in any case

			if (mManagedReturnType is not null)
			{
				if (mPtr->rtype is not null)
				{
					mPtr->rtype = null;
				}

				mManagedReturnType = null;
			}

			if (mManagedArgumentTypes is not null)
			{
				if (mPtr->arg_types is not null)
				{					
					// we're responsible for the native memory backing the mPtr->arg_types field in any case

					Ffi.Allocator.Free(mPtr->arg_types);
					mPtr->arg_types = null;
				}

				mManagedArgumentTypes = null;
			}

			Ffi.Allocator.Free(mPtr);
			mPtr = null;
		}
	}
}
