using Sdl3Sharp.Ffi.Internal.Interop;
using Sdl3Sharp.Ffi.Internal.Interop.NativeImportConditions;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ffi;

partial class Ffi
{
	[NativeImportFunction<Library, IsLP64>("ffi_get_version_number", CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial ulong ffi_get_version_number_LP64();

	[NativeImportFunction<Library, Not<IsLP64>>("ffi_get_version_number", CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial uint ffi_get_version_number_LLP64();
}
