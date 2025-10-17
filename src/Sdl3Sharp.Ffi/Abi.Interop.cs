using Sdl3Sharp.Ffi.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ffi;

partial struct Abi
{
	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl), typeof(CallConvSuppressGCTransition)])]
	internal static partial uint ffi_get_default_abi();

	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Status ffi_get_struct_offsets(Abi abi, Type.ffi_type* struct_type, nuint* offsets);
}
