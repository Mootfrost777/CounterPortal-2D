using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace CounterPortalServer.Classes
{
    public enum PlayerStatus
    {
        Ready, NotReady, Playing
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
        public bool isAlive;

        [JsonProperty("portals")]
        private List<Portal> portals = new List<Portal>();

        [JsonProperty("bullets")]
        private List<Bullet> bullets = new List<Bullet>();

        [JsonIgnore]
        public Socket ClientSocket;
        

        public Player()
        {
            
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Deserialize(string json)
        {
            Player player = JsonConvert.DeserializeObject<Player>(json);
            position = player.position;
            name = player.name;
            score = player.score;
            isAlive = player.isAlive;
            portals = player.portals;
            bullets = player.bullets;
            score = player.score;
            Id = player.Id;
            status = player.status;
        }
    }
}
