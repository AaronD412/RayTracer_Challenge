using RayTracerLogic;

namespace RayTracerTests
{
    public class DefaultWorld
    {
        public static World NewDefaultWorld()
        {
            World defaultWorld = new World();

            PointLight pointLight = new PointLight(
                new Point(-10, 10, -10),
                Color.GetWhite());

            defaultWorld.LightSources.Add(pointLight);

            Sphere sphere1 = new Sphere();

            sphere1.Material.Color = new Color(0.8, 1.0, 0.6);
            sphere1.Material.Diffuse = 0.7;
            sphere1.Material.Specular = 0.2;

            defaultWorld.Shapes.Add(sphere1);

            Sphere sphere2 = new Sphere();

            sphere2.Transform = Matrix.NewScalingMatrix(0.5, 0.5, 0.5);

            defaultWorld.Shapes.Add(sphere2);

            return defaultWorld;
        }
    }
}
