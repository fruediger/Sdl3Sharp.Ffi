using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ffi.Internal.Interop.NativeImportConditions;

internal sealed class Not<TCondition> : INativeImportCondition
	where TCondition : INativeImportCondition
{
	private Not() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool INativeImportCondition.Evaluate() => !INativeImportCondition.Evaluate<TCondition>();
}
