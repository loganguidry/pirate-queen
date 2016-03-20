using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PirateQueen
{
    class Player
    {
        // Attributes:
        public Texture2D sprite;
        public Vector2 position;
        Vector2 velocity;
        bool onGround;
        int health;

        // Constructor:
        public Player (Texture2D img, Vector2 pos)
        {
            sprite = img;
            position = pos;
            velocity = Vector2.Zero;
            onGround = true;
            health = 100;
        }

        // Reset:
        public void Reset ()
        {
            position = new Vector2(Game1.screenSize.X / 2, Game1.groundPosition);
            velocity = Vector2.Zero;
            onGround = true;
        }

        // Movement:
        public void Move (KeyboardState kbState)
        {
            // Friction for horizontal movement:
            if (!kbState.IsKeyDown(Keys.Left) && !kbState.IsKeyDown(Keys.A) && !kbState.IsKeyDown(Keys.Right) && !kbState.IsKeyDown(Keys.D) && onGround)
            {
                velocity.X *= Game1.PLAYER_FRICTION;
            }

            // Acceleration for horizontal movement:
            if (kbState.IsKeyDown(Keys.Left) || kbState.IsKeyDown(Keys.A))
            {
                velocity.X -= Game1.PLAYER_ACCELERATION;
                if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                {
                    if (velocity.X < -Game1.PLAYER_RUNNING_SPEED)
                    {
                        velocity.X = -Game1.PLAYER_RUNNING_SPEED;
                    }
                }
                else
                {
                    if (velocity.X < -Game1.PLAYER_WALKING_SPEED)
                    {
                        velocity.X = -Game1.PLAYER_WALKING_SPEED;
                    }
                }
            }
            if (kbState.IsKeyDown(Keys.Right) || kbState.IsKeyDown(Keys.D))
            {
                velocity.X += Game1.PLAYER_ACCELERATION;
                if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                {
                    if (velocity.X > Game1.PLAYER_RUNNING_SPEED)
                    {
                        velocity.X = Game1.PLAYER_RUNNING_SPEED;
                    }
                }
                else
                {
                    if (velocity.X > Game1.PLAYER_WALKING_SPEED)
                    {
                        velocity.X = Game1.PLAYER_WALKING_SPEED;
                    }
                }
            }

            // Check if on ground:
            onGround = position.Y >= Game1.groundPosition;

            // Jump:
            if (onGround && kbState.IsKeyDown(Keys.Space))// KeyPress(Keys.Space))
            {
                velocity.Y = -Game1.PLAYER_JUMP_FORCE;
            }

            // Gravity:
            if (!onGround)
            {
                velocity.Y += Game1.GRAVITY;
            }

            // Move player:
            position += velocity;

            // Keep on-screen:
            if (Game1.currentLevel == 1 && Game1.currentLevelStage == 0)
            {
                // Can't walk into the ocean on level 1, frame 1:
                if (position.X <= 350)
                {
                    position.X = 350;
                    velocity.X = 0;
                }
            }
            if (position.X <= sprite.Width / 2)
            {
                position.X = sprite.Width / 2;
                velocity.X = 0;
            }
            if (position.X >= Game1.screenSize.X - (sprite.Width / 2))
            {
                position.X = Game1.screenSize.X - (sprite.Width / 2);
                velocity.X = 0;
            }

            // Keep on ground:
            if (position.Y > Game1.groundPosition)
            {
                position.Y = Game1.groundPosition;
                velocity.Y = 0;
                onGround = true;
            }
        }
    }
}
