using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        SolidBrush whiteBrush = new SolidBrush(Color.White);

        Rectangle[] storage = new Rectangle[30];

        List<Rectangle> blocks = new List<Rectangle>();
        

        int x1 = 100;
        int incrementx = 50;
        int incrementy = 25;
        int y1 = 20;
        int width = 30;
        int height = 5;
        int index = 0;

        


        




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
            for (int i = 0; i < index; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, storage[i]);
                index++;
            }

           

           

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            blockCreation();

            Refresh();
        }

        public void blockCreation()
        {
            for (int i = 0; i < blocks.Count(); i++)
            {
                blocks.Add(new Rectangle(x1, y1, width, height));
                x1 += incrementx;
                

                if (blocks.Count() % 3 == 0)
                {
                    y1 += incrementy;

                }
                if (blocks.Count() == 30)
                {
                    blocks.CopyTo(storage);
                    
                }

            }
          
            

            




            
           
                
               
            
           
            
            

            
        }
    }
}
