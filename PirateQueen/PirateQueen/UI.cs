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

            // Draw health bar:
            sb.Draw(Game1.white2x2square, new Rectangle(175, 25, 350, 60), Color.MonoGameOrange);
            sb.Draw(Game1.white2x2square, new Rectangle(175, 25, (int)(350 * (Game1.displayHealth / (double)Player.MAX_HEALTH)), 60), Color.DarkSeaGreen);

			// Draw health bar bevel effect:
			sb.Draw(Game1.white2x2square, new Rectangle(175, 55, (int)(350 * (Game1.displayHealth / (double)Player.MAX_HEALTH)), 30), new Color(0, 0, 0, 10));

			// Draw level bevel effect:
			sb.DrawString(Game1.uiFont, "Level", new Vector2(39, 29), Color.White);
			sb.DrawString(Game1.uiFontLarge, Game1.currentLevel.ToString(), new Vector2(64, 49), Color.White);
			sb.DrawString(Game1.uiFont, "Level", new Vector2(41, 31), Color.Black);
			sb.DrawString(Game1.uiFontLarge, Game1.currentLevel.ToString(), new Vector2(66, 51), Color.Black);
			
			// Draw level:
			sb.DrawString(Game1.uiFont, "Level", new Vector2(40, 30), Color.Wheat);
			sb.DrawString(Game1.uiFontLarge, Game1.currentLevel.ToString(), new Vector2(65, 50), Color.Wheat);
		}
    }
}
