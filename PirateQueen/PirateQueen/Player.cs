using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PirateQueen
{
    public class Player
    {
        // Attributes:
        public Texture2D debugSprite;
        public Vector2 position;
        Vector2 velocity;
        bool onGround;
        int health;
        AnimatedSprite animIdle;
        AnimatedSprite animWalk;
        AnimatedSprite animRun;
        AnimatedSprite animAttack;
        string currentAnimation;

        // Constructor:
        public Player (Texture2D debug, Texture2D idle, Texture2D walk, Texture2D run, Texture2D attack, Vector2 pos)
        {
            // Set attributes:
            debugSprite = debug;
            position = pos;
            velocity = Vector2.Zero;
            onGround = true;
            health = 100;
            currentAnimation = "Idle";

            // Load animations:
            animIdle = new AnimatedSprite(idle, 6, 3, 2, new Vector2(72, 72), 100);
            animWalk = new AnimatedSprite(walk, 6, 3, 2, new Vector2(72, 72), 100);
            animRun = new AnimatedSprite(run, 6, 3, 2, new Vector2(72, 72), 100);
            animAttack = new AnimatedSprite(attack, 6, 3, 2, new Vector2(72, 72), 100);
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
            if (!kbState.IsKeyDown(Keys.A) && !kbState.IsKeyDown(Keys.D) && onGround)
            {
                velocity.X *= Game1.PLAYER_FRICTION;
            }

            // Acceleration for horizontal movement:
            if (kbState.IsKeyDown(Keys.A))
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
            if (kbState.IsKeyDown(Keys.D))
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
            position += new Vector2(velocity.X * (float)Game1.dt, velocity.Y * (float)Game1.dt);

            // Keep on-screen:
            if (Game1.currentLevel == 1 && Game1.currentLevelStage == 0)
            {
                // Can't walk into the ocean on level 1, frame 1:
                if (position.X <= 425)
                {
                    position.X = 425;
                    velocity.X = Math.Max (1, velocity.X);
                }
            }
            if (position.X <= debugSprite.Width / 2)
            {
                position.X = debugSprite.Width / 2;
                velocity.X = velocity.X = Math.Max(1, velocity.X);
            }
            if (position.X >= Game1.screenSize.X - (debugSprite.Width / 2))
            {
                position.X = Game1.screenSize.X - (debugSprite.Width / 2);
                velocity.X = velocity.X = Math.Min(-1, velocity.X);
            }

            // Keep on ground:
            if (position.Y > Game1.groundPosition)
            {
                position.Y = Game1.groundPosition;
                velocity.Y = 0;
                onGround = true;
            }
        }

        // Attack:
        public void Attack (KeyboardState kbState)
        {
            // Attack:
            if (kbState.IsKeyDown(Keys.Left))
            {

            }
            if (kbState.IsKeyDown(Keys.Right))
            {

            }
        }

        // Animation:
        public void Animate (GameTime gt)
        {
            // Change animation:
            if (velocity.X <= -0.1f && onGround)
                currentAnimation = "Walk Left";
            else if (velocity.X >= 0.1f && onGround)
                currentAnimation = "Walk Right";
            else
                currentAnimation = "Idle";

            // Update animations:
            if (currentAnimation == "Walk Left")
                animWalk.Update(gt);
            if (currentAnimation == "Walk Right")
                animWalk.Update(gt);
        }

        // Draw animation:
        public void Draw (SpriteBatch sb, Vector2 pos)
        {
            // Draw player (animation):
            if (currentAnimation == "Walk Left")
                animWalk.Draw(sb, pos, true);
            else if (currentAnimation == "Walk Right")
                animWalk.Draw(sb, pos);
            else
                animWalk.Draw(sb, pos);
        }

        // Take damage:
        public void Damage (int amount)
        {
            health -= amount;

            //if (health <= 0)
            //    Die();
        }
    }
}
