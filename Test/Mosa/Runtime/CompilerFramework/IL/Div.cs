﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class Div : CodeDomTestRunner
    {
        private static string CreateTestCode(string name, string typeIn, string typeOut)
        {
            return @"
                static class Test
                {
                    static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
                    {
                        return expect == (a / b);
                    }
                }";
        }

        private static string CreateTestCodeWithReturn(string name, string typeIn, string typeOut)
        {
            return @"
                static class Test
                {
                    static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
                    {
                        return (a / b);
                    }
                }";
        }
  
        private static string CreateConstantTestCode(string name, string typeIn, string typeOut, string constLeft, string constRight)
        {
            if (String.IsNullOrEmpty(constRight))
            {
                return @"
                    static class Test
                    {
                        static bool " + name + "(" + typeOut + " expect, " + typeIn + @" x)
                        {
                            return expect == (" + constLeft + @" / x);
                        }
                    }";
            }
            else if (String.IsNullOrEmpty(constLeft))
            {
                return @"
                    static class Test
                    {
                        static bool " + name + "(" + typeOut + " expect, " + typeIn + @" x)
                        {
                            return expect == (x / " + constRight + @");
                        }
                    }";
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private static string CreateConstantTestCodeWithReturn(string name, string typeIn, string typeOut, string constLeft, string constRight)
        {
            if (String.IsNullOrEmpty(constRight))
            {
                return @"
                    static class Test
                    {
                        static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + @" x)
                        {
                            return (" + constLeft + @" / x);
                        }
                    }";
            }
            else if (String.IsNullOrEmpty(constLeft))
            {
                return @"
                    static class Test
                    {
                        static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + @" x)
                        {
                            return (x / " + constRight + @");
                        }
                    }";
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        
        #region C
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool C_C_C([MarshalAs(UnmanagedType.U2)]char expect, [MarshalAs(UnmanagedType.U2)]char a, [MarshalAs(UnmanagedType.U2)]char b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(17, 128)]
        [TestCase('a', 'Z')]
        [TestCase(char.MinValue, char.MaxValue)]
        [Test]
        public void DivC(char a, char b)
        {
            CodeSource = CreateTestCode("DivC", "char", "char");
            Assert.IsTrue((bool)Run<C_C_C>("", "Test", "DivC", (char)(a / b), a, b));
        }
        
        delegate bool C_Constant_C(int expect, char x);
        delegate int C_Constant_C_Return(int expect, [MarshalAs(UnmanagedType.U2)]char x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        //[TestCase(0, 'a')]
        //[TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [Test]
        public void DivConstantCRight(char a, char b)
        {
            CodeSource = CreateConstantTestCodeWithReturn("DivConstantCRight", "char", "int", null, "'" + b.ToString() + "'");
            Assert.AreEqual(a / b, Run<C_Constant_C_Return>("", "Test", "DivConstantCRight", (a / b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase('a', 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase('-', '.')]
        [TestCase((char)97, (char)90)]
        [Test]
        public void DivConstantCLeft(char a, char b)
        {
            CodeSource = CreateConstantTestCodeWithReturn("DivConstantCLeft", "char", "int", "'" + a.ToString() + "'", null);
            Assert.AreEqual(a / b, Run<C_Constant_C_Return>("", "Test", "DivConstantCLeft", (a / b), (char)b));
        }
        #endregion
        
        #region I1
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool I4_I1_I1(int expect, sbyte a, sbyte b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(1, -2)]
        [TestCase(-1, 2)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(-17, -2)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -17)]
        // (MinValue, X) Cases
        [TestCase(sbyte.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(sbyte.MinValue, 1)]
        [TestCase(sbyte.MinValue, 17)]
        [TestCase(sbyte.MinValue, 123)]
        [TestCase(sbyte.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(sbyte.MinValue, -1)]
        [TestCase(sbyte.MinValue, -17)]
        [TestCase(sbyte.MinValue, -123)]
        // (MaxValue, X) Cases
        [TestCase(sbyte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(sbyte.MaxValue, 1)]
        [TestCase(sbyte.MaxValue, 17)]
        [TestCase(sbyte.MaxValue, 123)]
        [TestCase(sbyte.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(sbyte.MaxValue, -1)]
        [TestCase(sbyte.MaxValue, -17)]
        [TestCase(sbyte.MaxValue, -123)]
        // (X, MinValue) Cases
        [TestCase(0, sbyte.MinValue)]
        [TestCase(1, sbyte.MinValue)]
        [TestCase(17, sbyte.MinValue)]
        [TestCase(123, sbyte.MinValue)]
        [TestCase(-0, sbyte.MinValue)]
        [TestCase(-1, sbyte.MinValue)]
        [TestCase(-17, sbyte.MinValue)]
        [TestCase(-123, sbyte.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, sbyte.MaxValue)]
        [TestCase(1, sbyte.MaxValue)]
        [TestCase(17, sbyte.MaxValue)]
        [TestCase(123, sbyte.MaxValue)]
        [TestCase(-0, sbyte.MaxValue)]
        [TestCase(-1, sbyte.MaxValue)]
        [TestCase(-17, sbyte.MaxValue)]
        [TestCase(-123, sbyte.MaxValue)]
        // Extremvaluecases
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [TestCase(sbyte.MaxValue, sbyte.MinValue)]
        [TestCase(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void DivI1(sbyte a, sbyte b)
        {
            CodeSource = CreateTestCode("DivI1", "sbyte", "int");
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "DivI1", a / b, a, b));
        }
        
        delegate bool I4_Constant_I1(int expect, sbyte x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(2, -17)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void DivConstantI1Right(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("DivConstantI1Right", "sbyte", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "DivConstantI1Right", (a / b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(2, -17)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void DivConstantI1Left(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("DivConstantI1Left", "sbyte", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "DivConstantI1Left", (a / b), b));
        }
        #endregion

        #region U1
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool U4_U1_U1(uint expect, byte a, byte b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        // (MinValue, X) Cases
        [TestCase(byte.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(byte.MinValue, 1)]
        [TestCase(byte.MinValue, 17)]
        [TestCase(byte.MinValue, 123)]
        // (MaxValue, X) Cases
        [TestCase(byte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(byte.MaxValue, 1)]
        [TestCase(byte.MaxValue, 17)]
        [TestCase(byte.MaxValue, 123)]
        // (X, MinValue) Cases
        [TestCase(0, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(1, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(17, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(123, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [TestCase(0, byte.MaxValue)]
        [TestCase(1, byte.MaxValue)]
        [TestCase(17, byte.MaxValue)]
        [TestCase(123, byte.MaxValue)]
        // Extremvaluecases
        [TestCase(byte.MinValue, byte.MaxValue)]
        [TestCase(byte.MaxValue, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void DivU1(byte a, byte b)
        {
            CodeSource = CreateTestCode("DivU1", "byte", "uint");
            Assert.IsTrue((bool)Run<U4_U1_U1>("", "Test", "DivU1", (uint)(a / b), a, b));
        }
        
        delegate bool U4_Constant_U1(uint expect, byte x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void DivConstantU1Right(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("DivConstantU1Right", "byte", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "DivConstantU1Right", (uint)(a / b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void DivConstantU1Left(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("DivConstantU1Left", "byte", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "DivConstantU1Left", (uint)(a / b), b));
        }
        #endregion

        #region I2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool I4_I2_I2(int expect, short a, short b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(1, -2)]
        [TestCase(-1, 2)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(-17, -2)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -17)]
        // (MinValue, X) Cases
        [TestCase(short.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(short.MinValue, 1)]
        [TestCase(short.MinValue, 17)]
        [TestCase(short.MinValue, 123)]
        [TestCase(short.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(short.MinValue, -1)]
        [TestCase(short.MinValue, -17)]
        [TestCase(short.MinValue, -123)]
        // (MaxValue, X) Cases
        [TestCase(short.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(short.MaxValue, 1)]
        [TestCase(short.MaxValue, 17)]
        [TestCase(short.MaxValue, 123)]
        [TestCase(short.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(short.MaxValue, -1)]
        [TestCase(short.MaxValue, -17)]
        [TestCase(short.MaxValue, -123)]
        // (X, MinValue) Cases
        [TestCase(0, short.MinValue)]
        [TestCase(1, short.MinValue)]
        [TestCase(17, short.MinValue)]
        [TestCase(123, short.MinValue)]
        [TestCase(-0, short.MinValue)]
        [TestCase(-1, short.MinValue)]
        [TestCase(-17, short.MinValue)]
        [TestCase(-123, short.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, short.MaxValue)]
        [TestCase(1, short.MaxValue)]
        [TestCase(17, short.MaxValue)]
        [TestCase(123, short.MaxValue)]
        [TestCase(-0, short.MaxValue)]
        [TestCase(-1, short.MaxValue)]
        [TestCase(-17, short.MaxValue)]
        [TestCase(-123, short.MaxValue)]
        // Extremvaluecases
        [TestCase(short.MinValue, short.MaxValue)]
        [TestCase(short.MaxValue, short.MinValue)]
        [TestCase(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void DivI2(short a, short b)
        {
            CodeSource = CreateTestCode("DivI2", "short", "int");
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "DivI2", a / b, a, b));
        }
        
        delegate bool I4_Constant_I2(int expect, short x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 21)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void DivConstantI2Right(short a, short b)
        {
            CodeSource = CreateConstantTestCode("DivConstantI2Right", "short", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "DivConstantI2Right", (a / b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 21)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void DivConstantI2Left(short a, short b)
        {
            CodeSource = CreateConstantTestCode("DivConstantI2Left", "short", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "DivConstantI2Left", (a / b), b));
        }
        #endregion

        #region U2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool U4_U2_U2(uint expect, ushort a, ushort b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        // (MinValue, X) Cases
        [TestCase(ushort.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ushort.MinValue, 1)]
        [TestCase(ushort.MinValue, 17)]
        [TestCase(ushort.MinValue, 123)]
        // (MaxValue, X) Cases
        [TestCase(ushort.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ushort.MaxValue, 1)]
        [TestCase(ushort.MaxValue, 17)]
        [TestCase(ushort.MaxValue, 123)]
        // (X, MinValue) Cases
        [TestCase(0, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(1, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(17, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(123, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [TestCase(0, ushort.MaxValue)]
        [TestCase(1, ushort.MaxValue)]
        [TestCase(17, ushort.MaxValue)]
        [TestCase(123, ushort.MaxValue)]
        // Extremvaluecases
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [TestCase(ushort.MaxValue, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void DivU2(ushort a, ushort b)
        {
            CodeSource = CreateTestCode("DivU2", "ushort", "uint");
            Assert.IsTrue((bool)Run<U4_U2_U2>("", "Test", "DivU2", (uint)(a / b), a, b));
        }
        
        delegate bool U4_Constant_U2(uint expect, ushort x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(148, 23)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void DivConstantU2Right(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("DivConstantU2Right", "ushort", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "DivConstantU2Right", (uint)(a / b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 21)]
        [TestCase(148, 23)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void DivConstantU2Left(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("DivConstantU2Left", "ushort", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "DivConstantU2Left", (uint)(a / b), b));
        }
        #endregion
        
        #region I4
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool I4_I4_I4(int expect, int a, int b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(1, -2)]
        [TestCase(-1, 2)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(-17, -2)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -17)]
        // (MinValue, X) Cases
        [TestCase(int.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(int.MinValue, 1)]
        [TestCase(int.MinValue, 17)]
        [TestCase(int.MinValue, 123)]
        [TestCase(int.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(int.MinValue, -1, ExpectedException = typeof(OverflowException))]
        [TestCase(int.MinValue, -17)]
        [TestCase(int.MinValue, -123)]
        // (MaxValue, X) Cases
        [TestCase(int.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(int.MaxValue, 1)]
        [TestCase(int.MaxValue, 17)]
        [TestCase(int.MaxValue, 123)]
        [TestCase(int.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(int.MaxValue, -1)]
        [TestCase(int.MaxValue, -17)]
        [TestCase(int.MaxValue, -123)]
        // (X, MinValue) Cases
        [TestCase(0, int.MinValue)]
        [TestCase(1, int.MinValue)]
        [TestCase(17, int.MinValue)]
        [TestCase(123, int.MinValue)]
        [TestCase(-0, int.MinValue)]
        [TestCase(-1, int.MinValue)]
        [TestCase(-17, int.MinValue)]
        [TestCase(-123, int.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, int.MaxValue)]
        [TestCase(1, int.MaxValue)]
        [TestCase(17, int.MaxValue)]
        [TestCase(123, int.MaxValue)]
        [TestCase(-0, int.MaxValue)]
        [TestCase(-1, int.MaxValue)]
        [TestCase(-17, int.MaxValue)]
        [TestCase(-123, int.MaxValue)]
        // Extremvaluecases
        [TestCase(int.MinValue, int.MaxValue)]
        [TestCase(int.MaxValue, int.MinValue)]
        [TestCase(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void DivI4(int a, int b)
        {
            CodeSource = CreateTestCode("DivI4", "int", "int");
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "DivI4", (a / b), a, b));
        }
        
        delegate bool I4_Constant_I4(int expect, int x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 21)]
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void DivConstantI4Right(int a, int b)
        {
            CodeSource = CreateConstantTestCode("DivConstantI4Right", "int", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "DivConstantI4Right", (a / b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 21)]
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void DivConstantI4Left(int a, int b)
        {
            CodeSource = CreateConstantTestCode("DivConstantI4Left", "int", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "DivConstantI4Left", (a / b), b));
        }
        #endregion

        #region U4
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool U4_U4_U4(uint expect, uint a, uint b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        // (MinValue, X) Cases
        [TestCase(uint.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(uint.MinValue, 1)]
        [TestCase(uint.MinValue, 17)]
        [TestCase(uint.MinValue, 123)] 
        // (MaxValue, X) Cases
        [TestCase(uint.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(uint.MaxValue, 1)]
        [TestCase(uint.MaxValue, 17)]
        [TestCase(uint.MaxValue, 123)]
        // (X, MinValue) Cases
        [TestCase(0, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(1, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(17, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(123, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [TestCase(0, uint.MaxValue)]
        [TestCase(1, uint.MaxValue)]
        [TestCase(17, uint.MaxValue)]
        [TestCase(123, uint.MaxValue)]
        // Extremvaluecases
        [TestCase(uint.MinValue, uint.MaxValue)]
        [TestCase(uint.MaxValue, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(1, 0, ExpectedException = typeof(DivideByZeroException))]      
        [Test]
        public void DivU4(uint a, uint b)
        {
            CodeSource = CreateTestCode("DivU4", "uint", "uint");
            Assert.IsTrue((bool)Run<U4_U4_U4>("", "Test", "DivU4", (uint)(a / b), a, b));
        }
        
        delegate bool U4_Constant_U4(uint expect, uint x); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(123, uint.MaxValue)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void DivConstantU4Right(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("DivConstantU4Right", "uint", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "DivConstantU4Right", (uint)(a / b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(123, uint.MaxValue)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void DivConstantU4Left(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("DivConstantU4Left", "uint", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "DivConstantU4Left", (uint)(a / b), b));
        }
        #endregion

        #region U8
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool U8_U8_U8(ulong expect, ulong a, ulong b);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate ulong U8_U8_U8_Return(ulong expect, ulong a, ulong b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        // (MinValue, X) Cases
        [TestCase(ulong.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ulong.MinValue, 1)]
        [TestCase(ulong.MinValue, 17)]
        [TestCase(ulong.MinValue, 123)]
        // (MaxValue, X) Cases
        [TestCase(ulong.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(ulong.MaxValue, 1)]
        [TestCase(ulong.MaxValue, 17)]
        [TestCase(ulong.MaxValue, 123)]
        // (X, MinValue) Cases
        [TestCase(0, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(1, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(17, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(123, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        // (X, MaxValue) Cases
        [TestCase(0, ulong.MaxValue)]
        [TestCase(1, ulong.MaxValue)]
        [TestCase(17, ulong.MaxValue)]
        [TestCase(123, ulong.MaxValue)]
        // Extremvaluecases
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [TestCase(ulong.MaxValue, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void DivU8(ulong a, ulong b)
        {
            CodeSource = CreateTestCodeWithReturn("DivU8", "ulong", "ulong");
            Assert.AreEqual((ulong)(a / b), Run<U8_U8_U8_Return>("", "Test", "DivU8", (ulong)(a / b), a, b));
        }
        #endregion

        #region I8
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool I8_I8_I8(long expect, long a, long b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1, 2)]
        [TestCase(23, 21)]
        [TestCase(1, -2)]
        [TestCase(-1, 2)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(-17, -2)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -17)]
        // (MinValue, X) Cases
        [TestCase(long.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(long.MinValue, 1)]
        [TestCase(long.MinValue, 17)]
        [TestCase(long.MinValue, 123)]
        [TestCase(long.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(long.MinValue, -1, ExpectedException = typeof(OverflowException))]
        [TestCase(long.MinValue, -17)]
        [TestCase(long.MinValue, -123)]
        // (MaxValue, X) Cases
        [TestCase(long.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(long.MaxValue, 1)]
        [TestCase(long.MaxValue, 17)]
        [TestCase(long.MaxValue, 123)]
        [TestCase(long.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(long.MaxValue, -1)]
        [TestCase(long.MaxValue, -17)]
        [TestCase(long.MaxValue, -123)]
        // (X, MinValue) Cases
        [TestCase(0, long.MinValue)]
        [TestCase(1, long.MinValue)]
        [TestCase(17, long.MinValue)]
        [TestCase(123, long.MinValue)]
        [TestCase(-0, long.MinValue)]
        [TestCase(-1, long.MinValue)]
        [TestCase(-17, long.MinValue)]
        [TestCase(-123, long.MinValue)]
        // (X, MaxValue) Cases
        [TestCase(0, long.MaxValue)]
        [TestCase(1, long.MaxValue)]
        [TestCase(17, long.MaxValue)]
        [TestCase(123, long.MaxValue)]
        [TestCase(-0, long.MaxValue)]
        [TestCase(-1, long.MaxValue)]
        [TestCase(-17, long.MaxValue)]
        [TestCase(-123, long.MaxValue)]
        // Extremvaluecases
        [TestCase(long.MinValue, long.MaxValue)]
        [TestCase(long.MaxValue, long.MinValue)]
        [TestCase(1, 0, ExpectedException = typeof(DivideByZeroException))]
        [Test]
        public void DivI8(long a, long b)
        {
            CodeSource = CreateTestCode("DivI8", "long", "long");
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "DivI8", (a / b), a, b));
        }
        
        delegate bool I8_Constant_I8(long expect, long x);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 21)]
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void DivConstantI8Right(long a, long b)
        {
            CodeSource = CreateConstantTestCode("DivConstantI8Right", "long", "long", null, b.ToString());
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "DivConstantI8Right", (a / b), a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 21)]
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0, ExpectedException = typeof(DivideByZeroException))]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void DivConstantI8Left(long a, long b)
        {
            CodeSource = CreateConstantTestCode("DivConstantI8Left", "long", "long", a.ToString(), null);
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "DivConstantI8Left", (a / b), b));
        }
        
        #endregion
        
        #region R4
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool R4_R4_R4(float expect, float a, float b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1.0f, 2.0f)]
        [TestCase(2.0f, 1.0f)]
        [TestCase(1.0f, 2.5f)]
        [TestCase(1.7f, 2.3f)]
        [TestCase(2.0f, -1.0f)]
        [TestCase(1.0f, -2.5f)]
        [TestCase(1.7f, -2.3f)]
        [TestCase(-2.0f, 1.0f)]
        [TestCase(-1.0f, 2.5f)]
        [TestCase(-1.7f, 2.3f)]
        [TestCase(-2.0f, -1.0f)]
        [TestCase(-1.0f, -2.5f)]
        [TestCase(-1.7f, -2.3f)]
        [TestCase(1.0f, float.NaN)]
        [TestCase(float.NaN, 1.0f)]
        [TestCase(1.0f, float.PositiveInfinity)]
        [TestCase(float.PositiveInfinity, 1.0f)]
        [TestCase(1.0f, float.NegativeInfinity)]
        [TestCase(float.NegativeInfinity, 1.0f)]
        [Test]
        public void DivR4(float a, float b)
        {
            CodeSource = CreateTestCode("DivR4", "float", "float");
            Assert.IsTrue((bool)Run<R4_R4_R4>("", "Test", "DivR4", (a / b), a, b));
        }
        
        delegate bool R4_Constant_R4(float expect, float x);
        delegate float R4_Constant_R4_Return(float expect, float x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23f, 148.0016f)]
        [TestCase(17.2f, 1f)]
        [TestCase(0f, 0f)]
        //[TestCase(float.MinValue, float.MaxValue)]
        [Test]
        public void DivConstantR4Right(float a, float b)
        {
            CodeSource = CreateConstantTestCode("DivConstantR4Right", "float", "float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f");
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "DivConstantR4Right", (a / b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23f, 148.0016f)]
        [TestCase(17.2f, 1f)]
        [TestCase(0f, 0f)]
        //[TestCase(float.MinValue, float.MaxValue)]
        [Test]
        public void DivConstantR4Left(float a, float b)
        {

            CodeSource = CreateConstantTestCode("DivConstantR4Left", "float", "float", a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f", null);
            Assert.IsTrue((bool)Run<R4_Constant_R4>("", "Test", "DivConstantR4Left", (a / b), b));
        }
        #endregion
        
        #region R8
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool R8_R8_R8(double expect, double a, double b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(1.0, 2.0)]
        [TestCase(2.0, 1.0)]
        [TestCase(1.0, 2.5)]
        [TestCase(1.7, 2.3)]
        [TestCase(2.0, -1.0)]
        [TestCase(1.0, -2.5)]
        [TestCase(1.7, -2.3)]
        [TestCase(-2.0, 1.0)]
        [TestCase(-1.0, 2.5)]
        [TestCase(-1.7, 2.3)]
        [TestCase(-2.0, -1.0)]
        [TestCase(-1.0, -2.5)]
        [TestCase(-1.7, -2.3)]
        [TestCase(1.0, double.NaN)]
        [TestCase(double.NaN, 1.0)]
        [TestCase(1.0, double.PositiveInfinity)]
        [TestCase(double.PositiveInfinity, 1.0)]
        [TestCase(1.0, double.NegativeInfinity)]
        [TestCase(double.NegativeInfinity, 1.0)]
        [TestCase(1.0, 0.0)]
        [Test]
        public void DivR8(double a, double b)
        {
            CodeSource = CreateTestCode("DivR8", "double", "double");
            Assert.IsTrue((bool)Run<R8_R8_R8>("", "Test", "DivR8", (a / b), a, b));
        }
        
        delegate bool R8_Constant_R8(double expect, double x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148.0016)]
        [TestCase(17.2, 1.0)]
        [TestCase(0.0, 0.0)]
        [TestCase(-1.79769313486231E+308, 1.79769313486231E+308)]
        [Test]
        public void DivConstantR8Right(double a, double b)
        {
            CodeSource = CreateConstantTestCode("DivConstantR8Right", "double", "double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "DivConstantR8Right", (a / b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(23, 148.0016)]
        [TestCase(17.2, 1.0)]
        [TestCase(0.0, 0.0)]
        [TestCase(-1.79769313486231E+308, 1.79769313486231E+308)]
        [Test]
        public void DivConstantR8Left(double a, double b)
        {
            CodeSource = CreateConstantTestCode("DivConstantR8Left", "double", "double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
            Assert.IsTrue((bool)Run<R8_Constant_R8>("", "Test", "DivConstantR8Left", (a / b), b));
        }
        #endregion
    }
}
