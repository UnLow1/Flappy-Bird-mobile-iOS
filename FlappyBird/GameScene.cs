using System;
using System.Collections.Generic;
using CoreGraphics;
using FlappyBird.Model;
using Foundation;
using SpriteKit;
using UIKit;

namespace FlappyBird
{
    public class GameScene : SKScene
    {
        private Player player;
        private SKLabelNode infoLabel;
        private SKLabelNode scoreLabel;
        private bool gameOver = true;
        private readonly List<Obstacle> obstacles = new List<Obstacle>();
        private readonly string INFO_LABEL_TEXT = "Tap to start new game";
        private readonly int DISTANCE_BETWEEN_OBSTACLES = 300;
        private readonly int NUMBER_OF_OBSTACLES = 5;

        protected GameScene(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DidMoveToView(SKView view)
        {
            // Setup your scene here
            player = new Player(Frame.Width, Frame.Height);
            AddChild(player.Sprite);
            infoLabel = CreateInfoLabel();
            scoreLabel = CreateScoreLabel();
            PhysicsWorld.DidBeginContact += DidBeginContact;
        }

        void DidBeginContact(object sender, EventArgs e)
        {
            gameOver = true;
            //this.player.GameOver();
        }


        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            // Called when a touch begins
            if (!gameOver)
                player.Jump();
            else
                StartNewGame();
        }

        public override void Update(double currentTime)
        {
            // Called before each frame is rendered
            if (gameOver)
                GameOver();
            else
            {
                CheckScore();

                obstacles.FindAll(obstacle => obstacle.GetMostRightPoint() < 0)
                .ForEach(obstacle => RemoveChildren(new SKNode[] { obstacle.PipeUp.Sprite, obstacle.PipeDown.Sprite }));

                obstacles.RemoveAll(obstacle => obstacle.GetMostRightPoint() < 0);

                if (obstacles.Count < NUMBER_OF_OBSTACLES)
                {
                    var width = GetLastObstaclePositionX() + DISTANCE_BETWEEN_OBSTACLES;
                    CreateObstacles(width, Frame.Height / 2);
                }

                gameOver = player.Sprite.Position.Y < 0 || player.Sprite.Position.Y > Frame.Height;
            }
        }

        private nfloat GetLastObstaclePositionX()
        {
            return obstacles[obstacles.Count - 1].PipeDown.Sprite.Position.X;
        }

        private SKLabelNode CreateInfoLabel()
        {
            var label = new SKLabelNode("Chalkduster")
            {
                Text = INFO_LABEL_TEXT,
                ZPosition = 1,
                FontSize = 30,
                Position = new CGPoint(Frame.Width / 2, Frame.Height - 200)
            };
            AddChild(label);
            return label;
        }

        private SKLabelNode CreateScoreLabel()
        {
            var label = new SKLabelNode("Chalkduster")
            {
                Text = player.Score.ToString(),
                ZPosition = 1,
                FontSize = 30,
                Position = new CGPoint(Frame.Width / 2, Frame.Height - 100)
            };
            AddChild(label);
            return label;
        }

        private void StartNewGame()
        {
            player.Score = 0;
            scoreLabel.Text = player.Score.ToString();
            infoLabel.Text = String.Empty;
            gameOver = false;
            obstacles.ForEach(obstacle => RemoveChildren(new SKNode[] { obstacle.PipeUp.Sprite, obstacle.PipeDown.Sprite }));
            obstacles.Clear();
            player.SetStartupPosition(Frame.Width, Frame.Height);
            CreateObstacles(Frame.Width - 300, Frame.Height / 2);
        }

        private void CreateObstacles(nfloat width, nfloat height)
        {
            do
            {
                var obstacle = new Obstacle(width, height);
                obstacles.Add(obstacle);
                AddChild(obstacle.PipeUp.Sprite);
                AddChild(obstacle.PipeDown.Sprite);

                width += DISTANCE_BETWEEN_OBSTACLES;
            } while (obstacles.Count < NUMBER_OF_OBSTACLES);
         }

        private void GameOver()
        {
            infoLabel.Text = INFO_LABEL_TEXT;
            player.Sprite.RemoveAllActions();
            obstacles.ForEach(obstacle =>
            {
                obstacle.PipeUp.Sprite.RemoveAllActions();
                obstacle.PipeDown.Sprite.RemoveAllActions();
            });
        }

        private void CheckScore()
        {
            obstacles.FindAll(obstacle => !obstacle.Scored && obstacle.PipeDown.Sprite.Position.X < player.Sprite.Position.X)
            .ForEach(obstacle =>
            {
                obstacle.Scored = true;
                player.Score++;
                scoreLabel.Text = player.Score.ToString();
            });
        }
    }
}
