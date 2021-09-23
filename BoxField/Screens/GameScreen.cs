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
        int sharedX, sharedSize, sharedDir, nextSizeChange;
        Random randGen = new Random();

        //used to draw boxes on screen
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        //create a list to hold a column of boxes   
        List<Box> boxes = new List<Box>();


        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }

        /// <summary>
        /// Set initial game values here
        /// </summary>
        public void OnStart()
        {
            //TODO - set game start values
            sharedX = Convert.ToInt32(Math.Floor(this.Width / 2f));
            sharedSize = 250;
            nextSizeChange = 3;
            sharedDir = 1;
            List<int> tempColors = new List<int>();
            tempColors.Add(randGen.Next(100, 256));
            tempColors.Add(0);
            tempColors.Add(randGen.Next(100, 256));

            boxes.Add(new Box(sharedX - sharedSize, -25, 25, 15, tempColors));
            boxes.Add(new Box(sharedX + sharedSize - 25, -25, 25, 15, tempColors));
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


        private void gameLoop_Tick(object sender, EventArgs e)
        {
            //TODO - update location of all boxes (drop down screen)
            foreach (Box b in boxes)
            {
                b.y += b.speed;
            }

            //TODO - remove box if it has gone off screen
            /*for(int i = boxes.Count - 1;i >= 0;i--)
            {
                if(boxes[i].y > this.Height)
                {
                    boxes.RemoveAt(i);
                }
            }*/

            if(boxes[0].y > this.Height)
            {
                boxes.RemoveAt(0);
            }

            //TODO - add new box if it is time
            if(boxes[boxes.Count-1].y > 0)
            {
                if(sharedX - sharedSize < 25)
                {
                    sharedDir = 1;
                }
                else if(sharedX + sharedSize > this.Width)
                {
                    sharedDir = -1;
                }
                sharedX = sharedX + (randGen.Next(0, 16) * sharedDir);
                if(randGen.Next(0, 11) >= 10)
                {
                    sharedDir = -sharedDir;
                }
                if (sharedSize > 110 && nextSizeChange < 0)
                {
                    sharedSize--;
                    nextSizeChange = 3;
                }
                nextSizeChange--;
                List<int> tempColors = new List<int>();
                tempColors.Add(randGen.Next(100, 256));
                tempColors.Add(0);
                tempColors.Add(randGen.Next(100, 256));

                boxes.Add(new Box(sharedX - sharedSize, -25, 25, 15, tempColors));
                boxes.Add(new Box(sharedX + sharedSize - 25, -25, 25, 15, tempColors));
            }

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
        }
    }
}
