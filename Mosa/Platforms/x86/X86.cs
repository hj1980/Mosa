﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// Returns X86 opcodes.
	/// </summary>
	public static class X86
	{

		#region X86Intructions

#pragma warning disable 1591
		public static class X86Intruction
		{
			public static class Add
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 0);
				public static OpCode R_R = new OpCode(new byte[] { 0x03 });
				public static OpCode R_M_I8 = new OpCode(new byte[] { 0x02 });
				public static OpCode R_M = new OpCode(new byte[] { 0x03 });
				public static OpCode M_R_I8 = new OpCode(new byte[] { 0x00 });
				public static OpCode M_R = new OpCode(new byte[] { 0x01 });
			}
			public static class And
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 4);
				public static OpCode M_C = new OpCode(new byte[] { 0x81 }, 4);
				public static OpCode R_M = new OpCode(new byte[] { 0x23 });
				public static OpCode R_R = new OpCode(new byte[] { 0x23 });
				public static OpCode M_R = new OpCode(new byte[] { 0x21 });
			}
			public static class Div
			{
				public static OpCode R = new OpCode(new byte[] { 0xF7 }, 6);
				public static OpCode R_I8 = new OpCode(new byte[] { 0xF6 }, 6);
				public static OpCode R_I16 = new OpCode(new byte[] { 0x66, 0xF7 }, 6);
				public static OpCode M = new OpCode(new byte[] { 0xF7 }, 6);
				public static OpCode M_I8 = new OpCode(new byte[] { 0xF6 }, 6);
				public static OpCode M_16 = new OpCode(new byte[] { 0x66, 0xF7 }, 6);
			}
			public static class Mov
			{
				public static OpCode R_C = new OpCode(new byte[] { 0xC7 }, 0);
				public static OpCode M_C = new OpCode(new byte[] { 0xC7 }, 0);
				public static OpCode R_R = new OpCode(new byte[] { 0x8B });
				public static OpCode R_M = new OpCode(new byte[] { 0x8B });
				public static OpCode R_M_I8 = new OpCode(new byte[] { 0x0F, 0xBE });
				public static OpCode R_M_U8 = new OpCode(new byte[] { 0x0F, 0xB6 });
				public static OpCode R_M_I16 = new OpCode(new byte[] { 0x0F, 0xBF });
				public static OpCode R_M_U16 = new OpCode(new byte[] { 0x0F, 0xB7 });
				public static OpCode M_R = new OpCode(new byte[] { 0x89 });
				public static OpCode M_R_I8 = new OpCode(new byte[] { 0x88 });
			}
			public static class Neg
			{
				public static OpCode R = new OpCode(new byte[] { 0xF7 }, 3);
				public static OpCode M = new OpCode(new byte[] { 0xF7 }, 3);
				public static OpCode M_I8 = new OpCode(new byte[] { 0xF6 }, 3);
			}
			public static class Or
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 1);
				public static OpCode M_C = new OpCode(new byte[] { 0x81 }, 1);
				public static OpCode R_M = new OpCode(new byte[] { 0x0B });
				public static OpCode R_R = new OpCode(new byte[] { 0x0B });
				public static OpCode M_R = new OpCode(new byte[] { 0x09 });
			}
			public static class Shl
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xD3 }, 4);
				public static OpCode M_R = new OpCode(new byte[] { 0xD3 }, 4);
				public static OpCode R_C = new OpCode(new byte[] { 0xC1 }, 4);
				public static OpCode M_C = new OpCode(new byte[] { 0xC1 }, 4);
			}
			public static class Shr
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xD3 }, 5);
				public static OpCode M_R = new OpCode(new byte[] { 0xD3 }, 5);
				public static OpCode R_C = new OpCode(new byte[] { 0xC1 }, 5);
				public static OpCode M_C = new OpCode(new byte[] { 0xC1 }, 5);
			}
			public static class Sub
			{
				public static OpCode O_C = new OpCode(new byte[] { 0x81 }, 5);
				public static OpCode R_O = new OpCode(new byte[] { 0x2B });
				public static OpCode M_R = new OpCode(new byte[] { 0x29 });
				public static OpCode M_R_I8 = new OpCode(new byte[] { 0x28 });

			}
			public static class Xor
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 6);
				public static OpCode R_M = new OpCode(new byte[] { 0x33 });
				public static OpCode R_R = new OpCode(new byte[] { 0x33 });
				public static OpCode M_R = new OpCode(new byte[] { 0x31 });
			}
			public static class Cwd
			{
				public static OpCode ALL = new OpCode(new byte[] { 0x99 });
			}
			public static class Adc
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 });
				public static OpCode R_R = new OpCode(new byte[] { 0x11 });
				public static OpCode R_M = new OpCode(new byte[] { 0x13 });
				public static OpCode M_R = new OpCode(new byte[] { 0x11 });
			}
			public static class Out8
			{
				public static OpCode C_R = new OpCode(new byte[] { 0xE6 });
				public static OpCode R_R = new OpCode(new byte[] { 0xEE });
			}
			public static class Out32
			{
				public static OpCode C_R = new OpCode(new byte[] { 0xE7 });
				public static OpCode R_R = new OpCode(new byte[] { 0xEF });
			}
			public static class Xchg
			{
				public static OpCode R_M = new OpCode(new byte[] { 0x87 });
				public static OpCode R_R = new OpCode(new byte[] { 0x87 });
				public static OpCode M_R = new OpCode(new byte[] { 0x87 });
			}
			public static class Xsave
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0xAE }, 4);
			}
			public static class Dec
			{
				public static OpCode R = new OpCode(new byte[] { 0xFF }, 1);
				public static OpCode M = new OpCode(new byte[] { 0xFF }, 1);
			}
			public static class Inc
			{
				public static OpCode R = new OpCode(new byte[] { 0xFF }, 0);
				public static OpCode M = new OpCode(new byte[] { 0xFF }, 0);
			}
			public static class Not
			{
				public static OpCode R_M = new OpCode(new byte[] { 0xF7 }, 2);
				public static OpCode R_R = new OpCode(new byte[] { 0xF7 }, 2);
				public static OpCode M_R = new OpCode(new byte[] { 0xF7 }, 2);
			}
			public static class Cmp
			{
				public static OpCode M_R = new OpCode(new byte[] { 0x39 });
				public static OpCode R_M = new OpCode(new byte[] { 0x3B });
				public static OpCode R_R = new OpCode(new byte[] { 0x3B });
				public static OpCode M_C = new OpCode(new byte[] { 0x81 }, 7);
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 7);
			}
			public static class Cmpxchg
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0xB1 });
				public static OpCode M_R = new OpCode(new byte[] { 0x0F, 0xB1 });
			}
			public static class Mul
			{
				public static OpCode R = new OpCode(new byte[] { 0xF7 }, 4);
				public static OpCode M = new OpCode(new byte[] { 0xF7 }, 4);
			}
			public static class Sar
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xD3 }, 7);
				public static OpCode M_R = new OpCode(new byte[] { 0xD3 }, 7);
				public static OpCode R_C = new OpCode(new byte[] { 0xC1 }, 7);
				public static OpCode M_C = new OpCode(new byte[] { 0xC1 }, 7);
			}
			public static class Sfence
			{
				public static OpCode ALL = new OpCode(new byte[] { 0x0F, 0xAE }, 7);
			}
			public static class Sgdt
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 0);
			}
			public static class Sidt
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 1);
			}
			public static class Sldt
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x00 }, 0);
			}
			public static class Smsw
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 4);
				public static OpCode R = new OpCode(new byte[] { 0x0F, 0x01 }, 4);
			}
			public static class Stmxcsr
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0xAE }, 3);
			}
			public static class Shr_const
			{
				public static OpCode R = new OpCode(new byte[] { 0xD1 }, 5);
				public static OpCode M = new OpCode(new byte[] { 0xD1 }, 5);
			}
			public static class Rcr
			{
				public static OpCode R = new OpCode(new byte[] { 0xD1 }, 3);
				public static OpCode M = new OpCode(new byte[] { 0xD1 }, 3);
			}
			public static class Idiv
			{
				public static OpCode R = new OpCode(new byte[] { 0xF7 }, 7);
				public static OpCode M = new OpCode(new byte[] { 0xF7 }, 7);
			}
			public static class In8
			{
				public static OpCode R_C = new OpCode(new byte[] { 0xE4 });
				public static OpCode R_R = new OpCode(new byte[] { 0xEC });
			}
			public static class In32
			{
				public static OpCode R_C = new OpCode(new byte[] { 0xE5 });
				public static OpCode R_R = new OpCode(new byte[] { 0xED });
			}
			public static class Lgdt
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 2);
			}
			public static class Lidt
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 3);
			}
			public static class Lldt
			{
				public static OpCode R = new OpCode(new byte[] { 0x0F, 0x00 }, 2);
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x00 }, 2);
			}
			public static class Lmsw
			{
				public static OpCode R = new OpCode(new byte[] { 0x0F, 0x01 }, 6);
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 6);
			}
			public static class Movsx8
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0xBE });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0xBE });
			}
			public static class Movsx16
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0xBF });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0xBF });
			}
			public static class Movzx8
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0xB6 });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0xB6 });
			}
			public static class Movzx16
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0xB7 });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0xB7 });
			}
			public static class Sbb
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 3);
				public static OpCode M_C = new OpCode(new byte[] { 0x81 }, 3);
				public static OpCode R_R = new OpCode(new byte[] { 0x19 });
				public static OpCode M_R = new OpCode(new byte[] { 0x19 });
				public static OpCode R_M = new OpCode(new byte[] { 0x1B });
			}
			public static class Movsd
			{
				public static OpCode R_L = new OpCode(new byte[] { 0xF2, 0x0F, 0x10 });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x10 });
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x10 });
				public static OpCode M_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x11 });
			}
			public static class Movss
			{
				public static OpCode R_L = new OpCode(new byte[] { 0xF3, 0x0F, 0x10 });
				public static OpCode R_M = new OpCode(new byte[] { 0xF3, 0x0F, 0x10 });
				public static OpCode R_R = new OpCode(new byte[] { 0xF3, 0x0F, 0x10 });
				public static OpCode M_R = new OpCode(new byte[] { 0xF3, 0x0F, 0x11 });
			}
			public static class Cvtsd2ss
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x5A });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x5A });
			}
			public static class Cvtsi2sd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x2A });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x2A });
			}
			public static class Cvtsi2ss
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF3, 0x0F, 0x2A });
				public static OpCode R_M = new OpCode(new byte[] { 0xF3, 0x0F, 0x2A });
			}
			public static class Cvttsd2si
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x2C });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x2C });
			}
			public static class Cvttss2si
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF3, 0x0F, 0x2C });
				public static OpCode R_M = new OpCode(new byte[] { 0xF3, 0x0F, 0x2C });
			}
			public static class Cvtss2sd
			{
				public static OpCode R_L = new OpCode(new byte[] { 0xF3, 0x0F, 0x5A });
				public static OpCode R_M = new OpCode(new byte[] { 0xF3, 0x0F, 0x5A });
				public static OpCode R_R = new OpCode(new byte[] { 0xF3, 0x0F, 0x5A });
			}
			public static class Addsd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x58 });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x58 });
				public static OpCode R_L = new OpCode(new byte[] { 0xF2, 0x0F, 0x58 });
			}
			public static class Subsd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x5C });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x5C });
				public static OpCode R_L = new OpCode(new byte[] { 0xF2, 0x0F, 0x5C });
			}
			public static class Mulsd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x59 });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x59 });
				public static OpCode R_L = new OpCode(new byte[] { 0xF2, 0x0F, 0x59 });
			}
			public static class Divsd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x5E });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x5E });
			}
			public static class Cmpsd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0xC2 });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0xC2 });
				public static OpCode R_L = new OpCode(new byte[] { 0xF2, 0x0F, 0xC2 });
				public static OpCode R_C = new OpCode(new byte[] { 0xF2, 0x0F, 0xC2 });
			}
			public static class Comisd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
				public static OpCode R_M = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
				public static OpCode R_L = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
				public static OpCode R_C = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
			}
			public static class Comiss
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0x2F });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0x2F });
				public static OpCode R_L = new OpCode(new byte[] { 0x0F, 0x2F });
				public static OpCode R_C = new OpCode(new byte[] { 0x0F, 0x2F });
			}
			public static class Ucomisd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x66, 0x0F, 0x2E });
				public static OpCode R_M = new OpCode(new byte[] { 0x66, 0x0F, 0x2E });
				public static OpCode R_L = new OpCode(new byte[] { 0x66, 0x0F, 0x2E });
				public static OpCode R_C = new OpCode(new byte[] { 0x66, 0x0F, 0x2E });
			}
			public static class Ucomiss
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0x2E });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0x2E });
				public static OpCode R_L = new OpCode(new byte[] { 0x0F, 0x2E });
				public static OpCode R_C = new OpCode(new byte[] { 0x0F, 0x2E });
			}
		};
#pragma warning restore 1591

		#endregion

		#region Add
		/// <summary>
		/// Adds the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Add(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Add.R_C;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Add.R_R;

			if ((dest is RegisterOperand) && (src is MemoryOperand))
				if ((src.Type.Type == CilElementType.I1) || (src.Type.Type == CilElementType.U1))
					return X86Intruction.Add.R_M_I8;
				else
					return X86Intruction.Add.R_M;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
					return X86Intruction.Add.M_R_I8;
				else
					return X86Intruction.Add.M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region And
		/// <summary>
		/// Ands the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode And(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.And.R_C;

			if ((dest is MemoryOperand) && (src is ConstantOperand))
				return X86Intruction.And.M_C;

			if ((dest is RegisterOperand) && (src is MemoryOperand))
				return X86Intruction.And.R_M;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.And.R_R;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.And.M_C;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Div
		/// <summary>
		/// Returns the matching OpCode for DIV
		/// </summary>
		/// <param name="dest">Destination operand</param>
		/// <param name="src">Source Operand</param>
		/// <returns>The matching OpCode</returns>
		public static OpCode Div(Operand dest, Operand src)
		{
			if (src is RegisterOperand)
				if ((src.Type.Type == CilElementType.U1) || (src.Type.Type == CilElementType.I1))
					return X86Intruction.Div.R_I8;
				else if ((src.Type.Type == CilElementType.U2) || (src.Type.Type == CilElementType.I2))
					return X86Intruction.Div.R_I16;
				else
					return X86Intruction.Div.R;

			if (src is MemoryOperand)
				if ((src.Type.Type == CilElementType.U1) || (src.Type.Type == CilElementType.I1))
					return X86Intruction.Div.M_I8;
				else if ((src.Type.Type == CilElementType.U2) || (src.Type.Type == CilElementType.I2))
					return X86Intruction.Div.M_16;
				else
					return X86Intruction.Div.M;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Move
		/// <summary>
		/// Moves the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Move(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Mov.R_C;

			if ((dest is MemoryOperand) && (src is ConstantOperand))
				return X86Intruction.Mov.M_C;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Mov.R_R;

			if ((dest is RegisterOperand) && (src is MemoryOperand))
				if (src.Type.Type == CilElementType.U1)
					return X86Intruction.Mov.R_M_U8;
				else if (src.Type.Type == CilElementType.I1)
					return X86Intruction.Mov.R_M_I8;
				else if (src.Type.Type == CilElementType.U2)
					return X86Intruction.Mov.R_M_U16;
				else if (src.Type.Type == CilElementType.I2)
					return X86Intruction.Mov.R_M_I16;
				else
					return X86Intruction.Mov.R_M;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
					return X86Intruction.Mov.M_R_I8;
				else
					return X86Intruction.Mov.M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Neg
		/// <summary>
		/// Negates the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Neg(Operand dest)
		{
			if (dest is RegisterOperand)
				return X86Intruction.Neg.R;

			if (dest is MemoryOperand)
				if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
					return X86Intruction.Neg.M_I8;
				else
					return X86Intruction.Neg.M;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Or
		/// <summary>
		/// Ors the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Or(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Or.R_C;

			if ((dest is RegisterOperand) && (src is MemoryOperand))
				return X86Intruction.Or.R_M;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Or.R_R;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.Or.M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Shl
		/// <summary>
		/// SHLs the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Shl(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Shl.R_R;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.Shl.M_R;

			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Shl.R_C;

			if ((dest is MemoryOperand) && (src is ConstantOperand))
				return X86Intruction.Shl.M_C;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Shr
		/// <summary>
		/// SHRs the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Shr(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Shr.R_R;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.Shr.M_R;

			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Shr.R_C;

			if ((dest is MemoryOperand) && (src is ConstantOperand))
				return X86Intruction.Shr.M_C;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Sub
		/// <summary>
		/// Subs the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Sub(Operand dest, Operand src)
		{
			if ((dest is Operand) && (src is ConstantOperand))
				return X86Intruction.Sub.O_C;

			if ((dest is RegisterOperand) && (src is Operand))
				return X86Intruction.Sub.R_O;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
					return X86Intruction.Sub.M_R_I8;
				else
					return X86Intruction.Sub.M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}
		#endregion

		#region Xor
		/// <summary>
		/// Xors the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Xor(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Xor.R_C;

			if ((dest is RegisterOperand) && (src is MemoryOperand))
				return X86Intruction.Xor.R_M;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Xor.R_R;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.Xor.M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion
	}
}

