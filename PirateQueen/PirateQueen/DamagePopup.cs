using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PirateQueen
{
    public class DamagePopup
    {
        // Attributes:
        double timeCreated;
        public string text;
        public Vector2 position;
        public float transparency;
        float targetY;

        // Constructor:
        public DamagePopup(Vector2 pos, string txt)
        {
            position = pos;
            text = txt;
            timeCreated = Game1.currentFrameTime;
            targetY = pos.Y - 100;
            transparency = 1f;
        }

        // Movement:
        public bool Move()
        {
            position.Y += (targetY - position.Y) * 0.1f;
            transparency -= 0.03f;
            return transparency <= 0;
        }
    }
}
