using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

/***************************************************************************************************************
 * Author: Riley Cant
 * Course: ICS3U
 * Program: Breakout Summative
 * Description: A summative project that is based off the classic Atari game "Breakout", released in May of 1976
 * Date of writing: 06/20/22
 ***************************************************************************************************************/
namespace ICS3UFinalProjectBreakout

{
    public partial class Breakout : Form
    {

        System.Windows.Media.MediaPlayer backgroundMusic = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer buttonPress = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer gameMusic = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer collisionSound = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer gameOver = new System.Windows.Media.MediaPlayer();

        public Breakout()
        {
            InitializeComponent();

            //Sets the back color for the whole game
            this.BackColor = Color.FromArgb(26, 26, 26);

            //Music for the game
            backgroundMusic.Open(new Uri(Application.StartupPath + "/Resources/beginningMusic.mp3"));
            buttonPress.Open(new Uri(Application.StartupPath + "/Resources/buttonPress.mp3"));
            gameMusic.Open(new Uri(Application.StartupPath + "/Resources/gameMusic.mp3"));
            collisionSound.Open(new Uri(Application.StartupPath + "/Resources/collisionSound.mp3"));
            gameOver.Open(new Uri(Application.StartupPath + "/Resources/gameOver.mp3"));
          

            backgroundMusic.MediaEnded += new EventHandler(backgroundMusic_MediaEnded);
            gameMusic.MediaEnded += new EventHandler(gameMusic_MediaEnded);
            gameOver.MediaEnded += new EventHandler(gameOver_MediaEnded);

            backgroundMusic.Play();
    

        }
        //Boolean values for right and left
        bool leftArrowDown = false;
        bool rightArrowDown = false;

        //All of the rectangle stuff for the game (player, borders, ball)
        Rectangle borderLeft = new Rectangle(100 ,0, 10, 400);
        Rectangle borderRight = new Rectangle(340, 0, 10, 400);

        Rectangle player1 = new Rectangle(220, 375, 30, 10);
        Rectangle leftPlayer1 = new Rectangle(220, 375, 1, 10);
        Rectangle rightPlayer1 = new Rectangle(250, 375, 1, 10);

        Rectangle ball = new Rectangle(230, 300, 15, 15);

        //Color of the rectangles

        SolidBrush whiteBrush = new SolidBrush(Color.White);

        //List that stores the blocks
        List<Rectangle> initialBlocks = new List<Rectangle>();

        //Used to write easily to the score file by organizing the player and score without having to add to a list
        Dictionary<string, double> playerScores = new Dictionary<string, double>();
        
        //Initial position of blocks
        int x1 = 130;
        int y1 = 0;
        
        //The incrementation for when the blocks fall down
        int incrementx = 50;
        int incrementy = 15;
       
        //The width and height of the blocks
        int width = 40;
        int height = 10;

        //Player speed
        int playerSpeed = 8;

        //Ball speed
        int ballXSpeed; 
        int ballYSpeed;  

        //Score tracking
        int score1 = 0;

        //Losing the lives (explained more later on)
        int tracker = 4;

        //How quickly the blocks fall down
        int fallSpeed;
      
        //The initial game state to determine what gets drawn first
        string gameState = "Start";
        
        //Tracks when the blocks fall down
        Stopwatch time = new Stopwatch();
        

        //All of the iamges needed in the game
        Bitmap hearts = new Bitmap(Properties.Resources.Heart, 30, 30);
        Bitmap breakoutTitle0 = new Bitmap(Properties.Resources.breakout_title, 130, 130);
        Bitmap endlessPicture = new Bitmap(Properties.Resources.endless, 140, 120);
        Bitmap retryPhoto = new Bitmap(Properties.Resources.retry, 90, 90);

      

        //Overall time at the end of the match
        TimeSpan overAllTime;


        /// <summary>
        /// Key up for Left and Right keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Key down method for Left and Right keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Draws everything to screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Breakout_Paint(object sender, PaintEventArgs e)
        {
            //Beginning of the game
            if (gameState == "Start")
            {
               
                tracker = 4;
                initialBlocks.Clear();
                score1 = 0;
               
                this.Controls[1].Visible = false;
                this.Controls[22].Visible = false;
                this.Controls[23].Visible = false;
                this.Controls[24].Visible = false;
                this.Controls[30].Visible = false;
                this.Controls[31].Visible = false;
               

                this.Width = 500;
                this.Height = 325;
                this.Controls[3].Visible = true;
                this.Controls[7].Visible = true;
                this.Controls[8].Visible = true;
                this.Controls[9].Visible = true;
                this.Controls[16].Visible = true;
                this.Controls[16].Location = new Point(325, 250);

                

            }
            //Selecting the difficulty of the game
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
            //What mode of the game (in this case, only "Endless")
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
            //When the game gets started
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
            //When player has no more lives or when the blocks hit the player
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
                this.Controls[16].Visible = false;
                this.Controls[22].Visible = false;
                this.Controls[23].Visible = false;
                this.Controls[24].Visible = false;

                this.Controls[18].Visible = true;
                this.Controls[19].Visible = true;
                this.Controls[20].Visible = true;
                this.Controls[21].Visible = true;
                this.Controls[25].Visible = true;

                this.Controls[19].Text = $"Your score is: {score1}";
                this.Controls[20].Text = $"Time you endured: {overAllTime.Minutes}:{overAllTime.Seconds}";


            }
            //Asks the player if they want to try again or exit the game
            if (gameState == "Play Again")
            {
                
                this.Controls[18].Visible = false;
                this.Controls[19].Visible = false;
                this.Controls[20].Visible = false;
                this.Controls[21].Visible = false;
                this.Controls[26].Visible = false;
                this.Controls[27].Visible = false;
                this.Controls[25].Visible = false;
                this.Controls[28].Visible = false;

                this.Controls[16].Visible = true;
                this.Controls[16].Location = new Point(320, 230);
                this.Controls[22].Visible = true;
                this.Controls[23].Visible = true;
                this.Controls[24].Visible = true;

  




            }
            if (gameState == "Write leaderboard")
            {
                this.Controls[18].Visible = false;
                this.Controls[19].Visible = false;
                this.Controls[20].Visible = false;
                this.Controls[21].Visible = false;
                this.Controls[25].Visible = false;

                this.Controls[26].Visible = true;
                this.Controls[27].Visible = true;
                this.Controls[28].Visible = true;


                

            }
            if (gameState == "Show Leaderboard")
            {
                this.Controls[3].Visible = false;
                this.Controls[7].Visible = false;
                this.Controls[8].Visible = false;
                this.Controls[9].Visible = false;
                this.Controls[16].Visible = false;

                this.Controls[29].Visible = true;
                this.Controls[30].Visible = true;
                this.Controls[31].Visible = true;
            }
        }
        /// <summary>
        /// Loads all controls when the form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Breakout_Load(object sender, EventArgs e)
        {
            //Numbers are beside the labels to show what index it is in the list
            
            Label score = new Label(); //0
            score.Text = "Score:\n 0";
            score.Location = new Point(375, 70);
            score.Font = new Font("Consolas", 12);
            score.ForeColor = Color.White;
            score.AutoSize = true;
            score.Visible = false;
            score.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(score);

            Label timeLabel = new Label(); //1
            timeLabel.Text = $"Time:\n 00:00";
            timeLabel.Location = new Point(375, 150);
            timeLabel.Font = new Font("Consolas", 12);
            timeLabel.ForeColor = Color.White;
            timeLabel.AutoSize = true;  
            timeLabel.Visible = false;
            timeLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(timeLabel);

            Label lives = new Label();  //2
            lives.Text = "Lives:";
            lives.Location = new Point(380, 220);
            lives.Font = new Font("Consolas", 12);
            lives.ForeColor = Color.White;
            lives.AutoSize = true;
            lives.Visible = false;
            lives.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lives);

            Label title = new Label();  //3
            title.Text = "BREAKOUT!!!";
            title.Location = new Point(165, 40);
            title.ForeColor = Color.White;
            title.Font = new Font("Consolas", 20, FontStyle.Bold);
            title.Visible = false;
            title.AutoSize = true;
            title.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(title);
            

            PictureBox life1 = new PictureBox();  //4
            life1.Image = hearts;
            life1.Location = new Point(390, 250);
            life1.Visible = false;
            life1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(life1);

            PictureBox life2 = new PictureBox();  //5
            life2.Image = hearts;
            life2.Location = new Point(390, 295);
            life2.Visible = false;
            life2.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(life2);

            PictureBox life3 = new PictureBox();  //6
            life3.Image = hearts;
            life3.Location = new Point(390, 335);
            life3.Visible = false;
            life3.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(life3);

            PictureBox breakoutTitle = new PictureBox();  //7
            breakoutTitle.Image = breakoutTitle0;
            breakoutTitle.Location = new Point(190, 100);
            breakoutTitle.Visible = false;
            breakoutTitle.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(breakoutTitle);

            Button leaderboard = new Button();  //8
            leaderboard.BackColor = this.BackColor;
            leaderboard.Location = new Point(60, 250);
            leaderboard.Visible = false;
            leaderboard.Text = "Leaderboard";
            leaderboard.ForeColor = Color.White;
            leaderboard.Font = new Font("Consolas", 14, FontStyle.Bold);
            leaderboard.AutoSize = true;
            leaderboard.Click += leaderboard_Click;
            this.Controls.Add(leaderboard);

            Button continueButton = new Button();  //9
            continueButton.BackColor = this.BackColor;
            continueButton.Location = new Point(205, 250);
            continueButton.Visible = false;
            continueButton.Text = "Continue";
            continueButton.ForeColor = Color.White;
            continueButton.Click += continuebutton_Click;
            continueButton.Font = new Font("Consolas", 14, FontStyle.Bold);
            continueButton.AutoSize = true;
            this.Controls.Add(continueButton);

            Button easyButton = new Button();  //10
            easyButton.BackColor = Color.Green;
            easyButton.Location = new Point(205, 100);
            easyButton.Visible = false;
            easyButton.Text = "Easy";
            easyButton.ForeColor = Color.LightGreen;
            easyButton.Font = new Font("Consolas", 16, FontStyle.Bold);
            easyButton.AutoSize = true;
            easyButton.Click += easybutton_Click;
            this.Controls.Add(easyButton);

            Button mediumButton = new Button();  //11
            mediumButton.BackColor = Color.Yellow;
            mediumButton.Location = new Point(200, 150);
            mediumButton.Visible = false;
            mediumButton.Text = "Medium";
            mediumButton.ForeColor = Color.LightYellow;
            mediumButton.Font = new Font("Consolas", 16, FontStyle.Bold);
            mediumButton.AutoSize = true;
            mediumButton.Click += mediumbutton_Click;
            this.Controls.Add(mediumButton);

            Button hardButton = new Button();  //12
            hardButton.BackColor = Color.Red;
            hardButton.Location = new Point(205, 200);
            hardButton.Visible = false;
            hardButton.Text = "Hard";
            hardButton.ForeColor = Color.IndianRed;
            hardButton.Font = new Font("Consolas", 16, FontStyle.Bold);
            hardButton.AutoSize = true;
            hardButton.Click += hardbutton_Click;
            this.Controls.Add(hardButton);

            Label difficulty = new Label();  //13
            difficulty.BackColor = this.BackColor;
            difficulty.Location = new Point(110, 30);
            difficulty.Visible = false;
            difficulty.Text = "Choose your difficulty level";
            difficulty.ForeColor = Color.White;
            difficulty.Font = new Font("Consolas", 14, FontStyle.Bold);
            difficulty.AutoSize = true;
            this.Controls.Add(difficulty);

            Label mode = new Label();  //14
            mode.BackColor = this.BackColor;
            mode.Location = new Point(150, 20);
            mode.Visible = false;
            mode.Text = "Choose your mode:";
            mode.ForeColor = Color.White;
            mode.Font = new Font("Consolas", 16, FontStyle.Bold);
            mode.AutoSize = true;
            this.Controls.Add(mode);

            Button endLessButton = new Button();  //15
            endLessButton.BackColor = Color.Blue;
            endLessButton.Location = new Point(205, 255);
            endLessButton.Visible = false;
            endLessButton.Text = "Endless";
            endLessButton.ForeColor = Color.LightBlue;
            endLessButton.Font = new Font("Consolas", 14, FontStyle.Bold);
            endLessButton.AutoSize = true;
            endLessButton.Click += endlessbutton_Click;
            this.Controls.Add(endLessButton);

            Button exit = new Button();  //16
            exit.BackColor = this.BackColor;
            exit.ForeColor = Color.White;
            exit.Location = new Point(325, 250);
            exit.Visible = false;
            exit.Text = "Exit";
            exit.Font = new Font("Consolas", 14, FontStyle.Bold);
            exit.AutoSize = true;
            exit.Click += exit_Click;
            this.Controls.Add(exit);

            PictureBox endlessPictureBox = new PictureBox();  //17
            endlessPictureBox.Image = endlessPicture;
            endlessPictureBox.Location = new Point(180, 100);
            endlessPictureBox.Visible = false;
            endlessPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            endlessPictureBox.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(endlessPictureBox);

            Label gameOverTitle = new Label();  //18
            gameOverTitle.Text = "GAME OVER!";
            gameOverTitle.ForeColor = Color.White;
            gameOverTitle.BackColor = this.BackColor;
            gameOverTitle.Font = new Font("Consolas", 17, FontStyle.Underline);
            gameOverTitle.Location = new Point(180, 120);
            gameOverTitle.Visible = false;
            gameOverTitle.AutoSize = true;
            this.Controls.Add(gameOverTitle);

            Label finalScore = new Label();  //19
            finalScore.Text = "";
            finalScore.ForeColor = Color.Red;
            finalScore.BackColor = this.BackColor;
            finalScore.Font = new Font("Consolas", 14, FontStyle.Bold);
            finalScore.Location = new Point(170, 160);
            finalScore.Visible = false;
            finalScore.AutoSize = true;
            this.Controls.Add(finalScore);

            
            Label time = new Label();  //20
            time.Text = "";
            time.ForeColor = Color.Blue;
            time.BackColor = this.BackColor;
            time.Font = new Font("Consolas", 14, FontStyle.Bold);
            time.Location = new Point(170, 200);
            time.Visible = false;
            time.AutoSize = true;
            this.Controls.Add(time);

            Button playAgain = new Button();  //21
            playAgain.Text = "Continue";
            playAgain.BackColor = this.BackColor;
            playAgain.ForeColor = Color.White;
            playAgain.Location = new Point(100, 230);
            playAgain.Visible = false;
            playAgain.Font = new Font("Consolas", 14, FontStyle.Bold);
            playAgain.AutoSize = true;
            playAgain.Click += playagain_Click;
            this.Controls.Add(playAgain);

            Label playAgainTitle = new Label();  //22
            playAgainTitle.Text = "Would you like to play again?";
            playAgainTitle.ForeColor = Color.White;
            playAgainTitle.BackColor = this.BackColor;
            playAgainTitle.Font = new Font("Consolas", 16, FontStyle.Underline);
            playAgainTitle.Location = new Point(90, 60);
            playAgainTitle.Visible = false;
            playAgainTitle.AutoSize = true;
            this.Controls.Add(playAgainTitle);

            Button goBack = new Button();  //23
            goBack.Text = "Back to start screen";
            goBack.BackColor = this.BackColor;
            goBack.ForeColor = Color.White;
            goBack.Location = new Point(75, 230);
            goBack.Visible = false;
            goBack.Font = new Font("Consolas", 14, FontStyle.Bold);
            goBack.AutoSize = true;
            goBack.Click += goback_Click;
            this.Controls.Add(goBack);

            PictureBox retryPicture = new PictureBox();  //24
            retryPicture.Image = retryPhoto;
            retryPicture.Location = new Point(170, 100);
            retryPicture.Visible = false;
            retryPicture.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(retryPicture);

            Button leaderBoardButton = new Button();  //25
            leaderBoardButton.Text = "Write to leaderboard";
            leaderBoardButton.BackColor = this.BackColor;
            leaderBoardButton.ForeColor = Color.White;
            leaderBoardButton.Location = new Point(250, 230);
            leaderBoardButton.Visible = false;
            leaderBoardButton.Font = new Font("Consolas", 14, FontStyle.Bold);
            leaderBoardButton.AutoSize = true;
            leaderBoardButton.Click += leaderboardbutton_Click;
            this.Controls.Add(leaderBoardButton);

            Label leaderboardWriteTitle = new Label();  //26
            leaderboardWriteTitle.Text = "Write your name in the box below\nto be featured in the leaderboard!";
            leaderboardWriteTitle.ForeColor = Color.White;
            leaderboardWriteTitle.BackColor = this.BackColor;
            leaderboardWriteTitle.Font = new Font("Consolas", 16, FontStyle.Underline);
            leaderboardWriteTitle.Location = new Point(60, 60);
            leaderboardWriteTitle.Visible = false;
            leaderboardWriteTitle.AutoSize = true;
            this.Controls.Add(leaderboardWriteTitle);

            TextBox namePlayer = new TextBox();  //27
            namePlayer.ForeColor = Color.Black;
            namePlayer.BackColor = Color.White;
            namePlayer.Location = new Point(200, 180);
            namePlayer.Visible = false;
            namePlayer.KeyPress += nameplayer_TextChanged;
            this.Controls.Add(namePlayer);

            Button writeLeaderboard = new Button(); //28
            writeLeaderboard.Text = "Continue";
            writeLeaderboard.BackColor = this.BackColor;
            writeLeaderboard.ForeColor = Color.White;
            writeLeaderboard.Location = new Point(200, 230);
            writeLeaderboard.Visible = false;
            writeLeaderboard.Font = new Font("Consolas", 14, FontStyle.Bold);
            writeLeaderboard.AutoSize = true;
            writeLeaderboard.Click += writeLeaderboard_Click;
            this.Controls.Add(writeLeaderboard);

            Label leaderboardHeading = new Label(); //29
            leaderboardHeading.Text = "LEADERBOARD";
            leaderboardHeading.BackColor = this.BackColor;
            leaderboardHeading.ForeColor = Color.White;
            leaderboardHeading.Location = new Point(180, 40);
            leaderboardHeading.Visible = false;
            leaderboardHeading.Font = new Font("Consolas", 17, FontStyle.Underline);
            leaderboardHeading.AutoSize = true;
            this.Controls.Add(leaderboardHeading);

            Label leaderboardList = new Label(); //30
            leaderboardList.Text = "";
            leaderboardList.BackColor = this.BackColor;
            leaderboardList.ForeColor = Color.White;
            leaderboardList.Location = new Point(190, 80);
            leaderboardList.Visible = false;
            leaderboardList.Font = new Font("Consolas", 14, FontStyle.Bold);
            leaderboardList.AutoSize = true;
            this.Controls.Add(leaderboardList);

            Button backToMenu = new Button();  //31
            backToMenu.Text = "Menu";
            backToMenu.ForeColor = Color.White;
            backToMenu.BackColor = this.BackColor;
            backToMenu.Visible = false;
            backToMenu.Font = new Font("Consolas", 12, FontStyle.Bold);
            backToMenu.Location = new Point(330, 260);
            backToMenu.AutoSize = true;
            backToMenu.Click += backtomenu_Click;
            this.Controls.Add(backToMenu);

            Button clearButton = new Button(); //32
            clearButton.Text = "Clear Scores";
            clearButton.ForeColor = Color.White;
            clearButton.BackColor = this.BackColor;
            clearButton.Visible = false;
            clearButton.AutoSize = true;
            clearButton.Font = new Font("Consolas", 14, FontStyle.Bold);
            clearButton.Location = new Point(75, 260);
            this.Controls.Add(clearButton);


        }
        /// <summary>
        /// Main game loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {


            
            blockCreation();
            player1Movement();
            ballPlayerCollision();
            seeTime();
            sounds();
            Refresh();
        }
        /// <summary>
        /// The block creator for the game
        /// </summary>
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
            //Gets the total time that has elapsed
            TimeSpan spanOfTime = time.Elapsed;
            /*If the span of time + 4 (so that the blocks don't fall down right away) divided by the fall speed that the player sets with mode is
             less than or equal to one, create new blocks, otherwise...*/
            if ((spanOfTime.TotalSeconds + 4) % fallSpeed <= 1)
            {
                for (int i = 0; i < initialBlocks.Count(); i++)
                {
                    initialBlocks[i] = new Rectangle(initialBlocks[i].X, initialBlocks[i].Y + 2, width, height);
                }
            }
            /*...move the blocks down by the x increment and make it seem like blocks are coming off the screen*/
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
        /// <summary>
        /// Movement for the player and collision detection
        /// </summary>
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
        /// <summary>
        /// Ball and player collision with one another and with the blocks
        /// </summary>
        public void ballPlayerCollision()
        {
            ball.X += ballXSpeed;
            ball.Y += ballYSpeed;

            //Switches the Y coordinate if it hits player
            if (ball.IntersectsWith(player1))
            {
                ballYSpeed *= -1;
            }
            //Switches the X coordinate if it hits either of the borders
            else if (ball.IntersectsWith(borderRight) || ball.IntersectsWith(borderLeft))
            {
                ballXSpeed *= -1;
                
            }
            //Resets the ball at the initial position and makes it so that there are lives
            else if (ball.Y >= this.Height)
            {
                ball.X = 230;
                ball.Y = 300;
                lives();
            }
            //Side collision with the player and ball
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
            //Deals with block collision with ball and player
            for (int i = 0; i < initialBlocks.Count(); i++)
            {
                if (ball.IntersectsWith(initialBlocks[i]))
                {
                    initialBlocks.RemoveAt(i);
                    ballYSpeed *= -1;
                    scoring();
                    
                }
                else if (player1.IntersectsWith(initialBlocks[i]))
                {
                    time.Stop();
                    overAllTime = time.Elapsed;
                    gameTimer.Stop();
                    gameState = "Game Over";
                    gameTimer.Dispose();
                    time.Reset();
                    gameOver.Play();
                    

                }
                else
                {
                    continue;
                }
            }
            

        }
        /// <summary>
        /// Deals with score keeping
        /// </summary>
        public void scoring()
        {
            score1 += 5;

            this.Controls[0].Text = $"Score:\n {score1}";
             
        }
        /// <summary>
        /// Deals with how the player sees how much elapsed time has passed
        /// </summary>
        public void seeTime()
        {
            
            TimeSpan currentTime = time.Elapsed;
            //Makes the player see either two digits if the seconds is below ten and otherwise, keeps it regular
            if (currentTime.Seconds >= 10)
            {
                this.Controls[1].Text = $"Time: \n{currentTime.Minutes}:{currentTime.Seconds}";

            }
            else if (currentTime.Seconds < 10)
            {
                this.Controls[1].Text = $"Time: \n{currentTime.Minutes}:0{currentTime.Seconds}";
            }

           
           
        }

        /// <summary>
        /// Deals with keeping track of lives
        /// </summary>
        public void lives()
        {
           /*With the breakout_load method, it worked out that the lives were beside each other, 
            so I can merely start with an index if any of the conditions are met for this and 
           make the life pictureboxes disappear, whilst incrementing tracker so that next time
           the player loses a live, they lose the second and vice versa*/
           this.Controls[tracker].Visible = false;
           tracker++;
            
           
            
            if (tracker == 7)
            {
                gameMusic.Stop();
                gameOver.Play();

                //Stops everything dead in its tracks
              
                 
                time.Stop();
                overAllTime = time.Elapsed;
                gameTimer.Stop();
                gameState = "Game Over";
                gameTimer.Dispose();
                time.Reset();
                    
                    
                
              

            }
                       
        }
        /// <summary>
        /// Shows the leaderboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leaderboard_Click(object sender, EventArgs e)
        {
            
            buttonPress.Play();
            gameState = "Show Leaderboard";

            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path1 = Path.Combine(projectPath, "Resources");
            string pathToFile = Path.Combine(path1, "scores.txt");

            //Reads from the text file and prints out the output from it
            using (StreamReader sr = new StreamReader(pathToFile))
            {
                while (sr.Peek() != -1)
                {
                    
                    this.Controls[30].Text += $"\n{sr.ReadLine()}";

                }
                sr.Close();
                                                         
            }
        }
        /// <summary>
        /// Button that progress the game into the difficulty selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void continuebutton_Click(object sender, EventArgs e)
        {
            gameState = "Difficulty";
            buttonPress.Stop();
            buttonPress.Play();
        }
        /// <summary>
        /// Easy mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void easybutton_Click(object sender, EventArgs e)
        {
            //Sets values for the easy mode
            ballXSpeed = 4;
            ballYSpeed = 4;;
            fallSpeed = 12;
            gameState = "Mode";
            buttonPress.Stop();
            buttonPress.Play();

        }
        /// <summary>
        /// Medium mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediumbutton_Click(object sender, EventArgs e)
        {
            //Sets values for the medium mode
            ballXSpeed = 6;
            ballYSpeed = 6;
            fallSpeed = 10;
            gameState = "Mode";
            buttonPress.Stop();
            buttonPress.Play();

        }
        /// <summary>
        /// Hard mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hardbutton_Click(object sender, EventArgs e)
        {
            //Sets values for the hard mode
            ballXSpeed = 8;
            ballYSpeed = 8;
            fallSpeed = 8;
            gameState = "Mode";
            buttonPress.Stop();
            buttonPress.Play();

        }
        /// <summary>
        /// When player doesn't want to play anymore
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
            buttonPress.Stop();
            buttonPress.Play();
        }
        /// <summary>
        /// Calls up the initialization event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endlessbutton_Click(object sender, EventArgs e)
        {
            EndlessGameInit();
            buttonPress.Stop();
            buttonPress.Play();
        }
        /// <summary>
        /// Initializes everything needed for the endless mode
        /// </summary>
        public void EndlessGameInit()
        {
            Refresh();
            
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
            this.Controls[0].Text = "Score:\n 0";

            this.Controls[1].Visible = true;
            this.Controls[2].Visible = true;
            this.Controls[4].Visible = true;
            this.Controls[5].Visible = true;
            this.Controls[6].Visible = true;
          
            this.Focus();


        }
        /// <summary>
        /// Progress the game into the play again screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playagain_Click(object sender, EventArgs e)
        {

            gameState = "Play Again";
            buttonPress.Stop();
            buttonPress.Play();

            
        }
        /// <summary>
        /// Goes back to the main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goback_Click(object sender, EventArgs e)
        {
            gameState = "Start";
            buttonPress.Stop();
            buttonPress.Play();

            gameOver.Stop();
            backgroundMusic.Play();
        }
        private void backgroundMusic_MediaEnded(object sender, EventArgs e)
        {
            backgroundMusic.Stop();
            backgroundMusic.Play();
        }
        private void gameMusic_MediaEnded(object sender, EventArgs e)
        {
            gameMusic.Stop();
            gameMusic.Play();
        }
        public void sounds()
        {
            backgroundMusic.Stop();
            gameMusic.Play();

            if (ball.IntersectsWith(player1) || ball.IntersectsWith(borderLeft) || ball.IntersectsWith(borderRight))
            {
                collisionSound.Stop();
                collisionSound.Play();
            }
            else if (tracker == 7)
            {
                
                gameMusic.Stop();
                gameOver.Play();
                
            }
            
            
        }
        /// <summary>
        /// Game Over loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameOver_MediaEnded(object sender, EventArgs e)
        {
            gameOver.Stop();
            gameOver.Play();
        }
        /// <summary>
        /// Goes to the leaderboard screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leaderboardbutton_Click(object sender, EventArgs e)
        {
            buttonPress.Stop();
            buttonPress.Play();
            gameState = "Write leaderboard";

        }
        /// <summary>
        /// Writes the name to the text file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void writeLeaderboard_Click(object sender, EventArgs e)
        {
            //Gets the player name and adds it to the dictionary
            string getPlayer = this.Controls[27].Text;
            playerScores.Add(getPlayer, score1);

            //Gets the directory path for the text file
            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path1 = Path.Combine(projectPath, "Resources");
            string pathToFile = Path.Combine(path1, "scores.txt");

            //Opens the streamwriter to write from the dictionary to the text file
            using (StreamWriter sw = new StreamWriter(pathToFile, append:true))
            {
                foreach (KeyValuePair<string, double> pair in playerScores)
                {
                    sw.WriteLine(pair.Key + " " + pair.Value);
                }
                sw.Close();


            }
            this.Controls[27].ResetText();

            gameState = "Play Again";

            buttonPress.Stop();
            buttonPress.Play();
        }
        /// <summary>
        /// Only allows letters to be entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nameplayer_TextChanged(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
               
        }
        /// <summary>
        /// Goes back to the start screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backtomenu_Click(object sender, EventArgs e)
        {
            gameState = "Start";

            this.Controls[29].Visible = false;
            this.Controls[30].Text = "";
        }
        
    }   
}
