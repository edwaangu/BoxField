using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoxField
{
    public partial class GameScreen : UserControl
    {
        //player1 button control keys
        Boolean leftArrowDown, rightArrowDown;
        int sharedX, sharedSize, sharedDir, nextSizeChange, sharedSpeed, nextSpeedChange, sharedSpeedX, patternMode, nextPattern, previousPattern;

        int score = 0;

        Random randGen = new Random();

        //used to draw boxes on screen
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        //create a list to hold a column of boxes   
        List<Box> boxes = new List<Box>();

        //player
        Player p = new Player(0, 0, 20, 10);


        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }

        /// <summary>
        /// Set initial game values here
        /// </summary>
        /// 

        public void UpdateBoxPositions()
        {
            if(patternMode == 0){
                if (sharedX - sharedSize < 25)
                {
                    sharedDir = 1;
                    sharedSpeedX = randGen.Next(5, 20);
                }
                else if (sharedX + sharedSize > this.Width)
                {
                    sharedDir = -1;
                    sharedSpeedX = randGen.Next(5, 20);
                }
                sharedX = sharedX + (sharedSpeedX * sharedDir);
                if (randGen.Next(0, 11) >= 10)
                {
                    sharedDir = -sharedDir;
                    sharedSpeedX = randGen.Next(5, 20);
                }
                if (sharedSize > 110 && nextSizeChange < 0)
                {
                    sharedSize--;
                    nextSizeChange = 1;
                }
                nextSizeChange--;
            }
            else if(patternMode == 1)
            {

            }
            if(sharedSpeed < 20 && nextSpeedChange < 0)
            {
                sharedSpeed++;
                nextSpeedChange = 40;
                foreach(Box b in boxes)
                {
                    b.speed = sharedSpeed;
                }
            }

            if(nextPattern < 0)
            {
                nextPattern = randGen.Next(100, 300);
                previousPattern = 0;
                previousPattern += patternMode;

                patternMode = patternMode == 1 ? 2 : 1;
            }
            nextPattern--;
            nextSpeedChange--;
        }

        public void CreateBoxes()
        {
            List<int> tempColors = new List<int>();
            tempColors.Add(randGen.Next(100, 256));
            tempColors.Add(0);
            tempColors.Add(randGen.Next(100, 256));

            if (patternMode == 0)
            {
                boxes.Add(new Box(sharedX - sharedSize, -25, 25, sharedSpeed, tempColors));
                boxes.Add(new Box(sharedX + sharedSize - 25, -25, 25, sharedSpeed, tempColors));
            }
            else if(patternMode == 1)
            {
                boxes.Add(new Box(randGen.Next(0, this.Width - 25), -35, 25, sharedSpeed, tempColors));
            }
            else if(patternMode == 2)
            {
                sharedSize += 50;
                for(int i = sharedX - sharedSize; i > -25;i -= 30)
                {
                    boxes.Add(new Box(i, -25, 25, sharedSpeed, tempColors));
                }

                for (int i = sharedX + sharedSize - 25; i < this.Width; i += 30)
                {
                    boxes.Add(new Box(i, -25, 25, sharedSpeed, tempColors));
                }
                patternMode = 0;
            }
        }

        public void OnStart()
        {
            //TODO - set game start values
            score = 0;
            sharedSpeedX = 10;
            sharedSpeed = 8;
            nextSpeedChange = 20;
            sharedX = Convert.ToInt32(Math.Floor(this.Width / 2f));
            sharedSize = 200;
            nextSizeChange = 3;
            sharedDir = 1;
            patternMode = 2;
            previousPattern = 0;
            nextPattern = randGen.Next(100, 300);
            CreateBoxes();

            p.x = (this.Width / 2) - (p.size / 2);
            p.y = this.Height - p.size - 30;
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;           
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }

        }

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            //TODO - update location of all boxes (drop down screen)
            foreach (Box b in boxes)
            {
                b.Move();
            }

            //TODO - remove box if it has gone off screen

            if(boxes[0].y > this.Height)
            {
                boxes.RemoveAt(0);
            }

            //TODO - add new box if it is time
            if(boxes[boxes.Count-1].y >= 0)
            {
                UpdateBoxPositions();
                CreateBoxes();
            }

            // Update Player
            if (leftArrowDown)
            {
                p.Move(-1);
            }
            else if (rightArrowDown)
            {
                p.Move(1);
            }

            p.x = (p.x < 0) ? 0 : (p.x > this.Width - p.size ? this.Width - p.size : p.x);

            foreach(Box b in boxes)
            {
                if (p.Collision(b))
                {
                    gameLoop.Enabled = false;
                }
            }

            score++;
            scoreLabel.Text = $"{score}";

            Refresh();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //TODO - draw boxes to screen
            foreach (Box b in boxes)
            {
                whiteBrush.Color = Color.FromArgb(b.color[0], b.color[1], b.color[2]);
                e.Graphics.FillRectangle(whiteBrush, b.x, b.y, b.size, b.size);
            }

            whiteBrush.Color = Color.White;
            e.Graphics.FillRectangle(whiteBrush, p.x, p.y, p.size, p.size);
        }
    }
}
