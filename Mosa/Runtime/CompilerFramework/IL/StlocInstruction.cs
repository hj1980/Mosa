/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Initializes a new instance of <see cref="StlocInstruction"/>.
    /// </summary>
    public class StlocInstruction : UnaryInstruction, IStoreInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="StlocInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the unary instruction.</param>
        public StlocInstruction(OpCode code)
            : base(code, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StlocInstruction"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        public StlocInstruction(Operand target, Operand value) :
            base(OpCode.Stloc, 1)
        {
            SetResult(0, target);
            SetOperand(0, value);
        }

        #endregion // Construction

        #region UnaryInstruction Overrides

        /// <summary>
        /// Stloc has a result, but doesn't push it on the stack.
        /// </summary>
        public override bool PushResult
        {
            get { return false; }
        }

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// from the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            ushort locIdx;

            // Decode base classes first
            base.Decode(decoder);

            // Destination depends on the opcode
            switch (_code)
            {
                case OpCode.Stloc:
                    decoder.Decode(out locIdx);
                    break;

                case OpCode.Stloc_s:
                    {
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

            SetResult(0, decoder.Compiler.GetLocalOperand(locIdx));
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Stloc(this, arg);
        }

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            return String.Format("{0} ; {1} = {2}", base.ToString(), this.Results[0], this.Operands[0]);
        }

        #endregion // UnaryInstruction Overrides

        #region IStoreInstruction Members

        Operand IStoreInstruction.Source
        {
            get { return this.Operands[0]; }
        }

        Operand IStoreInstruction.Destination
        {
            get { return this.Results[0]; }
        }

        #endregion
    }
}
