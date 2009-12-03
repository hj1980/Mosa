﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class InitObjInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="InitObjInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public InitObjInstruction(OpCode opcode)
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

			// Retrieve the type reference
			TokenTypes token;
			decoder.Decode(out token);

            int size = ComputeSize(token, decoder.Compiler);
		    Metadata.Tables.FieldRow fieldRow;
		    decoder.Compiler.Assembly.Metadata.Read(token, out fieldRow);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.InitObj(context);
		}

        private static int ComputeSize(TokenTypes token, IMethodCompiler compiler)
        {
            IMetadataProvider metadata = compiler.Assembly.Metadata;

            Metadata.Tables.FieldRow fieldRow;
            metadata.Read(token, out fieldRow);
            FieldSignature signature = Signature.FromMemberRefSignatureToken(metadata, fieldRow.SignatureBlobIdx) as FieldSignature;

            int size, alignment;
            compiler.Architecture.GetTypeRequirements(signature.Type, out size, out alignment);
            
            return size;
        }

		#endregion Methods

	}
}
