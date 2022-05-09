using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CounterPortal_2D.Classes
{
    public enum SessionStatus
    {
        StartGame, WaitingForPlayers, EndGame, StateUpdate
    }
    public class SessionInstance
    {
        [JsonProperty("status")]
        public SessionStatus status = SessionStatus.WaitingForPlayers;

        [JsonProperty("players")]
        public List<Player> players;

        [JsonProperty("seed")]
        public int seed;

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        
        public void Deserialize(string json)
        {
            SessionInstance nm = JsonConvert.DeserializeObject<SessionInstance>(json);
            status = nm.status;
            players = nm.players;
            seed = nm.seed;
        } 
    }
}
