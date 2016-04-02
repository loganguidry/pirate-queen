﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PirateQueen
{
    class Enemy
    {
        // Attributes:
        public Texture2D debugSprite;
        public Vector2 position;
        private Vector2 velocity;
        private bool onGround;
        private int health;
        private AnimatedSprite animIdle;
        private AnimatedSprite animWalk;
        private AnimatedSprite animAttack;
        private string currentAnimation;
        private float speed;
        private string type;
        private Random rgen;

        // Constructor:
        public Enemy(Texture2D sprt, Texture2D walk, Vector2 pos, int randomSeed)
        {
            // Set attributes:
            debugSprite = sprt;
            position = pos;
            velocity = Vector2.Zero;
            onGround = true;
            health = 100;
            currentAnimation = "Idle";
            type = "normal";
            rgen = new Random(randomSeed);

            // Load enemy attributes:
            switch (type)
            {
                case "normal":
                default:
                    speed = rgen.Next (20, 50) / 10f;
                    break;
                case "fast":
                    speed = rgen.Next(51, 100) / 10f ;
                    break;
                case "heavy":
                    speed = rgen.Next(10, 19) / 10f;
                    break;
            }

            // Load animations:
            animWalk = new AnimatedSprite(walk, 6, 3, 2, new Vector2(72, 72), 100);
        }

        // Reset:
        public void Reset()
        {
            position = new Vector2(Game1.screenSize.X / 2, Game1.groundPosition);
            velocity = Vector2.Zero;
            onGround = true;
        }

        // Movement:
        public void Move(KeyboardState kbState)
        {
            // Get information:
            bool playerToLeft = Game1.player.position.X + (Game1.player.debugSprite.Width / 2f) < position.X;
            bool playerToRight = Game1.player.position.X - (Game1.player.debugSprite.Width / 2f) > position.X;
            bool jump = rgen.Next(0, 1000) == 1;

            // Friction for horizontal movement:
            if (!playerToLeft && !playerToRight && onGround)
            {
                velocity.X *= Game1.PLAYER_FRICTION;
            }

            // Acceleration for horizontal movement:
            if (playerToLeft)
            {
                velocity.X -= Game1.PLAYER_ACCELERATION;
                if (velocity.X < -speed)
                {
                    velocity.X = -speed;
                }
            }
            if (playerToRight)
            {
                velocity.X += Game1.PLAYER_ACCELERATION;
                if (velocity.X > speed)
                {
                    velocity.X = speed;
                }
            }

            // Check if on ground:
            onGround = position.Y >= Game1.groundPosition;

            // Jump:
            if (onGround && jump)
            {
                velocity.Y = -Game1.PLAYER_JUMP_FORCE;
            }

            // Gravity:
            if (!onGround)
            {
                velocity.Y += Game1.GRAVITY;
            }

            // Move enemy:
            position += new Vector2(velocity.X * (float)Game1.dt, velocity.Y * (float)Game1.dt);

            // Keep on-screen:
            if (Game1.currentLevel == 1 && Game1.currentLevelStage == 0)
            {
                // Can't walk into the ocean on level 1, frame 1:
                if (position.X <= 425)
                {
                    position.X = 425;
                    velocity.X = Math.Max(1, velocity.X);
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
        public void Attack(KeyboardState kbState)
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
        public void Animate(GameTime gt)
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
        public void Draw(SpriteBatch sb, Vector2 pos)
        {
            // Draw enemy (animation):
            if (currentAnimation == "Walk Left")
                animWalk.Draw(sb, pos, true);
            else if (currentAnimation == "Walk Right")
                animWalk.Draw(sb, pos);
            else
                animWalk.Draw(sb, pos);
        }

        // Take damage:
        public void Damage(int amount)
        {
            health -= amount;

            //if (health <= 0)
            //    Die();
        }
    }
}
