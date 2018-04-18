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

        public List<int[][,]> pieces = new List<int[][,]>();

        //public Color[] colors = {Color.Pink, Color.Red, Color.LawnGreen, Color.DarkBlue, Color.Orange, Color.Purple, Color.Yellow, Color.Black};

        public Image[] colors = { Properties.Resources.Cyan, Properties.Resources.Red, Properties.Resources.Green, Properties.Resources.Blue, Properties.Resources.Orange, Properties.Resources.Purple, Properties.Resources.Yellow };

        public int[][,] currentPiece;

        public int dim = 0;

        public int row = 0;

        public int column = 4;

        public int number = 0;

        public int rotation = 0;

        public int bestRotation = 0;

        public int bestColumn = 0;

        public double bestChoice = -1;





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

            //pieces is List.Add

            //0=l  1=Z  2=S  3=L  4=J  5=T  6=O

            {

                //pieces[3][1][i, j]

                int[][,] Ipiece = new int[][,]

            {

            new int[,]

            {

                { 1, 0, 0, 0 },

                { 1, 0, 0, 0 },

                { 1, 0, 0, 0 },

                { 1, 0, 0, 0 },

            },

            new int[,]

                        {

                { 1, 1, 1, 1 },

                { 0, 0, 0, 0 },

                { 0, 0, 0, 0 },

                { 0, 0, 0, 0 },

            }

            };



                pieces.Add(Ipiece);







                int[][,] Zpiece = new int[][,]

    {

            new int[,]

            {

                { 1, 1, 0 },

                { 0, 1, 1 },

                { 0, 0, 0 },

            },

            new int[,]

                        {

                { 0, 1, 0 },

                { 1, 1, 0 },

                { 1, 0, 0 },

            }

    };

                pieces.Add(Zpiece);



                int[][,] Spiece = new int[][,]

    {

            new int[,]

            {

                { 0, 1, 1 },

                { 1, 1, 0 },

                { 0, 0, 0 },

            },

            new int[,]

                        {

                { 1, 0, 0 },

                { 1, 1, 0 },

                { 0, 1, 0 },

            }

    };

                pieces.Add(Spiece);



                int[][,] Lpiece = new int[][,]

    {

            new int[,]

            {

                { 1, 0, 0 },

                { 1, 0, 0 },

                { 1, 1, 0 },

            },

            new int[,]

                        {

                { 0, 0, 0 },

                { 0, 0, 1 },

                { 1, 1, 1 },

            },

            new int[,]

            {

                { 0, 1, 1 },

                { 0, 0, 1 },

                { 0, 0, 1 },

            },

             new int[,]

            {

                { 1, 1, 1 },

                { 1, 0, 0 },

                { 0, 0, 0 },

            }

    };

                pieces.Add(Lpiece);





                int[][,] Jpiece = new int[][,]

    {

            new int[,]

            {

                { 0, 0, 1 },

                { 0, 0, 1 },

                { 0, 1, 1 },

            },

            new int[,]

                        {

                { 0, 0, 0 },

                { 1, 0, 0 },

                { 1, 1, 1 },

            },

            new int[,]

            {

                { 1, 1, 0 },

                { 1, 0, 0 },

                { 1, 0, 0 },

            },

             new int[,]

            {

                { 1, 1, 1 },

                { 0, 0, 1 },

                { 0, 0, 0 },

            }

    };

                pieces.Add(Jpiece);



                int[][,] Tpiece = new int[][,]

    {

            new int[,]

            {

                { 0, 0, 1 },

                { 0, 1, 1 },

                { 0, 0, 1 },

            },

            new int[,]

                        {

                { 0, 0, 0 },

                { 0, 1, 0 },

                { 1, 1, 1 },

            },

            new int[,]

            {

                { 1, 0, 0 },

                { 1, 1, 0 },

                { 1, 0, 0 },

            },

             new int[,]

            {

                { 1, 1, 1 },

                { 0, 1, 0 },

                { 0, 0, 0 },

            }

    };

                pieces.Add(Tpiece);



                int[][,] Opiece = new int[][,]

    {

            new int[,]

            {

                {1, 1 },

                {1, 1 },

            }

    };

                pieces.Add(Opiece);





            }



            Random();



            //AI goes here

            AI();



            Time();

        }



        //private void Next()

        //{

        //    Random();



        //    //AI goes here

        //    AI();



        //    Time();

        //}



        private void Random()

        {

            Random randomGenerator = new Random();

            int random = randomGenerator.Next(0, 6);

            number = random;

            //number = 5;



            currentPiece = pieces[number];

        }



        private void AI()

        {

            for (int p = 0; p < pieces[number].Length; p++)//loops all the rotations of the piece

            {

                for (int c = 0; c < 9 - currentPiece[rotation].GetLength(0); c++)//should go through all columns possible for 1 rotation

                {

                    AIDrop(p, c);

                    column++;

                }

            }

            row = 0;


            column = bestColumn;

            rotation = bestRotation;

        }



        private void AIDrop(int rotation, int column)

        {

            row = 0;

            bool AIcanMove = true;



            while (AIcanMove == true)

            { //canDrop

                int posRow = row + 1;



                for (int r = 0; r < currentPiece[rotation].GetLength(0); r++)//row gives y-pos   column gives x-pos

                {

                    for (int c = 0; c < currentPiece[rotation].GetLength(0); c++)

                    {

                        if (currentPiece[rotation][r, c] == 1)

                        {

                            if (r + posRow >= 20)//this is below the ground

                            {

                                AIcanMove = false;

                                break;

                            }

                            else if (boolBoard[column + c, posRow + r] == 1)//if the possible space has a block in it (boolBoard == 1)

                            {

                                AIcanMove = false;

                                break;

                            }

                        }

                    }

                }

                row++;

            }//canDrop end





            //AI LinesCleared

            bool isLine = true;

            int lines = 0;



            for (int r = 0; r < 20; r++)

            {

                isLine = true;

                for (int c = 0; c < 10; c++)

                {

                    if (boolBoard[c, r] == 0)

                    {

                        isLine = false;

                        break;

                    }

                }

                if (isLine == true)

                {

                    lines++;

                }

            }

            //AI is lines end

            double sum = (row * .5) + (lines * 1);

            if (bestChoice < sum)

            {

                bestChoice = sum;

                bestRotation = rotation;

                bestColumn = column;

            }



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

            //currentPiece = (int[][,])pieces[number][rotation].Clone();//creates the current piece



            dim = currentPiece[rotation].GetLength(0);

            bool canMove = CanMove();



            if (canMove != false)

            {

                row++;

                //clear the area where the piece was previously

                Erase(0, 1);//column is not moving == 0, row is moving == 1

                //draw at Board[4,0]

                Draw();



            }

            else

            {

                row = 0;

                Random();

                AI();

                //row = 0;





            }

            // Next();

        }



        private void Shift(int direction)//row gives y-pos   column gives x-pos

        {

            bool canShift;



            canShift = CanShift(direction);



            if (canShift != false)

            {



                column = column + direction;

                //clear the area where the piece was previously

                Erase(direction, 0);//column is moving left == -1, or right == 1, row is not moving == 0

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

                        if (currentPiece[rotation][r, c] == 1)//if the piece matrix is a 1

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

                    if (currentPiece[rotation][r, c] == 1)//if the piece matrix is a 1

                    {

                        //erase block at position

                        panelBoard[c + column - x, r + row - y].BackgroundImage = Properties.Resources.Black;

                    }

                }

            }

        }





        private bool CanMove()

        {

            int posRow = row + 1;//these will need to change for sideways movement

            int posColumn = column;



            for (int r = 0; r < dim; r++)//row gives y-pos   column gives x-pos

            {

                for (int c = 0; c < dim; c++)

                {

                    if (currentPiece[rotation][r, c] == 1)

                    {

                        if (r + posRow >= 20)//this is below the ground

                        {



                            //no go

                            AddToBoard();

                            return false;

                        }

                        else if (boolBoard[posColumn + c, posRow + r] == 1)//if the possible space has a block in it (boolBoard == 1)

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

            int posColumn = column + direction;



            for (int r = 0; r < dim; r++)//row gives y-pos   column gives x-pos

            {

                for (int c = 0; c < dim; c++)

                {

                    if (currentPiece[rotation][r, c] == 1)

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

                    if (currentPiece[rotation][r, c] == 1)

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

                for (int r = clearRow; r > -1; r--)

                {

                    if (boolBoard[c, r] == 1)//there is not a block there and the line is not full

                    {

                        boolBoard[c, r] = boolBoard[c, r - 1];

                        panelBoard[c, r].BackgroundImage = panelBoard[c, r - 1].BackgroundImage;

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

            switch (keyData)

            {

                case Keys.Up:

                    rotation++;//should loop 0,1,2,3

                    rotation = rotation % currentPiece.Length;

                    break;

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
