using RayTracerLogic;

namespace RayTracerTests
{
    public class DefaultWorld
    {
        public static World NewDefaultWorld()
        {
            World defaultWorld = new World();

            defaultWorld.LightSources.Add(new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1)));

            Sphere sphereOne = new Sphere();
            sphereOne.Material.Color = new Color(0.8, 1.0, 0.6);
            sphereOne.Material.Diffuse = 0.7;
            sphereOne.Material.Specular = 0.2;

            Sphere sphereTwo = new Sphere();
            sphereTwo.Transform = sphereTwo.Transform.Scale(0.5, 0.5, 0.5);

            defaultWorld.SceneObjects.Add(sphereOne);
            defaultWorld.SceneObjects.Add(sphereTwo);

            return defaultWorld;
        }
    }
}
