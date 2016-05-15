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
			offset = new Vector2(0, 20);

			// load animation
			animIdle = new AnimatedSprite(anims, 3, 0, 0, new Vector2(144, 144), 50);
            animWalk = new AnimatedSprite(anims, 4, 0, 0, new Vector2(144, 144), 50);
            animAttack = new AnimatedSprite(anims, 3, 0, 1, new Vector2(144, 144), 100);
            //animFireCannon = new AnimatedSprite(anims, 5, 0, 2, new Vector2(128, 128), 75);
            //animBullet = new AnimatedSprite(anims, 1, 0, 3, new Vector2(128, 128), 50);
        }
		
        public override void Draw(SpriteBatch sb, Vector2 pos)
        {
			// Automatically start moving after 2 seconds:
			if (Game1.currentFrameTime - spawnTime > 2000 && !canMove)
				canMove = true;
			
			base.Draw(sb, pos);
		}
		
    }

}
