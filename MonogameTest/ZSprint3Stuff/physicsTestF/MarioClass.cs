using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameTest;

public class MarioPhysiscsTest : ICamera
{
    private Dictionary<string, Animation> _animations;
    private AnimationPlayer _animationPlayer;
    public Vector2 Position;
    public PhysicsTest Physics;
    private Texture2D texture;
    private float scale = 0.15f;

    //movement variables
    //# changed as neededdddd
    private IPlayerState playerState;
     private float moveAcceleration = 1000f;
    private float maxMoveSpeed = 400f;
    private float groundFriction = 800f;
    private float airFriction = 100f;
    private float jump = -500f;
    private float movement = 0f;
    public MarioPhysiscsTest(Texture2D tex, Dictionary<string, Animation> animations)
    {
        texture = tex;
        Physics = new PhysicsTest();
        Physics.position = new Vector2(128, 80);
        _animations = animations;
        _animationPlayer = new AnimationPlayer();
        _animationPlayer.Play(_animations["Idle"]);
        //ChangeState(new PlayerIdleState());
    }
    public void SetAnimation(string name)
    {
        if (_animations.ContainsKey(name))
        {
            _animationPlayer.Play(_animations[name]);
        }
    }

    public void Update(GameTime gameTime, KeyboardState keyboard, Rectangle platformRect)
    {
        _animationPlayer.Update(gameTime);
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
        _animationPlayer.Draw(spriteBatch, Position);
        spriteBatch.Draw(texture, Physics.position,null,Color.White,0f,Vector2.Zero,scale,SpriteEffects.None,0f);//so long just to scale down the joke. It worth it tho lol
    }

    public Matrix GetViewMatrix()
    {
        throw new System.NotImplementedException();
    }

    public void LookAt(Vector2 target)
    {
        target = Physics.position;
    }

    public void Reset(Vector2 startPosition)
    {
        startPosition = Physics.position;
    }
}