using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class PatternTests
    {
        [Test()]
        public void CreatingAStripePattern()
        {
            // Given
            StripePattern pattern = new StripePattern(Color.GetWhite(), Color.GetBlack());

            // Then
            Assert.IsTrue(pattern.FirstColor.NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.SecondColor.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void AStripePatternIsConstantInY()
        {
            // Given
            StripePattern pattern = new StripePattern(Color.GetWhite(), Color.GetBlack());

            // Then
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 1, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 2, 0)).NearlyEquals(Color.GetWhite()));
        }

        [Test()]
        public void AStripePatternIsConstantInZ()
        {
            // Given
            StripePattern pattern = new StripePattern(Color.GetWhite(), Color.GetBlack());

            // Then
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 1)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 2)).NearlyEquals(Color.GetWhite()));
        }

        [Test()]
        public void AStripePatternAlternatesInX()
        {
            // Given
            StripePattern pattern = new StripePattern(Color.GetWhite(), Color.GetBlack());

            // Then
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0.9, 0, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(1, 0, 0)).NearlyEquals(Color.GetBlack()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(-0.1, 0, 0)).NearlyEquals(Color.GetBlack()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(-1, 0, 0)).NearlyEquals(Color.GetBlack()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(-1.1, 0, 0)).NearlyEquals(Color.GetWhite()));
        }

        [Test()]
        public void LightingWithAPetternApplied()
        {
            // Given
            StripePattern pattern = new StripePattern(Color.GetWhite(), Color.GetBlack());
            Material material = new Material();

            material.Pattern = pattern;
            material.Ambient = 1;
            material.Diffuse = 0;
            material.Specular = 0;

            Vector eyeVector = new Vector(0, 0, -1);
            Vector normalVector = new Vector(0, 0, -1);
            PointLight light = new PointLight(new Point(0, 0, -10), Color.GetWhite());

            // When
            Color colorOne = material.GetLighting(new Sphere(), light, new Point(0.9, 0, 0), eyeVector, normalVector, 0);
            Color colorTwo = material.GetLighting(new Sphere(), light, new Point(1.1, 0, 0), eyeVector, normalVector, 0);

            // Then
            Assert.IsTrue(colorOne.NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(colorTwo.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void StripesWithAnObjectTransformation()
        {
            // Given
            Shape sphere = new Sphere();
            sphere.Transform = sphere.Transform.Scale(2, 2, 2);

            StripePattern pattern = new StripePattern(Color.GetWhite(), Color.GetBlack());

            // When
            Color color = pattern.GetPatternAtShape(sphere, new Point(1.5, 0, 0));

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetWhite()));
        }

        [Test()]
        public void StripesWithAPatternTransformation()
        {
            // Given
            Shape sphere = new Sphere();

            StripePattern pattern = new StripePattern(Color.GetWhite(), Color.GetBlack());
            pattern.Transform = pattern.Transform.Scale(2, 2, 2);

            // When
            Color color = pattern.GetPatternAtShape(sphere, new Point(1.5, 0, 0));

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetWhite()));
        }

        [Test()]
        public void StripesWithBothAnObjectAndAPatternTransformation()
        {
            // Given
            Shape sphere = new Sphere();
            sphere.Transform = sphere.Transform.Scale(2, 2, 2);

            StripePattern pattern = new StripePattern(Color.GetWhite(), Color.GetBlack());
            pattern.Transform = pattern.Transform.Translate(0.5, 0, 0);

            // When
            Color color = pattern.GetPatternAtShape(sphere, new Point(2.5, 0, 0));

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetWhite()));
        }

        [Test()]
        public void TheDefaultPatternTransformation()
        {
            // Given
            Pattern pattern = new TestPattern();

            // Then
            Assert.IsTrue(pattern.Transform.NearlyEquals(Matrix.NewIdentityMatrix(4)));
        }

        [Test()]
        public void AssigningATransformation()
        {
            // Given
            Pattern pattern = new TestPattern();

            // When
            pattern.Transform = pattern.Transform.Translate(1, 2, 3);

            // Then
            Assert.IsTrue(pattern.Transform.NearlyEquals(Matrix.NewTranslationMatrix(1, 2, 3)));
        }

        [Test()]
        public void APatternWithAnObjectTransformation()
        {
            // Given
            Shape shape = new Sphere();
            shape.Transform = shape.Transform.Scale(2, 2, 2);

            Pattern pattern = new TestPattern();

            // WHen
            Color color = pattern.GetPatternAtShape(shape, new Point(2, 3, 4));

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(1, 1.5, 2)));
        }

        [Test()]
        public void APatternWithAPatternTransformation()
        {
            // Given
            Shape shape = new Sphere();

            Pattern pattern = new TestPattern();
            pattern.Transform = pattern.Transform.Scale(2, 2, 2);

            // When
            Color color = pattern.GetPatternAtShape(shape, new Point(2, 3, 4));

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(1, 1.5, 2)));
        }

        [Test()]
        public void AAptternWithBothAnObjectAnAPatternTransformation()
        {
            // Given
            Shape shape = new Sphere();
            shape.Transform = shape.Transform.Scale(2, 2, 2);

            Pattern pattern = new TestPattern();
            pattern.Transform = pattern.Transform.Translate(0.5, 1, 1.5);

            // When
            Color color = pattern.GetPatternAtShape(shape, new Point(2.5, 3, 3.5));

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.75, 0.5, 0.25)));
        }

        [Test()]
        public void AGradientLinearlyInterpolatesBetweenColors()
        {
            // Given
            Pattern pattern = new GradientPattern(Color.GetWhite(), Color.GetBlack());

            // Then
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0.25, 0, 0)).NearlyEquals(new Color(0.75, 0.75, 0.75)));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0.5, 0, 0)).NearlyEquals(new Color(0.5, 0.5, 0.5)));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0.75, 0, 0)).NearlyEquals(new Color(0.25, 0.25, 0.25)));
        }

        [Test()]
        public void ARingShouldExtendInBothXAndZ()
        {
            // Given
            Pattern pattern = new RingPattern(Color.GetWhite(), Color.GetBlack());

            // Then
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(1, 0, 0)).NearlyEquals(Color.GetBlack()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 1)).NearlyEquals(Color.GetBlack()));
            // 0.708 = just slightly more than Math.Sqrt(2) / 2
            Assert.IsTrue(pattern.GetPatternAt(new Point(0.708, 0, 0.708)).NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void CheckersShouldRepeatInX()
        {
            // Given
            Pattern pattern = new CheckersPattern(Color.GetWhite(), Color.GetBlack());

            // Then
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0.99, 0, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(1.01, 0, 0)).NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void CheckersShouldRepeatInY()
        {
            // Given
            Pattern pattern = new CheckersPattern(Color.GetWhite(), Color.GetBlack());

            // Then
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0.99, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 1.01, 0)).NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void CheckersShouldRepeatInZ()
        {
            // Given
            Pattern pattern = new CheckersPattern(Color.GetWhite(), Color.GetBlack());

            // Then
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 0)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 0.99)).NearlyEquals(Color.GetWhite()));
            Assert.IsTrue(pattern.GetPatternAt(new Point(0, 0, 1.01)).NearlyEquals(Color.GetBlack()));
        }
    }
}
