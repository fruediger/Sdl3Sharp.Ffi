﻿using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ffi.Internal.Interop.NativeImportConditions;

internal sealed class AndAlso<TLeftCondition, TRightCondition> : INativeImportCondition
	where TLeftCondition : INativeImportCondition
	where TRightCondition : INativeImportCondition
{
	private AndAlso() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	static bool INativeImportCondition.Evaluate() => INativeImportCondition.Evaluate<TLeftCondition>() && INativeImportCondition.Evaluate<TRightCondition>();
}
