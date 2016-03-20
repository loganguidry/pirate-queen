using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

/*
 * Team LASR
 * Logan Guidry, Ron Dodge, Andrew Harding, Siddie Schrock
 *
 * Milestone 2:
 * Logan Guidry, Andrew Harding
*/

namespace PirateQueen
{
    // Finite state machine:
    enum GameState
    {
        Intro, Menu, Gameplay, Transition, Win
    }

    public class Game1 : Game
    {
        // Constants:
        static public float GRAVITY = 1f;
        static public float PLAYER_WALKING_SPEED = 3f;
        static public float PLAYER_RUNNING_SPEED = 5f;
        static public float PLAYER_FRICTION = 0.9f;
        static public float PLAYER_ACCELERATION = 0.25f;
        static public float PLAYER_JUMP_FORCE = 16f;

        // Public static variables:
        static public int currentLevel;
        static public int currentLevelStage;
        static public Vector2 screenSize;
        static public Vector2 center;
        static public float groundPosition;

        // Attributes:
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState state;
        double dt;
        double lastFrameTime;
        double currentFrameTime;
        bool paused = false;
        Player player;

        // User input:
        KeyboardState kbState;
        KeyboardState oldKbState;
        MouseState mouseState;

        // Texture2Ds:
        Texture2D lasrLogo;
        Texture2D startScreen;
        Texture2D groundSprite;
        Texture2D cursorSprite;

        // Frames:
        Texture2D[] frameBackgrounds;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
        }
        
        protected override void Initialize()
        {
            // Initialize variables:
            state = GameState.Intro;
            screenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            center = screenSize / 2;
            groundPosition = screenSize.Y - 250;
            frameBackgrounds = new Texture2D[5];
            currentLevel = 0;
            currentLevelStage = 0;

            // Create player:
            player = new Player(Content.Load<Texture2D>("Player"), new Vector2(screenSize.X / 2, groundPosition));

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load sprites:
            lasrLogo = Content.Load<Texture2D>("LASR");
            startScreen = Content.Load<Texture2D>("Start Menu");
            groundSprite = Content.Load<Texture2D>("Floor1");
            cursorSprite = Content.Load<Texture2D>("Crosshair");
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            // Get delta time for smooth movement:
            lastFrameTime = currentFrameTime;
            currentFrameTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            dt = 1 - ((currentFrameTime - lastFrameTime) / (1 / 60.0));

            // Get keyboard input:
            oldKbState = kbState;
            kbState = Keyboard.GetState();

            // Get mouse input:
            mouseState = Mouse.GetState();

            switch (state)
            {
                case (GameState.Intro):
                    // Move to the main menu:
                    if (KeyPress(Keys.Enter))
                        state = GameState.Menu;
                    break;

                case (GameState.Transition):
                    break;

                case (GameState.Win):
                    if (KeyPress(Keys.Enter))
                        state = GameState.Menu;
                    break;

                case (GameState.Menu):
                    // Start game:
                    if (KeyPress(Keys.Enter))
                        StartGame();
                    break;

                case (GameState.Gameplay):
                    // Toggle pause:
                    if (KeyPress(Keys.Escape))
                        TogglePause();

                    // Skip the rest of this code if paused:
                    if (paused)
                        break;

                    // Control the player:
                    player.Move(kbState);

                    // Debug: Move on to the next frame:
                    if (KeyPress(Keys.E))
                        NextFrame();

                    break;
            }

            // Update the debugging text:
            //debugText = dt.ToString();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(123, 213, 255));

            spriteBatch.Begin();

            switch (state)
            {
                case (GameState.Intro):
                    GraphicsDevice.Clear(Color.MonoGameOrange);
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
                    // Draw the background:
                    spriteBatch.Draw(frameBackgrounds[currentLevelStage], new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
                    // Draw the ground:
                    //spriteBatch.Draw(groundSprite, new Vector2(0, groundPosition), Color.White);
                    // Draw the player:
                    spriteBatch.Draw(player.sprite, player.position - new Vector2(player.sprite.Width / 2, player.sprite.Height), Color.White);
                    break;
            }

            // Draw the cursor:
            spriteBatch.Draw(cursorSprite, new Vector2(mouseState.Position.X, mouseState.Position.Y) - (screenSize / 2) - new Vector2(cursorSprite.Width / 2f, cursorSprite.Height / 2f), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public bool KeyPress (Keys key)
        {
            return kbState.IsKeyDown(key) && !oldKbState.IsKeyDown(key);
        }

        public void TogglePause ()
        {
            paused = !paused;
        }

        public void StartGame()
        {
            // Set variables:
            currentLevel = 0;
            currentLevelStage = 0;

            // Reset player:
            player.Reset();

            // Start level 1:
            NextLevel();

            // Change to gameplay:
            state = GameState.Gameplay;
        }

        public void NextLevel ()
        {
            // Check if the player is on the last level:
            if (currentLevel >= 5)
            {
                WinGame();
                return;
            }

            // Increase level:
            currentLevel++;
            currentLevelStage = 0;

            try
            {
                // Load level backgrounds:
                frameBackgrounds[0] = Content.Load<Texture2D>("Level" + currentLevel + "/BG1");
                frameBackgrounds[1] = Content.Load<Texture2D>("Level" + currentLevel + "/BG2");
                frameBackgrounds[2] = Content.Load<Texture2D>("Level" + currentLevel + "/BG3");
                frameBackgrounds[3] = Content.Load<Texture2D>("Level" + currentLevel + "/BG4");
                frameBackgrounds[4] = Content.Load<Texture2D>("Level" + currentLevel + "/BG5");
            }
            catch
            {
                // If the backgrounds failed to load, set them to the default background:
                frameBackgrounds[0] = Content.Load<Texture2D>("Background");
                frameBackgrounds[1] = Content.Load<Texture2D>("Background");
                frameBackgrounds[2] = Content.Load<Texture2D>("Background");
                frameBackgrounds[3] = Content.Load<Texture2D>("Background");
                frameBackgrounds[4] = Content.Load<Texture2D>("Background");
            }
        }

        public void NextFrame ()
        {
            // Check if the player is on the last frame:
            if (currentLevelStage >= 4)
            {
                NextLevel();
                return;
            }

            // Increase frame:
            currentLevelStage++;

            // Animate (move background and stuff left)
        }

        public void WinGame ()
        {
            // Change to the win animation state:
            state = GameState.Win;
        }
    }
}
