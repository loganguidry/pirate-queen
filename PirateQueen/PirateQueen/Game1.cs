using Microsoft.Xna.Framework;
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
        Intro, Menu, Gameplay, Transition, Win, Lose
    }

    //ButtonState:
    /*enum ButtonState
    {
        Hover, Click
    }*/

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

        // Public static content:
        static public Texture2D white2x2square;
        static public Texture2D healthBarSprite;
        static public Texture2D healthPickupSprite;
        static public SpriteFont basicFont;

        // Public static variables:
        static public int currentLevel;
        static public int currentLevelStage;
        static public Vector2 screenSize;
        static public Vector2 center;
        static public float groundPosition;
        static public double dt;
        static public double currentFrameTime;
        static public Player player;
        static public List<Enemy> Enemies;
        static public List<DamagePopup> DamagePopups;

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
        bool paused = false;
        float leftFrameBackgroundPosition;
        float leftFrameBackgroundPositionTarget;
        float rightFrameBackgroundPosition;
        float rightFrameBackgroundPositionTarget;
        Random rgen;
        double lastEnemySpawnTime;
        double enemySpawnDelay;
        int stageEnemies;
        int spawnedEnemies;
        UI ui;
        Rectangle playButton;
        Rectangle settingsButton;
        List<HealthPickup> pickups;

        // User input:
        KeyboardState kbState;
        KeyboardState oldKbState;
        MouseState mCurrState;
        MouseState mPrevState;

        // Texture2Ds:
        Texture2D lasrLogo;
        Texture2D startScreen;
        Texture2D cursorSprite;
        Texture2D vignetteSprite;

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
            rgen = new Random();
            enemySpawnDelay = 2;
            ui = new UI();
            spawnedEnemies = 0;
            DamagePopups = new List<DamagePopup>();
            pickups = new List<HealthPickup>();

            // Create player:
            player = new Player(
                Content.Load<Texture2D>("Player"),
                Content.Load<Texture2D>("Animations/Player/Walk"),
                Content.Load<Texture2D>("Animations/Player/Walk"),
                Content.Load<Texture2D>("Animations/Player/Walk"),
                Content.Load<Texture2D>("Animations/Player/WalkSwing"),
                Content.Load<Texture2D>("Animations/Player/WalkSwing"),
                new Vector2(screenSize.X / 2, groundPosition)
            );

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
            basicFont = Content.Load<SpriteFont>("Arial");

            // Load sprites:
            lasrLogo = Content.Load<Texture2D>("Intro");
            startScreen = Content.Load<Texture2D>("ActualStartScreen");
            cursorSprite = Content.Load<Texture2D>("Crosshair");
            vignetteSprite = Content.Load<Texture2D>("Vignette");
            white2x2square = Content.Load<Texture2D>("White");
            healthBarSprite = Content.Load<Texture2D>("HealthBar");
            healthPickupSprite = Content.Load<Texture2D>("White");
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Get delta time for smooth movement:
            lastFrameTime = currentFrameTime;
            currentFrameTime = gameTime.TotalGameTime.TotalSeconds;
            dt = ((currentFrameTime - lastFrameTime) / (1 / 60.0));

            // Get keyboard input:
            oldKbState = kbState;
            kbState = Keyboard.GetState();

            // Get mouse input:
            mCurrState = Mouse.GetState();

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

                case (GameState.Lose):
                    if (KeyPress(Keys.Enter))
                        state = GameState.Menu;
                    break;

                case (GameState.Menu):
                    if (KeyPress(Keys.Enter))
                        StartGame();
                    break;

                case (GameState.Gameplay):
                    IsMouseVisible = false;

                    // Toggle pause:
                    if (KeyPress(Keys.P))
                        TogglePause();

                    // Skip the rest of this code if paused:
                    if (paused)
                        break;

                    // Move health pickups:
                    foreach (HealthPickup pickup in pickups)
                        pickup.Move();

                    // Control the player:
                    player.Move(kbState);
                    player.Animate(gameTime);

                    // Spawn new enemies:
                    SpawnEnemy();

                    // Enemy AI:
                    foreach (Enemy enemy in Enemies)
                    {
                        enemy.Move(kbState);
                        enemy.Attack();
                        enemy.Animate(gameTime);
                    }

                    // Animate backgrounds:
                    leftFrameBackgroundPosition += (leftFrameBackgroundPositionTarget - leftFrameBackgroundPosition) * 0.2f;
                    rightFrameBackgroundPosition += (rightFrameBackgroundPositionTarget - rightFrameBackgroundPosition) * 0.2f;

                    // Animate damage indicators:
                    foreach (DamagePopup popup in DamagePopups)
                    {
                        popup.Move();
                    }

                    // Check if the player is dead:
                    if (player.health <= 0)
                        state = GameState.Lose;

                    // Check if enemies are dead:
                    List<Enemy> deadEnemies = new List<Enemy>();
                    foreach (Enemy enemy in Enemies)
                    {
                        if (enemy.health <= 0)
                            deadEnemies.Add(enemy);
                    }
                    foreach (Enemy enemy in deadEnemies)
                    {
                        pickups.Add(new HealthPickup(enemy.position));
                        Enemies.Remove(enemy);
                    }
                    deadEnemies.Clear();

                    // All enemies killed:
                    if (Enemies.Count == 0 && spawnedEnemies == stageEnemies)
                        NextFrame();

                    // Debug: Move on to the next frame:
                    if (KeyPress(Keys.E))
                        NextFrame();

                    break;
            }

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
                    //Draw Start Screen Menu:
                    spriteBatch.Draw(startScreen, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);

                    // Draw vignette:
                    spriteBatch.Draw(vignetteSprite, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);

                    //Creating the button rectangles
                    playButton = new Rectangle(325, 275, 675, 175);
                    settingsButton = new Rectangle(650, 725, 450, 125);
                    Point mousePosition = mCurrState.Position;
                    //Is a button being clicked
                    if(playButton.Contains(mousePosition))
                    {
                        if(mCurrState.LeftButton == ButtonState.Pressed && 
                            mPrevState.LeftButton == ButtonState.Released)
                        {
                            StartGame();
                        }
                        mPrevState = mCurrState;
                    }

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
                    // Draw health pickups:
                    foreach (HealthPickup pickup in pickups)
                        pickup.Draw(spriteBatch);
                    // Draw player:
                    player.Draw(
                        spriteBatch,
                        player.position - new Vector2(player.debugSprite.Width + 5, player.debugSprite.Height + 30)
                    );
                    // Draw enemies:
                    foreach (Enemy enemy in Enemies)
                        enemy.Draw(spriteBatch, enemy.position - new Vector2(player.debugSprite.Width + 5, player.debugSprite.Height + 30));
                    // Draw damage indicators:
                    foreach (DamagePopup popup in DamagePopups)
                        spriteBatch.DrawString(basicFont, popup.text, popup.position, new Color(0f, 0f, 0f, popup.transparency));
                    // Draw UI:
                    ui.Draw(spriteBatch);
                    break;
            }
            
            // Draw the cursor:
            spriteBatch.Draw(cursorSprite, new Vector2(mCurrState.Position.X, mCurrState.Position.Y) - new Vector2(cursorSprite.Width / 2f, cursorSprite.Height / 2f), Color.White);

            // Debugging information:
            if (Debugging)
                spriteBatch.DrawString(basicFont,
                    "Player health: " + (player.health / (double)Player.MAX_HEALTH * 100).ToString() + "% [" + player.health + "/" + Player.MAX_HEALTH + "]\n" +
                    "Current time: " + currentFrameTime + "\n" +
                    "Level " + currentLevel + ", stage " + currentLevelStage + "\n" +
                    "State: " + state.ToString(),
                    Vector2.Zero, Color.Green);

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
            spawnedEnemies = 0;

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

            // Spawn enemies:
            lastEnemySpawnTime = currentFrameTime;
            stageEnemies = 5;
            spawnedEnemies = 0;
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
            lastEnemySpawnTime = currentFrameTime;
            stageEnemies = 5;
            spawnedEnemies = 0;
        }

        public void WinGame ()
        {
            // Change to the win animation state:
            state = GameState.Win;
        }

        public void SpawnEnemy ()
        {
            if (currentFrameTime - lastEnemySpawnTime >= enemySpawnDelay && spawnedEnemies < stageEnemies)
            {
                spawnedEnemies++;
                lastEnemySpawnTime = currentFrameTime;
                Enemy newEnemy = new Enemy(
                        Content.Load<Texture2D>("Player"),
                        Content.Load<Texture2D>("Animations/NormalEnemy/Walk"),
                        new Vector2(screenSize.X + rgen.Next(0, (int)screenSize.X), groundPosition),
                        rgen.Next(0, 99999),
                        "normal"
                    );
                Enemies.Add(newEnemy);
            }
        }
    }
}
