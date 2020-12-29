using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace RayTracerLogic
{
    public class Parser
    {
        #region Private Members

        private int ignoredLinesCount = 0;
        private Points vertices = new Points();
        private Group defaultGroup = new Group();
        private Dictionary<string, Group> groups = new Dictionary<string, Group>();
        private Vectors normalVectors = new Vectors();

        private double lowestX = double.PositiveInfinity;
        private double lowestY = double.PositiveInfinity;
        private double lowestZ = double.PositiveInfinity;

        private double highestX = double.NegativeInfinity;
        private double highestY = double.NegativeInfinity;
        private double highestZ = double.NegativeInfinity;

        private Regex regexVertices = new Regex(
            "^v\\s+(?<x>-?\\d+(\\.\\d+)?)\\s+(?<y>-?\\d+(\\.\\d+)?)\\s+(?<z>-?\\d+(\\.\\d+)?)\\r?$",
            RegexOptions.Compiled
        );

        private Regex regexFaces = new Regex(
            "^f(?:\\s+(\\d+(?:/\\d*/\\d+)?)){3,}\\r?$",
            RegexOptions.Compiled
        );

        private Regex regexFace = new Regex(
            "^(?<vertexIndex>\\d+)(?:/\\d*/(?<normalVectorIndex>\\d+))?\\r?$",
            RegexOptions.Compiled
        );

        private Regex regexGroups = new Regex(
            "^g\\s+(?<groupName>.*?)\\r?$",
            RegexOptions.Compiled
        );

        private Regex regexNormals = new Regex(
            "^vn\\s+(?<x>-?\\d+(\\.\\d+)?)\\s+(?<y>-?\\d+(\\.\\d+)?)\\s+(?<z>-?\\d+(\\.\\d+)?)\\r?$",
            RegexOptions.Compiled
        );

        #endregion

        #region Public Constructors

        public Parser(StreamReader objStreamReader) : this(objStreamReader.ReadToEnd())
        {
            // Do nothing
        }

        public Parser(string value)
        {
            defaultGroup.UseParentMaterial = true;

            string currentGroupName = null;

            foreach (string line in value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                Match matchVertices = regexVertices.Match(line);

                if (matchVertices.Success)
                {
                    vertices.Add(
                        new Point(
                            double.Parse(matchVertices.Groups["x"].Value, CultureInfo.InvariantCulture),
                            double.Parse(matchVertices.Groups["y"].Value, CultureInfo.InvariantCulture),
                            double.Parse(matchVertices.Groups["z"].Value, CultureInfo.InvariantCulture)
                        )
                    );

                    continue;
                }

                Match matchFaces = regexFaces.Match(line);

                if (matchFaces.Success)
                {
                    Points points = new Points();
                    Vectors normalVectors = new Vectors();

                    foreach (Capture capture in matchFaces.Groups[1].Captures)
                    {
                        Match matchFace = regexFace.Match(capture.Value);

                        points.Add(Vertices[int.Parse(matchFace.Groups["vertexIndex"].Value) - 1]);

                        if (matchFace.Groups["normalVectorIndex"].Success)
                        {
                            normalVectors.Add(NormalVectors[int.Parse(matchFace.Groups["normalVectorIndex"].Value) - 1]);
                        }
                    }

                    if (points.Count == normalVectors.Count || normalVectors.Count == 0)
                    {
                        if (currentGroupName == null)
                        {
                            defaultGroup.AddRange(FanTriangulation(points, normalVectors));
                        }
                        else
                        {
                            if (!groups.ContainsKey(currentGroupName))
                            {
                                Group group = new Group();
                                group.UseParentMaterial = true;
                                groups.Add(currentGroupName, group);
                            }

                            groups[currentGroupName].AddRange(FanTriangulation(points, normalVectors));
                        }

                        continue;
                    }
                }

                Match matchGroups = regexGroups.Match(line);

                if (matchGroups.Success)
                {
                    currentGroupName = matchGroups.Groups["groupName"].Value;

                    continue;
                }

                Match matchNormals = regexNormals.Match(line);

                if (matchNormals.Success)
                {
                    normalVectors.Add(
                        new Vector(
                            double.Parse(matchNormals.Groups["x"].Value, CultureInfo.InvariantCulture),
                            double.Parse(matchNormals.Groups["y"].Value, CultureInfo.InvariantCulture),
                            double.Parse(matchNormals.Groups["z"].Value, CultureInfo.InvariantCulture)
                        )
                    );

                    continue;
                }

                ignoredLinesCount++;
            }
        }

        #endregion

        #region Public Methods

        public Matrix GetUniformedSizedAndCenteredModelTransformationMatrix()
        {
            // Center the model
            double translateX = (highestX - lowestX) / 2 - highestX;
            double translateY = (highestY - lowestY) / 2 - highestY;
            double translateZ = (highestZ - lowestZ) / 2 - highestZ;

            Matrix translationMatrix = Matrix.NewTranslationMatrix(translateX, translateY, translateZ);

            double scalingFactor = GetScalingFactor();

            Matrix scalingMatrix = Matrix.NewScalingMatrix(scalingFactor, scalingFactor, scalingFactor);

            return translationMatrix * scalingMatrix;
        }

        #endregion

        #region Private Methods

        private Shapes FanTriangulation(Points vertices, Vectors normalVectors)
        {
            Shapes triangles = new Shapes();

            AdjustLowestAndHighestXYZ(vertices[0]);

            for (int index = 1; index < vertices.Count - 1; index++)
            {
                Triangle triangle;

                if (normalVectors.Count == 0)
                {
                    triangle = new Triangle(vertices[0], vertices[index], vertices[index + 1]);
                }
                else
                {
                    triangle = new SmoothTriangle(
                        vertices[0],
                        vertices[index],
                        vertices[index + 1],
                        normalVectors[0],
                        normalVectors[index],
                        normalVectors[index + 1]
                    );
                }

                triangle.UseParentMaterial = true;
                triangles.Add(triangle);

                AdjustLowestAndHighestXYZ(vertices[index]);
            }

            AdjustLowestAndHighestXYZ(vertices[vertices.Count - 1]);

            return triangles;
        }

        private double GetScalingFactor()
        {
            // Determine the scaling factor (2 is used, because the model should be 2 units
            // high, wide, or deep after scaling)
            double scalingFactorX = 2 / (highestX - lowestX);
            double scalingFactorY = 2 / (highestY - lowestY);
            double scalingFactorZ = 2 / (highestZ - lowestZ);

            double scalingFactor = Math.Min(scalingFactorX, Math.Min(scalingFactorY, scalingFactorZ));

            return scalingFactor;
        }

        private void AdjustLowestAndHighestXYZ(Point vertex)
        {
            lowestX = Math.Min(lowestX, vertex.X);
            lowestY = Math.Min(lowestY, vertex.Y);
            lowestZ = Math.Min(lowestZ, vertex.Z);

            highestX = Math.Max(highestX, vertex.X);
            highestY = Math.Max(highestY, vertex.Y);
            highestZ = Math.Max(highestZ, vertex.Z);
        }

        #endregion

        #region Public Properties

        public int IgnoredLinesCount
        {
            get
            {
                return ignoredLinesCount;
            }
        }

        public Points Vertices
        {
            get
            {
                return vertices;
            }
        }

        public Group DefaultGroup
        {
            get
            {
                return defaultGroup;
            }
        }

        public Dictionary<string, Group> Groups
        {
            get
            {
                return groups;
            }
        }

        public Group Group
        {
            get
            {
                Group group = new Group();

                group.AddChild(defaultGroup);
                group.AddRange(groups.Values);

                return group;
            }
        }

        public Vectors NormalVectors
        {
            get
            {
                return normalVectors;
            }
        }

        public double LowestX
        {
            get
            {
                return lowestX;
            }
        }

        public double LowestY
        {
            get
            {
                return lowestY;
            }
        }

        public double LowestZ
        {
            get
            {
                return lowestZ;
            }
        }

        public double HighestX
        {
            get
            {
                return highestX;
            }
        }

        public double HighestY
        {
            get
            {
                return highestY;
            }
        }

        public double HighestZ
        {
            get
            {
                return highestZ;
            }
        }

        public double LowestUniformedSizedAndCenteredX
        {
            get
            {
                double translate = (highestX - lowestX) / 2 - highestX;
                return (lowestX + translate) * GetScalingFactor();
            }
        }

        public double LowestUniformedSizedAndCenteredY
        {
            get
            {
                double translate = (highestY - lowestY) / 2 - highestY;
                return (lowestY + translate) * GetScalingFactor();
            }
        }

        public double LowestUniformedSizedAndCenteredZ
        {
            get
            {
                double translate = (highestZ - lowestZ) / 2 - highestZ;
                return (lowestZ + translate) * GetScalingFactor();
            }
        }

        public double HighestUniformedSizedAndCenteredX
        {
            get
            {
                double translate = (highestX - lowestX) / 2 - highestX;
                return (highestX + translate) * GetScalingFactor();
            }
        }

        public double HighestUniformedSizedAndCenteredY
        {
            get
            {
                double translate = (highestY - lowestY) / 2 - highestY;
                return (highestY + translate) * GetScalingFactor();
            }
        }

        public double HighestUniformedSizedAndCenteredZ
        {
            get
            {
                double translate = (highestZ - lowestZ) / 2 - highestZ;
                return (highestZ + translate) * GetScalingFactor();
            }
        }

        #endregion
    }
}
