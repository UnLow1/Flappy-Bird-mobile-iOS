using System;
using CoreGraphics;

namespace FlappyBird.Model
{
    public class Obstacle
    {
        public Pipe PipeDown { get; set; }
        public Pipe PipeUp { get; set; }
        public bool Scored { get; set; } = false;
        private readonly int MIN_GAP = 630;
        private readonly int MAX_GAP = 850;
        private readonly int MOVE_GAP_Y_RADIUS = 150;

        public Obstacle(nfloat width, nfloat height)
        {
            Random rand = new Random();
            var gap = rand.Next(MIN_GAP, MAX_GAP);
            var moveGapY = rand.Next(-MOVE_GAP_Y_RADIUS, MOVE_GAP_Y_RADIUS);
            
            var position = new CGPoint(width, height - gap / 2 - moveGapY);
            PipeDown = new Pipe(position);

            position.Y = height + gap / 2 - moveGapY;
            PipeUp = new Pipe(position, true);
        }

        public nfloat GetMostRightPoint()
        {
            return PipeDown.Sprite.Position.X + PipeDown.Sprite.Size.Width;
        }
    }
}
