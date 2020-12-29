namespace RayTracerLogic
{
    public class AreaLight : ILightSource
    {
        #region Private Members

        private readonly object syncLock = new object();
        private readonly Point corner;
        private readonly Vector uVector;
        private readonly Vector vVector;
        private readonly int uSteps;
        private readonly int vSteps;
        private readonly Point position;
        private readonly Color intensity;
        private Sequence jitterBy = new Sequence(0.5);

        #endregion

        #region Public Constructors

        public AreaLight(Point corner, Vector vector1, int uSteps, Vector vector2, int vSteps, Color intensity)
        {
            this.corner = corner;

            uVector = vector1 / uSteps;
            vVector = vector2 / vSteps;

            this.uSteps = uSteps;
            this.vSteps = vSteps;

            position = corner + vector1 / 2 + vector2 / 2;

            this.intensity = intensity;
        }

        #endregion

        #region Public Methods

        public bool NearlyEquals(ILightSource lightSource)
        {
            if (lightSource == null)
            {
                return false;
            }

            AreaLight areaLight = lightSource as AreaLight;

            if (areaLight == null)
            {
                return false;
            }

            return Position.NearlyEquals(areaLight.Position) &&
                Corner.NearlyEquals(areaLight.Corner) &&
                UVector.NearlyEquals(areaLight.UVector) &&
                VVector.NearlyEquals(areaLight.VVector) &&
                USteps == areaLight.USteps &&
                VSteps == areaLight.VSteps &&
                Samples == areaLight.Samples;
        }

        public double GetIntensityAt(Point point, World world)
        {
            double total = 0;

            for (int v = 0; v < vSteps; v++)
            {
                for (int u = 0; u < uSteps; u++)
                {
                    Point lightPosition = GetPointOnLight(u, v);

                    if (!world.IsShadowed(lightPosition, point))
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "point.x = " + point.X + "; " +
                            "point.y = " + point.Y + "; " +
                            "point.z = " + point.Z + "; " +
                            "corner.x = " + corner.X + "; " +
                            "corner.y = " + corner.Y + "; " +
                            "corner.z = " + corner.Z + "; " +
                            "u = " + u + "; " +
                            "v = " + v + "; " +
                            "light_position.x = " + lightPosition.X + "; " +
                            "light_position.y = " + lightPosition.Y + "; " +
                            "light_position.z = " + lightPosition.Z + "; " +
                            "is_shadowed = false");

                        total += 1.0;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "point.x = " + point.X + "; " +
                            "point.y = " + point.Y + "; " +
                            "point.z = " + point.Z + "; " +
                            "corner.x = " + corner.X + "; " +
                            "corner.y = " + corner.Y + "; " +
                            "corner.z = " + corner.Z + "; " +
                            "u = " + u + "; " +
                            "v = " + v + "; " +
                            "light_position.x = " + lightPosition.X + "; " +
                            "light_position.y = " + lightPosition.Y + "; " +
                            "light_position.z = " + lightPosition.Z + "; " +
                            "is_shadowed = true");
                    }
                }
            }

            return total / Samples;
        }

        public Point GetPointOnLight(int u, int v)
        {
            lock (syncLock)
            {
                double uJitter = jitterBy.Next;
                double vJitter = jitterBy.Next;

                System.Diagnostics.Debug.Write("u_jitter = " + uJitter + "; v_jitter = " + vJitter + "; ");

                return Corner + UVector * (u + uJitter) + VVector * (v + vJitter);
            }
        }

        #endregion

        #region Public Properties

        public Point Corner
        {
            get
            {
                return corner;
            }
        }

        public Vector UVector
        {
            get
            {
                return uVector;
            }
        }

        public Vector VVector
        {
            get
            {
                return vVector;
            }
        }

        public int USteps
        {
            get
            {
                return uSteps;
            }
        }

        public int VSteps
        {
            get
            {
                return vSteps;
            }
        }

        public int Samples
        {
            get
            {
                return uSteps * vSteps;
            }
        }

        public Point Position
        {
            get
            {
                return position;
            }
        }

        public Color Intensity
        {
            get
            {
                return intensity;
            }
        }

        public Sequence JitterBy
        {
            get
            {
                return jitterBy;
            }
            set
            {
                jitterBy = value;
            }
        }

        #endregion
    }
}
