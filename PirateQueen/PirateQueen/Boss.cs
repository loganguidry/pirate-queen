using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PirateQueen
{
    public class Boss:Enemy 
    {
        // Settings:
        //int ATTACK_DELAY = 30;

        // Attributes:
        private AnimatedSprite animIdle;
		private AnimatedSprite animWalk;
		private AnimatedSprite animAttack;
        private AnimatedSprite animFireCannon;
        private AnimatedSprite animBullet;
        public int hp;
		//private string currentAnimation;
		//private float speed;
		//private string type;
		//private Random rgen;
		//private int attackStep;
		//private bool nextToPlayer;
		double spawnTime;
        
        public Boss(Texture2D sprt, Texture2D anims, Vector2 pos, int randomSeed): base (sprt, anims, pos, randomSeed, "boss")
        {
			// Wait 2 seconds before moving):
			spawnTime = Game1.currentFrameTime;
			canMove = false;

			// Set attributes:
			speed = rgen.Next(12, 18) / 10f;
			damageMin = 75;
			damageMax = 200;
            hp = 275;
            health = 100;

            // load animation
            animIdle = new AnimatedSprite(anims, 1, 0, 0, new Vector2(128, 128), 50);
            animWalk = new AnimatedSprite(anims, 4, 0, 0, new Vector2(128, 128), 50);
            animAttack = new AnimatedSprite(anims, 3, 0, 1, new Vector2(128, 128), 100);
            //animFireCannon = new AnimatedSprite(anims, 5, 0, 2, new Vector2(128, 128), 75);
            //animBullet = new AnimatedSprite(anims, 1, 0, 3, new Vector2(128, 128), 50);
        }

        public override void Move(KeyboardState kbState)
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
            if (position.Y > (Game1.groundPosition - 90))
            {
                position.Y = Game1.groundPosition - 90;
                velocity.Y = 0;
                onGround = true;
            }
        }

        public override void Draw(SpriteBatch sb, Vector2 pos)
        {
			// Automatically start moving after 2 seconds:
			if (Game1.currentFrameTime - spawnTime > 2000 && !canMove)
				canMove = true;

            // Draw hitbox:
            //if (Game1.Debugging)
                //sb.Draw(debugSprite, new Rectangle((int)(position.X - (debugSprite.Width / 2.0f)), (int)(position.Y - debugSprite.Height / 2.0f), debugSprite.Width * 2, debugSprite.Height ), Color.MonoGameOrange);
            
            // Draw enemy (animation):
            if (currentAnimation == "Walk Left")
                animWalk.Draw(sb, pos, true);
            else if (currentAnimation == "Walk Right")
                animWalk.Draw(sb, pos, false);
            else if (currentAnimation == "Attack")
                animAttack.Draw(sb, pos, true);
            else
                animWalk.Draw(sb, pos, false);
        }

        public override void Damage(int amount)
        {
            hp -= amount;
            Game1.DamagePopups.Add(new DamagePopup(position + new Vector2(-debugSprite.Width / 3, -debugSprite.Height - 50), amount.ToString()));
        }

    }

}
