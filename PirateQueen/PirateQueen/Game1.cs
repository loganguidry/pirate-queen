﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

/*
 * Team LASR
 * Logan Guidry, Ron Dodge, Andrew Harding, Siddie Schrock
 *
 * Controls:
 *  WASD/Arrows - Move
 *  Space - Jump
 *  E - Advance to next 'stage' (debugging)
 *  Enter - Advance to next screen (intro -> main menu -> game)
*/

namespace PirateQueen
{
    // Finite state machine:
    enum GameState
    {
        Intro, Menu, Gameplay, Transition, Win
    }
    //ButtonState:
    enum ButtonState
    {
        Hover, Click
    }

    public class Game1 : Game
    {
        // Debug mode:
        static public bool Debugging = true;

        // Constants:
        static public float GRAVITY = 1f;
        static public float PLAYER_WALKING_SPEED = 3f;
        static public float PLAYER_RUNNING_SPEED = 5f;
        static public float PLAYER_FRICTION = 0.8f;
        static public float PLAYER_ACCELERATION = 1f;
        static public float PLAYER_JUMP_FORCE = 16f;

        // Public static variables:
        static public int currentLevel;
        static public int currentLevelStage;
        static public Vector2 screenSize;
        static public Vector2 center;
        static public float groundPosition;
        static public double dt;

        //Constants(ButtonState):
        const int NUM_OF_BUTTONS = 2,
            PLAY_BUTTON = 0,
            SETTINGS_BUTTON = 1,
            PLAY_HEIGHT = 100,
            PLAY_WIDTH = 250,
            SETTINGS_HEIGHT = 85,
            SETTINGS_WIDTH = 200;

        // Attributes:
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState state;
        double lastFrameTime;
        double currentFrameTime;
        bool paused = false;
        Player player;
        SpriteFont debugFont;
        List<Enemy> Enemies;
        float leftFrameBackgroundPosition;
        float leftFrameBackgroundPositionTarget;
        float rightFrameBackgroundPosition;
        float rightFrameBackgroundPositionTarget;

        // User input:
        KeyboardState kbState;
        KeyboardState oldKbState;
        MouseState mouseState;

        // Texture2Ds:
        Texture2D lasrLogo;
        //Texture2D menuBackgroundSprite;
        //Texture2D menuPlayButtonSprite;
        //Texture2D menuHeaderSprite;
        Texture2D startScreen;
        Texture2D cursorSprite;
        Texture2D vignetteSprite;
        Texture2D playButton;
        Texture2D settingsButton;


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
            Enemies = new List<Enemy>();
            leftFrameBackgroundPosition = 0f;
            leftFrameBackgroundPositionTarget = leftFrameBackgroundPosition;
            rightFrameBackgroundPosition = screenSize.X;
            rightFrameBackgroundPositionTarget = rightFrameBackgroundPosition;

            // Create player:
            player = new Player(
                Content.Load<Texture2D>("Player"),
                Content.Load<Texture2D>("Animations/Walk"),
                new Vector2(screenSize.X / 2, groundPosition)
            );
            player.debugSprite = Content.Load<Texture2D>("Debug_5x5");

            // Load data:
            Saving.LoadData();

            IsMouseVisible = true;
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load fonts:
            debugFont = Content.Load<SpriteFont>("Arial");

            // Load sprites:
            lasrLogo = Content.Load<Texture2D>("Intro");
            //menuBackgroundSprite = Content.Load<Texture2D>("MainMenuBackground");
            //menuPlayButtonSprite = Content.Load<Texture2D>("PlayButton");
            //menuHeaderSprite = Content.Load<Texture2D>("PirateQueenHeader");
            startScreen = Content.Load<Texture2D>("ActualStartScreen");
            cursorSprite = Content.Load<Texture2D>("Crosshair");
            vignetteSprite = Content.Load<Texture2D>("Vignette");
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
                    IsMouseVisible = false;
                    if (KeyPress(Keys.Escape))
                        TogglePause();

                    // Skip the rest of this code if paused:
                    if (paused)
                        break;

                    // Control the player:
                    player.Move(kbState);
                    player.Attack(kbState);
                    player.Animate(gameTime);

                    // Enemy AI:
                    foreach (Enemy enemy in Enemies)
                    {
                        enemy.Move(kbState);
                        enemy.Attack(kbState);
                        enemy.Animate(gameTime);
                    }

                    // Animate backgrounds:
                    leftFrameBackgroundPosition += (leftFrameBackgroundPositionTarget - leftFrameBackgroundPosition) * 0.05f;
                    rightFrameBackgroundPosition += (rightFrameBackgroundPositionTarget - rightFrameBackgroundPosition) * 0.05f;

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
                    // Draw logo:
                    spriteBatch.Draw(lasrLogo, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
                    // Draw vignette:
                    spriteBatch.Draw(vignetteSprite, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
                    break;

                case (GameState.Transition):
                    break;

                case (GameState.Win):
                    break;

                case (GameState.Menu):
                    /*
                    // Draw background:
                    spriteBatch.Draw(menuBackgroundSprite, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
                    // Draw play button:
                    spriteBatch.Draw(menuPlayButtonSprite, new Rectangle((int)((screenSize.X / 2) - (menuPlayButtonSprite.Width / 2)), (int)((screenSize.Y / 2) - (menuPlayButtonSprite.Height / 2)), menuPlayButtonSprite.Width, menuPlayButtonSprite.Height), Color.White);
                    // Draw header:
                    spriteBatch.Draw(menuHeaderSprite, new Rectangle((int)((screenSize.X / 2) - (menuHeaderSprite.Width / 2)), (int)((screenSize.Y / 4) - (menuHeaderSprite.Height / 2)), menuHeaderSprite.Width, menuHeaderSprite.Height), Color.White);
                    */
                    //Draw Start Screen Menu:
                    spriteBatch.Draw(startScreen, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);

                    // Draw vignette:
                    spriteBatch.Draw(vignetteSprite, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
                    break;

                case (GameState.Gameplay):
                    // Draw background:
                    if (currentLevelStage > 0)
                    {
                        spriteBatch.Draw(frameBackgrounds[currentLevelStage - 1], new Rectangle((int)leftFrameBackgroundPosition, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
                        spriteBatch.Draw(frameBackgrounds[currentLevelStage], new Rectangle((int)rightFrameBackgroundPosition, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
                    }
                    else
                        spriteBatch.Draw(frameBackgrounds[currentLevelStage], new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
                    // Draw player hitbox:
                    if (Debugging)
                        spriteBatch.Draw(player.sprite, player.position - new Vector2(player.sprite.Width / 2, player.sprite.Height), Color.White);
                    // Draw player:
                    player.Draw(
                        spriteBatch,
                        player.position - new Vector2(player.sprite.Width + 5, player.sprite.Height + 30)
                    );
                    // Draw enemies:
                    foreach (Enemy enemy in Enemies)
                        enemy.Draw(spriteBatch, enemy.position - new Vector2(player.sprite.Width + 5, player.sprite.Height + 30));
                    break;
            }
            
            // Draw the cursor:
            spriteBatch.Draw(cursorSprite, new Vector2(mouseState.Position.X, mouseState.Position.Y) - new Vector2(cursorSprite.Width / 2f, cursorSprite.Height / 2f), Color.White);

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
            Enemies.Clear();

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

            // Animate (move background and stuff left):
            leftFrameBackgroundPosition = 0;
            leftFrameBackgroundPositionTarget = -screenSize.X;
            rightFrameBackgroundPosition = screenSize.X;
            rightFrameBackgroundPositionTarget = 0;

            // Spawn enemies:
            for (int i = 0; i <= 5; i++)
            {
                Enemy enemy = new Enemy(
                    Content.Load<Texture2D>("Player"),
                    Content.Load<Texture2D>("Animations/Walk"),
                    new Vector2(screenSize.X - 100, groundPosition)
                );
                Enemies.Add(enemy);
            }
        }

        public void WinGame ()
        {
            // Change to the win animation state:
            state = GameState.Win;
        }
    }
}
