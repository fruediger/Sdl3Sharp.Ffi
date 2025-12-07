using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ffi.Internal.Interop;

internal sealed class Library : INativeImportLibrary
{
	static (string? libraryName, DllImportSearchPath? searchPath) INativeImportLibrary.GetLibraryNameAndSearchPath() => (
		"libffi",
		DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.UseDllDirectoryForDependencies | DllImportSearchPath.ApplicationDirectory | DllImportSearchPath.UserDirectories
	);

	static bool INativeImportLibrary.HandleSymbolImportError(string? symbolName, ExceptionDispatchInfo? symbolLoadErrorInfo)
	{
		switch (symbolName)
		{
			case nameof(Type.ffi_type_longdouble):
			{
				mTypeHasLongDoubleSupport = false;
				return true;
			}
			
			case nameof(Type.ffi_type_complex_float):
			case nameof(Type.ffi_type_complex_double):
			{
				mTypeHasComplexSupport = false;
				return true;
			}

			case nameof(Type.ffi_type_complex_longdouble):
			{
				mTypeHasComplexLongDoubleSupport = false;
				return true;
			}
		}

		symbolLoadErrorInfo?.Throw();

		return true;
	}

	private static bool mTypeHasLongDoubleSupport = true;
	public static bool TypeHasLongDoubleSupport { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTypeHasLongDoubleSupport; }

	private static bool mTypeHasComplexSupport = true;
	public static bool TypeHasComplexSupport { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTypeHasComplexSupport; }

	private static bool mTypeHasComplexLongDoubleSupport = true;
	public static bool TypeHasComplexLongDoubleSupport { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => mTypeHasComplexLongDoubleSupport; }
}
