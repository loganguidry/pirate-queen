using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace PirateQueen
{
    class Bullet
    {
        // Static public attributes:
        static public List<Bullet> globalBullets = new List<Bullet>();

        // Attributes:
        Vector2 position;
        Vector2 velocity;
        
        // Constructor:
        public Bullet(Vector2 pos, Vector2 vel)
        {
            position = pos;
            velocity = vel;
        }

        // Move:
        public void Move()
        {
            position += velocity;

            // Delete when off screen:
            if (new Rectangle(0, 0, (int)Game1.screenSize.X, (int)Game1.screenSize.Y).Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1)))
                globalBullets.Remove(this);
        }
    }
}
