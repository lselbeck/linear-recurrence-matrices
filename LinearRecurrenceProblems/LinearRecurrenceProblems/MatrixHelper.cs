using System;

namespace LinearRecurrenceProblems
{
	/// <summary>
	/// A 2x2 or 3x3 matrix.
	/// </summary>
	public static class MatrixHelper
	{
		private static readonly long[,] i3x3 = { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
		private static readonly long[,] i2x2 = { { 1, 0 }, { 0, 1 } };

		public static long[,] Multiply(long[,] a, long[,] b)
		{
			long[,] c = new long[a.GetLength(0), b.GetLength(1)];
			for (int i = 0; i < c.GetLength(0); i++)
			{
				for (int j = 0; j < c.GetLength(1); j++)
				{
					c[i, j] = 0;
					for (int k = 0; k < a.GetLength(1); k++) // OR k<b.GetLength(0)
						c[i, j] = c[i, j] + a[i, k] * b[k, j];
				}
			}
			return c;
		}

		public static long[,] PowerRecursive(long[,] a, long power)
		{
			if (a.GetLength(0) != a.GetLength(1)) throw new ArgumentException("Matrix must be square");
			if (power == 0) return a.GetLength(0) == 2 ? i2x2 : i3x3;
			if (power == 1) return a;
			if (power % 2 != 0) return Multiply(a, PowerRecursive(a, power - 1));
			var half = PowerRecursive(a, power / 2);
			return Multiply(half, half);
		}
	}
}
