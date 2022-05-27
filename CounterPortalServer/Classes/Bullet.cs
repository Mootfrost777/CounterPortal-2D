using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace CounterPortalServer.Classes
{
    internal class Bullet
    { 
        [JsonProperty("position")]
        private Vector2 position;
        
        [JsonProperty("isVisible")]
        public bool isVisible = true;
    }
}
