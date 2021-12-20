using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MixAndPaint.Algorithm
{
	static class Common
	{
		public static double GetDistance(Vector<double> target, Paint[] paints, double[] weights, out PaintRatio[] paintRatios)
		{
			paintRatios = new PaintRatio[paints.Length];
			double distance = GetDistance(target, paints, weights, paintRatios);
			return distance;
		}

		public static double GetDistance(Vector<double> target, Paint[] paints, double[] weights, PaintRatio[] paintRatios = null)
		{
			double weightSum = weights.Sum();
			Vector<double> weightedColor = new DenseVector(new double[] { 0, 0, 0 });
			for (int i = 0; i < weights.Length; i++)
			{
				var paint = paints[i];
				double ratio = weights[i] / weightSum;
				if (paintRatios != null)
					paintRatios[i] = new PaintRatio(paint, ratio);
				weightedColor += ratio * paint.Color;
			}
			double distance = (target - weightedColor).L2Norm();
			return distance;
		}

		public static void IterateOverPaints(Vector<double> target, Paint[] paints, int depth, Action<Vector<double>, Paint[]> handler)
		{
			var remainingPaints = new HashSet<Paint>(paints);
			var currentPaints = new List<Paint>();
			IterateOverPaints(target, remainingPaints, depth, currentPaints, handler);
		}

		private static void IterateOverPaints(Vector<double> target, HashSet<Paint> remainingPaints, int remainingDepth, List<Paint> currentPaints, Action<Vector<double>, Paint[]> handler)
		{
			if (remainingDepth == 0 || remainingPaints.Count == 0)
			{
				handler(target, currentPaints.ToArray());
				return;
			}
			int newRemainingDepth = remainingDepth - 1;
			foreach (var paint in remainingPaints)
			{
				var newRemainingPaints = new HashSet<Paint>(remainingPaints);
				newRemainingPaints.Remove(paint);
				var newCurrentPaints = new List<Paint>(currentPaints);
				newCurrentPaints.Add(paint);
				IterateOverPaints(target, newRemainingPaints, newRemainingDepth, newCurrentPaints, handler);
			}
		}
	}
}
