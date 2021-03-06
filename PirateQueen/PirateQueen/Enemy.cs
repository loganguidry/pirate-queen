﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PirateQueen
{
    public class Enemy
    {
        // Settings:
        int ATTACK_DELAY = 30;

        // Attributes:
        public Texture2D debugSprite;
        public Vector2 position;
        public int health;
		public int damageMin;
		public int damageMax;
		public Vector2 velocity;
        public bool onGround;
        public AnimatedSprite animIdle;
		public AnimatedSprite animWalk;
		public AnimatedSprite animAttack;
		public string currentAnimation;
		public float speed;
		public string type;
		public Random rgen;
		public int attackStep;
		public bool nextToPlayer;
		public bool canMove;
		public Vector2 offset;
		public bool takeDamage;
		public double lastDamageTime;
		public double lastAttackTime;
		public bool facingRight;

		// Constructor:
		public Enemy(Texture2D sprt, Texture2D anims, Vector2 pos, int randomSeed, string kind)
        {
            // Set attributes:
            debugSprite = sprt;
            position = pos;
            velocity = Vector2.Zero;
            onGround = true;
            health = 100;
            currentAnimation = "Idle";
            type = kind;
            rgen = new Random(randomSeed);
            nextToPlayer = false;
            attackStep = 0;
			canMove = true;
			offset = new Vector2(0, -4);
			takeDamage = false;
			lastDamageTime = -500;
			facingRight = false;

			// Load enemy attributes:
			switch (type)
            {
                case "normal":
                default:
                    speed = rgen.Next (20, 50) / 10f;
                    damageMin = 20;
                    damageMax = 40;
                    break;
                case "fast":
                    speed = rgen.Next(51, 100) / 10f;
                    damageMin = 10;
                    damageMax = 25;
                    break;
                case "heavy":
                    speed = rgen.Next(10, 19) / 10f;
                    damageMin = 35;
                    damageMax = 60;
                    break;
            }

            // Load animations:
            animWalk = new AnimatedSprite(anims, 4, 0, 0, new Vector2(72, 72), 100);
            animAttack = new AnimatedSprite(anims, 4, 0, 1, new Vector2(72, 72), 100);
        }

        // Reset:
        public void Reset()
        {
            position = new Vector2(Game1.screenSize.X / 2, Game1.groundPosition);
            velocity = Vector2.Zero;
            onGround = true;
        }

        // Movement:
        public virtual void Move(KeyboardState kbState)
        {
            // Get information:
            bool playerToLeft = Game1.player.position.X + (Game1.player.debugSprite.Width / 2f) < position.X - (debugSprite.Width / 2f);
            bool playerToRight = Game1.player.position.X - (Game1.player.debugSprite.Width / 2f) > position.X + (debugSprite.Width / 2f);
            bool jump = rgen.Next(0, 1000) == 1 && canMove;

            // Friction for horizontal movement:
            if (!playerToLeft && !playerToRight && onGround)
                velocity.X *= Game1.PLAYER_FRICTION;

            // Acceleration for horizontal movement:
            nextToPlayer = false;
			if (canMove)
			{
				if (playerToLeft)
				{
					velocity.X -= Game1.PLAYER_ACCELERATION;
					if (velocity.X < -speed)
						velocity.X = -speed;
				}
				else if (playerToRight)
				{
					velocity.X += Game1.PLAYER_ACCELERATION;
					if (velocity.X > speed)
						velocity.X = speed;
				}
				else
					nextToPlayer = true;
			}

            // Check if on ground:
            onGround = position.Y >= Game1.groundPosition;

            // Jump:
            if (onGround && jump)
                velocity.Y = -Game1.PLAYER_JUMP_FORCE;

            // Gravity:
            if (!onGround)
                velocity.Y += Game1.GRAVITY;

            // Move enemy:
            position += new Vector2(velocity.X, velocity.Y);

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

            // Keep on ground:
            if (position.Y > Game1.groundPosition)
            {
                position.Y = Game1.groundPosition;
                velocity.Y = 0;
                onGround = true;
            }
        }

        // Attack:
        public void Attack()
		{
			// If the boss just recently spawned, they can't attack:
			if (!canMove)
				return;

			// increase attack timer when enemy is next to player
			if (nextToPlayer)
            {
                attackStep++;
                if(attackStep >= ATTACK_DELAY)
                {
                    attackStep = 0;
                }
            }
            if(nextToPlayer == false)
            {
                attackStep = 1;
            }

            // Attack:
            if (nextToPlayer && attackStep == 0)
            {
                Game1.player.Damage(rgen.Next(damageMin, damageMax) + 50);
				lastAttackTime = Game1.currentFrameTime;
			}
        }

        // Animation:
        public void Animate(GameTime gt)
        {
			// Change animation:
			currentAnimation = "Idle";
			if (velocity.X <= -0.1f && onGround)
			{
				currentAnimation = "Walk Right";
				facingRight = true;
			}
			if (velocity.X >= 0.1f && onGround)
			{
				currentAnimation = "Walk Left";
				facingRight = false;
			}
            if (Game1.currentFrameTime - lastAttackTime <= 300)
                currentAnimation = "Attack";

            // Update animations:
            if (currentAnimation == "Walk Right")
                animWalk.Update(gt);
            else if (currentAnimation == "Walk Left")
                animWalk.Update(gt);
            else if (currentAnimation == "Attack")
                animAttack.Update(gt);

		}

		// Draw animation:
		public virtual void Draw(SpriteBatch sb, Vector2 pos)
        {
            // Draw hitbox:
            if (Game1.Debugging)
                sb.Draw(debugSprite, position - new Vector2(debugSprite.Width / 2, debugSprite.Height), Color.MonoGameOrange);

			// Flash red when hurt:
			if (takeDamage && Game1.currentFrameTime - lastDamageTime >= 100)
				takeDamage = false;

			// Draw enemy (animation):
			if (currentAnimation == "Walk Left")
                animWalk.Draw(sb, pos, true, offset, takeDamage);
            else if (currentAnimation == "Walk Right")
                animWalk.Draw(sb, pos, false, offset, takeDamage);
            else if (currentAnimation == "Attack")
                animAttack.Draw(sb, pos, !facingRight, offset, takeDamage);
            else
				animWalk.Draw(sb, pos, !facingRight, offset, takeDamage);
        }

        // Take damage:
        public virtual void Damage(int amount)
        {
			takeDamage = true;
			lastDamageTime = Game1.currentFrameTime;
			health -= amount;
            Game1.DamagePopups.Add(new DamagePopup(position + new Vector2(-debugSprite.Width / 4, -debugSprite.Height - 50), amount.ToString()));
        }
    }
}
