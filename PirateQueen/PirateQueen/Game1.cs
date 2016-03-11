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
        Vector2 screenSize;

        // User input:
        KeyboardState kbState;
        KeyboardState oldKbState;

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
            // Initialize variables:
            state = GameState.Intro;
            screenSize = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
            center = screenSize / 2;

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load sprites:
            lasrLogo = Content.Load<Texture2D>("LASR");
            startScreen = Content.Load<Texture2D>("Start Menu");
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            // Get keyboard input:
            oldKbState = kbState;
            kbState = Keyboard.GetState();

            if (KeyPress(Keys.Escape))
                Exit();
            
            switch (state)
            {
                case (GameState.Intro):
                    if (KeyPress(Keys.Enter))
                        state = GameState.Menu;
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
                    spriteBatch.Draw(startScreen, new Vector2(center.X - (startScreen.Bounds.Width / 2), center.Y - (startScreen.Bounds.Height / 2)), Color.White);
                    break;
                case (GameState.Gameplay):
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public bool KeyPress (Keys key)
        {
            return kbState.IsKeyDown(key) && !oldKbState.IsKeyDown(key);
        }
    }
}
