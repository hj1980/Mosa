﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class StlocInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StlocInstruction"/> class.
		/// </summary>
		public StlocInstruction()
		{
		}

		#endregion // Construction

		#region ICILInstruction Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="opcode">The opcode of the load.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, OpCode opcode, IInstructionDecoder decoder)
		{
			ushort locIdx;

			// Decode base classes first
			//base.Decode(decoder);

			// Destination depends on the opcode
			switch (opcode) {
				case OpCode.Stloc:
					decoder.Decode(out locIdx);
					break;

				case OpCode.Stloc_s: {
						byte loc;
						decoder.Decode(out loc);
						locIdx = loc;
					}
					break;

				case OpCode.Stloc_0:
					locIdx = 0;
					break;

				case OpCode.Stloc_1:
					locIdx = 1;
					break;

				case OpCode.Stloc_2:
					locIdx = 2;
					break;

				case OpCode.Stloc_3:
					locIdx = 3;
					break;

				default:
					throw new NotImplementedException();
			}

			instruction.Result = decoder.Compiler.GetLocalOperand(locIdx);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return ToString();
		}

		#endregion // ICILInstruction Overrides

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "CIL nop";
		}

		#endregion // Operand Overrides
	}
}