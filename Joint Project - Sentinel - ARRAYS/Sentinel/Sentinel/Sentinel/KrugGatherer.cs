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
    class KrugGatherer
    {
        //variables
        static Texture2D krugGathererTexture;
        static Texture2D textureLeft;
        static Texture2D textureRight;
        static int gathererHeight = 0;
        int[] laneHeight = { 220, 340, 460, 580}; 
        Vector2 position = new Vector2(0,0);
        bool crystalCollected = false;
        int lane = 0;
        bool goTowardsCrystal = true;
        bool alive = true;
        int counter = 0;
        int gathererWidth = 0;
        int health = 2;
        int gathererDamage = 10;
        int speed = 2;
        Random rnd = new Random();

        //constants
        const int GatheringPosition = 60;
        public KrugGatherer(int width,int height)
        {
            RadomLane();
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {//Load up the image of the gatherer
            krugGathererTexture = theContentManager.Load<Texture2D>(theAssetName + "Left");
            textureLeft = theContentManager.Load<Texture2D>(theAssetName + "Left");
            textureRight = theContentManager.Load<Texture2D>(theAssetName + "Right");
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {//Draw the gatherer
            if (alive)
            {
                theSpriteBatch.Draw(krugGathererTexture, position, Color.White);
            }
        }

        //update the krug gatherer
        public void Update(ContentManager theContentManager, int windowHeight, int windowWidth)
        {
            //variables
            gathererWidth = krugGathererTexture.Width;
            gathererHeight = krugGathererTexture.Height;
            //move the gatherer
            GathererMovement();
        }
        //moving left and right
        public void moveLeft()
        {
            position.X -= speed;
            krugGathererTexture = textureLeft;
        }
        public void moveRight()
        {
            position.X += speed;
            krugGathererTexture = textureRight;
        }
        //method for collecting the gem
        private void Collect()
        {
            counter++;
            if (counter >= 200)
            {
                    crystalCollected = true;
                    counter = 0;
                    
            }
        }
        //method for getting the bouding rectangle of the Gatherer sprite
        public Rectangle GathererRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, gathererWidth, gathererHeight);
        }
        /// <summary>
        /// Respawning the enemy in a random lane getting gatherer to move towards the crystal and setting cry
        /// </summary>
        public void Respawn()
        {
            alive = true;
            goTowardsCrystal = true;
            crystalCollected = false;
            health = 2;
            lane = rnd.Next(1, 5);
            RadomLane();
        }
        /// <summary>
        /// A method that generates a random lane for the gatherer
        /// </summary>
        private void RadomLane()
        {
            lane = rnd.Next(1, 5);
            if (lane == 1)
            {
                position = new Vector2(Game1.WindowWidth - gathererWidth, laneHeight[0]);
            }
            if (lane == 2)
            {
                position = new Vector2(Game1.WindowWidth - gathererWidth, laneHeight[1]);
            }
            if (lane == 3)
            {
                position = new Vector2(Game1.WindowWidth - gathererWidth, laneHeight[2]);
            }
            if (lane == 4)
            {
                position = new Vector2(Game1.WindowWidth - gathererWidth, laneHeight[3]);
            }
        }
        /// <summary>
        /// the method to make the gatherer move towards the crystal collect it and walk back with it
        /// </summary>
        private void GathererMovement()
        {
            if (goTowardsCrystal == true)
            {
                moveLeft();
            }
            if (position.X <= GatheringPosition)
            {
                Collect();
                goTowardsCrystal = false;
            }
            if (crystalCollected == true)
            {
                moveRight();
            }
            if (position.X + gathererWidth >= Game1.WindowWidth)
            {
                Respawn();
            }
        }
        /// <summary>
        /// the method that kills the gatherer
        /// </summary>
        public void Death()
        {
            alive = false;
            Respawn();
        }
        //**************PROPERTIES**************//
        /// <summary>
        ///A property to access and change the health of the gatherer
        /// </summary>
        public int GathererHealth
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
        /// returns the x position of the gatherer
        /// </summary>
        public float PositionX
        {
            get
            {
                return position.X;
            }
        }
        /// <summary>
        /// returns the Y position of the gatherer
        /// </summary>
        public float PositionY
        {
            get
            {
                return position.Y;
            }
        }
        /// <summary>
        /// Property of the alive bool
        /// </summary>
        public bool Alive
        {
            get
            {
                return alive;
            }
        }
        /// <summary>
        /// A property for the lane of the gatherer
        /// </summary>
        public int Lane
        {
            get
            {
                return lane;
            }
        }
        /// <summary>
        /// width property of the gatherer
        /// </summary>
        public int Width
        {
            get
            {
                return gathererWidth;
            }
        }
        /// <summary>
        /// property to get the damage of the gatherer
        /// </summary>
        public int Damage
        {
            get
            {
                return gathererDamage;
            }
        }
        /// <summary>
        /// property for the speed of the gatherer
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
        /// <summary>
        /// property for the crystal collected bool
        /// </summary>
        public bool CrystalCollected
        {
            get
            {
                return crystalCollected;
            }
        }
    }//end gatherer class
}//end namespace
