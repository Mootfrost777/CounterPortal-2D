using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CounterPortal_2D.Classes
{
    public class Wall
    {
        public static Texture2D texture;
        public Rectangle wallRect;
        
        public Wall(Rectangle wallRect)
        {
            this.wallRect = wallRect;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, wallRect, Color.White);
        }
    }
}
