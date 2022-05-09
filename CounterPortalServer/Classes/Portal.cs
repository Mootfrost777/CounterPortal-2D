using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CounterPortalServer.Classes
{
    internal class Portal
    {
        public Vector2 position;
        private int type;
        
        [JsonIgnore]
        public Texture2D texture;
    }
}
