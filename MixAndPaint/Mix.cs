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
			var tokens = Paints.Select(paintRatio => paintRatio.ToString()).ToList();
			tokens.Add($"distance: {Distance:0.00}");
			string output = string.Join(", ", tokens);
			return output;
		}
	}
}
