namespace RayTracerLogic
{
    public class PointLight : ILightSource
    {
        #region Private Members

        private readonly Point position;
        private readonly Color intensity;

        #endregion

        #region Public Constructors

        public PointLight(Point position, Color intensity)
        {
            this.position = position;
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

            PointLight pointLight = lightSource as PointLight;

            if (pointLight == null)
            {
                return false;
            }

            return Position.NearlyEquals(pointLight.Position) &&
                Intensity.NearlyEquals(pointLight.Intensity);
        }

        public double GetIntensityAt(Point point, World world)
        {
            double intensity = 0;

            foreach (ILightSource lightSource in world.LightSources)
            {
                if (!world.IsShadowed(lightSource.Position, point))
                {
                    intensity += 1;
                }
            }

            if (world.LightSources.Count == 0)
            {
                return 0;
            }

            return intensity / world.LightSources.Count;
        }

        public Point GetPointOnLight(int u, int v)
        {
            return position;
        }

        #endregion

        #region Public Properties

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

        public int USteps
        {
            get
            {
                return 1;
            }
        }

        public int VSteps
        {
            get
            {
                return 1;
            }
        }

        public int Samples
        {
            get
            {
                return 1;
            }
        }

        #endregion
    }
}
