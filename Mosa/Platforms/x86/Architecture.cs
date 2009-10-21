/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Platforms.x86.Constraints;

using CPUx86 = Mosa.Platforms.x86.CPUx86;

namespace Mosa.Platforms.x86
{

	/// <summary>
	/// This class provides a common base class for architecture
	/// specific operations.
	/// </summary>
	public class Architecture : BasicArchitecture
	{
		/// <summary>
		/// Defines the register set of the target architecture.
		/// </summary>
		private static readonly Register[] Registers = new Register[]
        {
            ////////////////////////////////////////////////////////
            // 32-bit general purpose registers
            ////////////////////////////////////////////////////////
            GeneralPurposeRegister.EAX,
            GeneralPurposeRegister.ECX,
            GeneralPurposeRegister.EDX,
            GeneralPurposeRegister.EBX,
            GeneralPurposeRegister.ESI,
            GeneralPurposeRegister.EDI,

            ////////////////////////////////////////////////////////
            // 128-bit floating point registers
            ////////////////////////////////////////////////////////
            SSE2Register.XMM0,
            SSE2Register.XMM1,
            SSE2Register.XMM2,
            SSE2Register.XMM3,
            SSE2Register.XMM4,
            SSE2Register.XMM5,
            SSE2Register.XMM6,
            SSE2Register.XMM7
        };

		/// <summary>
		/// Maps constraints to an instruction. Deprecated.
		/// </summary>
		private static readonly Dictionary<Type, Type> Constraints = new Dictionary<Type, Type>()
        {
            { typeof(CPUx86.AddInstruction), typeof(GPRConstraint) },
            { typeof(CPUx86.AdcInstruction), typeof(GPRConstraint) },
            { typeof(CPUx86.DivInstruction), typeof(DivConstraint) },
            { typeof(CPUx86.LogicalAndInstruction), typeof(LogicalAndConstraint) },
            { typeof(CPUx86.LogicalOrInstruction), typeof(LogicalOrConstraint) },
            { typeof(CPUx86.LogicalXorInstruction), typeof(LogicalXorConstraint) },
            { typeof(CPUx86.MovInstruction), typeof(MoveConstraint) },
            { typeof(CPUx86.MulInstruction), typeof(MulConstraint) },
            { typeof(CPUx86.SarInstruction), typeof(ShiftConstraint) },
            { typeof(CPUx86.ShlInstruction), typeof(ShiftConstraint) },
            { typeof(CPUx86.ShrInstruction), typeof(ShiftConstraint) },
            { typeof(CPUx86.SubInstruction), typeof(GPRConstraint) },
        };

		/// <summary>
		/// Specifies the architecture features to use in generated code.
		/// </summary>
		private ArchitectureFeatureFlags architectureFeatures;

		/// <summary>
		/// Initializes a new instance of the <see cref="Architecture"/> class.
		/// </summary>
		/// <param name="architectureFeatures">The features this architecture supports.</param>
		protected Architecture(ArchitectureFeatureFlags architectureFeatures)
		{
			this.architectureFeatures = architectureFeatures;
		}

		/// <summary>
		/// Retrieves the native integer size of the x86 platform.
		/// </summary>
		/// <value>This property always returns 32.</value>
		public override int NativeIntegerSize
		{
			get { return 32; }
		}

		/// <summary>
		/// Retrieves the register set of the x86 platform.
		/// </summary>
		public override Register[] RegisterSet
		{
			get { return Registers; }
		}

		/// <summary>
		/// Retrieves the stack frame register of the x86.
		/// </summary>
		public override Register StackFrameRegister
		{
			get { return GeneralPurposeRegister.EBP; }
		}

		/// <summary>
		/// Factory method for the Architecture class.
		/// </summary>
		/// <returns>The created architecture instance.</returns>
		/// <param name="architectureFeatures">The features available in the architecture and code generation.</param>
		/// <remarks>
		/// This method creates an instance of an appropriate architecture class, which supports the specific
		/// architecture features.
		/// </remarks>
		public static IArchitecture CreateArchitecture(ArchitectureFeatureFlags architectureFeatures)
		{
			if (architectureFeatures == ArchitectureFeatureFlags.AutoDetect) {
				architectureFeatures = ArchitectureFeatureFlags.MMX | ArchitectureFeatureFlags.SSE | ArchitectureFeatureFlags.SSE2;
			}

			return new Architecture(architectureFeatures);
		}

		/// <summary>
		/// Creates a new result operand of the requested type.
		/// </summary>
		/// <param name="signatureType">The type requested.</param>
		/// <param name="instructionLabel">The label of the instruction requesting the operand.</param>
		/// <param name="operandStackIndex">The stack index of the operand.</param>
		/// <returns>A new operand usable as a result operand.</returns>
		public override Operand CreateResultOperand(SigType signatureType, int instructionLabel, int operandStackIndex)
		{
			return new RegisterOperand(signatureType, GeneralPurposeRegister.EAX);
		}

		/// <summary>
		/// Extends the assembly compiler pipeline with x86 specific stages.
		/// </summary>
		/// <param name="assemblyCompilerPipeline">The assembly compiler pipeline to extend.</param>
		public override void ExtendAssemblyCompilerPipeline(CompilerPipeline<IAssemblyCompilerStage> assemblyCompilerPipeline)
		{
		}

		/// <summary>
		/// Extends the method compiler pipeline with x86 specific stages.
		/// </summary>
		/// <param name="methodCompilerPipeline">The method compiler pipeline to extend.</param>
		public override void ExtendMethodCompilerPipeline(CompilerPipeline<IMethodCompilerStage> methodCompilerPipeline)
		{
			// FIXME: Create a specific code generator instance using requested feature flags.
			// FIXME: Add some more optimization passes, which take advantage of advanced x86 instructions
			// and packed operations available with MMX/SSE extensions
			methodCompilerPipeline.AddRange(
				new IMethodCompilerStage[]
                {
                    new LongOperandTransformationStage(),
					new CILToX86TransformationStage(),
                    InstructionLogger.Instance,
                    new IRToX86TransformationStage(),
                    InstructionLogger.Instance,
					new FinalX86TransformationStage(),
                    //InstructionLogger.Instance,
					new CodeGenerationStage(),
                });
		}

		/// <summary>
		/// Retrieves a calling convention object for the requested calling convention.
		/// </summary>
		/// <param name="callingConvention">One of the defined calling conventions.</param>
		/// <returns>An instance of <see cref="ICallingConvention"/>.</returns>
		/// <exception cref="System.NotSupportedException"><paramref name="cc"/> is not a supported calling convention.</exception>
		public override ICallingConvention GetCallingConvention(CallingConvention callingConvention)
		{
			switch (callingConvention) {
				case CallingConvention.Default:
					return new DefaultCallingConvention(this);

				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Gets the type memory requirements.
		/// </summary>
		/// <param name="signatureType">The signature type.</param>
		/// <param name="memorySize">Receives the memory size of the type.</param>
		/// <param name="alignment">Receives alignment requirements of the type.</param>
		public override void GetTypeRequirements(SigType signatureType, out int memorySize, out int alignment)
		{
			if (signatureType == null)
				throw new ArgumentNullException(@"type");

			switch (signatureType.Type) {
				case CilElementType.R4:
					memorySize = alignment = 4;
					break;
				case CilElementType.R8:
					// Default alignment and size are 4
					memorySize = alignment = 8;
					break;

				case CilElementType.I8:
					goto case CilElementType.U8;
				case CilElementType.U8:
					memorySize = alignment = 8;
					break;

				case CilElementType.ValueType:
					memorySize = alignment = 4; // FIXME: HACK!
					break;
				default:
					memorySize = alignment = 4;
					break;
			}
		}

		/// <summary>
		/// Gets the intrinsics instruction by type
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public override IInstruction GetIntrinsicIntruction(Type type)
		{
			return CPUx86.Intrinsics.Get(type);
		}

		/// <summary>
		/// Gets the code emitter.
		/// </summary>
		/// <returns></returns>
		public override ICodeEmitter GetCodeEmitter()
		{
			return new MachineCodeEmitter();	
		}
	}
}