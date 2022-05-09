using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CounterPortal_2D.Classes
{
    internal class Portal
    {
        public Vector2 position;

        private int type;

        [JsonIgnore]
        public Texture2D texture;

        public Portal(Vector2 position, int type)
        {
            this.position = position;
            this.type = type;
        }

        public void LoadContent(ContentManager content)
        {
            if (type == 0)
            {
                texture = content.Load<Texture2D>("Portal_t0");
            }
            else
            {
                texture = content.Load<Texture2D>("Portal_t1");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }
    }
}
