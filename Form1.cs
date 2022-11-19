using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotAlphaBuild
{
    public partial class Form1 : Form
    {
        private const int GRID_OFFSET = 25; // Distance from upper-left side of window

        private int GRID_LENGTH = 200; // Size in pixels of grid
        private int CELL_LENGTH = 60;
        private bool[,] grid; // Stores on/off state of cells in grid
        private int margin;
        public Form1()
        {
            InitializeComponent();
            grid = new bool[5, 8];
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    grid[x, y] = true;
                }
            }
            margin = Height - btnExit.Bottom;
            btnExit.Left = margin / 2 + btnNewGame.Right;
            ResizeGrid();
        }



        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    //get proper pen and brush for on/off
                    //grid section
                    Brush brush;
                    Pen pen;

                    if (grid[c, r])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;
                    }

                    //determine (x.y) coord of row and col to draw rectangle
                    int x = c * CELL_LENGTH + GRID_OFFSET;
                    int y = r * CELL_LENGTH + GRID_OFFSET;

                    //draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, CELL_LENGTH, CELL_LENGTH);
                    g.FillRectangle(brush, x + 1, y + 1, CELL_LENGTH - 2, CELL_LENGTH - 2);

                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < GRID_OFFSET || e.X > CELL_LENGTH * 5 + GRID_OFFSET ||
                e.Y < GRID_OFFSET || e.Y > CELL_LENGTH * 8 + GRID_OFFSET)
            {
                return;
            }
            else
            {
                // find row, col, and mouse press;
                int r = (e.Y - GRID_OFFSET) / CELL_LENGTH;
                int c = (e.X - GRID_OFFSET) / CELL_LENGTH;

                grid[c, r] = !grid[c, r];

                UpdateOutputs();
                //redraw grid
                this.Invalidate();

            }
        }

        private void UpdateOutputs()
        {
            String output = "{";
            String output2 = "{";

            for (int i = 0; i < 5; i++)
            {
                int hexoutput = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (!grid[i, j])
                    {
                        hexoutput |= 1 << j;
                    }
                }
                output += "0x" + hexoutput.ToString("x2") + ",";
            }

            textBox1.Text = output.Substring(0,output.Length-1) + "}";
            textBox1.Update();

            for (int j = 0; j < 8; j++)
            {
                int hexoutput = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (!grid[i, j])
                    {
                        hexoutput |= 1 << i;
                    }
                }
                output2 += "0x" + hexoutput.ToString("x2") + ",";
            }
            textBox2.Text = output2.Substring(0,output2.Length - 1) + "}";
            textBox2.Update();

        }
        private void NewGame(object sender, EventArgs e)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    grid[c, r] = false;
                }
            }
            UpdateOutputs();
            this.Invalidate();
        }

        private void Exit(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ResizeGrid();
        }

        private void ResizeGrid()
        {
            btnExit.Top = btnNewGame.Top = this.Height - (margin + btnExit.Height);
            int center = (Width - (btnExit.Right - btnNewGame.Left)) / 2;
            btnNewGame.Left = center;
            btnExit.Left = btnNewGame.Right + margin / 2;
            //set new grid size
            GRID_LENGTH = Math.Min(this.Width - 4 * GRID_OFFSET, btnExit.Top - 2 * GRID_OFFSET);
            CELL_LENGTH = GRID_LENGTH / 8;



            this.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}