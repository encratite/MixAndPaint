using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Linq;

namespace MixAndPaint.Algorithm
{
	class FunctionGradient
	{
		public static Mix GetBestMix(Vector<double> target, Paint[] paints)
		{
			Mix bestMix = null;
			const int paintCount = 3;
			Common.IterateOverPaints(target, paints, paintCount, (target, mixPaints) =>
			{
				double getDistance(Vector<double> weights)
				{
					return Common.GetDistance(target, mixPaints, weights.ToArray());
				}

				Vector<double> getDistanceGradient(Vector<double> weights)
				{
					const double epsilon = 1e-5;
					double[] gradient = new double[weights.Count];
					for (int i = 0; i < weights.Count; i++)
					{
						double x = weights[i];
						double x1 = x + epsilon;
						double x2 = x - epsilon;
						double[] epsilonWeights1 = weights.ToArray();
						double[] epsilonWeights2 = weights.ToArray();
						epsilonWeights1[i] = x1;
						epsilonWeights2[i] = x2;
						double y1 = Common.GetDistance(target, mixPaints, epsilonWeights1);
						double y2 = Common.GetDistance(target, mixPaints, epsilonWeights2);
						gradient[i] = (y2 - y1) / (x2 - x1);
					}
					return new DenseVector(gradient);
				}

				Vector<double> initializeVector(double limit)
				{
					var bound = new double[paintCount];
					for (int i = 0; i < bound.Length; i++)
						bound[i] = limit;
					return new DenseVector(bound);
				}

				var lowerBound = initializeVector(0.0);
				var upperBound = initializeVector(1.0);
				var initialGuess = initializeVector(1.0 / 3.0);
				const double gradientTolerance = 0.05;
				const double parameterTolerance = 0.05;
				const double functionProgressTolerance = 0.05;
				const int maxIterations = 1000;
				var weights = FindMinimum.OfFunctionGradientConstrained
				(
					getDistance,
					getDistanceGradient,
					lowerBound,
					upperBound,
					initialGuess,
					gradientTolerance,
					parameterTolerance,
					functionProgressTolerance,
					maxIterations
				);
				double weightSum = weights.ToArray().Sum();
				double[] ratios = new double[weights.Count];
				for (int i = 0; i < ratios.Length; i++)
					ratios[i] = weights[i] / weightSum;
				double distance = Common.GetDistance(target, mixPaints, ratios, out PaintRatio[] paintRatios);
				var mix = new Mix(paintRatios, distance);
				if (bestMix == null || mix.Distance < bestMix.Distance)
				{
					bestMix = mix;
					Console.WriteLine($"New best mix: {bestMix}");
				}
			});
			return bestMix;
		}
	}
}
