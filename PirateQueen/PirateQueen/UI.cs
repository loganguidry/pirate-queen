using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace PirateQueen
{
    public class UI
    {
        public void Draw(SpriteBatch sb)
        {
            // Draw health bar and weapon background:
            sb.Draw(Game1.healthBarSprite, new Vector2(5, 5), Color.White);

            // Draw health:
            sb.Draw(Game1.white2x2square, new Rectangle(175, 25, 350, 60), Color.MonoGameOrange);
            sb.Draw(Game1.white2x2square, new Rectangle(175, 25, (int)(350 * (Game1.player.health / 100.0)), 60), Color.DarkSeaGreen);

            // Draw weapon:
        }
    }
}
