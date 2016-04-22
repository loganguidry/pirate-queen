using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PirateQueen
{
    class HealthPickup
    {
        // Attributes:
        Vector2 position;
        Vector2 velocity;

        // Constructor:
        public HealthPickup(Vector2 pos)
        {
            // Set position:
            position = pos;

            // Set randomized upwards velocity:
            Random rgen = new Random();
            velocity = new Vector2(rgen.Next(-5, 5), -10f);
        }

        // Movement:
        public void Move()
        {
            // Gravity:
            velocity.Y += Game1.GRAVITY;

            // Move:
            position += velocity;

            // Land on ground:
            if (position.Y > Game1.groundPosition)
            {
                position.Y = Game1.groundPosition;
                velocity = Vector2.Zero;
            }

            // Hit wall:
            if (position.X > Game1.screenSize.X || position.X < 0)
                velocity.X = -velocity.X;
        }

        // Draw:
        public void Draw(SpriteBatch sb)
        {
            int width = 40;
            int height = 40;
            sb.Draw(Game1.healthPickupSprite, new Rectangle((int)position.X - (width / 2), (int)position.Y - (height / 2), width, height), Color.White);
        }
    }
}
