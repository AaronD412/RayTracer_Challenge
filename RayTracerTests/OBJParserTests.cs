using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class OBJParserTest
    {
        [Test()]
        public void IgnoringUnrecognizedLines()
        {
            // Given
            string gibberish = @"There was a young lady named Bright
                                who traveled much faster than light.
                                She set out one day
                                in a relative way,
                                and came back the previous night.";

            // When
            Parser parser = new Parser(gibberish);

            // Then
            Assert.AreEqual(5, parser.IgnoredLinesCount);
        }

        [Test()]
        public void VertexRecords()
        {
            // Given
            string value = @"v -1 1 0
v -1.0000 0.50000 0.0000
v 1 0 0
v 1 1 0";

            // When
            Parser parser = new Parser(value);

            // Then
            Assert.IsTrue(parser.Vertices[0].NearlyEquals(new Point(-1, 1, 0)));
            Assert.IsTrue(parser.Vertices[1].NearlyEquals(new Point(-1, 0.5, 0)));
            Assert.IsTrue(parser.Vertices[2].NearlyEquals(new Point(1, 0, 0)));
            Assert.IsTrue(parser.Vertices[3].NearlyEquals(new Point(1, 1, 0)));
        }

        [Test()]
        public void ParsingTriangleFaces()
        {
            // Given
            string value = @"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0

f 1 2 3
f 1 3 4";

            // When
            Parser parser = new Parser(value);
            Group group = parser.DefaultGroup;
            Triangle triangle1 = (Triangle)group[0];
            Triangle triangle2 = (Triangle)group[1];

            // Then
            Assert.IsTrue(triangle1.Point1.NearlyEquals(parser.Vertices[0]));
            Assert.IsTrue(triangle1.Point2.NearlyEquals(parser.Vertices[1]));
            Assert.IsTrue(triangle1.Point3.NearlyEquals(parser.Vertices[2]));

            Assert.IsTrue(triangle2.Point1.NearlyEquals(parser.Vertices[0]));
            Assert.IsTrue(triangle2.Point2.NearlyEquals(parser.Vertices[2]));
            Assert.IsTrue(triangle2.Point3.NearlyEquals(parser.Vertices[3]));
        }

        [Test()]
        public void TriangulatingPolygons()
        {
            // Given
            string value = @"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0
v 0 2 0
f 1 2 3 4 5";

            // When
            Parser parser = new Parser(value);
            Group group = parser.DefaultGroup;
            Triangle triangle1 = (Triangle)group[0];
            Triangle triangle2 = (Triangle)group[1];
            Triangle triangle3 = (Triangle)group[2];

            // Then
            Assert.IsTrue(triangle1.Point1.NearlyEquals(parser.Vertices[0]));
            Assert.IsTrue(triangle1.Point2.NearlyEquals(parser.Vertices[1]));
            Assert.IsTrue(triangle1.Point3.NearlyEquals(parser.Vertices[2]));

            Assert.IsTrue(triangle2.Point1.NearlyEquals(parser.Vertices[0]));
            Assert.IsTrue(triangle2.Point2.NearlyEquals(parser.Vertices[2]));
            Assert.IsTrue(triangle2.Point3.NearlyEquals(parser.Vertices[3]));

            Assert.IsTrue(triangle3.Point1.NearlyEquals(parser.Vertices[0]));
            Assert.IsTrue(triangle3.Point2.NearlyEquals(parser.Vertices[3]));
            Assert.IsTrue(triangle3.Point3.NearlyEquals(parser.Vertices[4]));
        }

        [Test()]
        public void TrianglesInGroups()
        {
            // Given
            string value = @"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0

g FirstGroup
f 1 2 3
g SecondGroup
f 1 3 4";
            // When
            Parser parser = new Parser(value);
            Group group1 = parser.Groups["FirstGroup"];
            Group group2 = parser.Groups["SecondGroup"];
            Triangle triangle1 = (Triangle)group1[0];
            Triangle triangle2 = (Triangle)group2[0];

            // Then
            Assert.IsTrue(triangle1.Point1.NearlyEquals(parser.Vertices[0]));
            Assert.IsTrue(triangle1.Point2.NearlyEquals(parser.Vertices[1]));
            Assert.IsTrue(triangle1.Point3.NearlyEquals(parser.Vertices[2]));

            Assert.IsTrue(triangle2.Point1.NearlyEquals(parser.Vertices[0]));
            Assert.IsTrue(triangle2.Point2.NearlyEquals(parser.Vertices[2]));
            Assert.IsTrue(triangle2.Point3.NearlyEquals(parser.Vertices[3]));
        }

        [Test()]
        public void ConvertingAnObjFileToAGroup()
        {
            // Given
            string value = @"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0

g FirstGroup
f 1 2 3
g SecondGroup
f 1 3 4";

            Parser parser = new Parser(value);

            // When
            Group group = parser.Group;

            // Then
            Assert.IsTrue(group.Contains(parser.Groups["FirstGroup"]));
            Assert.IsTrue(group.Contains(parser.Groups["SecondGroup"]));
        }

        [Test()]
        public void VertexNormalRecords()
        {
            // Given
            string value = @"vn 0 0 1
vn 0.707 0 -0.707
vn 1 2 3";

            // When
            Parser parser = new Parser(value);

            // Then
            Assert.IsTrue(parser.NormalVectors[0].NearlyEquals(new Vector(0, 0, 1)));
            Assert.IsTrue(parser.NormalVectors[1].NearlyEquals(new Vector(0.707, 0, -0.707)));
            Assert.IsTrue(parser.NormalVectors[2].NearlyEquals(new Vector(1, 2, 3)));
        }

        [Test()]
        public void FacesWithNormals()
        {
            // Given
            string value = @"v 0 1 0
v -1 0 0
v 1 0 0

vn -1 0 0
vn 1 0 0
vn 0 1 0

f 1//3 2//1 3//2
f 1/0/3 2/102/1 3/14/2";

            // When
            Parser parser = new Parser(value);
            Group group = parser.DefaultGroup;
            SmoothTriangle triangle1 = (SmoothTriangle)group[0];
            SmoothTriangle triangle2 = (SmoothTriangle)group[1];

            // Then
            Assert.AreSame(parser.Vertices[0], triangle1.Point1);
            Assert.AreSame(parser.Vertices[1], triangle1.Point2);
            Assert.AreSame(parser.Vertices[2], triangle1.Point3);

            Assert.AreSame(parser.NormalVectors[2], triangle1.NormalVector1);
            Assert.AreSame(parser.NormalVectors[0], triangle1.NormalVector2);
            Assert.AreSame(parser.NormalVectors[1], triangle1.NormalVector3);

            Assert.IsTrue(triangle1.NearlyEquals(triangle2));
        }
    }
}