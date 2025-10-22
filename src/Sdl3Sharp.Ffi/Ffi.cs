using Sdl3Sharp.Ffi.Internal.Interop.NativeImportConditions;
using Sdl3Sharp.SourceGeneration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ffi;

public static partial class Ffi
{
	public static Version Version
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get => Unsafe.BitCast<uint, Version>(
			INativeImportCondition.Evaluate<IsLP64>()
				? unchecked((uint)ffi_get_version_number_LP64())
				: ffi_get_version_number_LLP64()
		);
	}
	
	[DoesNotReturn]
	static void FailArgumentNull(int index) => throw new ArgumentNullException(message: $"The argument at index {index} is null", paramName: default);

	[DoesNotReturn]
	static void FailCouldNotPromotedVariadicArgument(int index) => throw new ArgumentException(message: $"The argument at index {index} could not be promoted to a compatible type that's expected for variadic arguments", paramName: default);

	[DoesNotReturn]
	static void FailFixedArgumentTypeIncompatible(int index, System.Type type) => throw new ArgumentException(message: $"The type of the argument at index {index} \"{type}\" is not compatible with expected types for fixed arguments", paramName: default);

	[DoesNotReturn]
	static void FailReturnTypeIncompatible<TResult>(out TResult result) => throw new ArgumentException(message: $"The return type \"{typeof(TResult)}\" is not compatible with expected types for return values", paramName: nameof(result));

	[DoesNotReturn]
	static void FailVariadicArgumentTypeIncompatible(int index, System.Type type) => throw new ArgumentException(message: $"The type of the argument at index {index} \"{type}\" is not compatible with expected types for variadic arguments", paramName: default);

	[DoesNotReturn]
	private static void FailStatusNotOk(Status status) => throw new StatusException(status);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static Type TypeStruct(int size) => new(size: (nuint)size, alignment: default, TypeKind.Struct);	

	public static bool TryGetFixedArgumentTypeOrReturnTypeForClrType(System.Type? clrType, [NotNullWhen(true)] out Type? fixedArgumentTypeOrReturnType)
	{
		if (clrType is not null)
		{
			     if (clrType == typeof(void))                              { fixedArgumentTypeOrReturnType = Type.Void;       return true; }
			else if (clrType == typeof(bool)
				  || clrType == typeof(byte))                              { fixedArgumentTypeOrReturnType = Type.UInt8;      return true; }
			else if (clrType == typeof(sbyte))                             { fixedArgumentTypeOrReturnType = Type.SInt8;      return true; }
			else if (clrType == typeof(ushort)
				  || clrType == typeof(char))                              { fixedArgumentTypeOrReturnType = Type.UInt16;     return true; }
			else if (clrType == typeof(short))                             { fixedArgumentTypeOrReturnType = Type.SInt16;     return true; }
			else if (clrType == typeof(uint))                              { fixedArgumentTypeOrReturnType = Type.UInt32;     return true; }
			else if (clrType == typeof(int))                               { fixedArgumentTypeOrReturnType = Type.SInt32;     return true; }
			else if (clrType == typeof(ulong))                             { fixedArgumentTypeOrReturnType = Type.UInt64;     return true; }
			else if (clrType == typeof(long))                              { fixedArgumentTypeOrReturnType = Type.SInt64;     return true; }
			else if (clrType == typeof(float))                             { fixedArgumentTypeOrReturnType = Type.Float;      return true; }
			else if (clrType == typeof(double))                            { fixedArgumentTypeOrReturnType = Type.Double;     return true; }
			else if (clrType == typeof(nuint)
				  || clrType == typeof(nint)
				  || clrType.IsPointer
				  || clrType.IsFunctionPointer
				  || clrType.IsUnmanagedFunctionPointer)                   { fixedArgumentTypeOrReturnType = Type.Pointer;     return true; }
			else if (clrType.IsEnum)                                       { return TryGetFixedArgumentTypeOrReturnTypeForClrType(clrType.GetEnumUnderlyingType(), out fixedArgumentTypeOrReturnType); }
			else if (clrType.IsValueType
				  && RuntimeHelpers.SizeOf(clrType.TypeHandle) is var size
				  && size is > 0)                                          { fixedArgumentTypeOrReturnType = TypeStruct(size); return true; }
		}

		fixedArgumentTypeOrReturnType = null;
		return false;
	}

	public static bool TryGetVariadicArgumentTypeForClrType(System.Type? clrType, [NotNullWhen(true)] out Type? variadicArgumentType)
	{
		if (clrType is not null)
		{
			     if (clrType == typeof(bool)
				  || clrType == typeof(byte)
				  || clrType == typeof(sbyte)
				  || clrType == typeof(ushort)
				  || clrType == typeof(short)
				  || clrType == typeof(char)
				  || clrType == typeof(int))                               { variadicArgumentType = Type.SInt32;      return true; }
			else if (clrType == typeof(uint))                              { variadicArgumentType = Type.UInt32;      return true; }
			else if (clrType == typeof(ulong))                             { variadicArgumentType = Type.UInt64;      return true; }
			else if (clrType == typeof(long))                              { variadicArgumentType = Type.SInt64;      return true; }
			else if (clrType == typeof(float)
				  || clrType == typeof(double))                            { variadicArgumentType = Type.Double;      return true; }
			else if (clrType == typeof(nuint)
				  || clrType == typeof(nint)
				  || clrType.IsPointer
				  || clrType.IsFunctionPointer
				  || clrType.IsUnmanagedFunctionPointer)                   { variadicArgumentType = Type.Pointer;     return true; }
			else if (clrType.IsEnum)                                       { return TryGetVariadicArgumentTypeForClrType(clrType.GetEnumUnderlyingType(), out variadicArgumentType); }
			else if (clrType.IsValueType
				  && RuntimeHelpers.SizeOf(clrType.TypeHandle) is var size
				  && size is > 0)                                          { variadicArgumentType = TypeStruct(size); return true; }
		}

		variadicArgumentType = null;
		return false;
	}
	
	public static bool TryPromoteVariadicArgument(object? argument, [NotNullWhen(true)] out object? promotedArgument)
	{
		if (argument is not null)
		{
			var argType = argument.GetType();
						
			     if (argType == typeof(uint)
				  || argType == typeof(int)
				  || argType == typeof(ulong)
				  || argType == typeof(long)
				  || argType == typeof(double)
				  || argType == typeof(nuint)
				  || argType == typeof(nint)
				  || argType.IsPointer
				  || argType.IsFunctionPointer
				  || argType.IsUnmanagedFunctionPointer) { promotedArgument = argument;                                                   return true; }
			else if (argType == typeof(bool))            { promotedArgument = unchecked((int)Unsafe.BitCast<bool, byte>((bool)argument)); return true; }
			else if (argType == typeof(byte))            { promotedArgument = unchecked((int)(byte)argument);                             return true; }
			else if (argType == typeof(sbyte))           { promotedArgument = unchecked((int)(sbyte)argument);                            return true; }
			else if (argType == typeof(ushort))          { promotedArgument = unchecked((int)(ushort)argument);                           return true; }
			else if (argType == typeof(short))           { promotedArgument = unchecked((int)(short)argument);                            return true; }
			else if (argType == typeof(char))            { promotedArgument = unchecked((int)(char)argument);                             return true; }
			else if (argType == typeof(float))           { promotedArgument = unchecked((double)(float)argument);                         return true; }
			else if (argType.IsEnum)
			{
				argType = argType.GetEnumUnderlyingType();

				     if (argType == typeof(bool))        { promotedArgument = unchecked((int)Unsafe.BitCast<bool, byte>((bool)Convert.ChangeType(argument, typeof(bool)))); }
				else if (argType == typeof(byte)
					  || argType == typeof(sbyte)
					  || argType == typeof(ushort)
					  || argType == typeof(short))       { promotedArgument = Convert.ChangeType(argument, typeof(int)); }
				else if (argType == typeof(char))        { promotedArgument = unchecked((int)(char)Convert.ChangeType(argument, typeof(char))); }
				else                                     { promotedArgument = argument; }

				return true;
			}
			else if (argType.IsValueType)                { promotedArgument = argument;                                                   return true; }
		}

		promotedArgument = null;
		return false;
	}

	private unsafe static void InvokeImpl(Abi abi, IntPtr functionPointer, Type.ffi_type* rtype, void* rvalue, ReadOnlySpan<object> arguments)
	{
		var argTypeHandles = stackalloc GCHandle[arguments.Length];
		var argTypeHandle = argTypeHandles;

		try
		{
			var argValueHandles = stackalloc GCHandle[arguments.Length];
			var argValueHandle = argValueHandles;

			try
			{
				var argtypes = stackalloc Type.ffi_type*[arguments.Length + 1];					
				var argtype = argtypes;

				var argvalues = stackalloc void*[arguments.Length + 1];
				var argvalue = argvalues;

				var i = 0;
				foreach (var argument in arguments)
				{
					if (argument is null)
					{
						FailArgumentNull(i);
					}

					var argumentType = argument.GetType();
					if (!TryGetFixedArgumentTypeOrReturnTypeForClrType(argumentType, out var argType))
					{
						FailFixedArgumentTypeIncompatible(i, argumentType);
					}
					
					i++;
						
					*argTypeHandle++ = GCHandle.Alloc(argType, GCHandleType.Normal);
					*argtype++ = argType.Raw;

					var argHandle = GCHandle.Alloc(argument, GCHandleType.Pinned);
					*argValueHandle++ = argHandle;
					*argvalue++ = unchecked((void*)argHandle.AddrOfPinnedObject());
				}
				*argtype = null;
				*argvalue = null;
			
				Unsafe.SkipInit(out CallInterface.ffi_cif cif);

				var status = CallInterface.ffi_prep_cif(&cif, abi, unchecked((uint)arguments.Length), rtype, argtypes);

				if (status is not Status.Ok)
				{
					FailStatusNotOk(status);
				}

				CallInterface.ffi_call(&cif, unchecked((void*)functionPointer), rvalue, argvalues);
			}
			finally
			{
				while (argValueHandle > argValueHandles)
				{
					var handle = *--argValueHandle;
					if (handle.IsAllocated)
					{
						handle.Free();
					}
				}
			}			
		}
		finally
		{
			while (argTypeHandle > argTypeHandles)
			{
				var handle = *--argTypeHandle;
				if (handle.IsAllocated)
				{
					Unsafe.As<Type>(handle.Target)!.Dispose();
					handle.Free();
				}
			}
		}
	}

	public static void Invoke(Abi abi, IntPtr functionPointer, params ReadOnlySpan<object> arguments)
	{
		unsafe
		{
			Unsafe.SkipInit(out CallInterface.ffi_arg rvalue);

			InvokeImpl(abi, functionPointer, Type.Void.Raw, &rvalue, arguments);
		}
	}

	public static void Invoke<TResult>(Abi abi, IntPtr functionPointer, out TResult result, params ReadOnlySpan<object> arguments)
		where TResult : unmanaged
	{
		unsafe
		{
			if (!TryGetFixedArgumentTypeOrReturnTypeForClrType(typeof(TResult), out var returnType))
			{
				FailReturnTypeIncompatible(out result);
			}
			
			using (returnType)
			{
				if (Unsafe.SizeOf<TResult>() >= Unsafe.SizeOf<CallInterface.ffi_arg>())
				{
					fixed (TResult* rvalue = &result)
					{
						InvokeImpl(abi, functionPointer, returnType.Raw, rvalue, arguments);
					}
				}
				else
				{
					Unsafe.SkipInit(out CallInterface.ffi_arg rvalue);

					InvokeImpl(abi, functionPointer, returnType.Raw, &rvalue, arguments);

					result = *(TResult*)&rvalue;
				}
			}
		}
	}

	private unsafe static void InvokeVariadicImpl(Abi abi, IntPtr functionPointer, int fixedArguments, Type.ffi_type* rtype, void* rvalue, ReadOnlySpan<object> arguments)
	{
		var argTypeHandles = stackalloc GCHandle[arguments.Length];
		var argTypeHandle = argTypeHandles;

		try
		{
			var argValueHandles = stackalloc GCHandle[arguments.Length];
			var argValueHandle = argValueHandles;

			try
			{
				var argtypes = stackalloc Type.ffi_type*[arguments.Length + 1];					
				var argtype = argtypes;

				var argvalues = stackalloc void*[arguments.Length + 1];
				var argvalue = argvalues;

				var i = 0;
				foreach (var argument in arguments)
				{
					if (argument is null)
					{
						FailArgumentNull(i);
					}

					Type? argType;
					object? promotedArgument;
					if (i < fixedArguments)
					{
						var argumentType = argument.GetType();
						if (!TryGetFixedArgumentTypeOrReturnTypeForClrType(argumentType, out argType))
						{
							FailFixedArgumentTypeIncompatible(i, argumentType);
						}

						promotedArgument = argument;
					}
					else
					{
						var argumentType = argument.GetType();
						if (!TryGetVariadicArgumentTypeForClrType(argumentType, out argType))
						{
							FailVariadicArgumentTypeIncompatible(i, argumentType);
						}

						try
						{
							if (!TryPromoteVariadicArgument(argument, out promotedArgument))
							{
								FailCouldNotPromotedVariadicArgument(i);
							}
						}
						catch
						{
							argType.Dispose();
							throw;
						}
					}

					i++;
						
					*argTypeHandle++ = GCHandle.Alloc(argType, GCHandleType.Normal);
					*argtype++ = argType.Raw;

					var argHandle = GCHandle.Alloc(promotedArgument, GCHandleType.Pinned);
					*argValueHandle++ = argHandle;
					*argvalue++ = unchecked((void*)argHandle.AddrOfPinnedObject());
				}
				*argtype = null;
				*argvalue = null;
			
				Unsafe.SkipInit(out CallInterface.ffi_cif cif);

				var status = CallInterface.ffi_prep_cif_var(&cif, abi, unchecked((uint)fixedArguments), unchecked((uint)arguments.Length), rtype, argtypes);

				if (status is not Status.Ok)
				{
					FailStatusNotOk(status);
				}

				CallInterface.ffi_call(&cif, unchecked((void*)functionPointer), rvalue, argvalues);
			}
			finally
			{
				while (argValueHandle > argValueHandles)
				{
					var handle = *--argValueHandle;
					if (handle.IsAllocated)
					{
						handle.Free();
					}
				}
			}			
		}
		finally
		{
			while (argTypeHandle > argTypeHandles)
			{
				var handle = *--argTypeHandle;
				if (handle.IsAllocated)
				{
					Unsafe.As<Type>(handle.Target)!.Dispose();
					handle.Free();
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static void ValidateFixedArgumentsArgument(int fixedArguments, ReadOnlySpan<object> arguments)
	{
		if (fixedArguments is < 0 || fixedArguments > arguments.Length)
		{
			failFixedArgumentsArgumentOutOfRange();
		}

		[DoesNotReturn]
		static void failFixedArgumentsArgumentOutOfRange() => throw new ArgumentOutOfRangeException(nameof(fixedArguments));
	}

	public static void InvokeVariadic(Abi abi, IntPtr functionPointer, int fixedArguments, params ReadOnlySpan<object> arguments)
	{
		unsafe
		{
			ValidateFixedArgumentsArgument(fixedArguments, arguments);

			Unsafe.SkipInit(out CallInterface.ffi_arg rvalue);

			InvokeVariadicImpl(abi, functionPointer, fixedArguments, Type.Void.Raw, &rvalue, arguments);
		}
	}

	public static void InvokeVariadic<TResult>(Abi abi, IntPtr functionPointer, int fixedArguments, out TResult result, params ReadOnlySpan<object> arguments)
		where TResult : unmanaged
	{
		unsafe
		{			
			ValidateFixedArgumentsArgument(fixedArguments, arguments);

			if (!TryGetFixedArgumentTypeOrReturnTypeForClrType(typeof(TResult), out var returnType))
			{
				FailReturnTypeIncompatible(out result);
			}
			
			using (returnType)
			{
				if (Unsafe.SizeOf<TResult>() >= Unsafe.SizeOf<CallInterface.ffi_arg>())
				{
					fixed (TResult* rvalue = &result)
					{
						InvokeVariadicImpl(abi, functionPointer, fixedArguments, returnType.Raw, rvalue, arguments);
					}
				}
				else
				{
					Unsafe.SkipInit(out CallInterface.ffi_arg rvalue);

					InvokeVariadicImpl(abi, functionPointer, fixedArguments, returnType.Raw, &rvalue, arguments);

					result = *(TResult*)&rvalue;
				}
			}
		}
	}
}
