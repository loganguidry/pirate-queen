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
        Vector2 center;

        // Texture2Ds:
        Texture2D lasrLogo;
        Texture2D startScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            state = GameState.Intro;
            center = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load sprites:
            lasrLogo = Content.Load<Texture2D>("LASR");
            //

        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Exit();
            
            switch (state)
            {
                case (GameState.Intro):
                    break;
                case (GameState.Transition):
                    break;
                case (GameState.Win):
                    break;
                case (GameState.Menu): //menu meaning the start menu
                        startScreen = Content.Load<Texture2D>("Start Menu.png");
                        break;
                case (GameState.Gameplay):
                    break;
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MonoGameOrange);

            spriteBatch.Begin();

            switch (state)
            {
                case (GameState.Intro):
                    spriteBatch.Draw(lasrLogo, new Vector2(center.X - (lasrLogo.Bounds.Width / 2) + 35, center.Y - (lasrLogo.Bounds.Height / 2) - 100), Color.White);
                    break;
                case (GameState.Transition):
                    break;
                case (GameState.Win):
                    break;
                case (GameState.Menu):
                    //spriteBatch.Draw(startScreen, new Vector2(center.X - ()))
                    break;
                case (GameState.Gameplay):
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
