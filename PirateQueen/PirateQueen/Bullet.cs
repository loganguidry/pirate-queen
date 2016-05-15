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
        int width = 22;
        int height = 4;
        string owner;
        Random rgen;
        
        // Constructor:
        public Bullet(Vector2 pos, Vector2 vel, string own)
        {
            position = pos;
            velocity = vel;
            owner = own;
            rgen = new Random();
        }

        // Move:
        public bool Move()
        {
            // Move bullet:
            position += velocity;

            // Check collisions:
            Rectangle bulletRect = new Rectangle((int)position.X - (width / 2), (int)position.Y - (height / 2), width, height);
            bool deleteThis = false;
            if (owner == "player")
            {
                foreach (Enemy enemy in Game1.Enemies)
                {
                    Rectangle enemyRect = new Rectangle((int)enemy.position.X, (int)(enemy.position.Y - (enemy.debugSprite.Height / 2)), enemy.debugSprite.Width, enemy.debugSprite.Height);
                    if (enemyRect.Intersects(bulletRect))
                    {
                        deleteThis = true;
                        enemy.Damage(rgen.Next(15, 25));
                        break;
                    }
                }
            }
            if (deleteThis)
                return false;

            // Delete when off screen:
            return new Rectangle(0, 0, (int)Game1.screenSize.X, (int)Game1.screenSize.Y).Intersects(bulletRect);
        }

        // Draw:
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Game1.white2x2square, new Rectangle((int)position.X - (width / 2), (int)position.Y - (height / 2) + 1, width, height), Color.Black);
            sb.Draw(Game1.white2x2square, new Rectangle((int)position.X - (width / 2), (int)position.Y - (height / 2), width, height), Color.White);
        }

        // Static public method to move all bullets:
        static public void MoveBullets()
        {
            List<Bullet> deadBullets = new List<Bullet>();
            foreach (Bullet bullet in globalBullets)
            {
                if (!bullet.Move())
                    deadBullets.Add(bullet);
            }
            foreach (Bullet bullet in deadBullets)
                globalBullets.Remove(bullet);
            deadBullets.Clear();
        }

        // Static public method to draw all bullets:
        static public void DrawBullets(SpriteBatch sb)
        {
            foreach (Bullet bullet in globalBullets)
                bullet.Draw(sb);
        }
    }
}
