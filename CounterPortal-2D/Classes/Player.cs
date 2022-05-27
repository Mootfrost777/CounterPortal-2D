using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace CounterPortal_2D.Classes
{
    public enum PlayerStatus
    {
        Ready, NotReady
    }
    public class Player
    {
        [JsonProperty("id")]
        public string Id;
        
        [JsonProperty("status")]
        public PlayerStatus status = PlayerStatus.NotReady;
        
        [JsonProperty("position")]
        public Vector2 position = new Vector2(100, 100);

        [JsonProperty("rotation")]
        public float rotation = 0;

        [JsonProperty("name")]
        public string name;

        [JsonProperty("score")]
        public int score;

        [JsonProperty("isAlive")]
        public bool isAlive = true;

        [JsonProperty("portals")]
        public List<Portal> portals = new List<Portal>();

        [JsonProperty("bullets")]
        private List<Bullet> bullets = new List<Bullet>();

        [JsonIgnore]
        public static Texture2D texture;
        private int speed = 1;
        private int portalsPlaced = 0;
        private KeyboardState oldKeyboardState;
        private MouseState oldMouseState;

        [JsonIgnore]
        public Rectangle playerRect;

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var portal in portals)
            {
                portal.Draw(spriteBatch);
            }

            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
            playerRect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            spriteBatch.Draw(texture, position, null, Color.White, rotation * (float)Math.PI / 180, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
        }

        public void Update()
        {
            bullets.RemoveAll(bullet => !bullet.isVisible);
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            if (keyState.IsKeyDown(Keys.W))
            {
                position.Y -= speed;
                rotation = -90;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                position.Y += speed;
                rotation = 90;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                position.X -= speed;
                rotation = 180;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                position.X += speed;
                rotation = default;
            }
            if (keyState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                if (portalsPlaced % 2 == 0)
                {
                    portals[0].SetPosition(position - new Vector2(Portal.texture_t1.Width / 2, Portal.texture_t1.Height / 2));
                }
                else
                {
                    portals[1].SetPosition(position - new Vector2(Portal.texture_t2.Width / 2, Portal.texture_t2.Height / 2));
                }
                portalsPlaced++;
            }
            if (keyState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E))
            {
                Teleport();
            }
            if (mouseState.LeftButton.Equals(ButtonState.Pressed) && oldMouseState.LeftButton.Equals(ButtonState.Released) && bullets.Count < 5)
            {
                Vector2 destinantion = mouseState.Position.ToVector2() - position;
                destinantion.Normalize();
                bullets.Add(new Bullet(position, destinantion * speed * 2));
            }
            oldKeyboardState = keyState;
            oldMouseState = mouseState;
            foreach (var bullet in bullets)
            {
                bullet.Update();
            }
            WallCollide();
        }

        /// <summary>
        ///  Какая-то черная магия, все по логике работает, но на самом деле - нет. Что-то не то с rectangle либо у игрока либо у стены.
        /// </summary>
        private void WallCollide()
        {
            foreach (var wall in Game1._walls)
            {
                if (playerRect.Right == wall.wallRect.Left && playerRect.Bottom > wall.wallRect.Top && playerRect.Top < wall.wallRect.Bottom)
                {
                    position.X -= speed;
                }
                else if (playerRect.Left == wall.wallRect.Right && playerRect.Bottom > wall.wallRect.Top && playerRect.Top < wall.wallRect.Bottom)
                {
                    position.X += speed;
                }
                else if (playerRect.Bottom == wall.wallRect.Top && playerRect.Left > wall.wallRect.Left && playerRect.Right < wall.wallRect.Right)
                {
                    position.Y -= speed;
                }
                else if (playerRect.Top == wall.wallRect.Bottom && playerRect.Left > wall.wallRect.Left && playerRect.Right < wall.wallRect.Right)
                {
                    position.Y += speed;
                }
            }
        }

        private void Teleport()
        {
            for (int i = 0; i < portals.Count; i++)            
            { 

                if (position.X + texture.Width / 2 < portals[i].position.X + Portal.texture_t2.Width &&
                     position.X + texture.Width / 2 > portals[i].position.X &&
                     position.Y + texture.Height / 2 < portals[i].position.Y + Portal.texture_t2.Height &&
                     position.Y + texture.Height / 2> portals[i].position.Y)
                {
                    if (i == 0)
                    {
                        position = portals[1].position;
                    }
                    else
                    {
                        position = portals[0].position;
                    }
                    break;
                }
            }
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(DeepCopy(this));
        }

        private Player DeepCopy(Player player)
        {
            Player player1 = new Player();
            player1.Id = player.Id;
            player1.name = player.name;
            player1.position = player.position;
            player1.rotation = player.rotation;
            player1.score = player.score;
            player1.isAlive = player.isAlive;
            player1.rotation = player.rotation;
            player1.portals = new List<Portal>();
            foreach (var portal in player.portals)
            {
                player1.portals.Add(portal);
            }
            player1.bullets = new List<Bullet>();
            foreach (var bullet in player.bullets)
            {
                player1.bullets.Add(bullet);
            }
            return player1;
        }
    }
}
