using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace Tetris3
{
    public partial class Form1 : Form
    {
        public int[,] boolBoard = new int[10, 20];
        public Panel[,] panelBoard = new Panel[10, 20];
        public List<int[,]> pieces = new List<int[,]>();
        //public Color[] colors = {Color.Pink, Color.Red, Color.LawnGreen, Color.DarkBlue, Color.Orange, Color.Purple, Color.Yellow, Color.Black};
        public Image[] colors = { Properties.Resources.Cyan, Properties.Resources.Red, Properties.Resources.Green, Properties.Resources.Blue, Properties.Resources.Orange, Properties.Resources.Purple, Properties.Resources.Yellow };
        public int[,] currentPiece;
        public int dim = 0;
        public int row = 0;
        public int column = 0;
        public int number = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < 20; y++)// makes all locations empty / 0
            {
                for (int x = 0; x < 10; x++)
                {
                    boolBoard[x, y] = 0;
                }
            }

            for (int y = 0; y < 20; y++)// makes all locations black
            {
                for (int x = 0; x < 10; x++)
                {
                    Panel panel = new Panel();
                    // panel.BackColor = Color.Black;
                    panel.BackgroundImage = Properties.Resources.Black;
                    panel.BackgroundImageLayout = ImageLayout.Stretch;
                    panel.Size = new Size(25, 25);
                    panel.Location = new Point((x * 25) + 10, (y * 25) + 10);
                    //panel.BorderStyle = BorderStyle.Fixed3D;
                    this.Controls.Add(panel);
                    panelBoard[x, y] = panel;

                }
            }


            pieces.Add(new int[4, 4]{{0,0,0,0},// I Shape
                                     {1,1,1,1},
                                     {0,0,0,0},
                                     {0,0,0,0},});

            pieces.Add(new int[3, 3]{{0,1,1},// s Shape
                                     {1,1,0},
                                     {0,0,0}});

            pieces.Add(new int[3, 3]{{1,1,0},// z Shape
                                     {0,1,1},
                                     {0,0,0}});

            pieces.Add(new int[3, 3]{{1,0,0},// L Shape
                                     {1,0,0},
                                     {1,1,0}});

            pieces.Add(new int[3, 3]{{0,0,1},// J Shape
                                     {0,0,1},
                                     {0,1,1}});

            pieces.Add(new int[3, 3]{{0,0,0},// T Shape
                                     {1,1,1},
                                     {0,1,0}});

            pieces.Add(new int[2, 2]{{1,1},// o Shape
                                     {1,1}});

            Random();
            Time();


        }

        private void Random()
        {
            Random randomGenerator = new Random();
            int random = randomGenerator.Next(0, 6);
            number = random;
            //number = 6;
        }

      /*  private void ClearLocation(int dimension)
        {
            if (row > 0 && row < 21)
            {
                for (int x = 0; x < dimension; x++)
                {
                    panelBoard[column+x, row - 1].BackColor = Color.Transparent;
                    boolBoard[column+x, row - 1] = 0;
                }
            }
        }*/



        private void Drop()//row gives y-pos   column gives x-pos
        {
            currentPiece = (int[,])pieces[number].Clone();//creates the current piece
            dim = currentPiece.GetLength(0);
            bool canMove;

            canMove = CanMove();

            if (canMove != false)
            {

                row++;
               //clear the area where the piece was previously
                Erase(0,1);//column is not moving == 0, row is moving == 1
                //draw at Board[4,0]
                Draw();
               
            }
            else
            {
                row = 0;
                Random();
            }
        }

        private void Shift(int direction)//row gives y-pos   column gives x-pos
        {
            bool canShift;

            canShift = CanShift(direction);

            if (canShift != false)
            {

                column = column + direction;
                //clear the area where the piece was previously
                Erase(direction,0);//column is moving left == -1, or right == 1, row is not moving == 0
                //draw at Board[c,r]
                Draw();
            }
            else
            {
                //do nothing, don't touch column or spawn a new piece
            }
        }

        private void Draw()
        {
            //if (canMove != false)
            {
                //row gives y-pos   column gives x-pos
                for (int r = 0; r < dim; r++)//r = row
                {
                    for (int c = 0; c < dim; c++)//c = column
                    {
                        if (currentPiece[r, c] == 1)//if the piece matrix is a 1
                        {
                            //draw block at position
                            panelBoard[c + column, r + row].BackgroundImage = colors[number];
                        }
                    }
                }
            }
        }


        private void Erase(int x, int y)//column and row movement
        {
                //row gives y-pos   column gives x-pos
                for (int r = 0; r < dim; r++)//r = row
                {
                    for (int c = 0; c < dim; c++)//c = column
                    {
                        if (currentPiece[r, c] == 1)//if the piece matrix is a 1
                        {
                            //erase block at position
                            panelBoard[c + column - x, r + row - y].BackgroundImage = Properties.Resources.Black;
                        }
                    }
                }
        }


        private bool CanMove()
        {
            int posRow = row+1;//these will need to change for sideways movement
            int posColumn = column;

            for (int r = 0; r < dim; r++)//row gives y-pos   column gives x-pos
            {
                for(int c = 0; c < dim; c++)
                {
                    if (currentPiece[r,c] == 1)
                    {
                        if (r + posRow >= 20)//this is below the ground
                        {
                            //no go
                            AddToBoard();
                            return false;
                        }
                        else if (boolBoard[posColumn+c,posRow+r] == 1)//if the possible space has a block in it (boolBoard == 1)
                        {
                            //no go
                            AddToBoard();
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //if left, int direction == -1    if right, int direction == 1
        private bool CanShift(int direction)
        {
            int posRow = row;//these will need to change for sideways movement
            int posColumn = column+direction;
            
            for (int r = 0; r < dim; r++)//row gives y-pos   column gives x-pos
            {
                for (int c = 0; c < dim; c++)
                {
                    if (currentPiece[r, c] == 1)
                    {
                        if (c + posColumn < 0 || c + posColumn > 9)//if trying to move piece out the side to columns -1 or 10
                        {
                            //no go
                            return false;
                        }
                        if (boolBoard[posColumn + c, posRow + r] == 1)//if the possible space has a block in it (boolBoard == 1)
                        {//should be the same as when dropping
                            //no go                          
                            return false;
                        }
                    }
                }
            }//if it does not get caught by the loops it should be free to move sideways
            return true;
        }

        private void AddToBoard()
        {
            for (int r = 0; r < dim; r++)
            {
                for (int c = 0; c < dim; c++)
                {
                    if (currentPiece[r, c] == 1)
                    {
                        boolBoard[c + column, r + row] = 1;
                        

                    }
                }
            }
           IsLine();
        }



        private void IsLine()
        {
            bool isLine = true;


            for (int r = 0; r < 20; r++)
            {
                isLine = true;
                for (int c = 0; c < 10; c++)
                {
                    if (boolBoard[c, r] == 0)//of the 
                    {
                        isLine = false;
                    }
                }
                if (isLine == true)
                {
                    EraseLine(r);

                }
            }

        }


        /*  private void isLine()
          {
             bool lineFull = true;

              for (int r = 0; r < 20; r++)//row gives y-pos   column gives x-pos
              {
                  for (int c = 0; c < 10; c++)
                  {
                      if (boolBoard[c, r] == 0)//there is not a block there and the line is not full
                      {
                          lineFull = false;
                          //break;//if it reaches here the line is not full so exit the loop for this row
                      }
                      //else//if lineFull = true
                      //{

                          //Array.Copy(boolBoard, (r * 10 + c), boolBoard, 8, 4);
                      //}
                  }
                  //if it makes it here there are all 1's in the row
                  if (lineFull == true)
                  {
                      EraseLine(r);
                  }
              }
          }*/

        private void EraseLine(int clearRow)
            {
                for (int c = 0; c < 10; c++)//row gives y-pos   column gives x-pos
                {
                    for (int r = clearRow; r > -1; r --)
                    {
                        if (boolBoard[c, r] == 1)//there is not a block there and the line is not full
                        {
                            boolBoard[c, r] = boolBoard[c, r-1];
                            panelBoard[c, r].BackgroundImage = panelBoard[c,r-1].BackgroundImage;
                        }
                    }
                }
            }
        

        //arrow buttons
        /* protected override void OnKeyDown(KeyEventArgs e)
         {
             if (e.KeyCode == Keys.Left)//if the left arrow is pressed
             {
                 column--;
             }
             if (e.KeyCode == Keys.Right)
             {
                 column++;
             }
         }*/
            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch(keyData)
            {
                case Keys.Left:
                    Shift(-1);
                    break;
                case Keys.Right:
                    Shift(1);
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }



        private void Time()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 200;
            timer.Elapsed += Tick;
            timer.AutoReset = true;
            timer.Enabled = true;

        }

        private void Tick(Object source, System.Timers.ElapsedEventArgs e)
        {

            Drop();

           // ColorChange(vcount);
           // vcount++;
        }
    }
}
