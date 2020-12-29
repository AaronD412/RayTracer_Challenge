namespace RayTracerLogic
{
    public class Material
    {
        #region Private Members

        private Color color = Color.GetWhite();
        private double ambient = 0.1;
        private double diffuse = 0.9;
        private double specular = 0.9;
        private double shininess = 200;
        private double reflective = 0.0;
        private double transparency = 0.0;
        private double refractiveIndex = 1.0;
        private Pattern pattern = null;

        #endregion

        #region Public Methods

        public Color GetLighting(Shape shape, ILightSource light, Point position, Vector eyeVector, Vector normalVector, double lightIntensity)
        {
            Color color;

            if (pattern != null)
            {
                color = pattern.GetPatternAtShape(shape, position);
            }
            else
            {
                color = this.color;
            }

            Color effectiveColor = color * light.Intensity;
            Color ambient = effectiveColor * Ambient;
            Color sum = Color.GetBlack();

            for (int u = 0; u < light.USteps; u++)
            {
                for (int v = 0; v < light.VSteps; v++)
                {
                    Vector lightVector = (light.GetPointOnLight(u, v) - position).Normalize();
                    double lightDotNormal = lightVector.Dot(normalVector);

                    if (lightDotNormal >= 0)
                    {
                        Color diffuse = effectiveColor * Diffuse * lightDotNormal;

                        Color specular = Color.GetBlack();
                        Vector reflectVector = (-lightVector).GetReflect(normalVector);
                        double reflectDotEye = reflectVector.Dot(eyeVector);

                        if (reflectDotEye > 0)
                        {
                            double factor = System.Math.Pow(reflectDotEye, Shininess);
                            specular = light.Intensity * Specular * factor;
                        }

                        sum += diffuse + specular;
                    }
                }
            }

            return ambient + (sum  * lightIntensity);
        }

        public bool NearlyEquals(Material material)
        {
            return color.NearlyEquals(material.Color) &&
                ambient.NearlyEquals(material.Ambient) &&
                diffuse.NearlyEquals(material.Diffuse) &&
                specular.NearlyEquals(material.Specular) &&
                shininess.NearlyEquals(material.Shininess);
        }

        #endregion

        #region Public Properties

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public double Ambient
        {
            get
            {
                return ambient;
            }
            set
            {
                ambient = value;
            }
        }

        public double Diffuse
        {
            get
            {
                return diffuse;
            }
            set
            {
                diffuse = value;
            }
        }

        public double Specular
        {
            get
            {
                return specular;
            }
            set
            {
                specular = value;
            }
        }

        public double Shininess
        {
            get
            {
                return shininess;
            }
            set
            {
                shininess = value;
            }
        }

        public double Reflective
        {
            get
            {
                return reflective;
            }
            set
            {
                reflective = value;
            }
        }

        public double Transparency
        {
            get
            {
                return transparency;
            }
            set
            {
                transparency = value;
            }
        }

        public double RefractiveIndex
        {
            get
            {
                return refractiveIndex;
            }
            set
            {
                refractiveIndex = value;
            }
        }

        public Pattern Pattern
        {
            get
            {
                return pattern;
            }
            set
            {
                pattern = value;
            }
        }

        #endregion
    }
}
