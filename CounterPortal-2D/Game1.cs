using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CounterPortal_2D.Classes;
using CounterPortal_2D.Classes.UI;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Text;
using System;
using System.Threading;

namespace CounterPortal_2D
{
    public enum GameState
    {
        Menu, Game, End, Pause, ConnectToServer, Reset, Exit
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static GameState _gameState = GameState.Menu;
        private MapGenerator _mapGenerator;

        private Player _player;
        public static List<Player> _opponents;
        private Aim _aim;

        private Texture2D _background;
        public static List<Wall> _walls = new List<Wall>();

        Menu _menu;

        private int borderWidth = 10;

        public static int screenWidth;
        public static int screenHeight;
        private int seed;

        public static string ip;
        public static int port;
        public Socket socket;

        private Thread stateSender;
        private Thread stateReceiver;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
           /* _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;*/
        }

        protected override void Initialize()
        {
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
            _player = new Player();
            _aim = new Aim();
            _menu = new Menu();
            base.Initialize();
            stateReceiver = new Thread(() =>
            {
                while (true)
                {
                    SessionInstance session = new SessionInstance();
                    byte[] data = new byte[4096];
                    int bytes = socket.Receive(data);
                    string json = Encoding.UTF8.GetString(data, 0, bytes);
                    session.Deserialize(json);
                    switch (session.status)
                    {
                        case SessionStatus.StartGame:
                            stateSender.Start();
                            session.players.Remove(session.players.Find(x => x.Id == _player.Id));
                            _opponents = session.players;
                            _mapGenerator = new MapGenerator(session.seed, borderWidth);
                            _walls = _mapGenerator.GenerateMap();
                            _gameState = GameState.Game;
                            break;
                        case SessionStatus.EndGame:
                            session.players.Remove(session.players.Find(x => x.Id == _player.Id));
                            _opponents = session.players;
                            socket.Close();  // Игра кончилась - отключаемся, показываем результаты
                            _gameState = GameState.End;
                            break;
                        case SessionStatus.StateUpdate:
                            session.players.Remove(session.players.Find(x => x.Id == _player.Id));
                            _opponents = session.players;
                            break;
                        default:
                            break;
                    }
                }
            });
            stateSender = new Thread(() =>
            {
                while (true)
                {
                    byte[] data = Encoding.ASCII.GetBytes(_player.Serialize());
                    socket.Send(data);
                    Thread.Sleep(17);
                }
            });
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //_background = Content.Load<Texture2D>("background");
            Wall.texture = new Texture2D(GraphicsDevice, 1, 1);
            Wall.texture.SetData(new[] { Color.White });


            // This textures are static because there will be many instances of them
            Player.texture = Content.Load<Texture2D>("Player");
            Bullet.texture = Content.Load<Texture2D>("Bullet");
            Portal.texture_t1 = Content.Load<Texture2D>("Portal_t0");
            Portal.texture_t2 = Content.Load<Texture2D>("Portal_t1");

            _player.portals.Add(new Portal(new Vector2(-1000, -1000), 1));  // Add portals
            _player.portals.Add(new Portal(new Vector2(-1000, -1000), 2));

            _aim.LoadContent(Content);
            _menu.LoadContent(Content);
        }

        private static byte[] data;
        protected override void Update(GameTime gameTime)
        {
            _aim.Update(gameTime);
            
            switch (_gameState)
            {
                case GameState.Menu:
                    _menu.Update();
                    break;
                case GameState.Game:
                    UpdateGame();
                    break;
                case GameState.Pause:
                    break;
                case GameState.ConnectToServer:
                    Connect connect = new Connect();
                    if (connect.ShowDialog() == DialogResult.OK)
                    {
                        ip = connect.IP;
                        port = connect.Port;
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {
                            socket.Connect(ip, port);
                            _player.Id = Guid.NewGuid().ToString();
                            data = Encoding.ASCII.GetBytes(_player.Serialize());
                            socket.Send(data);
                        }
                        catch
                        {
                            MessageBox.Show("Server is not responding");
                            _gameState = GameState.Menu;
                        }

                        stateReceiver.Start();

                        GameMessage gm = new GameMessage();
                        gm.Message = "Waiting for players...";
                        //gm.ShowDialog();

                        while (_gameState == GameState.ConnectToServer) 
                        {
                            Console.WriteLine();
                        }
                        //gm.Close();
                    }
                    else 
                    {
                        System.Windows.Forms.MessageBox.Show("Error connecting to server. \nData may be incorrect.");
                        _gameState = GameState.Menu;
                    }
                    break;
                case GameState.Reset:
                    Reset();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            switch (_gameState)
            {
                case GameState.Menu:
                    _menu.Draw(_spriteBatch);
                    break;
                case GameState.Game:
                    DrawGame();
                    break;
                case GameState.Pause:
                    break;
            }
            _aim.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGame()
        {
            foreach (var wall in _walls)
            {
                wall.Draw(_spriteBatch);
            }
            foreach (var player in _opponents)
            {
                player.Draw(_spriteBatch);
            }
            _player.Draw(_spriteBatch);
        }

        private void UpdateGame()
        {
            _player.Update();
            /*foreach (var player in _opponents)
            {
                player.Update();
            }*/
        }

        private void Reset()
        {
            _mapGenerator = new MapGenerator(seed, borderWidth);
            _walls = _mapGenerator.GenerateMap();
            _gameState = GameState.Menu;
        }
    }
}
