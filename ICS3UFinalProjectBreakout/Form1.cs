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

        Rectangle borderLeft = new Rectangle(200 ,0, 10, 400);

        SolidBrush whiteBrush = new SolidBrush(Color.White);


        




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

        }
    }
}
