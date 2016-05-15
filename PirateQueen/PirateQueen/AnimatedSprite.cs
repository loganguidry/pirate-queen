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
    public class AnimatedSprite
    {
        // Attributes:
        Texture2D spriteSheet;
        int frame;
        int frames;
        int rows;
        int columns;
        int timeSinceLastFrame;
        int stepDelay;
        Vector2 frameSize;
        Rectangle currentFrameRect;

        // Constructor:
        public AnimatedSprite (Texture2D img, int frms, int cl, int rw, Vector2 size, int spd)
        {
            spriteSheet = img;
            frame = 0;
            frames = frms;
            rows = rw;
            columns = cl;
            stepDelay = spd;
            frameSize = size;
            currentFrameRect = new Rectangle(0, 0, (int)size.X, (int)size.Y);
        }

        // Animate:
        public void Update (GameTime gt)
        {
            timeSinceLastFrame += gt.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > stepDelay)
            {
                // Next frame:
                timeSinceLastFrame = 0;
                frame++;
                if (frame >= frames)
                    frame = 0;

                // Get location of frame in spritesheet:
                // int row = (int)Math.Floor(frame / (double)columns);
                // int column = frame - (row * columns);
                currentFrameRect = new Rectangle(columns * (int)frameSize.X + frame * (int)frameSize.X, 
                                                 rows * (int)frameSize.Y,
                                                 (int)frameSize.X,
                                                 (int)frameSize.Y);
            }
		}

		// Draw:
		public void Draw(SpriteBatch sb, Vector2 pos, bool flipped)
		{
			if (flipped)
				sb.Draw(spriteSheet,
						pos,
						currentFrameRect,
						Color.White,
						0,
						Vector2.Zero,
						2f,
						SpriteEffects.FlipHorizontally,
						0);
			else
				sb.Draw(spriteSheet,
						pos,
						currentFrameRect,
						Color.White,
						0,
						Vector2.Zero,
						2f,
						SpriteEffects.None,
						0);


		}

		// Draw with offset:
		public void Draw(SpriteBatch sb, Vector2 pos, bool flipped, Vector2 offset)
		{
			if (flipped)
				sb.Draw(spriteSheet,
						pos,
						currentFrameRect,
						Color.White,
						0,
						offset,
						2f,
						SpriteEffects.FlipHorizontally,
						0);
			else
				sb.Draw(spriteSheet,
						pos,
						currentFrameRect,
						Color.White,
						0,
						offset,
						2f,
						SpriteEffects.None,
						0);
		}

		// Draw with red flash:
		public void Draw(SpriteBatch sb, Vector2 pos, bool flipped, bool flash)
		{
			Color col = Color.White;
			if (flash)
				col = Color.Red;

			if (flipped)
				sb.Draw(spriteSheet,
						pos,
						currentFrameRect,
						col,
						0,
						Vector2.Zero,
						2f,
						SpriteEffects.FlipHorizontally,
						0);
			else
				sb.Draw(spriteSheet,
						pos,
						currentFrameRect,
						col,
						0,
						Vector2.Zero,
						2f,
						SpriteEffects.None,
						0);
		}

		// Draw with red flash with offset:
		public void Draw(SpriteBatch sb, Vector2 pos, bool flipped, Vector2 offset, bool flash)
		{
			Color col = Color.White;
			if (flash)
				col = Color.Red;

			if (flipped)
				sb.Draw(spriteSheet,
						pos,
						currentFrameRect,
						col,
						0,
						offset,
						2f,
						SpriteEffects.FlipHorizontally,
						0);
			else
				sb.Draw(spriteSheet,
						pos,
						currentFrameRect,
						col,
						0,
						offset,
						2f,
						SpriteEffects.None,
						0);
		}
	}
}
