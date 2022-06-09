using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace ICS3UFinalProjectBreakout
{
    public partial class Breakout : Form
    {
        public Breakout()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(26, 26, 26);

            //*Temporary time placement* Insert in gameinit function
            time.Start();
           
            
        }

        bool leftArrowDown = false;
        bool rightArrowDown = false;

        Rectangle borderLeft = new Rectangle(100 ,0, 10, 400);
        Rectangle borderRight = new Rectangle(340, 0, 10, 400);

        Rectangle player1 = new Rectangle(220, 375, 30, 10);
        Rectangle leftPlayer1 = new Rectangle(220, 375, 1, 10);
        Rectangle rightPlayer1 = new Rectangle(250, 375, 1, 10);

        Rectangle ball = new Rectangle(230, 300, 15, 15);


        SolidBrush whiteBrush = new SolidBrush(Color.White);

        

        List<Rectangle> initialBlocks = new List<Rectangle>();
        
        

        int x1 = 130;
        int incrementx = 50;
        int incrementy = 15;
        int y1 = 0;
        int width = 40;
        int height = 10;
        int playerSpeed = 8;
        int ballXSpeed = 4;
        int ballYSpeed = 4;
        int score1 = 0;
        
        

        Stopwatch time = new Stopwatch();

        Bitmap hearts = new Bitmap(Properties.Resources.hearts3, 50, 50);










        private void Breakout_KeyUp(object sender, KeyEventArgs e)
        {
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

        private void Breakout_KeyDown(object sender, KeyEventArgs e)
        {
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

        private void Breakout_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(whiteBrush, borderLeft);
            e.Graphics.FillRectangle(whiteBrush, borderRight);

            e.Graphics.FillRectangle(whiteBrush, player1);
            e.Graphics.FillRectangle(whiteBrush, leftPlayer1);
            e.Graphics.FillRectangle(whiteBrush, rightPlayer1);

            e.Graphics.FillEllipse(whiteBrush, ball);

            for (int i = 0; i < initialBlocks.Count(); i++)
            {
                e.Graphics.FillRectangle(whiteBrush, initialBlocks[i]);
                       
            }
        }
        private void Breakout_Load(object sender, EventArgs e)
        {
            //Note: make everything invisible once done with the lives
            Label score = new Label();
            score.Text = "Score:\n 0";
            score.Location = new Point(375, 70);
            score.Font = new Font("Consolas", 12);
            score.ForeColor = Color.White;
            score.AutoSize = true;
            score.Visible = true;
            score.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(score);

            Label timeLabel = new Label();
            timeLabel.Text = $"Time:\n 00:00";
            timeLabel.Location = new Point(375, 150);
            timeLabel.Font = new Font("Consolas", 12);
            timeLabel.ForeColor = Color.White;
            timeLabel.AutoSize = true;  
            timeLabel.Visible = true;
            timeLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(timeLabel);

            PictureBox life1 = new PictureBox();
            life1.Image = hearts;
            life1.Location = new Point(375, 200);
            life1.Visible = true;
            life1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(life1);

            
            


        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            
            blockCreation();
            player1Movement();
            ballPlayerCollision();
            seeTime();
            //lives();
            Refresh();
        }

        public void blockCreation()
        {
          
            //Initial generation of all of the blocks
            while (initialBlocks.Count() < 40)
            {
                initialBlocks.Add(new Rectangle(x1, y1, width, height));
                x1 += incrementx;

                if (initialBlocks.Count() % 4 == 0)
                {
                    x1 = 130;
                    y1 += incrementy;

                }
                if (initialBlocks.Count() >= 40)
                {
                    y1 = 0;
                    x1 = 130;

                    break;
                }
            }
         
            TimeSpan spanOfTime = time.Elapsed;
            
            if ((spanOfTime.TotalSeconds + 4) % 12 <= 1)
            {
                for (int i = 0; i < initialBlocks.Count(); i++)
                {
                    initialBlocks[i] = new Rectangle(initialBlocks[i].X, initialBlocks[i].Y + 2, width, height);
                }
            }

            else if ((initialBlocks.All(ele => ele.Y != y1)))
            {
                for (int i = 0; i < 4; i++)
                {
                    initialBlocks.Insert(i, new Rectangle(x1, initialBlocks[i].Y - incrementy, width, height)); ;
                    x1 += incrementx;
                    
                }
                x1 = 130;
                

            }
            
            
           
        }
        public void player1Movement()
        {
            if (leftArrowDown == true && leftPlayer1.IntersectsWith(borderLeft) == false)
            {
                player1.X -= playerSpeed;
                leftPlayer1.X -= playerSpeed;
                rightPlayer1.X -= playerSpeed;


            }
            if (rightArrowDown == true && rightPlayer1.IntersectsWith(borderRight) == false)
            {
                player1.X += playerSpeed;
                leftPlayer1.X += playerSpeed;
                rightPlayer1.X += playerSpeed;
            }

        }
        public void ballPlayerCollision()
        {
            ball.X += ballXSpeed;
            ball.Y += ballYSpeed;

            if (ball.IntersectsWith(player1))
            {
                ballYSpeed *= -1;
            }
            else if (ball.IntersectsWith(borderRight) || ball.IntersectsWith(borderLeft))
            {
                ballXSpeed *= -1;
                
            }
            else if (ball.Y >= this.Height)
            {
                ball.X = 230;
                ball.Y = 300;
            }
            else if (ball.IntersectsWith(leftPlayer1))
            {
                ballXSpeed *= -1;
                ball.X = player1.X - ball.Width;
            }
            else if (ball.IntersectsWith(rightPlayer1))
            {
                ballXSpeed *= -1;
                ball.X = player1.X + ball.Width;

            }
            for (int i = 0; i < initialBlocks.Count(); i++)
            {
                if (ball.IntersectsWith(initialBlocks[i]))
                {
                    initialBlocks.RemoveAt(i);
                    ballYSpeed *= -1;
                    scoring();
                    
                }
                else
                {
                    continue;
                }
            }
            

        }
        public void scoring()
        {
            score1 += 5;
            this.Controls[0].Text = $"Score:\n {score1}";
        }
        public void seeTime()
        {
            TimeSpan currentTime = time.Elapsed;
            if (currentTime.Seconds >= 10)
            {
                this.Controls[1].Text = $"Time: \n{currentTime.Minutes}:{currentTime.Seconds}";

            }
            else if (currentTime.Seconds < 10)
            {
                this.Controls[1].Text = $"Time: \n{currentTime.Minutes}:0{currentTime.Seconds}";
            }
            
        }

    }   
}
