using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FeF_TD
{
    class Intersections
    {
        static Color[] mobTextureData;
        static Color[] missileTextureData;

        public static bool intersectPixel(Texture2D missileTexture, Texture2D mobTexture, Rectangle rectangleA, Rectangle rectangleB)
        {
            try
            {
                int top = Math.Max(rectangleA.Top, rectangleB.Top);
                int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
                int left = Math.Max(rectangleA.Left, rectangleB.Left);
                int right = Math.Min(rectangleA.Right, rectangleB.Right);

                // Extract collision data
                mobTextureData = new Color[mobTexture.Width * mobTexture.Height];
                mobTexture.GetData(mobTextureData);
                missileTextureData = new Color[missileTexture.Width * missileTexture.Height];
                missileTexture.GetData(missileTextureData);

                for (int y = top; y < bottom; y++)
                {
                    for (int x = left; x < right; x++)
                    {
                        // Get the color of both pixels at this point
                        Color colorA = missileTextureData[(x - rectangleA.Left) +
                                             (y - rectangleA.Top) * rectangleA.Width];
                        Color colorB = mobTextureData[(x - rectangleB.Left) +
                                             (y - rectangleB.Top) * rectangleB.Width];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }
                }

                // No intersection found
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool intersectPixelMouse(Vector2 mousePosition, Mob mob)
        {
            try
            {
                Rectangle rectangle = new Rectangle((int)mob.Position.X, (int)mob.Position.Y, mob.Sprite.Width, mob.Sprite.Height);

                mobTextureData = new Color[mob.Sprite.Width * mob.Sprite.Height];
                mob.Sprite.GetData(mobTextureData);


                Color color = mobTextureData[((int)mousePosition.X - rectangle.Left) + ((int)mousePosition.Y - rectangle.Top) * rectangle.Width];

                if (color.A != 0)
                {
                    // then an intersection has been found
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
