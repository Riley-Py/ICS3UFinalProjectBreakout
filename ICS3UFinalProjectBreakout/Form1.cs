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
            //time.Start();
           
            
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
        int tracker = 3;
        string gameState = "Start";
        
        

        Stopwatch time = new Stopwatch();

        Bitmap hearts = new Bitmap(Properties.Resources.Heart, 30, 30);
        Bitmap breakoutTitle0 = new Bitmap(Properties.Resources.breakout_title, 130, 130);










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
            //e.Graphics.FillRectangle(whiteBrush, borderLeft);
            //e.Graphics.FillRectangle(whiteBrush, borderRight);

            //e.Graphics.FillRectangle(whiteBrush, player1);
            //e.Graphics.FillRectangle(whiteBrush, leftPlayer1);
            //e.Graphics.FillRectangle(whiteBrush, rightPlayer1);

            //e.Graphics.FillEllipse(whiteBrush, ball);

            for (int i = 0; i < initialBlocks.Count(); i++)
            {
                e.Graphics.FillRectangle(whiteBrush, initialBlocks[i]);
                       
            }
            if (gameState == "Start")
            {
                this.Width = 500;
                this.Height = 325;
                this.Controls[3].Visible = true;
                this.Controls[7].Visible = true;
                this.Controls[8].Visible = true;
                this.Controls[9].Visible = true;

            }
            if (gameState == "Difficulty")
            {
                this.Controls[3].Visible = false;
                this.Controls[7].Visible = false;
                this.Controls[8].Visible = false;
                this.Controls[9].Visible = false;

                this.Controls[10].Visible = true;
                this.Controls[11].Visible = true;
                this.Controls[12].Visible = true;
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
            score.Visible = false;
            score.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(score);

            Label timeLabel = new Label();
            timeLabel.Text = $"Time:\n 00:00";
            timeLabel.Location = new Point(375, 150);
            timeLabel.Font = new Font("Consolas", 12);
            timeLabel.ForeColor = Color.White;
            timeLabel.AutoSize = true;  
            timeLabel.Visible = false;
            timeLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(timeLabel);

            Label lives = new Label();
            lives.Text = "Lives:";
            lives.Location = new Point(380, 220);
            lives.Font = new Font("Consolas", 12);
            lives.ForeColor = Color.White;
            lives.AutoSize = true;
            lives.Visible = false;
            lives.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lives);

            Label title = new Label();
            title.Text = "BREAKOUT!!!";
            title.Location = new Point(165, 40);
            title.ForeColor = Color.White;
            title.Font = new Font("Consolas", 20, FontStyle.Bold);
            title.Visible = false;
            title.AutoSize = true;
            title.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(title);
            

            PictureBox life1 = new PictureBox();
            life1.Image = hearts;
            life1.Location = new Point(390, 250);
            life1.Visible = false;
            life1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(life1);

            PictureBox life2 = new PictureBox();
            life2.Image = hearts;
            life2.Location = new Point(390, 295);
            life2.Visible = false;
            life2.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(life2);

            PictureBox life3 = new PictureBox();
            life3.Image = hearts;
            life3.Location = new Point(390, 335);
            life3.Visible = false;
            life3.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(life3);

            PictureBox breakoutTitle = new PictureBox();
            breakoutTitle.Image = breakoutTitle0;
            breakoutTitle.Location = new Point(190, 100);
            breakoutTitle.Visible = false;
            breakoutTitle.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(breakoutTitle);

            Button leaderboard = new Button();
            leaderboard.BackColor = this.BackColor;
            leaderboard.Location = new Point(90, 250);
            leaderboard.Visible = false;
            leaderboard.Text = "Leaderboard";
            leaderboard.ForeColor = Color.White;
            leaderboard.Font = new Font("Consolas", 14, FontStyle.Bold);
            leaderboard.AutoSize = true;
            leaderboard.Click += leaderboard_Click;
            this.Controls.Add(leaderboard);

            Button continueButton = new Button();
            continueButton.BackColor = this.BackColor;
            continueButton.Location = new Point(240, 250);
            continueButton.Visible = false;
            continueButton.Text = "Continue";
            continueButton.ForeColor = Color.White;
            continueButton.Click += continuebutton_Click;
            continueButton.Font = new Font("Consolas", 14, FontStyle.Bold);
            continueButton.AutoSize = true;
            this.Controls.Add(continueButton);

            Button easyButton = new Button();
            easyButton.BackColor = Color.Green;
            easyButton.Location = new Point(175, 100);
            easyButton.Visible = false;
            easyButton.Text = "Easy";
            easyButton.ForeColor = Color.LightGreen;
            easyButton.Font = new Font("Consolas", 16, FontStyle.Bold);
            easyButton.AutoSize = true;
            this.Controls.Add(easyButton);

            Button mediumButton = new Button();
            mediumButton.BackColor = Color.Yellow;
            mediumButton.Location = new Point(175, 130);
            mediumButton.Visible = false;
            mediumButton.Text = "Medium";
            mediumButton.ForeColor = Color.LightYellow;
            mediumButton.Font = new Font("Consolas", 16, FontStyle.Bold);
            mediumButton.AutoSize = true;
            this.Controls.Add(mediumButton);

            Button hardButton = new Button();
            hardButton.BackColor = Color.Red;
            hardButton.Location = new Point(175, 160);
            hardButton.Visible = false;
            hardButton.Text = "Hard";
            hardButton.ForeColor = Color.IndianRed;
            hardButton.Font = new Font("Consolas", 16, FontStyle.Bold);
            hardButton.AutoSize = true;
            this.Controls.Add(hardButton);




        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            
            blockCreation();
            player1Movement();
            ballPlayerCollision();
            seeTime();
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
                lives();
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

        public void lives()
        {
            
           this.Controls[tracker].Visible = false;
           tracker++;
            
            if (tracker == 6)
            {
                timer1.Stop();
            }
        }

        private void leaderboard_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Works!");
        }
        private void continuebutton_Click(object sender, EventArgs e)
        {
            gameState = "Difficulty";
        }

    }   
}
