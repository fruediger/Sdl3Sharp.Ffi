using Sdl3Sharp.Ffi.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ffi;

partial class CallInterface
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct ffi_cif
	{
		public Abi abi;
		public uint nargs;
		public Type.ffi_type** arg_types;
		public Type.ffi_type* rtype;
		public uint bytes;
		public uint flags;
		private ExtraFields _;

		[InlineArray(32)] // Allocating 32 extra bytes should be enough for each platform
		private struct ExtraFields { private byte _; }
	}	

	[InlineArray(2)] // This larger than what ffi_arg in libffi is usally defined, but it doesn't hurt (too much) and aids towards it's safetiness
	internal struct ffi_arg { private nint _; }

	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Status ffi_prep_cif(ffi_cif* cif, Abi abi, uint nargs, Type.ffi_type* rtype, Type.ffi_type** argtypes);

	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial Status ffi_prep_cif_var(ffi_cif* cif, Abi abi, uint nfixedargs, uint ntotalargs, Type.ffi_type* rtype, Type.ffi_type** argtypes);

	[NativeImportFunction<Library>(CallConvs = [typeof(CallConvCdecl)])]
	internal unsafe static partial void ffi_call(ffi_cif* cif, void* fn, void* rvalue, void** avalues);
}
