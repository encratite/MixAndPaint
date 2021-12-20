using MathNet.Numerics.LinearAlgebra.Double;
using MixAndPaint.Algorithm;
using System;
using System.Reflection;

namespace MixAndPaint
{
	class Program
	{
		static void Main(string[] arguments)
		{
			if (arguments.Length != 3)
			{
				var assembly = Assembly.GetExecutingAssembly();
				var name = assembly.GetName();
				Console.WriteLine("Determine how to mix a color using a set of predefined paints.");
				Console.WriteLine(string.Empty);
				Console.WriteLine("Usage:");
				Console.WriteLine($"{name.Name} <red> <green> <blue>");
				return;
			}
			Func<int, int> getArgumentInt = i => int.Parse(arguments[i]);
			int red = getArgumentInt(0);
			int green = getArgumentInt(1);
			int blue = getArgumentInt(2);
			DetermineBestMixes(red, green, blue);
		}

		private static void DetermineBestMixes(int red, int green, int blue)
		{
			var target = new DenseVector(new double[] { red, green, blue });
			Console.WriteLine($"Approximating color: {target[0]}, {target[1]}, {target[2]}");
			var paints = Paints.Definitions;
			var bestMix1 = PlaneLineIntersection.GetBestMix(target, paints);
			Console.WriteLine($"Best mix with line-plane intersection: {bestMix1}");
			var bestMix2 = FunctionGradient.GetBestMix(target, paints);
			Console.WriteLine($"Best mix with Broyden–Fletcher–Goldfarb–Shanno: {bestMix2}");
		}
	}
}
