using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameTest;

public class MarioPhysiscsTest
{
    public PhysicsTest Physics;
    private Texture2D texture;
    private float scale = 1f; 

    //movement variables
    //# changed as neededdddd
     private float moveAcceleration = 1000f;
    private float maxMoveSpeed = 400f;
    private float groundFriction = 800f;
    private float airFriction = 100f;
    private float jump = -500f;
    private float movement = 0f;
    public MarioPhysiscsTest(Texture2D tex)
    {
        texture = tex;
        Physics = new PhysicsTest();
        Physics.position = new Vector2(100, 100); 
    }

    public void Update(GameTime gameTime, KeyboardState keyboard, Rectangle platformRect)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;//delta time


        if (keyboard.IsKeyDown(Keys.D))
        {
            movement = 1f;
        }
        else if (keyboard.IsKeyDown(Keys.A)) {
            movement = -1f; 
        } 
        else //this is needed kek
        {
            movement = 0f;
        }

        Physics.velocity.X += movement * moveAcceleration * dt;
        //limits speed
        if (Physics.velocity.X > maxMoveSpeed){
            Physics.velocity.X = maxMoveSpeed;
        }
        else if (Physics.velocity.X < -maxMoveSpeed){
            Physics.velocity.X = -maxMoveSpeed;
        }
        //friction yay!!!
        if (movement == 0)
        {
            float friction = Physics.isGrounded ? groundFriction : airFriction;
            if (Physics.velocity.X > 0)
            {
                Physics.velocity.X -= friction * dt;
                if (Physics.velocity.X < 0)
                { //stops infinite sliding
                    Physics.velocity.X = 0;
                }
            }
            else if (Physics.velocity.X < 0)
            {
                Physics.velocity.X += friction * dt;
                if (Physics.velocity.X > 0)
                { //stops infinite sliding
                    Physics.velocity.X = 0;
                }
            }
        }
        Physics.velocity.X = MathHelper.Clamp(Physics.velocity.X, -maxMoveSpeed, maxMoveSpeed);
        //soo many friction bugs...
        // Jump
        if (keyboard.IsKeyDown(Keys.W) && Physics.isGrounded)
        {
            Physics.velocity.Y = jump;
            Physics.isGrounded = false;
        }

        // Update physics
        Physics.Update(gameTime);
        // Simple platform collision
        Rectangle playerRect = new Rectangle(
       (int)Physics.position.X,
       (int)Physics.position.Y,
       (int)(texture.Width * scale), // we'll add scale next
       (int)(texture.Height * scale)
   );
    if (playerRect.Intersects(platformRect) && Physics.velocity.Y >= 0)
    {
        // place player on top of platform
        Physics.position.Y = platformRect.Top - playerRect.Height;
        Physics.velocity.Y = 0;
        Physics.isGrounded = true;
    }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Physics.position,null,Color.White,0f,Vector2.Zero,scale,SpriteEffects.None,0f);//so long just to scale down the joke. It worth it tho lol
    }
}