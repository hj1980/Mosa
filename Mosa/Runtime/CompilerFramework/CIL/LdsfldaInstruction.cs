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

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LdsfldaInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdsfldaInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdsfldaInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// Read the _stackFrameIndex From the code
			TokenTypes token;
			decoder.Decode(out token);
			throw new NotImplementedException();

			/*
				Debug.Assert(TokenTypes.Field == (TokenTypes.TableMask & token) ||
							 TokenTypes.MemberRef == (TokenTypes.TableMask & token), @"Invalid token type.");
				MemberDefinition memberDef = MetadataMemberReference.FromToken(decoder.Metadata, token).Resolve();

				DefaultTypeSystem.GetField(decoder.Method.Module, token);

				_field = memberDef as FieldDefinition;
				Debug.Assert((_field.CustomAttributes & FieldAttributes.Static) == FieldAttributes.Static, @"Static _stackFrameIndex access on non-static _stackFrameIndex.");
				_results[0] = CreateResultOperand(new ReferenceTypeSpecification(_field.Type));
			*/
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldsflda(context);
		}

		#endregion Methods


	}
}
