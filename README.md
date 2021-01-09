# The Ray Tracer Challenge

## A Test-Driven Guide to Your First 3d Renderer

---
[![Issues][issues-shield]][issues-url]
[![Stargazers][stars-shield]][stars-url]
[![Forks][forks-shield]][forks-url]
[![Contributors][contributors-shield]][contributors-url]
[![MIT License][license-shield]][license-url]
[![Code Size][code-size]][code-url]
[![Repo Size][repo-size]][repo-url]
[![Downloads][downloads]][download-url]
[![Release][release]][download-url]

I like programming since I was in 8th grade and chose computer science there. Through my uncle, who always taught me some computer science, I got further and further into the world of programming. In the 11th grade we had to write a term paper and there I chose the Ray Tracer as my topic, because I had already started this project and had a basis for my term paper.

After the paper, which is now more than 2 years ago, the project was on hold, but now I will continue and develop the Ray Tracer. In the following ReadMe file, all steps of the chapters are listed. Since I had already finished the Ray Tracer up to chapter 14, I can't show any code from before, but from chapter 14 of the book, you can see every code change.

Note that all chapters of the book (except 12 and 13) can be found as a file and rendered to vividly show how the Ray Tracer evolves. Maybe Chapter 12 and 13 will be added. 



The Book "The Ray Tracer Challenge" by Jamis Buck can be purchased on the following website [https://pragprog.com/titles/jbtracer/the-ray-tracer-challenge/].

Brace yourself for a fun challenge: build a photorealistic 3D renderer from scratch! It’s easier than you think. In just a couple of weeks, build a ray tracer that renders beautiful scenes with shadows, reflections, brilliant refraction effects, and subjects composed of various graphics primitives: spheres, cubes, cylinders, triangles, and more. With each chapter, implement another piece of the puzzle and move the renderer that much further forward. Do all of this in whichever language and environment you prefer, and do it entirely test-first, so you know it’s correct. Recharge yourself with this project’s immense potential for personal exploration, experimentation, and discovery.

## Chapter 1

In this chapter, unfortunately, nothing visible is built yet, the basics, like tuples, points and vectors are implemented.

## Chapter 2

In this chapter, the first visible result is a parabola drawn on a canvas. The single points describe the trajectory of a projectile depending on start position, velocity, gravity, wind and time. Basis are the classes implemented up to chapter 2, especially Tuple, Point, Vector and Canvas.

## Chapter 4

This development step shows a circle consisting of twelve points10. Compared to the
first development step, matrices and matrix transformations - in this case
displacement and rotation around the 𝑧-axis - are applied.

## Chapter 5

This image forms the next visible result of the development. The circle was calculated from the
intersections of rays and a sphere, which were "shot" into the world for each pixel.
shot" for each pixel. At this point, the ray tracer is not yet able to correctly include light and shading in the calculations, so the result is a simple circle.
is a simple circle

## Chapter 6

This result is a continuation of the circle from chapter 5 of the book. The virtual
world a light source was added. The illumination of the sphere consists of 3 single
components, calculated using Phong Shading:

- Ambient light reflection
  illuminates the sphere evenly from all sides. It
  ensures that the lower right side of the sphere is not completely black.
  sphere is not completely black.
- In the example, diffuse reflection ensures that the side of the
  example, the side of the sphere in the upper left corner of the
  image appears brighter because it is facing the light source.
  to the light source. The diffuse reflection depends exclusively
  on the angle between the light source and the norma-
  len vector of a surface.
- Specular reflection additionally depends on the observer's
  depends on the viewer's point of view and appears as a white spot
  as a white spot in the image.

## Chapter 7

Further expanded, a scene is now created that makes it possible to represent more than just one sphere. For this purpose matrices are needed to move, scale and rotate spheres. The walls shown are very flat, large "spheres" (simulating planes), because at this point planes are not yet implemented.

The position of the spheres in the room is difficult to estimate, because no shadows can be seen yet. Shadows are not yet visible, which will be implemented in the next development step. The right "sphere" is twice as wide as high and deep due to transformations and additionally rotated around the 𝑦-axis slightly rotated.

## Chapter 8

The next stage of development adds shadows to the previous scene. Note how much better it is now possible to estimate how the spheres are placed in space. The shadows are not only cast on the floor, but in the image you can see that the white sphere the white ball also casts a shadow on the yellow ball. The yellow ball in turn casts a part of the shadow on the wall on the right side.

## Chapter 9

In this step, layers were implemented. Planes have an infinite extension. To show this in the image, the walls were omitted. As a result, you see more shadows due to a light source further ahead, which was also present in the previous development step, but due to the wall, the shadows were correctly not rendered.

## Chapter 10

The next step deals with the implementation of different patterns. On the left you can see a checkerboard pattern, in the middle a stripe pattern with two colors and on the far right a gradient (from red to blue). The bottom has a circle pattern. All patterns that have been implemented can be applied to the floor and to the spheres, and can be moved, scaled and rotated as you like.

## Chapter 11

The next step was to add the reflection, as you can see in the middle sphere which reflects the spheres next to it and the ground also reflects, as can be seen from the reflections of the spheres on the floor. Note how realistic the ray tracing method works here:

- The reflection takes into account the perspective distortion on spheres.
- Patterns and colors are rendered correctly in the reflections.
- Even the illumination is handled correctly, e.g. you can see the specular reflection (small white "spot", very hard to see in the printout) on the
  red/blue sphere, which is also found in the reflection on the ground.

The center sphere now lets light through, allowing for a transparent look. Again, you can see the great advantage of the ray tracing method: realism. Thus, the perspective distortion/light calculation of the transparency is handled correctly here as well, which can be controlled by parameters in the code.

## Chapter 12

In this chapter cubes were implemented, which can stand in space just like all other objects.

## Chapter 13

In this chapter cylinders were implemented, which can stand in space just like all other objects.

## Chapter 14

In this chapter, groups have been implemented that allow multiple objects to be joined together and rendered or displayed as one object.

## Chapter 15

This chapter is about a basic form for ray tracing - triangles. But since it would not be so exciting to render only triangles, a parser for .obj files was implemented in this chapter. With this you can render from files with the obj file format with this ray tracer. 


[issues-url]: https://github.com/omit2c/RayTracer_Challenge/issues
[issues-shield]: https://img.shields.io/github/issues/omit2c/RayTracer_Challenge.svg?style=for-the-badge

[stars-shield]: https://img.shields.io/github/stars/omit2c/RayTracer_Challenge.svg?style=for-the-badge
[stars-url]: https://github.com/omit2c/RayTracer_Challenge/stargazers

[contributors-shield]: https://img.shields.io/github/contributors/omit2c/RayTracer_Challenge.svg?style=for-the-badge
[contributors-url]: https://github.com/omit2c/RayTracer_Challenge/graphs/contributors

[forks-shield]: https://img.shields.io/github/forks/omit2c/RayTracer_Challenge.svg?style=for-the-badge
[forks-url]: https://github.com/omit2c/RayTracer_Challenge/network/members

[license-shield]: https://img.shields.io/github/license/omit2c/RayTracer_Challenge.svg?style=for-the-badge
[license-url]: https://github.com/omit2c/RayTracer_Challenge/blob/main/LICENSE

[code-size]:https://img.shields.io/github/languages/code-size/omit2c/RayTracer_Challenge.svg?style=for-the-badge
[code-url]: https://github.com/omit2c/RayTracer_Challenge.git

[repo-size]:https://img.shields.io/github/repo-size/omit2c/RayTracer_Challenge.svg?style=for-the-badge
[repo-url]: https://github.com/omit2c/RayTracer_Challenge.git

[downloads]:https://img.shields.io/github/downloads/omit2c/RayTracer_Challenge/total.svg?style=for-the-badge
[download-url]: https://github.com/omit2c/RayTracer_Challenge.git

[release]:https://img.shields.io/github/v/release/omit2c/RayTracer_Challenge.svg?include_prereleases&style=for-the-badge
