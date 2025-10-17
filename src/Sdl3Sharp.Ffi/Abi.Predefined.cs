using System.Runtime.CompilerServices;

namespace Sdl3Sharp.Ffi;

partial struct Abi
{	
	public static Abi Default { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ffi_get_default_abi()); }

	public static class AArch64
	{
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi Win64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }
	}

	public static class Alpha
	{
		public static Abi Osf { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Arc
	{
		public static Abi Arc64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi Arcompact { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Arm
	{
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi Vfp { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }
	}

	public static class Avr32
	{
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class BFin
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Cris
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Csky
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class FrV
	{		
		public static Abi EAbi { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Ia64
	{		
		public static Abi Unix { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Kvx
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class LoongArch64
	{		
		public static Abi LP64S { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi LP64F { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }

		public static Abi LP64D { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(3); }
	}

	public static class M32R
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class M68K
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class M88K
	{		
		public static Abi Obsd { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Metag
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class MicroBlaze
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Mips
	{		
		public static Abi O32 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi N32 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }

		public static Abi N64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(3); }

		public static Abi O32SoftFloat { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(4); }

		public static Abi N32SoftFloat { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(5); }

		public static Abi N64SoftFloat { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(6); }
	}

	public static class Moxie
	{		
		public static Abi EAbi { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class OR1K
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Pa
	{		
		public static Abi Pa32 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi Pa64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class PowerPC
	{		
		public static Abi Aix { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi Darwin { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }

		public static Abi CompatSysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi CompatGccSysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }
		
		public static Abi CompatLinux64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(3); }

		public static Abi CompatLinux { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(4); }
		
		public static Abi CompatLinuxSoftFloat { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(5); }		
		
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Abi Linux(bool structAlign, bool longDouble128, bool longDoubleIeee128)
		{
			var value = 8u;

			if (structAlign)
			{
				value |= 1;
			}

			if (longDouble128)
			{
				value |= 2;

				if (longDoubleIeee128)
				{
					value |= 4;
				}
			}

			return new(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Abi SysV(bool softFloat, bool structReturn, bool longDoubleIBM, bool longDouble128)
		{
			var value = 8u;

			if (softFloat)
			{
				value |= 1;
			}

			if (structReturn)
			{
				value |= 2;
			}

			if (longDoubleIBM)
			{
				value |= 4;
			}

			if (longDouble128)
			{
				value |= 16;
			}

			return new(value);
		}
	}

	public static class RiscV
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi Unused1 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }

		public static Abi Unused2 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(3); }

		public static Abi Unused3 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(4); }
	}

	public static class S390
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class SH
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class SH64
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Sparc
	{		
		public static Abi V9 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi V8 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Tile
	{		
		public static Abi Unix { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Vax
	{		
		public static Abi ElfBsd { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}

	public static class Wasm
	{		
		public static Abi Wasm32 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi Wasm32Emscripten { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }

		public static Abi Wasm64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

		public static Abi Wasm64Emscripten { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }
	}

	public static class X86
	{
		public static class W64
		{
			public static Abi Win64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

			public static Abi GnuW64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }
		}

		public static class E64
		{
			public static Abi Unix64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }

			public static Abi Win64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(3); }

			public static Abi Efi64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(3); }

			public static Abi GnuW64 { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(4); }
		}

		public static class W32
		{
			public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

			public static Abi StdCall { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(2); }

			public static Abi ThisCall { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(3); }

			public static Abi FastCall { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(4); }

			public static Abi MsCdecl { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(5); }

			public static Abi Pascal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(6); }

			public static Abi Register { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(7); }
		}

		public static class E32
		{
			public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }

			public static Abi ThisCall { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(3); }

			public static Abi FastCall { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(4); }

			public static Abi StdCall { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(5); }

			public static Abi Pascal { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(6); }

			public static Abi Register { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(7); }

			public static Abi MsCdecl { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(8); }
		}
	}

	public static class Xtensa
	{		
		public static Abi SysV { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(1); }
	}
}
