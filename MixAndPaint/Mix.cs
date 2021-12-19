using System.Linq;

namespace MixAndPaint
{
	class Mix
	{
		public PaintRatio[] Paints { get; set; }
		public double Distance { get; set; }

		public Mix(PaintRatio[] paints, double distance)
		{
			Paints = paints.OrderByDescending(paint => paint.Ratio).ToArray();
			Distance = distance;
		}

		public override string ToString()
		{
			var tokens = Paints.Select(paintRatio => paintRatio.ToString());
			string output = string.Join(", ", tokens);
			return output;
		}
	}
}
