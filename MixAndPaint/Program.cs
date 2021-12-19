﻿using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Linq;

namespace MixAndPaint
{
	class Program
	{
		static void Main(string[] args)
		{
			var paints = new Paint[]
			{
				new Paint("Titanium White", 248, 245, 253),
				new Paint("Lemon Yellow", 237, 209, 1),
				new Paint("Mid Yellow", 238, 195, 17),
				new Paint("Yellow Ochre", 209, 150, 35),
				new Paint("Orange Yellow", 225, 85, 54),
				new Paint("Scarlet Red", 198, 63, 53),
				new Paint("Crimson Red", 155, 57, 52),
				new Paint("Rose", 175, 60, 125),

				new Paint("Violet", 76, 57, 109),
				new Paint("Prussian Blue", 19, 49, 74),
				new Paint("Phthalo Blue", 22, 53, 80),
				new Paint("Ultramarine Blue", 23, 65, 119),
				new Paint("Cobalt Blue", 77, 100, 153),
				new Paint("Cerulean Blue", 50, 95, 141),
				new Paint("Phthalo Green", 5, 100, 77),
				new Paint("Emerald Green", 36, 146, 92),

				new Paint("Prussian Green", 41, 140, 61),
				new Paint("Pale Green", 4, 129, 64),
				new Paint("Raw Sienna", 153, 111, 44),
				new Paint("Burnt Sienna", 117, 71, 51),
				new Paint("Burnt Umber", 75, 61, 48),
				new Paint("Raw Umber", 65, 49, 48),
				new Paint("Cold Gray", 122, 124, 133),
				new Paint("Mars Black", 8, 6, 15),
			};
			var target = new DenseVector(new double[] { 241, 226, 193 });
			Console.WriteLine($"Approximating color: {target[0]}, {target[1]}, {target[2]}");
			var bestMix = GetBestMix(target, paints);
			Console.WriteLine($"Best mix: {bestMix}");
		}

		private static Mix GetBestMix(Vector<double> target, Paint[] paints)
		{
			Mix bestMix = null;
			for (int i = 0; i < paints.Length; i++)
			{
				for (int j = 0; j < paints.Length; j++)
				{
					for (int k = 0; k < paints.Length; k++)
					{
						if (i < j && j < k)
						{
							var mix = GetMix(target, new Paint[] { paints[i], paints[j], paints[k] });
							if (bestMix == null || mix.Distance < bestMix.Distance)
							{
								bestMix = mix;
								Console.WriteLine($"New best mix: {bestMix}");
							}
						}
					}
				}
			}
			return bestMix;
		}

		private static Mix GetMix(Vector<double> target, Paint[] paints)
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
			double weightSum = weights.Sum();
			var paintRatios = new PaintRatio[paints.Length];
			Vector<double> weightedColor = new DenseVector(new double[] { 0, 0, 0 });
			for (int i = 0; i < paints.Length; i++)
			{
				var paint = paints[i];
				double ratio = weights[i] / weightSum;
				paintRatios[i] = new PaintRatio(paint, ratio);
				weightedColor += ratio * paint.Color;
			}
			double distance = (target - weightedColor).L2Norm();
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
