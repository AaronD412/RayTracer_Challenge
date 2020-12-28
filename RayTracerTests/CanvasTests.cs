using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class CanvasTests
    {
        [Test()]
        public void CreatingACanvas()
        {
            // Given
            Canvas canvas = new Canvas(10, 20);

            // Then
            Assert.AreEqual(canvas.Width, 10);
            Assert.AreEqual(canvas.Height, 20);

            Color black = new Color(0, 0, 0);

            for (int x = 0; x < canvas.Width; x++)
            {
                for (int y = 0; y < canvas.Height; y++)
                {
                    Assert.IsTrue(canvas[x, y].NearlyEquals(black));
                }
            }
        }

        [Test()]
        public void WritingPixelsToACanvas()
        {
            // Given
            Canvas canvas = new Canvas(10, 20);
            Color red = new Color(1, 0, 0);

            // Then
            canvas[2, 3] = red;

            Assert.IsTrue(canvas[2, 3].NearlyEquals(new Color(1, 0, 0)));
        }

        [Test()]
        public void ColorsAreRedGreenBlueTuples()
        {
            // Given
            Color color = new Color(-0.5, 0.4, 1.7);

            // Then
            Assert.AreEqual(color.Red, -0.5);
            Assert.AreEqual(color.Green, 0.4);
            Assert.AreEqual(color.Blue, 1.7);
        }

        [Test()]
        public void AddingColors()
        {
            // Given
            Color color1 = new Color(0.9, 0.6, 0.75);
            Color color2 = new Color(0.7, 0.1, 0.25);

            // Then
            Color result = color1 + color2;

            Assert.IsTrue(result.NearlyEquals(new Color(1.6, 0.7, 1.0)));
        }

        [Test()]
        public void SubtractingColors()
        {
            // Given
            Color color1 = new Color(0.9, 0.6, 0.75);
            Color color2 = new Color(0.7, 0.1, 0.25);

            // Then
            Color result = color1 - color2;

            Assert.IsTrue(result.NearlyEquals(new Color(0.2, 0.5, 0.5)));
        }

        [Test()]
        public void MultiplyingAColorByAScalar()
        {
            // Given
            Color color = new Color(0.2, 0.3, 0.4);

            // Then
            Color result = color * 2;

            Assert.IsTrue(result.NearlyEquals(new Color(0.4, 0.6, 0.8)));
        }

        [Test()]
        public void MultiplyingColors()
        {
            // Given
            Color color1 = new Color(1, 0.2, 0.4);
            Color color2 = new Color(0.9, 1, 0.1);

            // Then
            Color result = color1 * color2;

            Assert.IsTrue(result.NearlyEquals(new Color(0.9, 0.2, 0.04)));
        }

        [Test()]
        public void ConstructingThePpmHeader()
        {
            // Given
            Canvas canvas = new Canvas(5, 3);

            // When
            string ppm = canvas.ToPpm();

            // Then
            Assert.IsTrue(ppm.StartsWith("P3\n5 3\n255", System.StringComparison.InvariantCulture));   
        }

        [Test()]
        public void ConstructingThePpmPixelData()
        {
            // Given
            Canvas canvas = new Canvas(5, 3);
            Color color1 = new Color(1.5, 0 ,0);
            Color color2 = new Color(0, 0.5, 0);
            Color color3 = new Color(-0.5, 0, 1);

            // When
            canvas[0, 0] = color1;
            canvas[2, 1] = color2;
            canvas[4, 2] = color3;

            string ppm = canvas.ToPpm();

            // Then
            string[] ppmRows = ppm.Split('\n');
            
            Assert.IsTrue(ppmRows[3] == "255 0 0 0 0 0 0 0 0 0 0 0 0 0 0");
            Assert.IsTrue(ppmRows[4] == "0 0 0 0 0 0 0 128 0 0 0 0 0 0 0");
            Assert.IsTrue(ppmRows[5] == "0 0 0 0 0 0 0 0 0 0 0 0 0 0 255");
        }

        [Test()]
        public void SplittingLongLinesInPpmFiles()
        {
            // Given
            Canvas canvas = new Canvas(10, 2);

            // When
            Color color = new Color(1, 0.8, 0.6);

            for (int y = 0; y < canvas.Height; y++)
            {
                for (int x = 0; x < canvas.Width; x++)
                {
                    canvas[x, y] = color;
                }
            }

            string ppm = canvas.ToPpm();

            // Then
            string[] ppmRows = ppm.Split('\n');

            Assert.IsTrue(ppmRows[3] == "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204");
            Assert.IsTrue(ppmRows[4] == "153 255 204 153 255 204 153 255 204 153 255 204 153");
            Assert.IsTrue(ppmRows[5] == "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204");
            Assert.IsTrue(ppmRows[6] == "153 255 204 153 255 204 153 255 204 153 255 204 153");
        }
    }
}
