using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents the Material.
    /// </summary>
    public class Material
    {
        #region Public Members

        /// <summary>
        /// The color.
        /// </summary>
        private Color color = new Color(1, 1, 1);

        /// <summary>
        /// The ambient.
        /// </summary>
        private double ambient = 0.1;

        /// <summary>
        /// The diffuse.
        /// </summary>
        private double diffuse = 0.9;

        /// <summary>
        /// The specular.
        /// </summary>
        private double specular = 0.9;

        /// <summary>
        /// The shininess.
        /// </summary>
        private double shininess = 200;

        /// <summary>
        /// The pattern.
        /// </summary>
        private Pattern pattern = null;

        /// <summary>
        /// The reflective.
        /// </summary>
        private double reflective = 0.0;

        /// <summary>
        /// The transparency.
        /// </summary>
        private double transparency = 0.0;

        /// <summary>
        /// The refractive index.
        /// </summary>
        private double refractiveIndex = 1.0;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the lighting at one point in the scene.
        /// </summary>
        /// <returns>The lighting.</returns>
        /// <param name="sceneObject">Scene object.</param>
        /// <param name="light">Light.</param>
        /// <param name="point">Point.</param>
        /// <param name="eyeVector">Eye vector.</param>
        /// <param name="normalVector">Normal vector.</param>
        /// <param name="inShadow">If set to <c>true</c> in shadow.</param>
        public Color GetLighting(SceneObject sceneObject, PointLight light, Point point, Vector eyeVector, Vector normalVector, bool inShadow)
        {
            // Find the direction to the light source
            Vector lightVector = (light.Position - point).Normalize();
            Color color;

            if (pattern != null)
            {
                color = pattern.GetPatternAtObject(sceneObject, point);
            }
            else
            {
                color = this.color;
            }

            // Combine the surface color with the light's color/intensity
            Color effectiveColor = color * light.Intensity;

            // Compute the ambient contribution
            Color ambientColor = effectiveColor * this.ambient;
            Color diffuseColor;
            Color specularColor;

            // lightDotNormal represents the cosine of the angle between the
            // light vector and the normal vector. A negative number means the
            // light is on the other side of the surface
            double lightDotNormal = lightVector.Dot(normalVector);

            if (lightDotNormal < 0 || inShadow)
            {
                diffuseColor = Color.GetBlack();
                specularColor = Color.GetBlack();
            }
            else
            {
                // Compute the diffuse contribution
                diffuseColor = effectiveColor * this.diffuse * lightDotNormal;

                // reflectDotEye represents the cosine of the angle between the
                // reflection vector and the eye vector. A negative number means the
                // light reflects away from the eye.
                Vector reflectVector = -lightVector.GetReflect(normalVector);
                double reflectDotEye = reflectVector.Dot(eyeVector);

                if (reflectDotEye <= 0)
                {
                    specularColor = Color.GetBlack();
                }
                else
                {
                    // Compute the specular contribution
                    double factor = Math.Pow(reflectDotEye, this.shininess);
                    specularColor = light.Intensity * this.specular * factor;
                }
            }

            // Add the three contributions together to get the final shading
            return ambientColor + diffuseColor + specularColor;
        }

        /// <summary>
        /// Proofs if the values are equal.
        /// </summary>
        /// <returns><c>true</c>, if equals was nearlyed, <c>false</c> otherwise.</returns>
        /// <param name="material">Material.</param>
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

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
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

        /// <summary>
        /// Gets or sets the ambient.
        /// </summary>
        /// <value>The ambient.</value>
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

        /// <summary>
        /// Gets or sets the diffuse.
        /// </summary>
        /// <value>The diffuse.</value>
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

        /// <summary>
        /// Gets or sets the specular.
        /// </summary>
        /// <value>The specular.</value>
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

        /// <summary>
        /// Gets or sets the shininess.
        /// </summary>
        /// <value>The shininess.</value>
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

        /// <summary>
        /// Gets or sets the pattern.
        /// </summary>
        /// <value>The pattern.</value>
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

        /// <summary>
        /// Gets or sets the reflective.
        /// </summary>
        /// <value>The reflective.</value>
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

        /// <summary>
        /// Gets or sets the transparency.
        /// </summary>
        /// <value>The transparency.</value>
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

        /// <summary>
        /// Gets or sets the index of the refractive.
        /// </summary>
        /// <value>The index of the refractive.</value>
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

        #endregion
    }
}
