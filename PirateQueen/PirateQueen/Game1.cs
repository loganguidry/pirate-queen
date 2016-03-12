﻿using Microsoft.Xna.Framework;
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
        float PLAYER_WALKING_SPEED = 2.5f;
        float PLAYER_RUNNING_SPEED = 5f;

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

        // User input:
        KeyboardState kbState;
        KeyboardState oldKbState;

        // Texture2Ds:
        Texture2D lasrLogo;
        Texture2D startScreen;
        Texture2D groundSprite;
        Texture2D background;

        // Player:
        Texture2D playerSprite;

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
            playerPosition = new Vector2(screenSize.X / 2, screenSize.Y - 200);

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load sprites:
            lasrLogo = Content.Load<Texture2D>("LASR");
            startScreen = Content.Load<Texture2D>("Start Menu");
            background = Content.Load<Texture2D>("Background");
            groundSprite = Content.Load<Texture2D>("Floor1");
            playerSprite = Content.Load<Texture2D>("Player");
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

            if (KeyPress(Keys.Escape))
                Exit();
            
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
                    break;

                case (GameState.Menu):
                    // Start game:
                    if (KeyPress(Keys.Enter))
                        state = GameState.Gameplay;
                    break;

                case (GameState.Gameplay):
                    // Toggle pause:
                    //if (KeyPress(Keys.Escape))
                    //    TogglePause();

                    // Movement:
                    if (kbState.IsKeyDown(Keys.Left) || kbState.IsKeyDown(Keys.A))
                    {
                        // Walk or run:
                        if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                            playerPosition.X -= PLAYER_RUNNING_SPEED;
                        else
                            playerPosition.X -= PLAYER_WALKING_SPEED;

                        // Keep in-screen:
                        if (playerPosition.X <= playerSprite.Width / 2)
                            playerPosition.X = playerSprite.Width / 2;
                    }
                    if (kbState.IsKeyDown(Keys.Right) || kbState.IsKeyDown(Keys.D))
                    {
                        // Walk or run:
                        if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
                            playerPosition.X += PLAYER_RUNNING_SPEED;
                        else
                            playerPosition.X += PLAYER_WALKING_SPEED;

                        // Keep in-screen:
                        if (playerPosition.X >= screenSize.X - (playerSprite.Width / 2))
                            playerPosition.X = screenSize.X - (playerSprite.Width / 2);
                    }

                    break;
            }

            // Update the debugging text:
            //debugText = dt.ToString();

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
                    // Draw the background:
                    spriteBatch.Draw(background, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
                    // Draw the ground:
                    spriteBatch.Draw(groundSprite, new Vector2(0, screenSize.Y - 200), Color.White);
                    // Draw the player:
                    spriteBatch.Draw(playerSprite, playerPosition - new Vector2(playerSprite.Width / 2, playerSprite.Height), Color.White);
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
