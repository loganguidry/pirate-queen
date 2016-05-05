using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PirateQueen
{
    // reps fast enemy, child of Enemy
    class FastEnemy:Enemy
    {
        public FastEnemy(Texture2D sprt, Texture2D walk, Vector2 pos, int randomSeed, string kind) : base(sprt, walk, pos, randomSeed, kind)
        {

        }
    }
}
