using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CounterPortal_2D.Classes
{
    public class Portal
    {
        [JsonProperty("position")]
        public Vector2 position;

        [JsonProperty("type")]
        private int type;

        [JsonIgnore]
        public static Texture2D texture_t1;
        public static Texture2D texture_t2;

        public Portal(Vector2 position, int type)
        {
            this.position = position;
            this.type = type;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            switch (type)
            {
                case 1:
                    spriteBatch.Draw(texture_t1, position, Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(texture_t2, position, Color.White);
                    break;
            }
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
