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

        private Vector2 position;
        private Vector2 speed;

        public Rectangle boundingBox;
        public bool isVisible = true;

        [JsonIgnore]
        private Texture2D texture;

        public Bullet(Texture2D texture, Vector2 position, Vector2 speed)
        {
            this.texture = texture;
            this.position = position;
            this.speed = speed;
        }
    }
}
