﻿using System;
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
        int incrementy = 25;
        int y1 = 0;
        int width = 40;
        int height = 10;
        int playerSpeed = 8;
        int ballXSpeed = 4;
        int ballYSpeed = 4;
        int space = 10;
        

        Stopwatch time = new Stopwatch();
        










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

        private void timer1_Tick(object sender, EventArgs e)
        {


            
            blockCreation();
            player1Movement();
            ballPlayerCollision();
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
                }
                else
                {
                    continue;
                }
            }
            

        }

        private void Breakout_Load(object sender, EventArgs e)
        {
            Label score = new Label();
            score.Text = "Score:\n0";
            score.Location = new Point(450, 500);
            score.Font = new Font("Consolas", 12);
            score.ForeColor = Color.White;
            score.Visible = true;
            this.Controls.Add(score);
        }
    }
}
