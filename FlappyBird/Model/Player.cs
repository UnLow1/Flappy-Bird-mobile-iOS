using System;
using CoreGraphics;
using SpriteKit;

namespace FlappyBird.Model
{
    public class Player
    {
        public SKSpriteNode Sprite { get; set; }
        public int Score { get; set; } = 0;
        private readonly nfloat GRAVITY = -100;
        private readonly nfloat JUMP_HEIGHT = 130;

        public Player(nfloat width, nfloat height)
        {
            Sprite = new SKSpriteNode("Bird")
            {
                XScale = 0.1f,
                YScale = 0.1f,
                Position = new CGPoint(width / 2.5, height / 2)
            };

            var action = SKAction.MoveBy(0, GRAVITY, 1);
            Sprite.RunAction(SKAction.RepeatActionForever(action));

            Sprite.PhysicsBody = SKPhysicsBody.Create(Sprite.Texture, Sprite.Size);
            Sprite.PhysicsBody.AffectedByGravity = false;
            Sprite.PhysicsBody.UsesPreciseCollisionDetection = true;
            Sprite.PhysicsBody.CategoryBitMask = (uint)PhysicsCategoryEnum.Player;
            Sprite.PhysicsBody.CollisionBitMask = (uint)(PhysicsCategoryEnum.Player | PhysicsCategoryEnum.Pipe);
            Sprite.PhysicsBody.ContactTestBitMask = (uint)(PhysicsCategoryEnum.Player | PhysicsCategoryEnum.Pipe);;
            Sprite.PhysicsBody.Mass = 0.1f;
        }

        public void Jump()
        {
            Sprite.RunAction(SKAction.MoveBy(0, JUMP_HEIGHT, 0.5));
        }

        public void SetStartupPosition(nfloat width, nfloat height)
        {
            Sprite.Position = new CGPoint(width / 2.5, height / 2);
            var action = SKAction.MoveBy(0, GRAVITY, 1);
            Sprite.RunAction(SKAction.RepeatActionForever(action));
        }

        public void GameOver()
        {
            //this.player.Sprite.RunAction(SKAction.RotateByAngle(NMath.PI, 1));
            //this.player.Sprite.RunAction(SKAction.MoveBy(0,200,0.5));
            //this.player.Sprite.RunAction(SKAction.MoveBy(0, -800, 2));
        }
    }
}
