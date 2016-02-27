using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PirateQueen
{
    // Finite state machine:
    enum GameState
    {
        Intro, Menu, Gameplay, Transition, Win
    }

    public class Game1 : Game
    {
        // Attributes:
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState state;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            state = GameState.Intro;

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            switch (state)
            {
                case (GameState.Intro):
                    break;
                case (GameState.Transition):
                    break;
                case (GameState.Win):
                    break;
                case (GameState.Menu):
                    break;
                case (GameState.Gameplay):
                    break;
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MonoGameOrange);

            switch (state)
            {
                case (GameState.Intro):
                    break;
                case (GameState.Transition):
                    break;
                case (GameState.Win):
                    break;
                case (GameState.Menu):
                    break;
                case (GameState.Gameplay):
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
