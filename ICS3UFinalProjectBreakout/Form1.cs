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
        int ballXSpeed; 
        int ballYSpeed;  
        int score1 = 0;
        int tracker = 4;
        int fallSpeed;
        string gameState = "Start";
        
        
        Stopwatch time = new Stopwatch();
        

        Bitmap hearts = new Bitmap(Properties.Resources.Heart, 30, 30);
        Bitmap breakoutTitle0 = new Bitmap(Properties.Resources.breakout_title, 130, 130);
        Bitmap endlessPicture = new Bitmap(Properties.Resources.endless, 140, 120);

        TimeSpan overAllTime;

        

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
            
            if (gameState == "Start")
            {
                this.Width = 500;
                this.Height = 325;
                this.Controls[3].Visible = true;
                this.Controls[7].Visible = true;
                this.Controls[8].Visible = true;
                this.Controls[9].Visible = true;
                this.Controls[16].Visible = true;

            }
            if (gameState == "Difficulty")
            {
                this.Controls[3].Visible = false;
                this.Controls[7].Visible = false;
                this.Controls[8].Visible = false;
                this.Controls[9].Visible = false;
                this.Controls[16].Visible = false;

                this.Controls[10].Visible = true;
                this.Controls[11].Visible = true;
                this.Controls[12].Visible = true;
                this.Controls[13].Visible = true;
            }
            if (gameState == "Mode")
            {
                this.Controls[10].Visible = false;
                this.Controls[11].Visible = false;
                this.Controls[12].Visible = false;
                this.Controls[13].Visible = false;

                this.Controls[14].Visible = true;
                this.Controls[15].Visible = true;
                this.Controls[17].Visible = true;

            }
            if (gameState == "Endless")
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
            if (gameState == "Game Over")
            {
                this.Width = 500;
                this.Height = 325;
                this.Controls[0].Visible = false;
                this.Controls[1].Visible = false;
                this.Controls[2].Visible = false;
                this.Controls[4].Visible = false;
                this.Controls[5].Visible = false;
                this.Controls[6].Visible = false;

                this.Controls[18].Visible = true;
                this.Controls[19].Visible = true;
                this.Controls[20].Visible = true;

                this.Controls[19].Text = $"Your score is: {score1}";
                this.Controls[20].Text = $"Time you endured: {overAllTime.Minutes}:{overAllTime.Seconds}";
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
            leaderboard.Location = new Point(60, 250);
            leaderboard.Visible = false;
            leaderboard.Text = "Leaderboard";
            leaderboard.ForeColor = Color.White;
            leaderboard.Font = new Font("Consolas", 14, FontStyle.Bold);
            leaderboard.AutoSize = true;
            leaderboard.Click += leaderboard_Click;
            this.Controls.Add(leaderboard);

            Button continueButton = new Button();
            continueButton.BackColor = this.BackColor;
            continueButton.Location = new Point(205, 250);
            continueButton.Visible = false;
            continueButton.Text = "Continue";
            continueButton.ForeColor = Color.White;
            continueButton.Click += continuebutton_Click;
            continueButton.Font = new Font("Consolas", 14, FontStyle.Bold);
            continueButton.AutoSize = true;
            this.Controls.Add(continueButton);

            Button easyButton = new Button();
            easyButton.BackColor = Color.Green;
            easyButton.Location = new Point(205, 100);
            easyButton.Visible = false;
            easyButton.Text = "Easy";
            easyButton.ForeColor = Color.LightGreen;
            easyButton.Font = new Font("Consolas", 16, FontStyle.Bold);
            easyButton.AutoSize = true;
            easyButton.Click += easybutton_Click;
            this.Controls.Add(easyButton);

            Button mediumButton = new Button();
            mediumButton.BackColor = Color.Yellow;
            mediumButton.Location = new Point(200, 150);
            mediumButton.Visible = false;
            mediumButton.Text = "Medium";
            mediumButton.ForeColor = Color.LightYellow;
            mediumButton.Font = new Font("Consolas", 16, FontStyle.Bold);
            mediumButton.AutoSize = true;
            mediumButton.Click += mediumbutton_Click;
            this.Controls.Add(mediumButton);

            Button hardButton = new Button();
            hardButton.BackColor = Color.Red;
            hardButton.Location = new Point(205, 200);
            hardButton.Visible = false;
            hardButton.Text = "Hard";
            hardButton.ForeColor = Color.IndianRed;
            hardButton.Font = new Font("Consolas", 16, FontStyle.Bold);
            hardButton.AutoSize = true;
            hardButton.Click += hardbutton_Click;
            this.Controls.Add(hardButton);

            Label difficulty = new Label();
            difficulty.BackColor = this.BackColor;
            difficulty.Location = new Point(110, 30);
            difficulty.Visible = false;
            difficulty.Text = "Choose your difficulty level";
            difficulty.ForeColor = Color.White;
            difficulty.Font = new Font("Consolas", 14, FontStyle.Bold);
            difficulty.AutoSize = true;
            this.Controls.Add(difficulty);

            Label mode = new Label();
            mode.BackColor = this.BackColor;
            mode.Location = new Point(150, 20);
            mode.Visible = false;
            mode.Text = "Choose your mode:";
            mode.ForeColor = Color.White;
            mode.Font = new Font("Consolas", 16, FontStyle.Bold);
            mode.AutoSize = true;
            this.Controls.Add(mode);

            Button endLessButton = new Button();
            endLessButton.BackColor = Color.Blue;
            endLessButton.Location = new Point(85, 255);
            endLessButton.Visible = false;
            endLessButton.Text = "Endless";
            endLessButton.ForeColor = Color.LightBlue;
            endLessButton.Font = new Font("Consolas", 14, FontStyle.Bold);
            endLessButton.AutoSize = true;
            endLessButton.Click += endlessbutton_Click;
            this.Controls.Add(endLessButton);

            Button exit = new Button();
            exit.BackColor = this.BackColor;
            exit.ForeColor = Color.White;
            exit.Location = new Point(325, 250);
            exit.Visible = false;
            exit.Text = "Exit";
            exit.Font = new Font("Consolas", 14, FontStyle.Bold);
            exit.AutoSize = true;
            exit.Click += exit_Click;
            this.Controls.Add(exit);

            PictureBox endlessPictureBox = new PictureBox();
            endlessPictureBox.Image = endlessPicture;
            endlessPictureBox.Location = new Point(65, 100);
            endlessPictureBox.Visible = false;
            endlessPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            endlessPictureBox.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(endlessPictureBox);

            Label gameOverTitle = new Label();
            gameOverTitle.Text = "GAME OVER!";
            gameOverTitle.ForeColor = Color.White;
            gameOverTitle.BackColor = this.BackColor;
            gameOverTitle.Font = new Font("Consolas", 17, FontStyle.Bold);
            gameOverTitle.Location = new Point(200, 180);
            gameOverTitle.Visible = false;
            gameOverTitle.AutoSize = true;
            this.Controls.Add(gameOverTitle);

            Label finalScore = new Label();
            finalScore.Text = "";
            finalScore.ForeColor = Color.Red;
            finalScore.BackColor = this.BackColor;
            finalScore.Font = new Font("Consolas", 14, FontStyle.Bold);
            finalScore.Location = new Point(200, 230);
            finalScore.Visible = false;
            finalScore.AutoSize = true;
            this.Controls.Add(finalScore);

            
            Label time = new Label();
            time.Text = "";
            time.ForeColor = Color.Blue;
            time.BackColor = this.BackColor;
            time.Font = new Font("Consolas", 14, FontStyle.Bold);
            time.Location = new Point(200, 270);
            time.Visible = false;
            time.AutoSize = true;
            this.Controls.Add(time);









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
            
            if ((spanOfTime.TotalSeconds + 4) % fallSpeed <= 1)
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
            
           
            
            if (tracker == 7)
            {
               
                time.Stop();
                overAllTime = time.Elapsed;
                gameTimer.Stop();
                gameState = "Game Over";
                




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

        private void easybutton_Click(object sender, EventArgs e)
        {
            ballXSpeed = 4;
            ballYSpeed = 4;
            fallSpeed = 12;
            gameState = "Mode";

        }
        private void mediumbutton_Click(object sender, EventArgs e)
        {
            ballXSpeed = 6;
            ballYSpeed = 6;
            fallSpeed = 10;
            gameState = "Mode";

        }
        private void hardbutton_Click(object sender, EventArgs e)
        {
            ballXSpeed = 8;
            ballYSpeed = 8;
            fallSpeed = 8;
            gameState = "Mode";

        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void endlessbutton_Click(object sender, EventArgs e)
        {
            EndlessGameInit();
        }
        public void EndlessGameInit()
        {
            gameState = "Endless";

            time.Start();
            gameTimer.Enabled = true;
            gameTimer.Start();

            this.Width = 450;
            this.Height = 400;

            this.Controls[14].Visible = false;
            this.Controls[15].Visible = false;
            this.Controls[17].Visible = false;
            

            this.Controls[0].Visible = true;
            this.Controls[1].Visible = true;
            this.Controls[2].Visible = true;
            this.Controls[4].Visible = true;
            this.Controls[5].Visible = true;
            this.Controls[6].Visible = true;
            

            this.Focus();


        }
        

    }   
}
