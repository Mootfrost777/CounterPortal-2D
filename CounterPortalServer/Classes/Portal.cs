using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CounterPortalServer.Classes
{
    public class Portal
    {
        [JsonProperty("position")]
        public Vector2 position;

        [JsonProperty("type")]
        private int type;
    }
}
