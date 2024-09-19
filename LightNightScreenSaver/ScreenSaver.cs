using HPScreen.Admin;
using HPScreen.Entities;
using LightNightScreenSaver.Entities;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SharpDX.Direct2D1;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;

namespace HPScreen
{
    public class ScreenSaver : Game
    {
        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private int loadFrames = 0;
        private const int LOAD_FRAMES_THRESH = 10;
        private CannonSuite backgroundCannons;
        private CannonSuite foregroundCannons;
        private WindowManager Windows { get; set; }

        protected bool RunSetup { get; set; }
        public ScreenSaver()
        {
            Graphics.Current.GraphicsDM = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            // Set the following property to false to ensure alternating Step() and Draw() functions
            // Set the property to true to (hopefully) improve game smoothness by ignoring some draw calls if needed.
            IsFixedTimeStep = false;

            RunSetup = true;
        }
        protected override void Initialize()
        {
            // Note: This takes places BEFORE LoadContent()
            Graphics.Current.Init(this.GraphicsDevice, this.Window, true);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Graphics.Current.SpriteB = new Microsoft.Xna.Framework.Graphics.SpriteBatch(Graphics.Current.GraphicsDM.GraphicsDevice);

            // Add your Sprites here like the following:
            Graphics.Current.SpritesByName.Add("ball", Content.Load<Texture2D>("Sprites/Ball"));
            Graphics.Current.SpritesByName.Add("cannon", Content.Load<Texture2D>("Sprites/Cannon"));
            Graphics.Current.SpritesByName.Add("bg", Content.Load<Texture2D>("Sprites/CityBackground"));
            Graphics.Current.SpritesByName.Add("buildings", Content.Load<Texture2D>("Sprites/CityBuildings"));
            Graphics.Current.SpritesByName.Add("light", Content.Load<Texture2D>("Sprites/light"));

            Graphics.Current.Fonts = new Dictionary<string, SpriteFont>();
            Graphics.Current.Fonts.Add("arial-48", Content.Load<SpriteFont>($"Fonts/arial_48"));
            Graphics.Current.Fonts.Add("arial-72", Content.Load<SpriteFont>($"Fonts/arial_72"));
            Graphics.Current.Fonts.Add("arial-96", Content.Load<SpriteFont>($"Fonts/arial_96"));
            Graphics.Current.Fonts.Add("arial-144", Content.Load<SpriteFont>($"Fonts/arial_144"));
        }
        protected override void Update(GameTime gameTime)
        {
            CheckInput(); // Used to exit game when input detected (aka screensaver logic)
            if (RunSetup) { Setup(); }

            backgroundCannons.Update();
            foregroundCannons.Update();
            ParticleLayers.Current.Update(gameTime);

            UpdateDarkness();

            Windows.Update();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            Graphics.Current.Device.SetRenderTarget(Graphics.Current.GameWorldTarget);
            Graphics.Current.GraphicsDM.GraphicsDevice.Clear(Color.Transparent);

            DrawBackground();
            backgroundCannons.Draw();
            ParticleLayers.Current.DrawBackgroundEffects(gameTime);

            DrawForeground();

            Graphics.Current.Device.SetRenderTarget(Graphics.Current.LightTarget);
            Graphics.Current.GraphicsDM.GraphicsDevice.Clear(Color.Transparent);
            DrawLightSources();

            Graphics.Current.Device.SetRenderTarget(null);
            Graphics.Current.GraphicsDM.GraphicsDevice.Clear(Color.Black);

            // Draw World
            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(Graphics.Current.GameWorldTarget, new Rectangle(0, 0, Graphics.Current.ScreenWidth, Graphics.Current.ScreenHeight), Color.White);
            Graphics.Current.SpriteB.End();

            // Draw Light Sources
            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(Graphics.Current.LightTarget, new Rectangle(0, 0, Graphics.Current.ScreenWidth, Graphics.Current.ScreenHeight), Color.White);
            Graphics.Current.SpriteB.End();

            

            foregroundCannons.Draw();
            ParticleLayers.Current.DrawForegroundEffects(gameTime);
            Windows.Draw();

            base.Draw(gameTime);
        }

        protected void DrawLightSources()
        {
            // Draw Black Rectangle:
            float MaxDarkness = 0.8f;
            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.DarknessTexture,
                new Rectangle(0, 0, Graphics.Current.ScreenWidth, Graphics.Current.ScreenHeight),
                Color.Black * Math.Min(Graphics.Current.Darkness, MaxDarkness));
            Graphics.Current.SpriteB.End();

            // Subtract from the Black Rectangle:
            Graphics.Current.SpriteB.Begin(SpriteSortMode.Deferred, Graphics.Current.BlendStateSubtract);
            foreach (var effect in ParticleLayers.Current.BackgroundEffects)
            {
                Vector2 pos = effect.ParticleEffect.Position;
                int dim = (int)(effect.Radius * 65 * (1 - effect.PercentLifeLeft));
                int drawx = (int)(pos.X - dim / 2);
                int drawy = (int)(pos.Y - dim / 2);
                float sizetocolorfactor = 1.25f - (Firework.MAX_RADIUS - effect.Radius) / Firework.MAX_RADIUS;
                Color color = 1.4f * sizetocolorfactor * (Color.Black * (effect.PercentLifeLeft));
                Graphics.Current.SpriteB.Draw(Graphics.Current.SpritesByName["light"], new Rectangle(drawx, drawy, dim, dim), color);
                
            }

            foreach (Window window in Windows.AllWindows)
            {
                Graphics.Current.SpriteB.Draw(Graphics.Current.SpritesByName["light"], window.LightRectangle, Color.Black * window.Transparency);
            }
            Graphics.Current.SpriteB.End();
        }
        protected void UpdateDarkness()
        {
            Graphics.Current.Darkness += Graphics.Current.DarknessRate;

            if (Graphics.Current.Darkness <= Graphics.MIN_DARKNESS) { Graphics.Current.DarknessRate = Graphics.DARKNESS_RATE; }
            else if (Graphics.Current.Darkness > Graphics.MAX_DARKNESS) { Graphics.Current.DarknessRate = -Graphics.DARKNESS_RATE; }
        }

        protected void Setup()
        {
            // Any logic that needs to run at the beginning of the game only:
            backgroundCannons = new CannonSuite(CannonSuite.SuiteLayer.Background);
            foregroundCannons = new CannonSuite(CannonSuite.SuiteLayer.Foreground);
            Windows = new WindowManager();
            RunSetup = false;
        }

        protected void DrawBackground()
        {
            // Draw your background image here if you want one:

            Rectangle destinationRectangle = new Rectangle(0, 0, Graphics.Current.ScreenWidth, Graphics.Current.ScreenHeight);
            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["bg"],
                destinationRectangle,
                Color.White
            );
            Graphics.Current.SpriteB.End();
        }
        protected void DrawForeground()
        {
            Rectangle destinationRectangle = new Rectangle(0, 0, Graphics.Current.ScreenWidth, Graphics.Current.ScreenHeight);
            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(
                Graphics.Current.SpritesByName["buildings"],
                destinationRectangle,
                Color.White
            );
            Graphics.Current.SpriteB.End();
        }
        protected void CheckInput()
        {
            loadFrames++;
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if (loadFrames >= LOAD_FRAMES_THRESH)
            {
                // Check if any key was pressed
                if (_currentKeyboardState.GetPressedKeys().Length > 0 && _previousKeyboardState.GetPressedKeys().Length == 0)
                {
                    Exit();
                }

                // Check if the mouse has moved
                Vector2 currentPos = new Vector2(_currentMouseState.Position.X, _currentMouseState.Position.Y);
                Vector2 previousPos = new Vector2(_previousMouseState.Position.X, _previousMouseState.Position.Y);
                if (Global.ApproxDist(currentPos, previousPos) >= 1)
                {
                    Exit();
                }
            }
        }
    }
}