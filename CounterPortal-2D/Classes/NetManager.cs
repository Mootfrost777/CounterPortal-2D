using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CounterPortal_2D.Classes
{
    public class NetManager
    {
        [JsonProperty("status")]
        public string status = "OK";

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
            NetManager nm = JsonConvert.DeserializeObject<NetManager>(json);
            status = nm.status;
            players = nm.players;
            seed = nm.seed;
        } 
    }
}
