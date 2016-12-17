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
    //an enumerator for the directions
    public enum Direction { None, North, South, East, West };

    class Bullet
    {
        //variables
        Texture2D bulletTexture;
        Texture2D textureUp;
        Texture2D textureRight;
        Texture2D textureDown;
        Texture2D textureLeft;
        int bulletWidth;
        int bulletHeight;
        bool bulletFired = false;
        bool active = false;
        Direction bulletHeading = Direction.None;
        Vector2 position;
        Rectangle bulletRectangle;
        //constants
        const int bulletSpeed = 7;
        int damage = 1;

        /// <summary>
        /// default constructor sets bullet to 0,0
        /// </summary>
        public Bullet()
        {
            position = new Vector2(500, 500);
        }
        /// <summary>
        /// Loads up all the textures for the object
        /// </summary>
        /// <param name="theContentManager"></param>
        /// <param name="theAssetName"></param>
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            bulletTexture = theContentManager.Load<Texture2D>(theAssetName + "Right");
            textureRight = theContentManager.Load<Texture2D>(theAssetName + "Right");
            textureDown = theContentManager.Load<Texture2D>(theAssetName + "Down");
            textureLeft = theContentManager.Load<Texture2D>(theAssetName + "Left");
            textureUp = theContentManager.Load<Texture2D>(theAssetName + "Up");
        }
        /// <summary>
        /// draws the object
        /// </summary>
        /// <param name="theSpriteBatch"></param>
        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (active)
            {//draw while active
                theSpriteBatch.Draw(bulletTexture, position, Color.White);
            }
        }
        /// <summary>
        /// update the bullet
        /// </summary>
        /// <param name="theContentManager"></param>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        public void Update(ContentManager theContentManager, int windowWidth, int windowHeight)
        {
            bulletWidth = bulletTexture.Width;
            bulletHeight = bulletTexture.Height;
            //variables
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true)
            {//check if space was pressed
                bulletFired = true;
            }
            if (active)
            {//do boundary checking only while bullet is active
                BoundaryCheck(bulletTexture.Width, bulletTexture.Height, windowHeight, windowWidth);
            }
            if (bulletFired == true)    //if fired activate set texture and move bullet
            {
                active = true;
                //switch statement for the directions
                switch (bulletHeading)
                {
                    case Direction.None:
                        break;
                    case Direction.North:
                        position.Y -= bulletSpeed;
                        break;
                    case Direction.South:
                        position.Y += bulletSpeed;
                        break;
                    case Direction.East:
                        position.X += bulletSpeed;
                        break;
                    case Direction.West:
                        position.X -= bulletSpeed;
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// boundary checking for the bullet
        /// </summary>
        /// <param name="bulletWidth"></param>
        /// <param name="bulletHeight"></param>
        /// <param name="windowHeight"></param>
        /// <param name="windowWidth"></param>
        private void BoundaryCheck(int bulletWidth, int bulletHeight, int windowHeight, int windowWidth)
        {
            if (position.Y <= Game1.LevelHeight || position.Y >= windowHeight - bulletHeight || position.X <= 0 || position.X >= windowWidth - bulletWidth)
            {//check if bullet is hitting border of the level
                position = new Vector2(500, 500);       //reset bullet position.
                bulletFired = false;    //set bullet fired to false
                active = false;         //if its outside bullet is not active
            }
        }
        /// <summary>
        /// make bullet follow the player
        /// </summary>
        public void FollowPlayer(Player sentinel)
        {
            if (sentinel.SentinelHeading == Direction.North)   //for up
            {
                bulletHeading = Direction.North;
                bulletTexture = textureUp;
                position.X = sentinel.PlayerPositionX + 5;
                position.Y = sentinel.PlayerPositionY - bulletHeight;
            }
            if (sentinel.SentinelHeading == Direction.South)   //for down
            {
                bulletHeading = Direction.South;
                bulletTexture = textureDown;
                position.X = sentinel.PlayerPositionX + 5;
                position.Y = sentinel.PlayerPositionY + sentinel.HeroHeight;
            }
            if (sentinel.SentinelHeading == Direction.West)    //for left
            {
                bulletHeading = Direction.West;
                bulletTexture = textureLeft;
                position.X = sentinel.PlayerPositionX - bulletWidth;
                position.Y = sentinel.PlayerPositionY + (sentinel.HeroHeight / 2);
            }
            if (sentinel.SentinelHeading == Direction.East)    //for right
            {
                bulletHeading = Direction.East;
                bulletTexture = textureRight;
                position.X = sentinel.PlayerPositionX + sentinel.HeroWidth;
                position.Y = sentinel.PlayerPositionY + (sentinel.HeroHeight / 2);
            }
        }//end follow player
        /// <summary>
        /// sets up a rectangle for the bullet
        /// </summary>
        /// <returns></returns>
        public Rectangle BulletRectangle()
        {
            bulletRectangle = new Rectangle((int)position.X, (int)position.Y, bulletWidth, bulletHeight);
            return bulletRectangle;
        }
        //**************PROPERTIES**************//
        //to see how much damage the bullet does
        public int Damage
        {
            get
            {
                return damage;
            }
        }
        /// <summary>
        /// position of the bullet access
        /// </summary>
        public Vector2 BulletPosition
        {
            set
            {
                position = value;
            }
            get
            {
                return position;
            }
        }
        /// <summary>
        /// a property for if the bullet is active
        /// </summary>
        public bool Active
        {
            set
            {
                active = value;
            }
            get
            {
                return active;
            }
        }
        /// <summary>
        /// Property for the fired bullet
        /// </summary>
        public bool BulletFired
        {
            set
            {
                bulletFired = value;
            }
            get
            {
                return bulletFired;
            }
        }
    }//end Bullet
}//end namespace
