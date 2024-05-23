using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;


namespace Space_Race
{
    public partial class Form1 : Form
    {
        Rectangle player1 = new Rectangle(150, 575, 10, 20);
        Rectangle player2 = new Rectangle(450, 575, 10, 20);

        Rectangle safteyLine = new Rectangle(0, 550, 600, 3);

        SolidBrush playerBrush = new SolidBrush(Color.Purple);
        SolidBrush player2Brush = new SolidBrush(Color.Orange);
        SolidBrush borderBrush = new SolidBrush(Color.Gold);
        SolidBrush astroidBrush = new SolidBrush(Color.White);

        int p1Score = 0;
        int p2Score = 0;
        int playerSpeed = 4;

        Random randGen = new Random();
        int randValue = 0;



        List<Rectangle> astroidList = new List<Rectangle>();
        List<Rectangle> astroidList2 = new List<Rectangle>();
        List<int> speedList = new List<int>();
        List<int> sizeList = new List<int>();
        int astroidSpeed = 6;
        int astroidSize = 6;

        int safezone = 50;

        bool wPressed = false;
        bool sPressed = false;
        bool upPressed = false;
        bool downPressed = false;
        public Form1()
        {
            InitializeComponent();

            SoundPlayer soundplayer1 = new SoundPlayer(Properties.Resources.intro);
            soundplayer1.Play();
        }
        public void InitializeGame()
        {
            titleLabel.Text = "";
            subtitleLabel.Text = "";
            winLabel.Text = "";


            p1Score = 0;
            p2Score = 0;
            p1scoreLabel.Text = $"{p2Score}";
            p2scoreLabel.Text = $"{p1Score}";

            gameTimer.Enabled = true;

            player1.Y = 575;
            player2.Y = 575;

            SoundPlayer soundplayer1 = new SoundPlayer(Properties.Resources.intro);
            soundplayer1.Stop();

            astroidList.Clear();
            speedList.Clear();
            sizeList.Clear();
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
                case Keys.Up:
                    upPressed = false;
                    break;
                case Keys.Down:
                    downPressed = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
                case Keys.Up:
                    upPressed = true;
                    break;
                case Keys.Down:
                    downPressed = true;
                    break;
                case Keys.Escape:
                    if (gameTimer.Enabled == false)
                    {
                        Application.Exit();
                    }

                    break;
                case Keys.Space:
                    if (gameTimer.Enabled == false)
                    {
                        InitializeGame();
                    }
                    break;
            }
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {

            for (int i = 0; i < astroidList.Count(); i++)
            {
                //get new position of X

                int x = astroidList[i].X + speedList[i];

                //update the ball object
                astroidList[i] = new Rectangle(x, astroidList[i].Y, sizeList[i], sizeList[i]);
            }

            randValue = randGen.Next(1, 100);

            if (randValue < 30)  // left side start
            {

                int randY = randGen.Next(30, this.Height);
                Rectangle astroid = new Rectangle(0, randY, astroidSize, astroidSize);
                astroidList.Add(astroid);
                speedList.Add(randGen.Next(4, 6));
                sizeList.Add(randGen.Next(4, 8));
 
            }
            else if (randValue < 50)  //right side start
            {
                int randY = randGen.Next(30, this.Height);
                Rectangle astroid = new Rectangle(this.Width, randY, astroidSize, astroidSize);
                astroidList.Add(astroid);
                speedList.Add(randGen.Next(-6, -4));
                sizeList.Add(randGen.Next(4, 8));
            }
            for (int i = 0; i < astroidList.Count(); i++)
            {
                if (astroidList[i].Y > this.Height - 55)
                {
                    astroidList.RemoveAt(i);
                    speedList.RemoveAt(i);
                    sizeList.RemoveAt(i);
                }
            }
            //Move player 1
            if (wPressed == true && player1.Y < 590)
            {
                player1.Y -= playerSpeed;
            }
            if (sPressed == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += playerSpeed;
            }
            //Move Player 2
            if (upPressed == true && player2.Y < 590)
            {
                player2.Y -= playerSpeed;
            }
            if (downPressed == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += playerSpeed;
            }
            //When player 1 hits the top he goes back down to the start and scores a point
            if (wPressed == true && player1.Y <= 0)
            { 
                SoundPlayer ohyeah = new SoundPlayer(Properties.Resources.ohyeah);
                ohyeah.Play();
                player1.Y = 575;
                p1Score = p1Score + 1;
                p1scoreLabel.Text = $"{p1Score}";
            }
            //When player 2 hits the top he goes back down to the start and scores a point
            if (upPressed == true && player2.Y <= 0)
            {
                SoundPlayer ohyeah = new SoundPlayer(Properties.Resources.ohyeah);
                ohyeah.Play();
                player2.Y = 575;
                p2Score = p2Score + 1;
                p2scoreLabel.Text = $"{p2Score}";

            }
            for (int i = 0; i < astroidList.Count(); i++)
            {
                if (astroidList[i].IntersectsWith(player1))
                {
                    player1.Y = 575;
                    SoundPlayer explosion = new SoundPlayer(Properties.Resources.explosion);
                    explosion.Play();
                }
            }
            for (int i = 0; i < astroidList.Count(); i++)
            {
                if (astroidList[i].IntersectsWith(player2))
                {
                    player2.Y = 575;
                    SoundPlayer explosion = new SoundPlayer(Properties.Resources.explosion);
                    explosion.Play();
                }
            }
            if (p1Score == 3)
            {
                SoundPlayer soundplayer1 = new SoundPlayer(Properties.Resources.intro);
                soundplayer1.Play();
                gameTimer.Enabled = false;
                winLabel.Text = "Purple wins";
                player1.Y = 575;
                player2.Y = 575;
                wPressed = false;
                sPressed = false;
                upPressed = false;
                downPressed = false;
                titleLabel.Text = "GAME OVER";
                subtitleLabel.Text = "Press SPACE to play again Press ESC to EXIT";
            }
            if (p2Score == 3)
            {
                SoundPlayer soundplayer1 = new SoundPlayer(Properties.Resources.intro);
                soundplayer1.Play();
                gameTimer.Enabled = false;
                winLabel.Text = "Orange wins";
                player1.Y = 575;
                player2.Y = 575;
                wPressed = false;
                sPressed = false;
                upPressed = false;
                downPressed = false;
                titleLabel.Text = "GAME OVER";
                subtitleLabel.Text = "Press SPACE to play again Press ESC to EXIT";
            }
            Refresh();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if(gameTimer.Enabled == true)
            {
                e.Graphics.FillRectangle(playerBrush, player1);
                e.Graphics.FillRectangle(player2Brush, player2);

                e.Graphics.FillRectangle(borderBrush, safteyLine);

                for (int i = 0; i < astroidList.Count(); i++)
                {
                    e.Graphics.FillRectangle(astroidBrush, astroidList[i]);
                }
            }
        }

    }
}