﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Kai P. Reisert (<mailto:kpreisert@googlemail.com>)
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
    public class Or : CodeDomTestRunner
    {
        private static string CreateTestCode(string name, string typeIn, string typeOut)
        {
            return @"
                static class Test
                {
                    static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
                    {
                        return expect == (a | b);
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
                            return expect == (" + constLeft + @" | x);
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
                            return expect == (x | " + constRight + @");
                        }
                    }";
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        
        #region B
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expect"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate bool B_B_B(bool expect, bool a, bool b);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [Test]
        public void OrB(bool a, bool b)
        {
            CodeSource = CreateTestCode("OrB", "bool", "bool");
            Assert.IsTrue((bool)Run<B_B_B>("", "Test", "OrB", (a | b), a, b));
        }
        
        delegate bool B_Constant_B(bool expect, bool x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [Test]
        public void OrConstantBRight(bool a, bool b)
        {
            CodeSource = CreateConstantTestCode("OrConstantBRight", "bool", "bool", null, b.ToString().ToLower());
            Assert.IsTrue((bool)Run<B_Constant_B>("", "Test", "OrConstantBRight", (a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [Test]
        public void OrConstantBLeft(bool a, bool b)
        {
            CodeSource = CreateConstantTestCode("OrConstantBLeft", "bool", "bool", a.ToString().ToLower(), null);
            Assert.IsTrue((bool)Run<B_Constant_B>("", "Test", "OrConstantBLeft", (a | b), b));
        }
        #endregion
        
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
        [TestCase((char)0, (char)0)]
        [TestCase((char)0, (char)1)]
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [TestCase(char.MinValue, char.MaxValue)]
        [Test]
        public void OrC(char a, char b)
        {
            CodeSource = CreateTestCode("OrC", "char", "char");
            Assert.IsTrue((bool)Run<C_C_C>("", "Test", "OrC", (char)(a | b), a, b));
        }
        
        delegate bool C_Constant_C([MarshalAs(UnmanagedType.U2)]char expect, [MarshalAs(UnmanagedType.U2)]char x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((char)0, 'a')]
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [Test]
        public void OrConstantCRight(char a, char b)
        {
            CodeSource = CreateConstantTestCode("OrConstantCRight", "char", "char", null, "'" + b.ToString() + "'");
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "OrConstantCRight", (char)(a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase('-', '.')]
        [TestCase('a', 'Z')]
        [Test]
        public void OrConstantCLeft(char a, char b)
        {
            CodeSource = CreateConstantTestCode("OrConstantCLeft", "char", "char", "'" + a.ToString() + "'", null);
            Assert.IsTrue((bool)Run<C_Constant_C>("", "Test", "OrConstantCLeft", (char)(a | b), b));
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
        [TestCase((sbyte)1, (sbyte)2)]
        [TestCase((sbyte)23, (sbyte)21)]
        [TestCase((sbyte)1, (sbyte)-2)]
        [TestCase((sbyte)-1, (sbyte)2)]
        [TestCase((sbyte)0, (sbyte)0)]
        [TestCase((sbyte)-17, (sbyte)-2)]
        // And reverse
        [TestCase((sbyte)2, (sbyte)1)]
        [TestCase((sbyte)21, (sbyte)23)]
        [TestCase((sbyte)-2, (sbyte)1)]
        [TestCase((sbyte)2, (sbyte)-1)]
        [TestCase((sbyte)-2, (sbyte)-17)]
        // (MinValue, X) Cases
        [TestCase(sbyte.MinValue, (sbyte)0)]
        [TestCase(sbyte.MinValue, (sbyte)1)]
        [TestCase(sbyte.MinValue, (sbyte)17)]
        [TestCase(sbyte.MinValue, (sbyte)123)]
        [TestCase(sbyte.MinValue, (sbyte)-0)]
        [TestCase(sbyte.MinValue, (sbyte)-1)]
        [TestCase(sbyte.MinValue, (sbyte)-17)]
        [TestCase(sbyte.MinValue, (sbyte)-123)]
        // (MaxValue, X) Cases
        [TestCase(sbyte.MaxValue, (sbyte)0)]
        [TestCase(sbyte.MaxValue, (sbyte)1)]
        [TestCase(sbyte.MaxValue, (sbyte)17)]
        [TestCase(sbyte.MaxValue, (sbyte)123)]
        [TestCase(sbyte.MaxValue, (sbyte)-0)]
        [TestCase(sbyte.MaxValue, (sbyte)-1)]
        [TestCase(sbyte.MaxValue, (sbyte)-17)]
        [TestCase(sbyte.MaxValue, (sbyte)-123)]
        // (X, MinValue) Cases
        [TestCase((sbyte)0, sbyte.MinValue)]
        [TestCase((sbyte)1, sbyte.MinValue)]
        [TestCase((sbyte)17, sbyte.MinValue)]
        [TestCase((sbyte)123, sbyte.MinValue)]
        [TestCase((sbyte)-0, sbyte.MinValue)]
        [TestCase((sbyte)-1, sbyte.MinValue)]
        [TestCase((sbyte)-17, sbyte.MinValue)]
        [TestCase((sbyte)-123, sbyte.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((sbyte)0, sbyte.MaxValue)]
        [TestCase((sbyte)1, sbyte.MaxValue)]
        [TestCase((sbyte)17, sbyte.MaxValue)]
        [TestCase((sbyte)123, sbyte.MaxValue)]
        [TestCase((sbyte)-0, sbyte.MaxValue)]
        [TestCase((sbyte)-1, sbyte.MaxValue)]
        [TestCase((sbyte)-17, sbyte.MaxValue)]
        [TestCase((sbyte)-123, sbyte.MaxValue)]
        // Extremvaluecases
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [TestCase(sbyte.MaxValue, sbyte.MinValue)]
        [Test]
        public void OrI1(sbyte a, sbyte b)
        {
            CodeSource = CreateTestCode("OrI1", "sbyte", "int");
            Assert.IsTrue((bool)Run<I4_I1_I1>("", "Test", "OrI1", a | b, a, b));
        }
        
        delegate bool I4_Constant_I1(int expect, sbyte x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((sbyte)-42, (sbyte)48)]
        [TestCase((sbyte)17, (sbyte)1)]
        [TestCase((sbyte)0, (sbyte)0)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void OrConstantI1Right(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI1Right", "sbyte", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "OrConstantI1Right", (a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((sbyte)-42, (sbyte)48)]
        [TestCase((sbyte)17, (sbyte)1)]
        [TestCase((sbyte)0, (sbyte)0)]
        [TestCase(sbyte.MinValue, sbyte.MaxValue)]
        [Test]
        public void OrConstantI1Left(sbyte a, sbyte b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI1Left", "sbyte", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I1>("", "Test", "OrConstantI1Left", (a | b), b));
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
        [TestCase((byte)1, (byte)2)]
        [TestCase((byte)23, (byte)21)]
        // And reverse
        [TestCase((byte)2, (byte)1)]
        [TestCase((byte)21, (byte)23)]
        // (MinValue, X) Cases
        [TestCase(byte.MinValue, (byte)0)]
        [TestCase(byte.MinValue, (byte)1)]
        [TestCase(byte.MinValue, (byte)17)]
        [TestCase(byte.MinValue, (byte)123)]
        // (MaxValue, X) Cases
        [TestCase(byte.MaxValue, (byte)0)]
        [TestCase(byte.MaxValue, (byte)1)]
        [TestCase(byte.MaxValue, (byte)17)]
        [TestCase(byte.MaxValue, (byte)123)]
        // (X, MinValue) Cases
        [TestCase((byte)0, byte.MinValue)]
        [TestCase((byte)1, byte.MinValue)]
        [TestCase((byte)17, byte.MinValue)]
        [TestCase((byte)123, byte.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((byte)0, byte.MaxValue)]
        [TestCase((byte)1, byte.MaxValue)]
        [TestCase((byte)17, byte.MaxValue)]
        [TestCase((byte)123, byte.MaxValue)]
        // Extremvaluecases
        [TestCase(byte.MinValue, byte.MaxValue)]
        [TestCase(byte.MaxValue, byte.MinValue)]
        [Test]
        public void OrU1(byte a, byte b)
        {
            CodeSource = CreateTestCode("OrU1", "byte", "uint");
            Assert.IsTrue((bool)Run<U4_U1_U1>("", "Test", "OrU1", (uint)(a | b), a, b));
        }
        
        delegate bool U4_Constant_U1(uint expect, byte x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((byte)23, (byte)148)]
        [TestCase((byte)17, (byte)1)]
        [TestCase((byte)0, (byte)0)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void OrConstantU1Right(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU1Right", "byte", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "OrConstantU1Right", (uint)(a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((byte)23, (byte)148)]
        [TestCase((byte)17, (byte)1)]
        [TestCase((byte)0, (byte)0)]
        [TestCase(byte.MinValue, byte.MaxValue)]
        [Test]
        public void OrConstantU1Left(byte a, byte b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU1Left", "byte", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U1>("", "Test", "OrConstantU1Left", (uint)(a | b), b));
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
        [TestCase((short)1, (short)2)]
        [TestCase((short)23, (short)21)]
        [TestCase((short)1, (short)-2)]
        [TestCase((short)-1, (short)2)]
        [TestCase((short)0, (short)0)]
        [TestCase((short)-17, (short)-2)]
        // And reverse
        [TestCase((short)2, (short)1)]
        [TestCase((short)21, (short)23)]
        [TestCase((short)-2, (short)1)]
        [TestCase((short)2, (short)-1)]
        [TestCase((short)-2, (short)-17)]
        // (MinValue, X) Cases
        [TestCase(short.MinValue, (short)0)]
        [TestCase(short.MinValue, (short)1)]
        [TestCase(short.MinValue, (short)17)]
        [TestCase(short.MinValue, (short)123)]
        [TestCase(short.MinValue, (short)-0)]
        [TestCase(short.MinValue, (short)-1)]
        [TestCase(short.MinValue, (short)-17)]
        [TestCase(short.MinValue, (short)-123)]
        // (MaxValue, X) Cases
        [TestCase(short.MaxValue, (short)0)]
        [TestCase(short.MaxValue, (short)1)]
        [TestCase(short.MaxValue, (short)17)]
        [TestCase(short.MaxValue, (short)123)]
        [TestCase(short.MaxValue, (short)-0)]
        [TestCase(short.MaxValue, (short)-1)]
        [TestCase(short.MaxValue, (short)-17)]
        [TestCase(short.MaxValue, (short)-123)]
        // (X, MinValue) Cases
        [TestCase((short)0, short.MinValue)]
        [TestCase((short)1, short.MinValue)]
        [TestCase((short)17, short.MinValue)]
        [TestCase((short)123, short.MinValue)]
        [TestCase((short)-0, short.MinValue)]
        [TestCase((short)-1, short.MinValue)]
        [TestCase((short)-17, short.MinValue)]
        [TestCase((short)-123, short.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((short)0, short.MaxValue)]
        [TestCase((short)1, short.MaxValue)]
        [TestCase((short)17, short.MaxValue)]
        [TestCase((short)123, short.MaxValue)]
        [TestCase((short)-0, short.MaxValue)]
        [TestCase((short)-1, short.MaxValue)]
        [TestCase((short)-17, short.MaxValue)]
        [TestCase((short)-123, short.MaxValue)]
        // Extremvaluecases
        [TestCase(short.MinValue, short.MaxValue)]
        [TestCase(short.MaxValue, short.MinValue)]
        [Test]
        public void OrI2(short a, short b)
        {
            short e = (short)(a | b);
            CodeSource = CreateTestCode("OrI2", "short", "int");
            Assert.IsTrue((bool)Run<I4_I2_I2>("", "Test", "OrI2", (a | b), a, b));
        }
        
        delegate bool I4_Constant_I2(int expect, short x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((short)-23, (short)148)]
        [TestCase((short)17, (short)1)]
        [TestCase((short)0, (short)0)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void OrConstantI2Right(short a, short b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI2Right", "short", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "OrConstantI2Right", (a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((short)-23, (short)148)]
        [TestCase((short)17, (short)1)]
        [TestCase((short)0, (short)0)]
        [TestCase(short.MinValue, short.MaxValue)]
        [Test]
        public void OrConstantI2Left(short a, short b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI2Left", "short", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I2>("", "Test", "OrConstantI2Left", (a | b), b));
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
        [TestCase((ushort)1, (ushort)2)]
        [TestCase((ushort)23, (ushort)21)]
        // And reverse
        [TestCase((ushort)2, (ushort)1)]
        [TestCase((ushort)21, (ushort)23)]
        // (MinValue, X) Cases
        [TestCase(ushort.MinValue, (ushort)0)]
        [TestCase(ushort.MinValue, (ushort)1)]
        [TestCase(ushort.MinValue, (ushort)17)]
        [TestCase(ushort.MinValue, (ushort)123)]
        // (MaxValue, X) Cases
        [TestCase(ushort.MaxValue, (ushort)0)]
        [TestCase(ushort.MaxValue, (ushort)1)]
        [TestCase(ushort.MaxValue, (ushort)17)]
        [TestCase(ushort.MaxValue, (ushort)123)]
        // (X, MinValue) Cases
        [TestCase((ushort)0, ushort.MinValue)]
        [TestCase((ushort)1, ushort.MinValue)]
        [TestCase((ushort)17, ushort.MinValue)]
        [TestCase((ushort)123, ushort.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((ushort)0, ushort.MaxValue)]
        [TestCase((ushort)1, ushort.MaxValue)]
        [TestCase((ushort)17, ushort.MaxValue)]
        [TestCase((ushort)123, ushort.MaxValue)]
        // Extremvaluecases
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [TestCase(ushort.MaxValue, ushort.MinValue)]
        [Test]
        public void OrU2(ushort a, ushort b)
        {
            ushort e = (ushort)(a | b);
            CodeSource = CreateTestCode("OrU2", "ushort", "uint");
            Assert.IsTrue((bool)Run<U4_U2_U2>("", "Test", "OrU2", (uint)(a | b), a, b));
        }
        
        delegate bool U4_Constant_U2(uint expect, ushort x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ushort)23, (ushort)148)]
        [TestCase((ushort)17, (ushort)1)]
        [TestCase((ushort)0, (ushort)0)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void OrConstantU2Right(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU2Right", "ushort", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "OrConstantU2Right", (uint)(a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ushort)23, (ushort)148)]
        [TestCase((ushort)17, (ushort)1)]
        [TestCase((ushort)0, (ushort)0)]
        [TestCase(ushort.MinValue, ushort.MaxValue)]
        [Test]
        public void OrConstantU2Left(ushort a, ushort b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU2Left", "ushort", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U2>("", "Test", "OrConstantU2Left", (uint)(a | b), b));
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
        [TestCase(0, 0)]
        [TestCase(-17, -2)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -17)]
        // (MinValue, X) Cases
        [TestCase(int.MinValue, 0)]
        [TestCase(int.MinValue, 1)]
        [TestCase(int.MinValue, 17)]
        [TestCase(int.MinValue, 123)]
        [TestCase(int.MinValue, -0)]
        [TestCase(int.MinValue, -1)]
        [TestCase(int.MinValue, -17)]
        [TestCase(int.MinValue, -123)]
        // (MaxValue, X) Cases
        [TestCase(int.MaxValue, 0)]
        [TestCase(int.MaxValue, 1)]
        [TestCase(int.MaxValue, 17)]
        [TestCase(int.MaxValue, 123)]
        [TestCase(int.MaxValue, -0)]
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
        [Test]
        public void OrI4(int a, int b)
        {
            CodeSource = CreateTestCode("OrI4", "int", "int");
            Assert.IsTrue((bool)Run<I4_I4_I4>("", "Test", "OrI4", (a | b), a, b));
        }
        
        delegate bool I4_Constant_I4(int expect, int x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void OrConstantI4Right(int a, int b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI4Right", "int", "int", null, b.ToString());
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "OrConstantI4Right", (a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(int.MinValue, int.MaxValue)]
        [Test]
        public void OrConstantI4Left(int a, int b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI4Left", "int", "int", a.ToString(), null);
            Assert.IsTrue((bool)Run<I4_Constant_I4>("", "Test", "OrConstantI4Left", (a | b), b));
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
        [TestCase((uint)1, (uint)2)]
        [TestCase((uint)23, (uint)21)]
        // And reverse
        [TestCase((uint)2, (uint)1)]
        [TestCase((uint)21, (uint)23)]
        // (MinValue, X) Cases
        [TestCase(uint.MinValue, (uint)0)]
        [TestCase(uint.MinValue, (uint)1)]
        [TestCase(uint.MinValue, (uint)17)]
        [TestCase(uint.MinValue, (uint)123)]
        // (MaxValue, X) Cases
        [TestCase(uint.MaxValue, (uint)0)]
        [TestCase(uint.MaxValue, (uint)1)]
        [TestCase(uint.MaxValue, (uint)17)]
        [TestCase(uint.MaxValue, (uint)123)]
        // (X, MinValue) Cases
        [TestCase((uint)0, uint.MinValue)]
        [TestCase((uint)1, uint.MinValue)]
        [TestCase((uint)17, uint.MinValue)]
        [TestCase((uint)123, uint.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((uint)0, uint.MaxValue)]
        [TestCase((uint)1, uint.MaxValue)]
        [TestCase((uint)17, uint.MaxValue)]
        [TestCase((uint)123, uint.MaxValue)]
        // Extremvaluecases
        [TestCase(uint.MinValue, uint.MaxValue)]
        [TestCase(uint.MaxValue, uint.MinValue)]
        [Test]
        public void OrU4(uint a, uint b)
        {
            CodeSource = CreateTestCode("OrU4", "uint", "uint");
            Assert.IsTrue((bool)Run<U4_U4_U4>("", "Test", "OrU4", (uint)(a | b), a, b));
        }
        
        delegate bool U4_Constant_U4(uint expect, uint x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((uint)23, (uint)148)]
        [TestCase((uint)17, (uint)1)]
        [TestCase((uint)0, (uint)0)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void OrConstantU4Right(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU4Right", "uint", "uint", null, b.ToString());
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "OrConstantU4Right", (uint)(a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((uint)23, (uint)148)]
        [TestCase((uint)17, (uint)1)]
        [TestCase((uint)0, (uint)0)]
        [TestCase(uint.MinValue, uint.MaxValue)]
        [Test]
        public void OrConstantU4Left(uint a, uint b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU4Left", "uint", "uint", a.ToString(), null);
            Assert.IsTrue((bool)Run<U4_Constant_U4>("", "Test", "OrConstantU4Left", (uint)(a | b), b));
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
        [TestCase(0, 0)]
        [TestCase(-17, -2)]
        // And reverse
        [TestCase(2, 1)]
        [TestCase(21, 23)]
        [TestCase(-2, 1)]
        [TestCase(2, -1)]
        [TestCase(-2, -17)]
        // (MinValue, X) Cases
        [TestCase(long.MinValue, 0)]
        [TestCase(long.MinValue, 1)]
        [TestCase(long.MinValue, 17)]
        [TestCase(long.MinValue, 123)]
        [TestCase(long.MinValue, -0)]
        [TestCase(long.MinValue, -1)]
        [TestCase(long.MinValue, -17)]
        [TestCase(long.MinValue, -123)]
        // (MaxValue, X) Cases
        [TestCase(long.MaxValue, 0)]
        [TestCase(long.MaxValue, 1)]
        [TestCase(long.MaxValue, 17)]
        [TestCase(long.MaxValue, 123)]
        [TestCase(long.MaxValue, -0)]
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
        [Test]
        public void OrI8(long a, long b)
        {
            CodeSource = CreateTestCode("OrI8", "long", "long");
            Assert.IsTrue((bool)Run<I8_I8_I8>("", "Test", "OrI8", (a | b), a, b));
        }
        
        delegate bool I8_Constant_I8(long expect, long x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void OrConstantI8Right(long a, long b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI8Right", "long", "long", null, b.ToString());
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "OrConstantI8Right", (a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase(-23, 148)]
        [TestCase(17, 1)]
        [TestCase(0, 0)]
        [TestCase(long.MinValue, long.MaxValue)]
        [Test]
        public void OrConstantI8Left(long a, long b)
        {
            CodeSource = CreateConstantTestCode("OrConstantI8Left", "long", "long", a.ToString(), null);
            Assert.IsTrue((bool)Run<I8_Constant_I8>("", "Test", "OrConstantI8Left", (a | b), b));
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
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ulong)1, (ulong)2)]
        [TestCase((ulong)23, (ulong)21)]
        // And reverse
        [TestCase((ulong)2, (ulong)1)]
        [TestCase((ulong)21, (ulong)23)]
        // (MinValue, X) Cases
        [TestCase(ulong.MinValue, (ulong)0)]
        [TestCase(ulong.MinValue, (ulong)1)]
        [TestCase(ulong.MinValue, (ulong)17)]
        [TestCase(ulong.MinValue, (ulong)123)]
        // (MaxValue, X) Cases
        [TestCase(ulong.MaxValue, (ulong)0)]
        [TestCase(ulong.MaxValue, (ulong)1)]
        [TestCase(ulong.MaxValue, (ulong)17)]
        [TestCase(ulong.MaxValue, (ulong)123)]
        // (X, MinValue) Cases
        [TestCase((ulong)0, ulong.MinValue)]
        [TestCase((ulong)1, ulong.MinValue)]
        [TestCase((ulong)17, ulong.MinValue)]
        [TestCase((ulong)123, ulong.MinValue)]
        // (X, MaxValue) Cases
        [TestCase((ulong)0, ulong.MaxValue)]
        [TestCase((ulong)1, ulong.MaxValue)]
        [TestCase((ulong)17, ulong.MaxValue)]
        [TestCase((ulong)123, ulong.MaxValue)]
        // Extremvaluecases
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [TestCase(ulong.MaxValue, ulong.MinValue)]
        [Test]
        public void OrU8(ulong a, ulong b)
        {
            CodeSource = CreateTestCode("OrU8", "ulong", "ulong");
            Assert.IsTrue((bool)Run<U8_U8_U8>("", "Test", "OrU8", (ulong)(a | b), a, b));
        }
        
        delegate bool U8_Constant_U8(ulong expect, ulong x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ulong)23, (ulong)148)]
        [TestCase((ulong)17, (ulong)1)]
        [TestCase((ulong)0, (ulong)0)]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void OrConstantU8Right(ulong a, ulong b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU8Right", "ulong", "ulong", null, b.ToString());
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "OrConstantU8Right", (ulong)(a | b), a));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [TestCase((ulong)23, (ulong)148)]
        [TestCase((ulong)17, (ulong)1)]
        [TestCase((ulong)0, (ulong)0)]
        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [Test]
        public void OrConstantU8Left(ulong a, ulong b)
        {
            CodeSource = CreateConstantTestCode("OrConstantU8Left", "ulong", "ulong", a.ToString(), null);
            Assert.IsTrue((bool)Run<U8_Constant_U8>("", "Test", "OrConstantU8Left", (ulong)(a | b), b));
        }
        #endregion
    }
}
