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
        private int health;
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
            this.health = 400;

            // load animation
            animIdle = new AnimatedSprite(anims, 1, 0, 0, new Vector2(128, 128), 50);
            animWalk = new AnimatedSprite(anims, 4, 0, 0, new Vector2(128, 128), 100);
            animAttack = new AnimatedSprite(anims, 3, 0, 1, new Vector2(128, 128), 100);
            animFireCannon = new AnimatedSprite(anims, 5, 0, 2, new Vector2(128, 128), 75);
            animBullet = new AnimatedSprite(anims, 1, 0, 3, new Vector2(128, 128), 50);
        }

        

        public override void Draw(SpriteBatch sb, Vector2 pos)
        {
			// Automatically start moving after 2 seconds:
			if (Game1.currentFrameTime - spawnTime > 2000 && !canMove)
				canMove = true;

            // Draw hitbox:
            if (Game1.Debugging)
                sb.Draw(debugSprite, new Rectangle((int)(position.X - (debugSprite.Width / 2.0f)), (int)(position.Y - debugSprite.Height), debugSprite.Width, debugSprite.Height), Color.MonoGameOrange);
            
            // Draw enemy (animation):
            if (currentAnimation == "Walk Left")
                animWalk.Draw(sb, pos, true);
            else if (currentAnimation == "Walk Right")
                animWalk.Draw(sb, pos, false);
            else
                animWalk.Draw(sb, pos, false);
        }

    }

}
