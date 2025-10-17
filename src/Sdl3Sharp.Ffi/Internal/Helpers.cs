using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ffi.Internal;

internal static class Helpers
{
	private static readonly MethodInfo UnsafeSizeOf = typeof(Unsafe).GetMethod(nameof(Unsafe.SizeOf))!;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal static int SizeOfOrDefault(System.Type type) => RuntimeFeature.IsDynamicCodeSupported
		? (int)UnsafeSizeOf.MakeGenericMethod(type).Invoke(null, null)!
		: default;
}
