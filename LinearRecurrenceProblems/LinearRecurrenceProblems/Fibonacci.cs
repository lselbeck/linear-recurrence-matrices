using System;
using System.Diagnostics;

namespace LinearRecurrenceProblems
{
	/// <summary>
	/// Takes int n as user input, outputs the nth fibonacci number.
	/// </summary>
	class Fibonacci
	{
		static void Main()
		{
			while (true)
			{
				int n = int.Parse(Console.ReadLine());
				if (n < 1) throw new ArgumentOutOfRangeException(nameof(n), "N cannot be negative.");
				const int iterations = 5;

				//long result1;
				//var t = new Thread(() =>
				//{
				//	result1 = GetFibMatrixRecursive(n);
				//});
				//t.Start();

				Console.WriteLine($"Matrix method with recursive power method: {GetFibMatrixRecursive(n)}");
				Console.WriteLine($"Benchmark (ticks): {Benchmark(() => GetFibMatrixRecursive(n), iterations)}\n");

				Console.WriteLine($"Matrix method with eigen power method: {GetFibMatrixConstant(n):0.#}");
				Console.WriteLine($"Benchmark (ticks): {Benchmark(() => GetFibMatrixConstant(n), iterations)}\n");

				Console.WriteLine($"Simple recursion: {GetFibRecursive(n)}");
				Console.WriteLine($"Benchmark (ticks): {Benchmark(() => GetFibRecursive(n), iterations)}\n");
			}
		}

		public static long GetFibMatrixRecursive(long n)
		{
			long[,] transform = { { 0, 1 }, { 1, 1 } };
			long[,] baseCases = { { 1 }, { 1 } };

			return MatrixHelper.Multiply(MatrixHelper.PowerRecursive(transform, n - 1), baseCases)[0, 0];
		}

		public static long GetFibRecursive(long n)
		{
			if (n < 1) return 0;
			if (n == 1 || n == 2) return 1;
			return GetFibRecursive(n - 1) + GetFibRecursive(n - 2);
		}

		/// <summary>
		/// This equation is derived from the eigenvalue method of matrix powers:
		/// T^n = P * D^n * P^(-1)
		/// Where T is the transformation matrix.
		/// Only accurate when n is less than 70, due to imprecise floating points
		/// </summary>
		public static double GetFibMatrixConstant(long n)
		{
			double sqrt5 = Math.Sqrt(5);
			double fib = Math.Pow(2, -1 - n) * Math.Pow(1 - sqrt5, n) * (1 + sqrt5) / sqrt5
						- Math.Pow((1 - sqrt5) / 2.0, n) * (1 + sqrt5) / (sqrt5 * (-1 + sqrt5))
						+ (-1 + sqrt5) * Math.Pow(1 + sqrt5, -1 + n) / (Math.Pow(2, n) * sqrt5) +
						 Math.Pow(2, -1 - n) * (-1 + sqrt5) * Math.Pow(1 + sqrt5, n) / sqrt5;
			return Math.Round(fib);
		}

		private static long Benchmark(Action act, int iterations)
		{
			GC.Collect();
			act.Invoke(); // run once outside of loop to avoid initialization costs
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				act.Invoke();
			}
			sw.Stop();
			return sw.ElapsedTicks / iterations;
		}
	}
}
