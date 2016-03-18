using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
        float GRAVITY = 1f;
        float PLAYER_WALKING_SPEED = 3f;
        float PLAYER_RUNNING_SPEED = 5f;
        float PLAYER_FRICTION = 0.9f;
        float PLAYER_ACCELERATION = 0.25f;
        float PLAYER_JUMP_FORCE = 16f;

        // Attributes:
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState state;
        Vector2 center;
        Vector2 screenSize;
        Vector2 playerPosition;
        double dt;
        double lastFrameTime;
        double currentFrameTime;
        bool paused = false;
        int currentLevelFrame;
        int currentLevel;

        // User input:
        KeyboardState kbState;
        KeyboardState oldKbState;
        MouseState mouseState;

        // Texture2Ds:
        Texture2D lasrLogo;
        Texture2D startScreen;
        Texture2D groundSprite;
        Texture2D cursorSprite;

        // Player:
        Texture2D playerSprite;
        Vector2 playerVelocity;
        bool onGround;

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
            playerPosition = new Vector2(screenSize.X / 2, screenSize.Y - 200);
            onGround = true;
            frameBackgrounds = new Texture2D[5];
            currentLevelFrame = 0;
            currentLevel = 0;

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
            playerSprite = Content.Load<Texture2D>("Player");
            cursorSprite = Content.Load<Texture2D>("Crosshair");
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            // Get delta time:
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
                    ControlPlayer();

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
                    spriteBatch.Draw(frameBackgrounds[currentLevelFrame], new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
                    // Draw the ground:
                    //spriteBatch.Draw(groundSprite, new Vector2(0, screenSize.Y - 200), Color.White);
                    // Draw the player:
                    spriteBatch.Draw(playerSprite, playerPosition - new Vector2(playerSprite.Width / 2, playerSprite.Height), Color.White);
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

        public void ControlPlayer ()
        {
            // Friction for horizontal movement:
            if (!kbState.IsKeyDown(Keys.Left) && !kbState.IsKeyDown(Keys.A) && !kbState.IsKeyDown(Keys.Right) && !kbState.IsKeyDown(Keys.D) && onGround)
            {
                playerVelocity.X *= PLAYER_FRICTION;
            }

            // Acceleration for horizontal movement:
            if (kbState.IsKeyDown(Keys.Left) || kbState.IsKeyDown(Keys.A))
            {
                playerVelocity.X -= PLAYER_ACCELERATION;
                if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                {
                    if (playerVelocity.X < -PLAYER_RUNNING_SPEED)
                    {
                        playerVelocity.X = -PLAYER_RUNNING_SPEED;
                    }
                }
                else
                {
                    if (playerVelocity.X < -PLAYER_WALKING_SPEED)
                    {
                        playerVelocity.X = -PLAYER_WALKING_SPEED;
                    }
                }
            }
            if (kbState.IsKeyDown(Keys.Right) || kbState.IsKeyDown(Keys.D))
            {
                playerVelocity.X += PLAYER_ACCELERATION;
                if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                {
                    if (playerVelocity.X > PLAYER_RUNNING_SPEED)
                    {
                        playerVelocity.X = PLAYER_RUNNING_SPEED;
                    }
                }
                else
                {
                    if (playerVelocity.X > PLAYER_WALKING_SPEED)
                    {
                        playerVelocity.X = PLAYER_WALKING_SPEED;
                    }
                }
            }

            // Check if on ground:
            onGround = playerPosition.Y >= screenSize.Y - 200;

            // Jump:
            if (onGround && kbState.IsKeyDown(Keys.Space))// KeyPress(Keys.Space))
            {
                playerVelocity.Y = -PLAYER_JUMP_FORCE;
            }

            // Gravity:
            if (!onGround)
            {
                playerVelocity.Y += GRAVITY;
            }

            // Move player:
            playerPosition += playerVelocity;

            // Keep on-screen:
            if (playerPosition.X <= playerSprite.Width / 2)
            {
                playerPosition.X = playerSprite.Width / 2;
                playerVelocity.X = 0;
            }
            if (playerPosition.X >= screenSize.X - (playerSprite.Width / 2))
            {
                playerPosition.X = screenSize.X - (playerSprite.Width / 2);
                playerVelocity.X = 0;
            }

            // Keep on ground:
            if (playerPosition.Y > screenSize.Y - 200)
            {
                playerPosition.Y = screenSize.Y - 200;
                playerVelocity.Y = 0;
                onGround = true;
            }
        }

        public void TogglePause ()
        {
            paused = !paused;
        }

        public void StartGame()
        {
            // Set variables:
            currentLevel = 0;
            currentLevelFrame = 0;

            // Reset player:
            playerPosition = new Vector2(screenSize.X / 2, screenSize.Y - 200);
            playerVelocity = Vector2.Zero;
            onGround = true;

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
            currentLevelFrame = 0;

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
            if (currentLevelFrame >= 4)
            {
                NextLevel();
                return;
            }

            // Increase frame:
            currentLevelFrame++;

            // Animate (move background and stuff left)
        }

        public void WinGame ()
        {
            // Change to the win animation state:
            state = GameState.Win;
        }
    }
}
