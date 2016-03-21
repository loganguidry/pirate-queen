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
    class AnimatedSprite
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
        Vector2 currentFrameRect;

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
            currentFrameRect = new Vector2(0, 0);
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
                int row = (int)Math.Floor(frame / (double)columns);
                int column = frame - (row * columns);
                currentFrameRect.X = column * frameSize.X;
                currentFrameRect.Y = row * frameSize.Y;
            }
        }

        // Draw:
        public void Draw (SpriteBatch sb, Vector2 pos)
        {
            sb.Draw(spriteSheet, pos,
                new Rectangle((int)currentFrameRect.X, (int)currentFrameRect.Y, (int)frameSize.X, (int)frameSize.Y),
                Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
        }
    }
}
