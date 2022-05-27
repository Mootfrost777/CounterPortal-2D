using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CounterPortalServer.Classes
{
    public enum SessionStatus
    {
        StartGame, EndGame, StateUpdate, WaitingForPlayers
    }
    public class SessionInstance
    {
        [JsonProperty("status")]
        public SessionStatus status = SessionStatus.StartGame;

        [JsonProperty("players")]
        public List<Player> players = new List<Player>();

        [JsonProperty("seed")]
        public int seed;
        
        public void Deserialize(string json)
        {
            SessionInstance nm = JsonConvert.DeserializeObject<SessionInstance>(json);
            status = nm.status;
            players = nm.players;
            seed = nm.seed;
        }

        public SessionInstance DeepCopy()
        {
            SessionInstance nm = new SessionInstance();
            nm.status = this.status;
            nm.seed = this.seed;
            nm.players = new List<Player>();
            foreach (Player p in this.players)
            {
                nm.players.Add(p);
            }
            return nm;
        }
    }
}
