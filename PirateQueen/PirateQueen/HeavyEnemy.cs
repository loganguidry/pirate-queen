using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PirateQueen
{
    // reps heavy enemy, child of Enemy
    class HeavyEnemy:Enemy
    {
        public HeavyEnemy(Texture2D sprt, Texture2D walk, Vector2 pos, int randomSeed, string kind):base(sprt,walk,pos,randomSeed,kind)
        {

        }
    }
}
