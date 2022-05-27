﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;     // для ContentManager
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text;
using System;
using System.Windows;

using Microsoft.Xna.Framework.Audio;

namespace CounterPortal_2D.Classes.UI
{
    class Menu
    {
        public List<Label> items;
        private string[] elements = { "Play", "Info", "Exit" };
        private int select = 0;

        private KeyboardState keyboardState;
        private KeyboardState prevKeyboardState;

        public Vector2 Position;

        private Texture2D logo;

        public Menu()
        {
            SoundEffect.MasterVolume = 0.5f;
            items = new List<Label>();
            Vector2 position = new Vector2(30, Game1.screenHeight / 2);

            for (int i = 0; i < elements.Length; i++)
            {
                Label label = new Label(elements[i], position, Color.Orange);
                items.Add(label);
                position.Y += 60;
            }
        }

        public void LoadContent(ContentManager Content)
        {
            foreach (Label item in items)
            {
                item.LoadContent(Content);
            }
            logo = Content.Load<Texture2D>("logo");
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            items[select].Color = Color.Red;

            foreach (var item in items)
            {
                item.Draw(_spriteBatch);
            }
            _spriteBatch.Draw(logo, new Vector2(30, Game1.screenHeight / 2 - 100 - logo.Height), Color.White);
        }

        public void Update()
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Down) && (keyboardState != prevKeyboardState))
                if (select < items.Count - 1)
                {
                    items[select].ResetColor();
                    select++;
                }

            if (keyboardState.IsKeyDown(Keys.Up) && (keyboardState != prevKeyboardState))
                if (select > 0)
                {
                    items[select].ResetColor();
                    select--;
                }

            if (keyboardState.IsKeyDown(Keys.Enter) && (keyboardState != prevKeyboardState))
            {
                switch (select)
                {
                    case 0:
                        Game1._gameState = GameState.ConnectToServer;
                        break;
                    case 2:
                        Game1._gameState = GameState.Exit;
                        break;
                    default:
                        break;
                }
            }
            prevKeyboardState = keyboardState;
        }

        public void SetMenuPos(Vector2 Position)
        {
            this.Position = Position;
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Position = Position;
                Position.Y += 30;
            }
        }
    }
}