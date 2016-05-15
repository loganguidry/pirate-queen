using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Media;

/*
 * Team LASR
 * Logan Guidry, Ron Dodge, Andrew Harding, Siddie Schrock
 *
 * Controls:
 *  WASD - Move
 *  Shift - Sprint
 *  Space - Jump
 *  Left Click - Swing Cutlass Sword
 *  Right Click - Fire Flintlock Pistol
 *  E - Advance to next 'stage' (debugging mode only)
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
		static public bool Debugging = false;

		// Constants:
		static public float GRAVITY = 1f;
		static public float PLAYER_WALKING_SPEED = 6f;
		static public float PLAYER_RUNNING_SPEED = 10f;
		static public float PLAYER_FRICTION = 0.8f;
		static public float PLAYER_ACCELERATION = 1f;
		static public float PLAYER_JUMP_FORCE = 16f;
		static public double ENEMY_SPAWN_DELAY = 2000;

		// Public static content:
		static public Texture2D white2x2square;
		static public Texture2D healthBarSprite;
		static public Texture2D healthPickupSprite;
		static public SpriteFont basicFont;
		static public SpriteFont uiFont;
		static public SpriteFont uiFontLarge;
		static public SpriteFont uiFontPopup;
		static public Song bgMusic;

		// Public static variables:
		static public int currentLevel;
		static public int currentLevelStage;
		static public Vector2 screenSize;
		static public Vector2 center;
		static public float groundPosition;
		static public double currentFrameTime;
		static public Player player;
		static public List<Enemy> Enemies;
		static public List<DamagePopup> DamagePopups;
		static public int displayHealth;

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
		double pauseStartTime;

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
		Texture2D winScreen;
		Texture2D lossScreen;
		Texture2D enemy;

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
			enemySpawnDelay = ENEMY_SPAWN_DELAY;
			ui = new UI();
			spawnedEnemies = 0;
			DamagePopups = new List<DamagePopup>();
			pickups = new List<HealthPickup>();
			currentFrameTime = 0;

			// Create player:
			player = new Player(
				Content.Load<Texture2D>("Player"),
				Content.Load<Texture2D>("Animations/Player/PlayerAnims"),
				new Vector2(screenSize.X / 2, groundPosition)
			);
			displayHealth = player.health;

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
			uiFont = Content.Load<SpriteFont>("Vani_24");
			uiFontLarge = Content.Load<SpriteFont>("Vani_48");
			uiFontPopup = Content.Load<SpriteFont>("SimSun");

			// Load Music:
			bgMusic = Content.Load<Song>("piratemusic");

			// Load sprites:
			lasrLogo = Content.Load<Texture2D>("Intro");
			startScreen = Content.Load<Texture2D>("ActualStartScreen2");
			cursorSprite = Content.Load<Texture2D>("cursorGauntlet_blue");
			vignetteSprite = Content.Load<Texture2D>("Vignette");
			white2x2square = Content.Load<Texture2D>("White");
			healthBarSprite = Content.Load<Texture2D>("HealthBar");
			healthPickupSprite = Content.Load<Texture2D>("hp");
			winScreen = Content.Load<Texture2D>("NewWinScreen");
			lossScreen = Content.Load<Texture2D>("LossScreen");
			//enemy = Content.Load<Texture2D>();
		}

		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Environment.Exit(0);

			// Get time:
			currentFrameTime += gameTime.ElapsedGameTime.TotalMilliseconds;

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
					{
						StartGame();
						MediaPlayer.Play(bgMusic);
                        MediaPlayer.IsRepeating = true;
					}
					break;

				case (GameState.Gameplay):
					IsMouseVisible = false;

					// Toggle pause:
					if (KeyPress(Keys.P))
						TogglePause();

					// Skip the rest of this code if paused:
					if (paused)
						break;

					// Control the player:
					player.Move(kbState, mCurrState);
					player.Animate(gameTime, mCurrState);

					// Move health pickups:
					List<HealthPickup> deadPickups = new List<HealthPickup>();
					foreach (HealthPickup pickup in pickups)
					{
						pickup.Move();
						if (pickup.Touching())
						{
							deadPickups.Add(pickup);
							player.health += 200;
							if (player.health > 1000)
								player.health = 1000;
						}
					}
					foreach (HealthPickup pickup in deadPickups)
						pickups.Remove(pickup);
					deadPickups.Clear();

					// Move bullets:
					Bullet.MoveBullets();

					// Move health bar:
					displayHealth += (int)(Math.Round((player.health - displayHealth) * 0.1f));

					// Spawn new enemies:
					if (currentLevelStage <= 3)
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
					List<DamagePopup> deadDamageIndicators = new List<DamagePopup>();
					foreach (DamagePopup popup in DamagePopups)
					{
						if (popup.Move())
							deadDamageIndicators.Add(popup);
					}
					foreach (DamagePopup popup in deadDamageIndicators)
						DamagePopups.Remove(popup);
					deadDamageIndicators.Clear();

					// Check if the player is dead:
					if (player.health <= 0)
						state = GameState.Lose;

					// Check if enemies are dead:
					List<Enemy> deadEnemies = new List<Enemy>();
					foreach (Enemy enemy in Enemies)
					{
						if (enemy is Boss)
						{
							Boss boss = (Boss)enemy;
							if (boss.health <= 0)
								deadEnemies.Add(enemy);
						}
						else
						{
							if (enemy.health <= 0)
								deadEnemies.Add(enemy);
						}
					}
					foreach (Enemy enemy in deadEnemies)
					{
						if (rgen.Next(10) == 1)
							pickups.Add(new HealthPickup(enemy.position - new Vector2(0, enemy.debugSprite.Height / 2)));
						Enemies.Remove(enemy);
					}
					deadEnemies.Clear();

					// All enemies killed:
					if (Enemies.Count == 0 && (spawnedEnemies == stageEnemies || currentLevelStage == 4))
						NextFrame();

					// Debug: Move on to the next frame:
					if (KeyPress(Keys.E) && Debugging)
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
					// Draw win screen:
					spriteBatch.Draw(winScreen, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
					break;

				case (GameState.Lose):
					// Draw loss screen:
					spriteBatch.Draw(lossScreen, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);
					break;

				case (GameState.Menu):
					//Draw Start Screen Menu:
					spriteBatch.Draw(startScreen, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);

					// Draw vignette:
					spriteBatch.Draw(vignetteSprite, new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y), Color.White);

					// Creating the button rectangles
					playButton = new Rectangle(325, 275, 675, 175);
					settingsButton = new Rectangle(650, 725, 450, 125);
					Point mousePosition = mCurrState.Position;
					// Is a button being clicked
					if (playButton.Contains(mousePosition))
					{
						if (mCurrState.LeftButton == ButtonState.Pressed &&
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
					// Draw bullets:
					Bullet.DrawBullets(spriteBatch);
					// Draw damage indicators:
					foreach (DamagePopup popup in DamagePopups)
						spriteBatch.DrawString(uiFontPopup, popup.text, popup.position, new Color(0f, 0f, 0f, popup.transparency));
					// Draw UI:
					ui.Draw(spriteBatch);
					break;
			}

			// - new Vector2(cursorSprite.Width / 2f, cursorSprite.Height / 2f)
			// Draw the cursor:
			spriteBatch.Draw(cursorSprite, new Vector2(mCurrState.X + 180, mCurrState.Y + 130), Color.White);

			// Debugging information:
			/*
            if (Debugging)
                spriteBatch.DrawString(basicFont,
                    "Player health: " + (player.health / (double)Player.MAX_HEALTH * 100).ToString() + "% [" + player.health + "/" + Player.MAX_HEALTH + "]\n" +
                    "Current time: " + currentFrameTime + "\n" +
                    "Level " + currentLevel + ", stage " + currentLevelStage + "\n" +
                    "State: " + state.ToString() + "\n" +
                    "#Bullets: " + Bullet.globalBullets.Count.ToString() + "\n" +
                    "#Enemies: " + Enemies.Count.ToString() + "\n" +
                    "#DamagePopups: " + DamagePopups.Count.ToString() + "\n" +
                    "#HealthPickups: " + pickups.Count.ToString(),
                    Vector2.Zero, Color.Green);
            */

			spriteBatch.End();

			base.Draw(gameTime);
		}

		public bool KeyPress(Keys key)
		{
			return kbState.IsKeyDown(key) && !oldKbState.IsKeyDown(key);
		}

		public void TogglePause()
		{
			if (paused)
			{
				paused = false;
				currentFrameTime -= (currentFrameTime - pauseStartTime);
			}
			else
			{
				paused = true;
				pauseStartTime = currentFrameTime;
			}
		}

		public void StartGame()
		{
			// Set variables:
			currentLevel = 0;
			currentLevelStage = 0;
			Enemies.Clear();
			DamagePopups.Clear();
			Bullet.globalBullets.Clear();
			spawnedEnemies = 0;
			displayHealth = 1000;

			// Reset player:
			player.Reset();

			// Start level 1:
			NextLevel();

			// Change to gameplay:
			state = GameState.Gameplay;
		}

		public void NextLevel()
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

		public void NextFrame()
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

			// Spawn boss:
			if (currentLevelStage == 4)
				SpawnBoss();
		}

		public void WinGame()
		{
			// Change to the win animation state:
			state = GameState.Win;
		}

		public void SpawnEnemy()
		{
			if (currentFrameTime - lastEnemySpawnTime >= enemySpawnDelay && spawnedEnemies < stageEnemies)
			{
				spawnedEnemies++;
				lastEnemySpawnTime = currentFrameTime;
				Enemy newEnemy = new Enemy(
						Content.Load<Texture2D>("Player"),
						Content.Load<Texture2D>("Animations/NormalEnemy/BadGuys"),
						new Vector2(screenSize.X + 100, groundPosition),
						rgen.Next(0, 99999), "normal"
					);
				Enemies.Add(newEnemy);
			}
		}

		public void SpawnBoss()
		{
			// Create a boss above the screen:
			Boss newEnemy = new Boss(
						Content.Load<Texture2D>("BossDebug"),
						Content.Load<Texture2D>("Animations/Boss/blackbeard"),
						new Vector2(screenSize.X / 2f, 0),
						rgen.Next(0, 99999)
					);
			Enemies.Add(newEnemy);
		}
	}
}
