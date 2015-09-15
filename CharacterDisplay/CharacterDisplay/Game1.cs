using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CharacterDisplay
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // The Game World
        Texture2D cartoonCharacter;
        Texture2D dagger;
        Texture2D bg;
        Rectangle cartoonRect;
        Rectangle daggerRect;
        Rectangle bgRect;

        // Cartoon Position
        int CharacterX = 30;
        int CharacterY = 10;

        // Dagger Position
        int DaggerX = 1000;
        int DaggerY = 420;

        // Character Colour
        Color charColor = Color.White;

        // Player Starts Moving
        Boolean startGame = false;

        // Font
        SpriteFont font;
        SpriteFont points;

        // Health Bar
        int healthScore = 100;
        Color healthScoreColor = Color.Black;
        Boolean decreaseHealth = true;
        Boolean characterJump = true;

        // Add Points
        int gamePoints = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            int windowHeight = Window.ClientBounds.Height;
            int recHeight = 178;
            int recWidth = 140;
            CharacterY = windowHeight - recHeight;

            bgRect = new Rectangle(0,0,Window.ClientBounds.Width,Window.ClientBounds.Height);
            cartoonRect = new Rectangle(CharacterX, CharacterY, recWidth, recHeight);
            daggerRect = new Rectangle(DaggerX, DaggerY, 143, 54);
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

            bg = Content.Load<Texture2D>("bg_game");

            cartoonCharacter = this.Content.Load<Texture2D>("cartoonCharacterR");
            dagger = this.Content.Load<Texture2D>("dagger");

            font = Content.Load<SpriteFont>("GameFont");
            points = Content.Load<SpriteFont>("GameFont"); 

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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

            if(GamePad.GetState(PlayerIndex.One).IsConnected){

                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();

                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                {
                    startGame = true;
                }

                # region player move - right, left, up, down controls

                if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed) {

                    CharacterX+=5;
                    cartoonRect = new Rectangle(CharacterX, CharacterY, 140, 178);
                    cartoonCharacter = this.Content.Load<Texture2D>("cartoonCharacterR");

                }
            
                if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                {

                    CharacterX-=5;
                    cartoonRect = new Rectangle(CharacterX, CharacterY, 140, 178);
                    cartoonCharacter = this.Content.Load<Texture2D>("cartoonCharacterL");

                }

                /*
                if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed )
                {

                    CharacterY += 2;
                    cartoonRect = new Rectangle(CharacterX, CharacterY, 140, 178);

                }
                */
                //&& CharacterY < (Window.ClientBounds.Height - 178)-30

                /*
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                {
                    cartoonRect = new Rectangle(CharacterX, GraphicsDevice.Viewport.Width - 178, 140, 178);
                    CharacterY = Window.ClientBounds.Height - 178;
                }
                */

                /*
                if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                {

                    CharacterY-=2;
                    cartoonRect = new Rectangle(CharacterX, CharacterY, 140, 178);

                }
                */

                #endregion

                #region Character Jumping

                GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
                // TODO: Player Should jump over the Dagger When flying towards character 
                if (pad1.Buttons.A == ButtonState.Pressed && characterJump == true)
                {
                        CharacterY -= 20;
                        cartoonRect = new Rectangle(CharacterX, CharacterY, 140, 178);
                        characterJump = false;
                
                }

                if( pad1.Buttons.A == ButtonState.Released && CharacterY <= (Window.ClientBounds.Height - 178)){

                    CharacterY += 5;
                    cartoonRect = new Rectangle(CharacterX, CharacterY, 140, 178);

                }

                if( characterJump == false && CharacterY >= 200){
                    CharacterY -= 1;
                    cartoonRect = new Rectangle(CharacterX, CharacterY, 140, 178);

                    characterJump = true;
                }

                #endregion

            }   

            # region movement of Dagger

            if (startGame == true)
            {

                // Place Dagger in Position
                if(DaggerX >= -(dagger.Width)){

                    DaggerX -= 4;
                    daggerRect = new Rectangle(DaggerX, DaggerY, 143, 53);
                    dagger = this.Content.Load<Texture2D>("Dagger");
            
                }else{

                    DaggerX = Window.ClientBounds.Width + dagger.Width;
                    daggerRect = new Rectangle(DaggerX, DaggerY, 143, 53);
                    dagger = this.Content.Load<Texture2D>("Dagger");

                }

            }

            #endregion

            #region Inflict damange on character when weapon hits character

            Boolean daggerFront = (DaggerX <= (CharacterX + cartoonCharacter.Width) && 
                DaggerX >= CharacterX && 
                DaggerY <= (CharacterY + cartoonCharacter.Height) &&
                DaggerY >= CharacterY);

            if (daggerFront)
            {
                charColor = Color.IndianRed;

                if(decreaseHealth){
                    healthScore -= 10;
                    decreaseHealth = false;
                }
            }
            else { charColor = Color.White; decreaseHealth = true; }

            if(healthScore<=50){
                healthScoreColor = Color.Red;
            }

            #endregion

            #region Add points to Character
            if (DaggerX + dagger.Width <= CharacterX) {
                gamePoints++;
            }
            #endregion

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            Vector2 textVector = new Vector2(10, 10);
            Vector2 pointsVector = new Vector2((Window.ClientBounds.Width - 150), 10);

            spriteBatch.Begin();
            spriteBatch.Draw(bg, bgRect, Color.White);
            spriteBatch.DrawString(font, "Health: " + healthScore + "%", textVector, healthScoreColor);
            spriteBatch.DrawString(points, "Points: " + gamePoints, pointsVector, Color.Black);
            spriteBatch.Draw(cartoonCharacter, cartoonRect, charColor);
            spriteBatch.Draw(dagger, daggerRect, Color.White);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
