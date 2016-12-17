using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

/*
 * Joint Project 1: Sentinel (with arrays)
 * 
 * Name: Sebastian Kruzel
 * Student number: C00206244
 * Date Started: 02/01/2016
 * Known Bugs: [If the player is hit while at a border he gets knocked out of bounds.] [when the soldier follows up after speed increase possibility going back and forth.]
 * **********************Joint project with arrays******************************
 * Date started: 08/02/16
 * Time taken: [09:09 - 11:00] [12:00 - 14:00]
 * Improvements: Added an array of crystals.[game class]    Added an array of crystal positions[gatherer class]     Made many variables static/constant across all classes     
 *               renamed certain variables eg. sentinelSprite -> sentinel [because its an object not a sprite]
 */
namespace Sentinel
{
    public enum GameState {MainMenu, Game, GameOver, Win};
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GameState currentState = GameState.MainMenu;
        GraphicsDeviceManager graphics;
        Vector2 killCountDisplayPosition = new Vector2(400, 0);
        Vector2 lifeDisplayPosition = new Vector2(10, 0);
        Vector2 enemiesLeftDisplayPosition = new Vector2(400, 40);
        Vector2 scoreDisplayPosition = new Vector2(800, 0);
        Vector2 crystalPosition;
        SpriteBatch spriteBatch;
        Player sentinel;
        Bullet bullet;
        Crystal[] crystal = new Crystal[4];
        KrugGatherer gatherer;
        KrugSoldier soldier;
        SpriteFont font;
        Texture2D background;
        Texture2D menuScreen;
        Texture2D gameOverScreen;
        Texture2D victoryScreen;
        Rectangle mainFrame;
        Rectangle soldierRectangle;
        Rectangle sentinelRectangle;
        Rectangle bulletRectangle;
        SoundEffect playerHit;
        int enemiesLeft = 0;
        bool allCrystalsCollected = false;
        bool gathererHealthIncrease = false;
        bool soldierHealthIncrease = false;
        static int maxCrystalAmount = 4;
        int crystalsAmount = 4;
        static int windowHeight = 700;
        static int windowWidth = 1000;
        static int levelHeight = 220;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            crystalPosition = new Vector2(30, 220);
            allCrystalsCollected = false;
            enemiesLeft = 30;
            Random aRandGen = new Random();
            sentinel = new Player();//create the player object
            bullet = new Bullet();
            soldier = new KrugSoldier();
            for (int i = 0; i < maxCrystalAmount; i++)
            {
                crystal[i] = new Crystal(crystalPosition);
                crystalPosition.Y += 120;
                crystal[i].Active = true;
            }
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.ApplyChanges();
            windowWidth = graphics.GraphicsDevice.Viewport.Width;
            windowHeight = graphics.GraphicsDevice.Viewport.Height;
            gatherer = new KrugGatherer(windowWidth, windowHeight);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            background = Content.Load<Texture2D>("CaveBackground");
            menuScreen = Content.Load<Texture2D>("SentinelTitleScreen");
            gameOverScreen = Content.Load<Texture2D>("GameOverScreen");
            victoryScreen = Content.Load<Texture2D>("VictoryScreen");
            sentinel.LoadContent(this.Content, "Sentinel");
            gatherer.LoadContent(this.Content, "KrugGatherer");
            soldier.LoadContent(this.Content, "KrugSoldier");
            bullet.LoadContent(this.Content, "Bullet");
            for (int count = 0; count < maxCrystalAmount; count++)
            {
                crystal[count].LoadContent(this.Content, "Crystal");
            }
            playerHit = Content.Load<SoundEffect>("PlayerHurt");
            font = Content.Load<SpriteFont>("SpriteFont1");
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //The main menu state checking for key press
            if (currentState == GameState.MainMenu)
            {
                ChangeToGameCheck();
            }
            //The main game state
            if (currentState == GameState.Game)
            {
                //updating player
                sentinel.Update(this.Content, windowHeight, windowWidth);
                //updating gatherer enemy
                gatherer.Update(this.Content, windowHeight, windowWidth);
                //updating soldier enemy
                soldier.Update(this.Content, windowHeight, windowWidth);
                //updating bullet
                bullet.Update(this.Content, windowWidth, windowHeight);
                //updating crystals
                for (int count = 0; count < crystalsAmount; count++)
                {
                    crystal[count].Update(this.Content);
                }
                //soldier follows player
                Follow();
                UpdateBulletRectangle();
                UpdateBulletPosition();
                //checks all collisions
                Collisions();
                //a check if crystal is collected
                CrystalGatheringCheck();
                Progression();
                //if player reaches zero health game over screen
                if (sentinel.Life <= 0 || allCrystalsCollected == true)
                {
                    currentState = GameState.GameOver;
                }
                //if player kills all enemies win
                if (sentinel.KillCount == 30)
                {
                    currentState = GameState.Win;
                }
            }
            //if gameover screen check if user wishes to restart
            if (currentState == GameState.GameOver)
            {
                RestartGame();
            }
            //if player won check if want to restart
            if (currentState == GameState.Win)
            {
                RestartGame();
            }
            
            base.Update(gameTime);


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (currentState == GameState.MainMenu)
            {
                spriteBatch.Draw(menuScreen, mainFrame, Color.White);
            }
            else if (currentState == GameState.Game)
            {
                spriteBatch.Draw(background, mainFrame, Color.White);
                sentinel.Draw(this.spriteBatch);//the sprite draws itself here
                gatherer.Draw(this.spriteBatch);
                soldier.Draw(this.spriteBatch);
                bullet.Draw(this.spriteBatch);
                for (int count = 0; count < maxCrystalAmount; count++)
                   // for (int count = 0; count < crystalsAmount; count++)
                {
                    crystal[count].Draw(this.spriteBatch);
                }
                spriteBatch.DrawString(font, "Kill count: " + sentinel.KillCount, killCountDisplayPosition, Color.White);
                spriteBatch.DrawString(font, "Life: " + sentinel.Life + "%", lifeDisplayPosition, Color.White);
                spriteBatch.DrawString(font, "Score: " + sentinel.Score, scoreDisplayPosition, Color.White);
                spriteBatch.DrawString(font, "Enemies left: " + enemiesLeft, enemiesLeftDisplayPosition, Color.Red);
            }
            else if (currentState == GameState.GameOver)
            {
                spriteBatch.Draw(gameOverScreen, mainFrame, Color.White);
            }
            else if (currentState == GameState.Win)
            {
                spriteBatch.Draw(victoryScreen, mainFrame, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        /// <summary>
        /// All collisions method
        /// </summary>
        private void Collisions()
        {
            if (gatherer.Alive)
            {
            GathererPlayerCollision();
            BulletGathererCollision();
            }
            if (soldier.SoldierAlive)
            {
            BulletSoldierCollision();
            SoldierPlayerCollision();
            }
            
        }
        /// <summary>
        /// collision between the bullet and the soldier
        /// </summary>
        private void BulletSoldierCollision()
        {
            //checking if the bullet hit the soldier
            if (CollisionCheck(bulletRectangle, soldierRectangle) == true)
            {
                bullet.Active = false;
                bullet.BulletPosition = new Vector2(-50, -50);    //set bullet to inactive
                soldier.SoldierHealth -= bullet.Damage;   //take away soldiers health
                if (soldier.SoldierHealth == 0)
                {
                    sentinel.Score += 1000;
                    soldier.Death();  //soldier dies
                    soldierHealthIncrease = false;        //resetting health increase to false in order to allow the next soldiers to have 5 health.
                    sentinel.KillCount += 1;
                    enemiesLeft -= 1;
                }
            }
            UpdateBulletRectangle();
        }
        private void GathererBoundaryCheck()
        {
            if (gatherer.PositionX + gatherer.Width >= WindowWidth)
            {
                //gatherer respawns
                gatherer.Respawn();
            }
        }
        /// <summary>
        /// collision between the bullet and the gatherer
        /// </summary>
        private void BulletGathererCollision()
        {
        //checking if bullet hit the gatherer
        if (CollisionCheck(bulletRectangle, gatherer.GathererRectangle()) == true)
            {
                bullet.Active = false;
                bullet.BulletPosition = new Vector2(-50, -50);    //set bullet to inactive
                gatherer.GathererHealth -= bullet.Damage;   //take away gatherers health
                if (gatherer.GathererHealth == 0) // if health is zero
                {
                    sentinel.Score += 500;
                    gatherer.Death();
                    gathererHealthIncrease = false;   //resetting progress to false in order to allow the next gatherers to have 5 health.
                    sentinel.KillCount += 1;
                    enemiesLeft -= 1;
                }
            }
        }
        /// <summary>
        /// checks if bullet is active if yes updates the bounding rectangle
        /// </summary>
        private void UpdateBulletRectangle()
        {
            if (bullet.Active)    //when the bullet is active
            {
                bulletRectangle = bullet.BulletRectangle();   //checking the bullet rectangle
            }
        }
        /// <summary>
        /// While bullet inactive it follows the player
        /// </summary>
        private void UpdateBulletPosition()
        {
            //bullet follows player while inactive
            if (bullet.Active == false)
            {
                bullet.FollowPlayer(sentinel);
            }
        }
        /// <summary>
        /// Collision between player and soldier, if hit player is knocked back
        /// </summary>
        private void SoldierPlayerCollision()
        {
            //collision between soldier and player
            if (CollisionCheck(soldierRectangle, sentinelRectangle) == true)
            {
                playerHit.Play();
                if (soldier.SoldierHeading == Direction.West)
                {//knockback left
                    sentinel.PlayerPositionX -= 50;
                }
                if (soldier.SoldierHeading == Direction.East)
                {//knockback right
                    sentinel.PlayerPositionX += 50;
                }
                if (soldier.SoldierHeading == Direction.North)
                {//knockback up
                    sentinel.PlayerPositionY -= 50;
                }
                if (soldier.SoldierHeading == Direction.South)
                {//knockback Down
                    sentinel.PlayerPositionY += 50;
                }
                //if collided take away life
                sentinel.Life -= soldier.SoldierDamage;
            }
        }
        /// <summary>
        /// Collision between player and the gatherer enemy
        /// </summary>
        private void GathererPlayerCollision()
        {
            //collision between gatherer and player
            if (CollisionCheck(gatherer.GathererRectangle(), sentinelRectangle) == true)
            {
                playerHit.Play();
                if (gatherer.PositionY < sentinelRectangle.Y && sentinel.SentinelHeading == Direction.North)
                {//knockback up
                    sentinel.PlayerPositionY += 50;
                }
                else if (gatherer.PositionY > sentinelRectangle.Y && sentinel.SentinelHeading == Direction.South)
                {//knockback Down
                    sentinel.PlayerPositionY -= 50;
                }
                else if (gatherer.PositionX > sentinelRectangle.X)
                {//knockback left
                    sentinel.PlayerPositionX -= 50;
                }
                else if (gatherer.PositionX < sentinelRectangle.X)
                {//knockback right
                    sentinel.PlayerPositionX += 50;
                }
                //deal damage to player equal to damage of the gatherer
                sentinel.Life -= gatherer.Damage;
            }
        }

        /// <summary>
        /// Method for soldier to follow the hero
        /// </summary>
        private void Follow()
        {
            soldierRectangle = soldier.SoldierRectangle();
            sentinelRectangle = sentinel.PlayerRectangle();
            if (soldier.SoldierAlive)
            {
                if (soldierRectangle.X > sentinelRectangle.X)
                {//move left
                    soldier.GoLeft();
                }
                else if (soldierRectangle.Y < sentinelRectangle.Y)
                {//move down
                    soldier.GoDown();
                }
                else if (soldierRectangle.X < sentinelRectangle.X)
                {//move right
                    soldier.GoRight();
                }
                else if (soldierRectangle.Y > sentinelRectangle.Y)
                {//move up
                    soldier.GoUp();
                }
            }
        }
        /// <summary>
        /// a method that sets active of the crystals to false if the gatherer collects with the crystal
        /// </summary>
        private void CrystalGatheringCheck()
        {
            if (gatherer.CrystalCollected)
            {
                if (gatherer.Lane == 1 && crystal[0].Active == true)
                {
                    crystal[0].Active = false;
                    crystalsAmount--;
                }
                else if (gatherer.Lane == 2 && crystal[1].Active == true)
                {
                    crystal[1].Active = false;
                    crystalsAmount--;
                }
                else if (gatherer.Lane == 3 && crystal[2].Active == true)
                {
                    crystal[2].Active = false;
                    crystalsAmount--;
                }
                else if (gatherer.Lane == 4 && crystal[3].Active == true)
                {
                    crystal[3].Active = false;
                    crystalsAmount--;
                }
            }
            areAllCollected();
        }
        /// <summary>
        /// Collision checking
        /// </summary>
        /// <param name="rectOne"></param>
        /// <param name="rectTwo"></param>
        private bool CollisionCheck(Rectangle rectOne, Rectangle rectTwo)
        {
            bool collided = false;
            if (rectOne.Intersects(rectTwo))
            {
                collided = true;
            }
            else
            {
                collided = false;
            }
            return collided;
        }
        /// <summary>
        /// game progression increases difficulty
        /// </summary>
        private void Progression()
        {
            gathererDifficultyIncrease();
            soldierDifficultyIncrease();
        }
        private void areAllCollected()
        {
            if (crystal[0].Active == false && crystal[1].Active == false && crystal[2].Active == false && crystal[3].Active== false)
            {
                allCrystalsCollected = true;
            }
        }
        /// <summary>
        /// increase health and speed of gatherer depending on kill count
        /// </summary>
        private void gathererDifficultyIncrease()
        {
            if (sentinel.KillCount > 10 && gathererHealthIncrease == false)   //health increase for gatherer at 10 kills
            {
                gatherer.GathererHealth = 5;
                gathererHealthIncrease = true;
            }
            if (sentinel.KillCount > 12)      //at 12 kills increase speed of the gather
            {
                gatherer.Speed = 4;
            }
            if (sentinel.KillCount > 25)      //at 25 kills the gatherer reaches max speed
            {
                gatherer.Speed = 6;
            }
        }
        /// <summary>
        /// increase health and speed of the soldier depending on the kill count
        /// </summary>
        private void soldierDifficultyIncrease()
        {
            if (sentinel.KillCount > 5 && soldierHealthIncrease == false)     //health increase to 5 at 5 enemy kills
            {
                soldier.SoldierHealth = 5;
                soldierHealthIncrease = true;
            }
            if (sentinel.KillCount > 8)       //at kill count 8 soldiers speed increases as well as the damage output per hit
            {
                soldier.Speed = 3;
                soldier.SoldierDamage = 20;
            }
            if (sentinel.KillCount > 24)      //at 24 kills the soldier damage increases substancially
            {
                soldier.SoldierDamage = 40;
            }
        }
        /// <summary>
        /// Method for checking if enter was pressed if yes go to the Game state
        /// </summary>
        private void ChangeToGameCheck()
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            if (aCurrentKeyboardState.IsKeyDown(Keys.Enter) == true)
            {
                currentState = GameState.Game;
            }
        }
        /// <summary>
        /// when in game over screen press enter to restart the game
        /// </summary>
        private void RestartGame()
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            if (aCurrentKeyboardState.IsKeyDown(Keys.Enter) == true)
            {
                Initialize();   //when accepted to restart, reinitialise all objects
                currentState = GameState.Game;
            }
        }
        //**************PROPERTIES**************//
        // static properties in order to access them from the game class, they are not related to an object.
        /// <summary>
        /// property for the width of the window
        /// </summary>
        static public int WindowWidth
        {
            get
            {
                return windowWidth;
            }
        }
        /// <summary>
        /// property for height of the window
        /// </summary>
        static public int WindowHeight
        {
            get
            {
                return windowHeight;
            }
        }
        /// <summary>
        /// property for the level height
        /// </summary>
        static public int LevelHeight
        {
            get
            {
                return levelHeight;
            }
        }
    }//end game class
}//end namespace
