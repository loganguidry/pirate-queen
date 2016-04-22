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
        int width = 40;
        int height = 40;

        // Constructor:
        public HealthPickup(Vector2 pos)
        {
            // Set position:
            position = pos;

            // Set randomized upwards velocity:
            Random rgen = new Random();
            velocity = new Vector2(rgen.Next(-15, 15), -30f);
        }

        // Movement:
        public void Move()
        {
            // Gravity:
            velocity.Y += Game1.GRAVITY;

            // Move:
            position += velocity;

            // Land on ground:
            if (position.Y + (height / 2) > Game1.groundPosition)
            {
                position.Y = Game1.groundPosition - (height / 2);
                velocity = Vector2.Zero;
            }

            // Hit wall on first level:
            if (Game1.currentLevel == 1 && Game1.currentLevelStage == 0 && position.X < 425)
                velocity.X = -velocity.X;

            // Hit wall:
            if (position.X + (width / 2) > Game1.screenSize.X || position.X - (width / 2) < 0)
                velocity.X = -velocity.X;
        }

        // Draw:
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Game1.healthPickupSprite, new Rectangle((int)position.X - (width / 2), (int)position.Y - (height / 2), width, height), Color.White);
        }
    }
}
