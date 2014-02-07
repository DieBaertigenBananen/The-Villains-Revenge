using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheVillainsRevenge
{
    static class Circle
    {
        public static bool Intersects(Vector2 circle, int radius, Rectangle rect)
        {
            double dist_A = Math.Sqrt(Math.Pow(rect.X - circle.X, 2) + Math.Pow(rect.Y - circle.Y, 2));
            double dist_B = Math.Sqrt(Math.Pow(rect.X - circle.X, 2) + Math.Pow(rect.Y + rect.Width - circle.Y, 2));
            double dist_C = Math.Sqrt(Math.Pow(rect.X + rect.Width - circle.X, 2) + Math.Pow(rect.Y - circle.Y, 2));
            double dist_D = Math.Sqrt(Math.Pow(rect.X + rect.Width - circle.X, 2) + Math.Pow(rect.Y + rect.Width - circle.Y, 2));
            if (dist_A < radius || dist_B < radius || dist_C < radius || dist_D < radius)
                return true;
            else
                return false;
        }
        static public Vector2 Middle(int radius, int x, int y)
        {
            return new Vector2(x - radius, y - radius);
        }
        static public Texture2D createCircle(GraphicsDevice graphics, int durchmesser)
        {
            Texture2D texture = new Texture2D(graphics, durchmesser, durchmesser);
            Color[] colorData = new Color[durchmesser * durchmesser];

            float diam = durchmesser / 2;
            float diamsq = diam * diam;

            for (int x = 0; x < durchmesser; x++)
            {
                for (int y = 0; y < durchmesser; y++)
                {
                    int index = x * durchmesser + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
