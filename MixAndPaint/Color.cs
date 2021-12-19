using System;

namespace MixAndPaint
{
	struct Color
	{
		public double Red { get; set; }
		public double Green { get; set; }
		public double Blue { get; set; }

		public Color(int red, int green, int blue)
		{
			const int maximum = 255;
			Red = (double)red / maximum;
			Green = (double)green / maximum;
			Blue = (double)blue / maximum;
		}

		public Color(double red, double green, double blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}

		public double Magnitude()
		{
			double redSquare = Math.Pow(Red, 2);
			double greenSquare = Math.Pow(Green, 2);
			double blueSquare = Math.Pow(Blue, 2);
			double distance = Math.Sqrt(redSquare + greenSquare + blueSquare);
			return distance;
		}

		public static Color operator +(Color x, Color y)
		{
			return new Color(x.Red + y.Red, x.Green + y.Green, x.Blue + y.Blue);
		}

		public static Color operator -(Color x, Color y)
		{
			return new Color(x.Red - y.Red, x.Green - y.Green, x.Blue - y.Blue);
		}

		public static Color operator *(double x, Color y)
		{
			return new Color(x * y.Red, x * y.Green, x * y.Blue);
		}
	}
}
