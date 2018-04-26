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

        public int lDim = 0;
        public int hDim = 0;


        public int row = 0;

        public int column = 0;

        public int number = 0;

        public int rotation = 0;

        public int bestRotation = 0;

        public int bestColumn = 0;

        public double bestChoice = -1;

        public int count = 0;

        public int bestRow = 0;

        public int Lines = 0;

        public int Holes = 0;

        List<int> bag = new List<int>();
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

                { 1 },

                { 1},

                { 1 },

                { 1 },

            },

            new int[,]

                        {

                { 1, 1, 1, 1 }


            }
            };
                pieces.Add(Ipiece);







                int[][,] Zpiece = new int[][,]

    {
            new int[,]

            {

                { 1, 1, 0 },

                { 0, 1, 1 }

            },



            new int[,]

                        {

                { 0, 1 },

                { 1, 1 },

                { 1, 0 }
            }

    };

                pieces.Add(Zpiece);



                int[][,] Spiece = new int[][,]

    {

            new int[,]

            {

                { 0, 1, 1 },

                { 1, 1, 0 },

            },

            new int[,]

                        {

                { 1, 0 },

                { 1, 1 },

                { 0, 1 },

            }

    };

                pieces.Add(Spiece);



                int[][,] Lpiece = new int[][,]

    {

            new int[,]

            {

                { 1, 0 },

                { 1, 0 },

                { 1, 1 },
            },

            new int[,]

                        {

                { 0, 0, 1 },

                { 1, 1, 1 },

            },

            new int[,]

            {

                { 1, 1 },

                { 0, 1 },

                { 0, 1 },

            },

             new int[,]

            {

                { 1, 1, 1 },

                { 1, 0, 0 },

            }

    };

                pieces.Add(Lpiece);





                int[][,] Jpiece = new int[][,]

    {

            new int[,]

            {

                {  0, 1 },

                {  0, 1 },

                {  1, 1 },


            },

            new int[,]

             {

                { 1, 0, 0 },

                { 1, 1, 1 },

            },

            new int[,]

            {

                { 1, 1, },

                { 1, 0, },

                { 1, 0, },

            },

             new int[,]

            {

                { 1, 1, 1 },

                { 0, 0, 1 },

            }

    };

                pieces.Add(Jpiece);



                int[][,] Tpiece = new int[][,]

    {

            new int[,]

            {

                { 0, 1 },

                { 1, 1 },

                { 0, 1 },

            },

            new int[,]

                        {

                { 0, 1, 0 },

                { 1, 1, 1 },

            },

            new int[,]

            {

                { 1, 0 },

                { 1, 1 },

                { 1, 0 },

            },

             new int[,]

            {

                { 1, 1, 1 },

                { 0, 1, 0 },

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



            Time(true);

        }


        private void Random()

        {
            
            if (bag.Any())
            {
                number = bag.Last();
                bag.RemoveAt(bag.Count -1);
            }
            else
            {
                int length = 7;
                Random randomGenerator = new Random();
                bag.Add(0); bag.Add(1); bag.Add(2); bag.Add(3); bag.Add(4); bag.Add(5); bag.Add(6);
                // Shuffle it:

                for (int i = length - 1; i > 1; i--)
                {
                    // Pick an entry no later in the deck, or i itself.
                    int j = randomGenerator.Next(0, i + 1);

                    // Swap the order of the two entries.
                    int swap = bag[i];
                    bag[i] = bag[j];
                    bag[j] = swap;
                }

                number = bag.Last();
                bag.RemoveAt(bag.Count -1);
            }
            currentPiece = pieces[number];
        }



        private void AI()

        {
            bestRow = 0;
            bestChoice = -999;
            bestColumn = 0;
            bestRotation = 0;
            Holes = 0;


            currentPiece = pieces[number];
            count++;

            column = 0;
            int rotationsAva = pieces[number].Length;

            for (int p = 0; p < rotationsAva; p++)//loops all the rotations of the piece

            {
                if (count == 8)
                {
                    count = 0;
                }

                lDim = currentPiece[p].GetLength(1);
                hDim = currentPiece[p].GetLength(0);

                // for (int c = 0; c < 9 - currentPiece[p].GetLength(0); c++)//should go through all columns possible for 1 rotation
                for (int c = 0; c <= 10 - lDim; c++)
                {
                    row = 0;
                    Holes = 0;
                    AIDrop(p, c);

                    column++;
                }

            }

            column = bestColumn;

            rotation = bestRotation;
            lDim = currentPiece[rotation].GetLength(1);
            hDim = currentPiece[rotation].GetLength(0);

            //bestRow = row;

            row = 0;
        }



        private void AIDrop(int p, int column)

        {


            bool AIcanMove = true;

            while (AIcanMove == true)
            { //canDrop
                for (int r = 0; r < hDim; r++)//row gives y-pos   column gives x-pos
                {
                    for (int c = 0; c < lDim; c++)
                    {
                        if (currentPiece[p][r, c] == 1)
                        {
                            if (r + row + 1 >= 20 || row < 0)//this is below the ground
                            {
                                AIcanMove = false;
                                break;
                            }

                            else if (boolBoard[column + c, row + 1 + r] == 1)//if the possible space has a block in it (boolBoard == 1)
                            {
                                AIcanMove = false;
                                break;
                            }
                        }
                    }

                }
                if (AIcanMove == true)
                { row++; }
            }//canDrop end


            for (int r = 0; r < hDim; r++)//row gives y-pos   column gives x-pos
            {
                for (int c = 0; c < lDim; c++)
                {
                    if (currentPiece[p][r, c] == 1)

                    {
                        boolBoard[c + column, r + row] = 1;
                    }               
                }
            }
            bool isBlock = false;
            for (int x = 0; x < 10; x++)
            {
                isBlock = false;
                for (int y = 0; y < 20; y++)
                {
                     if (boolBoard[x,y] == 1)
                    {
                        isBlock = true;
                    }
                    else if (boolBoard[x, y] == 0 && isBlock == true)
                    {
                        Holes++;
                    }
                }
            }
            for (int r = 0; r < hDim; r++)//row gives y-pos   column gives x-pos
            {
                for (int c = 0; c < lDim; c++)
                {
                    if (currentPiece[p][r, c] == 1)

                    {
                        boolBoard[c + column, r + row] = 0;
                    }
                }
            }




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
            //row = the lower it is on the board, the higher the row, thus higher row is GOOD
            //holes = the more holes to worse it is, this higher is BAD
            double sum = (lines * 100) + (row * .5) - (Holes * 15);//heuristics

            if (bestChoice < sum)
            {

                bestChoice = sum;

                bestRotation = p;

                bestColumn = column;

                bestRow = row;

            }



        }


        private void Drop()//row gives y-pos   column gives x-pos

        {

            if (row < bestRow && row < 20)
            {
                row++;
                Erase();
                Draw();
                //Erase(0, 1);//column is not moving == 0, row is moving == 1
            }

            else
            {
                AddToBoard();
                Random();
                AI();
            }

        }


        private void Draw()

        {

            //if (canMove != false)

            {

                //row gives y-pos   column gives x-pos

                for (int r = 0; r < hDim; r++)//r = row

                {

                    for (int c = 0; c < lDim; c++)//c = column

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

        private void Erase()

        {

            //if (canMove != false)

            {

                //row gives y-pos   column gives x-pos

                for (int r = 0; r < hDim; r++)//r = row

                {

                    for (int c = 0; c < lDim; c++)//c = column

                    {

                        if (currentPiece[rotation][r, c] == 1)//if the piece matrix is a 1

                        {

                            //draw block at position

                            panelBoard[c + column, r + row - 1].BackgroundImage = Properties.Resources.Black;

                        }

                    }

                }

            }

        }

        private void AddToBoard()

        {

            for (int r = 0; r < hDim; r++)

            {

                for (int c = 0; c < lDim; c++)

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

                    Lines++;
                    this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { label2.Text = Lines.ToString(); });


                    EraseLine(r);



                }

            }



        }

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

        private void GAMEOVER()
        {
            Time(false);
        }


        private void Time(bool stop)

        {
            System.Timers.Timer timer = new System.Timers.Timer();

            timer.Interval = 25;

            timer.Elapsed += Tick;

            timer.AutoReset = true;

            timer.Enabled = true;


            if (stop == false)
            {
                timer.Stop();
            }


        }



        private void Tick(Object source, System.Timers.ElapsedEventArgs e)

        {

            Drop();

        }

    }

}
