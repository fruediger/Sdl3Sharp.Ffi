using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ffi.Internal.Interop;

internal sealed class Library : INativeImportLibrary
{
	internal static readonly bool TypeHasLongDoubleSupport        = true,
						          TypeHasComplexSupport           = true,
						          TypeHasComplexLongDoubleSupport = true;

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
				Unsafe.AsRef(in TypeHasLongDoubleSupport) = false;
				return true;
			}
			
			case nameof(Type.ffi_type_complex_float):
			case nameof(Type.ffi_type_complex_double):
			{
				Unsafe.AsRef(in TypeHasComplexSupport) = false;
				return true;
			}

			case nameof(Type.ffi_type_complex_longdouble):
			{
				Unsafe.AsRef(in TypeHasComplexLongDoubleSupport) = false;
				return true;
			}
		}

		symbolLoadErrorInfo?.Throw();

		return true;
	}
}
