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
    class Boss:Enemy 
    {
        // Settings:
        //int ATTACK_DELAY = 30;

        // Attributes:
        public Texture2D blackbeard;
        //public Vector2 position;
        //public int health;
        //private int damageMin;
        //private int damageMax;
        //private Vector2 velocity;
        //private bool onGround;
        //private AnimatedSprite animIdle;
        private AnimatedSprite animWalk;
        //private AnimatedSprite animAttack;
        private string currentAnimation;
        //private float speed;
        //private string type;
        //private Random rgen;
        //private int attackStep;
        //private bool nextToPlayer;
        
        public Boss(Texture2D sprt, Texture2D walk, Vector2 pos, int randomSeed, string kind, Texture2D bb): base (sprt, walk, pos, randomSeed, kind)
        {
            
        }

        public override void Draw(SpriteBatch sb, Vector2 pos)
        {
            // Draw hitbox:
            if (Game1.Debugging)
                sb.Draw(blackbeard, position - new Vector2(blackbeard.Width / 2, blackbeard.Height), Color.MonoGameOrange);
            
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
