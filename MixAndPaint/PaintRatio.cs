namespace MixAndPaint
{
	class PaintRatio
	{
		public Paint Paint { get; set; }
		public double Ratio { get; set; }

		public PaintRatio(Paint paint, double ratio)
		{
			Paint = paint;
			Ratio = ratio;
		}

		public override string ToString()
		{
			return $"{Paint.Name} ({Ratio:0%})";
		}
	}
}
