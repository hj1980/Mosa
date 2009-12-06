/*
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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.CIL
{
    /// <summary>
    /// Intermediate representation for various IL call operations.
    /// </summary>
    /// <remarks>
    /// Instances of this class are used to represent call, calli and callvirt
    /// instructions.
    /// </remarks>
    public sealed class CallInstruction : InvokeInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CallInstruction"/> class.
        /// </summary>
        public CallInstruction (OpCode opCode) : base(opCode)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the supported immediate metadata tokens in the instruction.
        /// </summary>
        /// <value></value>
        protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
        {
            get { return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef | InvokeSupportFlags.MethodSpec; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        /// <param name="context">The context.</param>
        public override void Visit (ICILVisitor visitor, Context context)
        {
            visitor.Call (context);
        }

        #endregion

    }
}
