using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace MixAndPaint
{
	class Paint
	{
		public string Name { get; set; }
		public Vector<double> Color { get; set; }

		public Paint(string name, int red, int green, int blue)
		{
			Name = name;
			Color = new DenseVector(new double[] { red, green, blue });
		}

		public override string ToString()
		{
			return Name;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
