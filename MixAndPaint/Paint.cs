namespace MixAndPaint
{
	class Paint
	{
		public string Name { get; set; }
		public Color Color { get; set; }

		public Paint(string name, int red, int green, int blue)
		{
			Name = name;
			Color = new Color(red, green, blue);
		}
	}
}
