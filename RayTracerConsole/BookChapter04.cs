using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Creates the result of the fourth chapter.
    /// </summary>
    public class BookChapter04
    {
        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("'Putting it together' example (book chapter 4)");

            Canvas canvas = new Canvas(500, 500);
            Point middle = new Point(0, 0, 0);
            Matrix translation = Matrix.NewTranslationMatrix(0, 200, 0);
            Color color = new Color(1, 1, 1);

            for (int count = 0; count < 12; count++)
            {
                Matrix rotation = Matrix.NewRotationZMatrix(-System.Math.PI / 6 * count);

                Point point = rotation * translation * middle;

                canvas[(int)point.X + canvas.Width / 2, (int)point.Y + canvas.Height / 2] = color;
            }

            canvas.ToPpm("book-chapter04-matrix-transformation.ppm");
            System.Console.WriteLine("    book-chapter04-matrix-transformation.ppm successfully written.");
        }
    }
}
