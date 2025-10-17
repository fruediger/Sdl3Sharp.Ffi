namespace Sdl3Sharp.Ffi;

public enum TypeKind : ushort
{
	Void       = 0,
	Int        = 1,
	Float      = 2,
	Double     = 3,
	LongDouble = 4,
	UInt8      = 5,
	SInt8      = 6,
	UInt16     = 7,
	SInt16     = 8,
	UInt32     = 9,
	SInt32     = 10,
	UInt64     = 11,
	SInt64     = 12,
	Struct     = 13,
	Pointer    = 14,
	Complex    = 15,
}
