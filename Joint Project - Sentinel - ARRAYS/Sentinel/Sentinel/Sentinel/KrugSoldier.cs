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
    class KrugSoldier
    {
        //variables

        static Texture2D krugSoldierTexture;
        static Texture2D textureLeft;
        static Texture2D textureRight;
        static Texture2D textureUp;
        static Texture2D textureDown;
        Vector2 position = new Vector2(0, 0);
        Direction soldierHeading;
        bool alive;
        int speed = 1;
        //constants
        int soldierWidth = 0;
        int soldierHeight = 0;
        int counter = 0;
        int health = 2;
        int soldierDamage = 10;
        Random rnd = new Random();

        public KrugSoldier()
        {//defalult constructor for krug soldier
            position = new Vector2(500, 350);
            alive = true;
        }
        
        //Load up the image of the krug soldier
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            krugSoldierTexture = theContentManager.Load<Texture2D>(theAssetName + "Left");
            textureLeft = theContentManager.Load<Texture2D>(theAssetName + "Left");
            textureRight = theContentManager.Load<Texture2D>(theAssetName + "Right");
            textureUp = theContentManager.Load<Texture2D>(theAssetName + "Up");
            textureDown = theContentManager.Load<Texture2D>(theAssetName + "Down");
        }

        //Draw method for the Krug soldier
        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (alive)
            {
                theSpriteBatch.Draw(krugSoldierTexture, position, Color.White);
            }
        }

        //Update method for the Krug soldier
        public void Update(ContentManager theContentManager, int windowHeight, int windowWidth)
        {
            counter++;
            if (counter >= 200)
            {
                alive = true;
                counter = 0;
            }
            //variables
            if (alive)
            {
                soldierWidth = krugSoldierTexture.Width;
                soldierHeight = krugSoldierTexture.Height;
            }

        }
        /// <summary>
        /// method for getting the bounding rectangle of the soldier
        /// </summary>
        /// <returns></returns>
        public Rectangle SoldierRectangle()
        {
            return (new Rectangle((int)position.X, (int)position.Y, soldierWidth, soldierHeight));
        }
        /// <summary>
        /// soldier moves left and changes texture
        /// </summary>
        public void GoLeft()
        {
            soldierHeading = Direction.West;
            krugSoldierTexture = textureLeft;
            position.X -= speed;
        }
        /// <summary>
        ///soldier moves right and changes sprite
        /// </summary>
        public void GoRight()
        {
            soldierHeading = Direction.East;
            krugSoldierTexture = textureRight;
            position.X += speed;
        }
        /// <summary>
        /// Soldier moves up and changes sprite
        /// </summary>
        public void GoUp()
        {
            soldierHeading = Direction.North;
            krugSoldierTexture = textureUp;
            position.Y -= speed;
        }
        /// <summary>
        /// soldier moves down and changes texture
        /// </summary>
        public void GoDown()
        {
            soldierHeading = Direction.South;
            krugSoldierTexture = textureDown;
            position.Y += speed;
        }
        /// <summary>
        /// The sprite becomes not active
        /// </summary>
        public void Death()
        {
            alive = false;
            Respawn();
        }
        /// <summary>
        /// A counter that decides if enemy should spawn
        /// </summary>
        public void WaitToSpawn()
        {
            counter++;
            if (counter >= 200)
            {
                alive = true;
                counter = 0;
            }
        }
        /// <summary>
        /// respawn the soldier at a random location
        /// </summary>
        public void Respawn()
        {
            WaitToSpawn();
            int randomX = rnd.Next(500, Game1.WindowWidth - soldierWidth);
            int randomY = rnd.Next(200, Game1.WindowHeight - soldierHeight);
            position = new Vector2(randomX, randomY);
            health = 2;
        }
        //**************PROPERTIES**************//
        /// <summary>
        /// a class property of soldier heading
        /// </summary>
        public Direction SoldierHeading
        {
            get
            {
                return soldierHeading;
            }
        }
        /// <summary>
        /// check if the soldier is alive
        /// </summary>
        public bool SoldierAlive
        {
            get
            {
                return alive;
            }
        }
        /// <summary>
        /// accessing the health of the soldier
        /// </summary>
        public int SoldierHealth
        {
            set
            {
                health = value;
            }
            get
            {
                return health;
            }
        }
        /// <summary>
        /// Property of the damage that the soldier deals.
        /// </summary>
        public int SoldierDamage
        {
            set
            {
                soldierDamage = value;
            }
            get
            {
                return soldierDamage;
            }
        }
        /// <summary>
        /// Property for speed
        /// </summary>
        public int Speed
        {
            set
            {
                speed = value;
            }
            get
            {
                return speed;
            }
        }
    }//end soldier class
}//end namespace
