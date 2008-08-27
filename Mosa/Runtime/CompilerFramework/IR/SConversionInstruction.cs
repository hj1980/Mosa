﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Intermediate representation of a signed conversion instruction.
    /// </summary>
    /// <remarks>
    /// This instruction takes the source operand and converts to the request size maintaining its sign.
    /// </remarks>
    public class SConversionInstruction : Instruction
    {
        #region Data members

        /// <summary>
        /// Holds the destination size of the conversion.
        /// </summary>
        private int _size;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SConversionInstruction"/>.
        /// </summary>
        public SConversionInstruction() :
            base(1, 1)
        {
            _size = 4;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SConversionInstruction"/>.
        /// </summary>
        /// <param name="destination">The destination operand for the conversion.</param>
        /// <param name="source">The source operand for the conversion.</param>
        public SConversionInstruction(Operand destination, Operand source) :
            base(1, 1)
        {
            SetOperand(0, source);
            SetResult(0, destination);
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the destination operand.
        /// </summary>
        public Operand Destination
        {
            get { return this.Results[0]; }
            set { this.SetResult(0, value); }
        }

        /// <summary>
        /// Gets or sets the size of the conversion result.
        /// </summary>
        public int Size
        {
            get { return _size; }
            set
            {
                if (value != 1 && value != 2 && value != 4 && value != 8)
                    throw new ArgumentOutOfRangeException(@"value");

                _size = value;
            }
        }

        /// <summary>
        /// Gets or sets the source operand.
        /// </summary>
        public Operand Source
        {
            get { return this.Operands[0]; }
            set { this.SetOperand(0, value); }
        }

        #endregion // Properties

        #region Instruction Overrides

        /// <summary>
        /// Returns a string representation of <see cref="SConversionInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the instruction.</returns>
        public override string ToString()
        {
            return String.Format(@"IR sconv {0} <- {1}", this.Destination, this.Source);
        }

        /// <summary>
        /// Implementation of the visitor pattern.
        /// </summary>
        /// <param name="visitor">The visitor requesting visitation. The object must implement <see cref="IIrVisitor"/>.</param>
        public override void Visit(IInstructionVisitor visitor)
        {
            IIrVisitor irv = visitor as IIrVisitor;
            if (null == irv)
                throw new ArgumentException(@"Must implement IIrVisitor!", @"visitor");

            irv.Visit(this);
        }

        #endregion // Instruction Overrides
    }
}