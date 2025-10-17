using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using unsafe Alloc = delegate* managed<nuint, void*>;
using unsafe Free = delegate* managed<void*, void>;

namespace Sdl3Sharp.Ffi;

partial class Ffi
{
	public static class Allocator
	{
		public unsafe static Alloc Alloc
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get;

			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			set
			{
				if (value is null)
				{
					failValueArgumentNull();
				}

				if ((void*)value != (void*)field)
				{
					field = value;
				}

				[DoesNotReturn]
				static void failValueArgumentNull() => throw new ArgumentNullException(nameof(value));
			}
		}

		public unsafe static Free Free
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get;

			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			set
			{
				if (value is null)
				{
					failValueArgumentNull();
				}

				if ((void*)value != (void*)field)
				{
					field = value;
				}

				[DoesNotReturn]
				static void failValueArgumentNull() => throw new ArgumentNullException(nameof(value));
			}
		}

#pragma warning disable IDE0079
#pragma warning disable CA2255
		[ModuleInitializer]
#pragma warning restore CA2255
#pragma warning restore IDE0079
		internal unsafe static void ModuleInitializer()
		{
			if (Alloc is null)
			{
				Alloc = &NativeMemory.Alloc;
			}

			if (Free is null)
			{
				Free = &NativeMemory.Free;
			}
		}
	}
}
