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
 */
namespace Sentinel
{
    class Player
    {   //variables
        static Texture2D heroTexture;
        static Texture2D textureUp;
        static Texture2D textureRight;
        static Texture2D textureDown;
        static Texture2D textureLeft;
        int score = 0;
        int heroWidth = 0;
        int heroHeight = 0;
        int life = 100;
        int killCount = 0;
        int speed = 5;
        bool active = true;
        Vector2 playerPosition;
        //const variables
        const int StartingPositionX = 150;
        const int StartingPositionY = 400;
        Direction sentinelHeading = Direction.East;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Player()
        {
            playerPosition = new Vector2(StartingPositionX, StartingPositionY);
        }

        /// <summary>
        /// Load images of the hero
        /// </summary>
        /// <param name="theContentManager"></param>
        /// <param name="theAssetName"></param>
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {//Load images of the sprite
            heroTexture = theContentManager.Load<Texture2D>(theAssetName + "Right");
            textureUp = theContentManager.Load<Texture2D>(theAssetName + "Up");
            textureDown = theContentManager.Load<Texture2D>(theAssetName + "Down");
            textureLeft = theContentManager.Load<Texture2D>(theAssetName + "Left");
            textureRight = theContentManager.Load<Texture2D>(theAssetName + "Right");
        }
        /// <summary>
        /// Draw the hero
        /// </summary>
        /// <param name="theSpriteBatch"></param>
        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (active)
            {
                theSpriteBatch.Draw(heroTexture, playerPosition, Color.White);
            }
        }
        /// <summary>
        /// Update the hero
        /// </summary>
        /// <param name="theContentManager"></param>
        /// <param name="windowHeight"></param>
        /// <param name="windowWidth"></param>
        public void Update(ContentManager theContentManager, int windowHeight, int windowWidth)
        {
            heroWidth = heroTexture.Width;
            heroHeight = heroTexture.Height;
            CheckKeyboard();
        }
        /// <summary>
        /// a method that handles all the keyboard input for player movement
        /// </summary>
        private void CheckKeyboard()
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
            {
                sentinelHeading = Direction.West;
                MoveLeft();
                BoundaryCheck(heroWidth, heroHeight, Game1.WindowHeight, Game1.WindowWidth);
                heroTexture = textureLeft;
            }
            if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
            {
                sentinelHeading = Direction.East;
                MoveRight();
                BoundaryCheck(heroWidth, heroHeight, Game1.WindowHeight, Game1.WindowWidth);
                heroTexture = textureRight;
            }
            if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
            {
                sentinelHeading = Direction.South;
                MoveDown();
                BoundaryCheck(heroWidth, heroHeight, Game1.WindowHeight, Game1.WindowWidth);
                heroTexture = textureDown;
            }
            if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
            {
                sentinelHeading = Direction.North;
                MoveUp();
                BoundaryCheck(heroWidth, heroHeight, Game1.WindowHeight, Game1.WindowWidth);
                heroTexture = textureUp;
            }
        }
        private void BoundaryCheck(int heroWidth, int heroHeight, int windowHeight, int windowWidth)
        {
            if (playerPosition.Y <= Game1.LevelHeight)
            {//check if sprite is hitting top of level
                MoveDown();    //if its outside move down
            }
            if (playerPosition.Y >= windowHeight - heroHeight)
            {
                MoveUp();     //if hitting the bottom move up
            }
            if (playerPosition.X <= 0)
            {//check if sprite is hitting left of screen
                MoveRight();    //if yes move right 
            }
            if (playerPosition.X >= windowWidth - heroWidth)
            {//check if sprite is hitting right of the screen
                MoveLeft();
            }
        }

        public void MoveDown()
        {//move down
            playerPosition.Y += speed;
        }

        public void MoveUp()
        {//move up
            playerPosition.Y -= speed;
        }

        public void MoveLeft()
        {//move left
            playerPosition.X -= speed;
        }
        public void MoveRight()
        {//move right
            playerPosition.X += speed;
        }
        //getting the bounding rectangle of the player sprite in order to use it within the game class
        public Rectangle PlayerRectangle()
        {
            return new Rectangle((int)playerPosition.X, (int)playerPosition.Y, heroWidth, heroHeight);
        }
        //**************PROPERTIES**************//
        /// <summary>
        /// Gets X position of the player
        /// </summary>
        /// <returns></returns>
        public float PlayerPositionX
        {
            set
            {
                playerPosition.X = value;
            }
            get
            {
                return playerPosition.X;
            }
        }
        /// <summary>
        /// Gets the Y position of the player
        /// </summary>
        public float PlayerPositionY
        {
            set
            {
                playerPosition.Y = value;
            }
            get
            {
                return playerPosition.Y;
            }
        }
        /// <summary>
        /// getting width of the hero
        /// </summary>
        public int HeroWidth
        {
            get
            {
                return heroWidth;
            }
        }
        /// <summary>
        /// gets the height of the hero
        /// </summary>
        public int HeroHeight
        {
            get
            {
                return heroHeight;
            }
        }
        /// <summary>
        /// property to check or set the lifes of the player
        /// </summary>
        public int Life
        {
            set
            {
                life = value;
            }
            get
            {
                return life;
            }
        }
        /// <summary>
        /// Property to access or change the kill count of the player
        /// </summary>
        public int KillCount
        {
            set
            {
                killCount = value;
            }
            get
            {
                return killCount;
            }
        }
        /// <summary>
        /// property for the score of the player.
        /// </summary>
        public int Score
        {
            set
            {
                score = value;
            }
            get
            {
                return score;
            }   
        }
        /// <summary>
        /// property for the heading of the sentinel
        /// </summary>
        public Direction SentinelHeading
        {
            get
            {
                return sentinelHeading;
            }
        }
    }//end player
}//end namespace
