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
            time.Start();
           
            
        }

        bool leftArrowDown = false;
        bool rightArrowDown = false;

        Rectangle borderLeft = new Rectangle(100 ,0, 10, 400);
        Rectangle borderRight = new Rectangle(340, 0, 10, 400);

        SolidBrush whiteBrush = new SolidBrush(Color.White);

        

        List<Rectangle> blocks = new List<Rectangle>();
        List<Rectangle> firstRow = new List<Rectangle>();
        

        int x1 = 130;
        int incrementx = 55;
        int incrementy = 25;
        int y1 = 20;
        int width = 30;
        int height = 10;

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
            for (int i = 0; i < blocks.Count(); i++)
            {
                e.Graphics.FillRectangle(whiteBrush, blocks[i]);
               
            }
            for (int i = 0; i < firstRow.Count(); i++)
            {
                e.Graphics.FillRectangle(whiteBrush, firstRow[i]);
            }

           

           

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           

            blockCreation();
            


            Refresh();
        }

        public void blockCreation()
        {
            



            while (blocks.Count() != 32)
            {
               

                blocks.Add(new Rectangle(x1, y1, width, height));
                x1 += incrementx;

                if (blocks.Count() % 4 == 0)
                {
                    x1 = 130;
                    y1 += incrementy;

                }
            }


            TimeSpan spanOfTime = time.Elapsed;


            if ((spanOfTime.TotalSeconds + 4) % 10 <= 1)
            {
                for (int i = 0; i < blocks.Count(); i++)
                {
                    blocks[i] = new Rectangle(blocks[i].X, blocks[i].Y + 2, width, height);
                    
                    
                }
                for (int i = 0; i < 4; i++)
                {
                    y1 = 20;
                    firstRow.Add(new Rectangle(blocks[i].X,y1, width, height));
                    y1 += incrementy;
                    

                    
                }
              
            }
            

            
          
            

            




            
           
                
               
            
           
            
            

            
        }
    }
}
