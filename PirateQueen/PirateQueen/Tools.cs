using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PirateQueen
{
    public class Tools
    {
        static public float Distance(Vector2 start, Vector2 end)
        {
            Vector2 dist = end - start;
            return (float)Math.Sqrt(dist.X * dist.X + dist.Y * dist.Y);
        }
    }
}
