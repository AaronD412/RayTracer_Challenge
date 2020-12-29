namespace RayTracerLogic
{
    public interface ILightSource
    {
        #region Methods

        bool NearlyEquals(ILightSource lightSource);
        double GetIntensityAt(Point point, World world);
        Point GetPointOnLight(int u, int v);

        #endregion

        #region Properties

        Point Position { get; }
        Color Intensity { get; }
        int USteps { get; }
        int VSteps { get; }
        int Samples { get; }

        #endregion
    }
}
