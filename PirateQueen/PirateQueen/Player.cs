using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace PirateQueen
{
    public class Player : Game
    {
        // Attributes:
        static public int MAX_HEALTH = 1000;
        static public int SWORD_REACH = 175;
        public int health;
        public Texture2D debugSprite;
        public Vector2 position;
        Vector2 velocity;
        bool onGround;
        AnimatedSprite animIdle;
        AnimatedSprite animWalk;
        AnimatedSprite animRun;
        AnimatedSprite animAttack;
        AnimatedSprite animAttackWalk;
        string currentAnimation;
        double lastAttackTime;
        double attackDelay = 0.5;
        bool facingLeft;
        public string weapon;
        Random rgen;
        

        // Constructor:
        public Player (Texture2D debug, Texture2D idle, Texture2D walk, Texture2D run, Texture2D attack, Texture2D attackWalk, Vector2 pos)
        {
            // Set attributes:
            debugSprite = debug;
            position = pos;
            velocity = Vector2.Zero;
            onGround = true;
            health = 1000;
            currentAnimation = "Idle";
            facingLeft = false;
            weapon = "Cutlass";
            rgen = new Random();

            // Load animations:
            animIdle = new AnimatedSprite(idle, 1, 1, 1, new Vector2(72, 72), 50);
            animWalk = new AnimatedSprite(walk, 3, 3, 1, new Vector2(72, 72), 50);
            animRun = new AnimatedSprite(run, 3, 3, 1, new Vector2(72, 72), 35);
            animAttack = new AnimatedSprite(attack, 3, 3, 1, new Vector2(72, 72), 50);
            animAttackWalk = new AnimatedSprite(attackWalk, 3, 3, 1, new Vector2(72, 72), 50);
        }

        // Reset:
        public void Reset ()
        {
            position = new Vector2(Game1.screenSize.X / 2, Game1.groundPosition);
            velocity = Vector2.Zero;
            onGround = true;
            health = 1000;
        }

        // Movement:
        public void Move (KeyboardState kbState)
        {
            // Friction for horizontal movement:
            if (!kbState.IsKeyDown(Keys.A) && !kbState.IsKeyDown(Keys.D) && onGround)
            {
                velocity.X *= Game1.PLAYER_FRICTION;
                currentAnimation = "Idle";
            }

            // Acceleration for horizontal movement:
            if (kbState.IsKeyDown(Keys.A))
            {
                facingLeft = true;
                currentAnimation = "Walk";
                velocity.X -= Game1.PLAYER_ACCELERATION;
                if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                {
                    currentAnimation = "Run";
                    if (velocity.X < -Game1.PLAYER_RUNNING_SPEED)
                        velocity.X = -Game1.PLAYER_RUNNING_SPEED;
                }
                else
                {
                    if (velocity.X < -Game1.PLAYER_WALKING_SPEED)
                        velocity.X = -Game1.PLAYER_WALKING_SPEED;
                }
            }
            if (kbState.IsKeyDown(Keys.D))
            {
                facingLeft = false;
                currentAnimation = "Walk";
                velocity.X += Game1.PLAYER_ACCELERATION;
                if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                {
                    currentAnimation = "Run";
                    if (velocity.X > Game1.PLAYER_RUNNING_SPEED)
                        velocity.X = Game1.PLAYER_RUNNING_SPEED;
                }
                else
                {
                    if (velocity.X > Game1.PLAYER_WALKING_SPEED)
                        velocity.X = Game1.PLAYER_WALKING_SPEED;
                }
            }

            // Check if on ground:
            onGround = position.Y >= Game1.groundPosition;

            // Jump:
            if (onGround && kbState.IsKeyDown(Keys.W))
                velocity.Y = -Game1.PLAYER_JUMP_FORCE;

            // Gravity:
            if (!onGround)
                velocity.Y += Game1.GRAVITY;

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
            
            if (kbState.IsKeyDown(Keys.Space))
                Attack();
        }

        // Attack:
        public void Attack ()
        {
            if (Game1.currentFrameTime - lastAttackTime >= attackDelay)
            {
                lastAttackTime = Game1.currentFrameTime;
                string dir = "Right";
                if (facingLeft)
                    dir = "Left";
                Console.WriteLine("Player attacking to the " + dir + " with " + weapon + ".");

                // Swing sword:
                if (weapon == "Cutlass")
                {
                    foreach (Enemy enemy in Game1.Enemies)
                    {
                        bool inFrontOfPlayer = false;
                        if (facingLeft)
                            inFrontOfPlayer = enemy.position.X <= position.X;
                        else
                            inFrontOfPlayer = enemy.position.X >= position.X;
                        if (Tools.Distance(enemy.position, position) <= SWORD_REACH && inFrontOfPlayer)
                            enemy.Damage(rgen.Next(20, 40));
                    }
                }
            }
        }

        // Animation:
        public void Animate (GameTime gt)
        {
            // Change animation:
            /*
            if (velocity.X <= -0.1f && onGround)
            {
                currentAnimation = "Walk";
                facingLeft = true;
            }
            else if (velocity.X >= 0.1f && onGround)
            {
                currentAnimation = "Walk";
                facingLeft = false;
            }
            else
                currentAnimation = "Idle";
            */

            // Update animations:
            if (currentAnimation == "Walk")
                animWalk.Update(gt);
            else if (currentAnimation == "Run")
                animRun.Update(gt);
            else if (currentAnimation == "Idle")
                animIdle.Update(gt);

            if (Game1.currentFrameTime - lastAttackTime <= 0.15)
            {
                if (currentAnimation == "Walk" || currentAnimation == "Run")
                {
                    animAttackWalk.Update(gt);
                    currentAnimation = "AttackWalk";
                }
                else
                {
                    animAttack.Update(gt);
                    currentAnimation = "Attack";
                }
            }
        }

        // Draw animation:
        public void Draw (SpriteBatch sb, Vector2 pos)
        {
            // Draw hitbox:
            if (Game1.Debugging)
                sb.Draw(debugSprite, position - new Vector2(debugSprite.Width / 2, debugSprite.Height), Color.White);

            // Draw player (animation):
            if (currentAnimation == "Walk")
                animWalk.Draw(sb, pos, facingLeft);
            else if (currentAnimation == "Run")
                animRun.Draw(sb, pos, facingLeft);
            else if (currentAnimation == "Idle")
                animIdle.Draw(sb, pos, facingLeft);
            else if (currentAnimation == "Attack")
                animAttack.Draw(sb, pos, facingLeft);
            else if (currentAnimation == "AttackWalk")
                animAttackWalk.Draw(sb, pos, facingLeft);
        }

        // Take damage:
        public void Damage (int amount)
        {
            health -= amount;
            Game1.DamagePopups.Add(new DamagePopup(position + new Vector2(-debugSprite.Width / 4, -debugSprite.Height - 50), amount.ToString()));
        }
    }
}
