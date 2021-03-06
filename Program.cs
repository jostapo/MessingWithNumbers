﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MessingWithNumbers
{
    // https://www.youtube.com/watch?v=mXTxQko-JH0&feature=youtu.be&t=399
    // http://nicolas.limare.net/pro/notes/2014/12/12_arit_speed/
    class Program
    {
        const ulong MAX_ITERATIONS = 10000000000;
        const ulong ITERATION_STEP = 10;
        const float MULT_F = .3F;
        const double MULT_D = MULT_F;
        const decimal MULT_DEC = (decimal)MULT_F;

        static uint result_uint;
        static ulong result_long;
        static float result_float;
        static double result_double;
        static decimal result_decimal;


        static long _time_inmath;
        static long _time_overrall;
        static long _time_overhead;

        //Lessons for numerics
        // 1) FLOAT and DOUBLE are effectively the same from a performance perspective (On X86 64 bit platform)
        // 2) FLOAT tuckers out very quickly, but doesn't throw errors when overflows start happening
        // 3) DECIMAL hangs in there, but is sometimes more than 20 times slower than DOUBLE

        static void Main(string[] args)
        {
            Console.WindowWidth = 200;
            Console.WindowHeight = 40;
            Console.CursorVisible = false;


            var sw = new Stopwatch();

            CalculatePi(10000); // comment out for lesson on Intel Speedstep and benchmarks
            
            sw.Start();

            for (ulong i = 10; i < MAX_ITERATIONS; i = i * ITERATION_STEP)
            {
                RunNumbers(i);
                
            }
            sw.Stop();

            _time_overrall = sw.ElapsedTicks;
            _time_overhead = _time_overrall - _time_inmath;

            Console.WriteLine();
            Console.WriteLine("{0,15}{1,20:N0}", "Time In Math", _time_inmath);
            Console.WriteLine("{0,15}{1,20:N0}", "Time Overall", _time_overrall);
            Console.WriteLine("{0,15}{1,20:N0}{2,10:P4}", "Overhead", _time_overhead , (decimal)_time_overhead / _time_overrall ); // comment out (decimal) for lesson on integer division
            Console.WriteLine();

            Console.WriteLine("Press[Enter] to quit...");
            Console.ReadLine();
        }

        private static void RunNumbers(ulong iterations)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Iterations: {0:N0}", iterations);

            long t_uint, t_long, t_float, t_double, t_decimal;

            t_uint = Loop_Int(iterations);

            t_long = Loop_Long(iterations);

            t_float = Loop_Float(iterations);

            t_double = Loop_Double(iterations);

            t_decimal = Loop_Decimal(iterations);

            decimal baseline = (decimal)iterations * (decimal)MULT_F;

            Console.WriteLine("{0,10}{1,30}{2,30}{3,20}{4,20}", "Test", "Result", "Delta", "Time", "X Faster");
            Console.WriteLine(new string('-', 115));
            Console.WriteLine("{0,10}{1,30:N10}", "Baseline", baseline);
            Console.WriteLine("{0,10}{1,30:N10}{2,30:N10}{3,20:N0}{4,20:0.00}", "Float", result_float, result_decimal - (decimal)result_float, t_float, (double)t_decimal / (double)t_float  );
            Console.WriteLine("{0,10}{1,30:N10}{2,30:N10}{3,20:N0}{4,20:0.00}", "Double", result_double, result_decimal - (decimal)result_double, t_double, (double)t_decimal / (double)t_double);
            Console.WriteLine("{0,10}{1,30:N10}{2,30:N10}{3,20:N0}{4,20:0.00}", "Decimal", result_decimal, result_decimal - result_decimal, t_decimal, (double)t_decimal / (double)t_decimal);
            Console.WriteLine("{0,10}{1,30:N10}{2,30}{3,20:N0}{4,20:0.00}", "Int", result_uint, "", t_uint, (double)t_long / (double)t_uint);
            Console.WriteLine("{0,10}{1,30:N10}{2,30}{3,20:N0}{4,20:0.00}", "Long", result_long, "", t_long, (double)t_long / (double)t_long);


            _time_inmath = _time_inmath + t_float + t_double + t_decimal + t_uint + t_long;
        }

        private static long Loop_Int(ulong iterations)
        {
            Stopwatch sw = new Stopwatch();

            sw.Reset();
            sw.Start();

            for (ulong i = 0; i < iterations; i++)
            {
                result_uint = result_uint + 1 * 2;
            }

            sw.Stop();


            return sw.ElapsedTicks;
        }

        private static long Loop_Long(ulong iterations)
        {
            Stopwatch sw = new Stopwatch();

            sw.Reset();
            sw.Start();

            for (ulong i = 0; i < iterations; i++)
            {
                result_long = result_long + 1 * 2;
            }

            sw.Stop();

            return sw.ElapsedTicks;
        }

        //[MethodImpl(MethodImplOptions.NoOptimization)]  //comment (all three) for lesson on .NET compiler optimizations (or compare DEBUG run vs RELEASE)
        private static long Loop_Float(ulong iterations)
        {
            Stopwatch sw = new Stopwatch();

            sw.Reset();
            sw.Start();

            for (ulong i = 0; i < iterations; i++)
            {
                result_float = result_float + 1 * MULT_F;
            }

            sw.Stop();

            return sw.ElapsedTicks;
        }

        //[MethodImpl(MethodImplOptions.NoOptimization)]
        private static long Loop_Double(ulong iterations)
        {
            Stopwatch sw = new Stopwatch();

            sw.Reset();
            sw.Start();

            for (ulong i = 0; i < iterations; i++)
            {
                result_double = result_double + 1 * MULT_D;
            }

            sw.Stop();

            return sw.ElapsedTicks;
        }

        //[MethodImpl(MethodImplOptions.NoOptimization)]
        private static long Loop_Decimal(ulong iterations)
        {
            Stopwatch sw = new Stopwatch();

            sw.Reset();
            sw.Start();
            
            for (ulong i = 0; i < iterations; i++)
            {
                result_decimal = result_decimal + 1 * MULT_DEC;
            }

            sw.Stop();

            return sw.ElapsedTicks;
        }

        public static string CalculatePi(int digits)
        {
            Console.Write("Doing something to warmup! ");
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            digits++;

            uint[] x = new uint[digits * 10 / 3 + 2];
            uint[] r = new uint[digits * 10 / 3 + 2];

            uint[] pi = new uint[digits];

            for (int j = 0; j < x.Length; j++)
                x[j] = 20;

            for (int i = 0; i < digits; i++)
            {
                uint carry = 0;
                for (int j = 0; j < x.Length; j++)
                {
                    uint num = (uint)(x.Length - j - 1);
                    uint dem = num * 2 + 1;

                    x[j] += carry;

                    uint q = x[j] / dem;
                    r[j] = x[j] % dem;

                    carry = q * num;
                }


                pi[i] = (x[x.Length - 1] / 10);


                r[x.Length - 1] = x[x.Length - 1] % 10; ;

                for (int j = 0; j < x.Length; j++)
                    x[j] = r[j] * 10;
            }

            var result = "";

            uint c = 0;

            for (int i = pi.Length - 1; i >= 0; i--)
            {
                pi[i] += c;
                c = pi[i] / 10;

                result = (pi[i] % 10).ToString() + result;
            }

            sw.Stop();
            Console.WriteLine("{0:N0}",sw.ElapsedTicks);

            return result;
        }
    }
}
