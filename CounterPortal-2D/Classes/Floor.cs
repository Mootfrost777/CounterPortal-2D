using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CounterPortal_2D.Classes
{
    class Floor
    {
        private Texture2D texture;
        private Vector2 position;
        private int speed;

        public Floor()
        {
            position = new Vector2(0, 0);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("floor");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}