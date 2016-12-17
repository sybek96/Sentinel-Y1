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
    class Crystal
    {
        //variables
        static Texture2D crystalTexture;
        bool active = true;
        int crystalWidth = 0;
        int crystalHeight = 0;
        
        Vector2 position;

        //default constructor for a crystal
        public Crystal(Vector2 crystalPosition)
        {
            position = (crystalPosition);
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {//load up the image of the crystal
            crystalTexture = theContentManager.Load<Texture2D>(theAssetName);
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {//draw method for the crystal
            if (active)
            {
                theSpriteBatch.Draw(crystalTexture, position, Color.White);
            }
        }

        //update the contents of the crystal class
        public void Update(ContentManager theContentManager)
        {
            crystalWidth = crystalTexture.Width;
            crystalHeight = crystalTexture.Height;
        }
        /// <summary>
        /// getting the bounding rectangle of the crystal
        /// </summary>
        /// <returns></returns>
        public Rectangle CrystalRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, crystalWidth, crystalHeight);
        }
        //**************PROPERTIES**************//
        /// <summary>
        ///A property to show if crystal is active
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
    }//end crystal
}//end namespace
