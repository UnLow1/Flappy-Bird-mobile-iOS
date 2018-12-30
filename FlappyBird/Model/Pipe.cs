using System;
using CoreGraphics;
using SpriteKit;

namespace FlappyBird.Model
{
    public class Pipe
    {
        public SKSpriteNode Sprite { get; set; }

        public Pipe(CGPoint position, bool isRotated = false)
        {
            Sprite = new SKSpriteNode("Pipe")
            {
                Position = position,
                XScale = 0.5f,
                YScale = 1.5f
            };
            Sprite.PhysicsBody = SKPhysicsBody.Create(Sprite.Texture, Sprite.Size);
            Sprite.PhysicsBody.AffectedByGravity = false;
            Sprite.PhysicsBody.CategoryBitMask = (uint)PhysicsCategoryEnum.Pipe;
            Sprite.PhysicsBody.UsesPreciseCollisionDetection = true;
            Sprite.RunAction(SKAction.RepeatActionForever(SKAction.MoveBy(-100, 0, 1)));
            if (isRotated)
                Sprite.RunAction(SKAction.RotateByAngle(NMath.PI, 0));
        }
    }
}
