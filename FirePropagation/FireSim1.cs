using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FirePropagation
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class FireSim1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D[] TileTextures;
        private Tile[,] Map;
        private Random m_RNG;
        public FireSim1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
            };

            Content.RootDirectory = "Content";
            TileTextures = new Texture2D[3];
            Map = new Tile[100,100];
            m_RNG = new Random();
            for (int i = Map.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = Map.GetLength(1) - 1; j >= 0; j--)
                {
                    Map[i,j] = new TreeTile(0,Map,new Point(i,j), m_RNG);
                }
            }

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TileTextures[0] = Content.Load<Texture2D>(@"Texture/TreeTexture");
            TileTextures[1] = Content.Load<Texture2D>(@"Texture/FireTexture");
            TileTextures[2] = Content.Load<Texture2D>(@"Texture/BurntTexture");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for (int i = Map.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = Map.GetLength(1) - 1; j >= 0; j--)
                {
                    Map[i, j].CurrentTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            for (int i = Map.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = Map.GetLength(1) - 1; j >= 0; j--)
                {
                    Map[i,j].Update(gameTime.TotalGameTime.TotalMilliseconds);
                }
            }

            if (Mouse.GetState().X / 10 > 0 && Mouse.GetState().X / 10 < Map.GetLength(0) && Mouse.GetState().Y  / 10 > 0 && Mouse.GetState().Y / 10 < Map.GetLength(1))
            {
                if (Map[Mouse.GetState().X / 10, Mouse.GetState().Y / 10].TypeTile == (int)TypeOfTile.Tree)
                {
                    Map[Mouse.GetState().X / 10, Mouse.GetState().Y / 10] = Map[Mouse.GetState().X / 10, Mouse.GetState().Y / 10].ToPrimitive();

                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            for (int i = Map.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = Map.GetLength(1) - 1; j >= 0; j--)
                {
                    spriteBatch.Draw(TileTextures[Map[i,j].TypeTile],new Rectangle(i*10,j*10, 10,10), Color.White);
                }
            }
            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
