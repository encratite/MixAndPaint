using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Linq;

namespace MixAndPaint.Algorithm
{
	static class PlaneLineIntersection
	{
		public static Mix GetBestMix(Vector<double> target, Paint[] paints)
		{
			Mix bestMix = null;
			Common.IterateOverPaints(target, paints, 3, (target, mixPaints) =>
			{
				var mix = GetIntersection(target, mixPaints);
				if (bestMix == null || mix.Distance < bestMix.Distance)
				{
					bestMix = mix;
					Console.WriteLine($"New best mix: {bestMix}");
				}
			});
			return bestMix;
		}

		private static Mix GetIntersection(Vector<double> target, Paint[] paints)
		{
			var p1 = paints[0].Color;
			var p2 = paints[1].Color;
			var p3 = paints[2].Color;
			var v = p2 - p1;
			var w = p3 - p1;
			var n = CrossProduct(v, w);
			var A = CreateMatrix.DenseOfColumnVectors(v, w, -n);
			var b = target - p1;
			var x = A.Solve(b);
			var intersection = p1 + x[0] * v + x[1] * w;
			var weights = paints.Select(paint => 1 / (paint.Color - intersection).L2Norm()).ToArray();
			double distance = Common.GetDistance(target, paints, weights, out PaintRatio[] paintRatios);
			var mix = new Mix(paintRatios, distance);
			return mix;
		}

		private static Vector<double> CrossProduct(Vector<double> left, Vector<double> right)
		{
			Vector result = new DenseVector(3);
			result[0] = left[1] * right[2] - left[2] * right[1];
			result[1] = -left[0] * right[2] + left[2] * right[0];
			result[2] = left[0] * right[1] - left[1] * right[0];
			return result;
		}
	}
}
