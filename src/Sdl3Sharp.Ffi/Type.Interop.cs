using Sdl3Sharp.Ffi.Internal.Interop;
using Sdl3Sharp.SourceGeneration;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Ffi;

partial class Type
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct ffi_type
	{
		public nuint size;
		public ushort alignment;
		public TypeKind type;
		public ffi_type** elements;
	}

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_void();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_uint8();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_sint8();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_uint16();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_sint16();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_uint32();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_sint32();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_uint64();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_sint64();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_float();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_double();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_pointer();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_longdouble();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_complex_float();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_complex_double();

	[NativeImportSymbol<Library>(Kind = NativeImportSymbolKind.Reference)]
	internal static partial ref readonly ffi_type ffi_type_complex_longdouble();
}
