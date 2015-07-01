//Tetris Game
//Antonuk O. M.
//14:32, 28.12.2014
using System;
using System.Threading.Tasks;
//Main
namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            IMenu m = new TetrisMenu();
            string choice;
            do
            {
                m.ShowMenu();
                choice = Console.ReadLine();
                m.ChoiceHandler(choice);
            }
            while (choice != "3");
        }
    }
}
//Abstract Classes
namespace Tetris
{
    abstract class GameObject
    {
        virtual public char Symbol { set; get; }
        virtual public int Width { set; get; }
        virtual public int Height { set; get; }
    }
    abstract class Game
    {
        abstract public void Play();
    }
    interface IMenu
    {
        int NumberOfItems { set; get; }
        string[] Item { set; get; }
        void ShowMenu();
        void ChoiceHandler(string choice);
    }
}
//Delegates and Events
namespace Tetris
{
    delegate void Up();
    delegate void Down();
    delegate void Left();
    delegate void Right();
    class EventUp
    {
        // подія
        public event Up UpEvent;
        // метод обробки
        public void UpUserEvent()
        {
            UpEvent();
        }
    }
    class EventDown
    {
        public event Down DownEvent;
        public void DownUserEvent()
        {
            DownEvent();
        }
    }
    class EventLeft
    {
        public event Left LeftEvent;
        public void LeftUserEvent()
        {
            LeftEvent();
        }
    }
    class EventRight
    {
        public event Right RightEvent;
        public void RightUserEvent()
        {
            RightEvent();
        }
    }
}
//Figures
namespace Tetris
{
    abstract class Figure : GameObject
    {
        public Figure()
        {
            Symbol = 'X';
            Width = 4;
            Height = 4;
            Matrix = new bool[Height, Width];
        }
        public void Clear(bool[,] m)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    m[i, j] = false;
        }
        public bool[,] Matrix { set; get; }
        public override int Width { get; set; }
        public override int Height { get; set; }
        public int Position { set; get; }
        abstract public void Create();
        virtual public void Rotate()
        {
            bool[,] tempFig = new bool[Height, Width];
            Clear(tempFig);
            for (int j = Height - 2, c = 0; j >= 0; j--, c++)
                for (int i = 0; i < 3; i++)
                    tempFig[c, i] = Matrix[i, j];
            Clear(Matrix);
            for (int f = 0; f < 3; f++)
                for (int d = 0; d < 3; d++)
                    Matrix[f, d] = tempFig[f, d];
            Position++;
        }
    }
    //1
    class Line : Figure
    {
        public override void Create()
        {
            Clear(Matrix);
            Position = 1;
            for (int i = 0; i < Width; i++)
            {
                Matrix[0, i] = true;
            }
        }
        public override void Rotate()
        {
            if (Position == Height)
            {
                Position = 1;
            }
            Position++;
            int k = 0;
            if (Matrix[0, 0] == true)
            {
                Clear(Matrix);
                for (k = 0; k < Width; k++)
                    Matrix[k, 1] = true;
            }
            else
            {
                Clear(Matrix);
                for (k = 0; k < Width; k++)
                    Matrix[0, k] = true;
            }
        }
    }
    //2
    class Square : Figure
    {
        public override void Create()
        {
            Clear(Matrix);
            Position = 1;
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    Matrix[i, j] = true;
        }
        public override void Rotate()
        {
            return;
        }

    }
    //3
    class RightL : Figure
    {
        public override void Create()
        {
            Clear(Matrix);
            Position = 1;
            for (int i = 0; i < 3; i++)
                Matrix[0, i] = true;
            Matrix[1, 0] = true;
        }
    }
    //3
    class LefL : Figure
    {
        public override void Create()
        {
            Clear(Matrix);
            Position = 1;
            for (int i = 0; i < 3; i++)
            {
                Matrix[0, i] = true;
            }
            Matrix[1, 2] = true;
        }
    }
    //5
    class Pyramide : Figure
    {
        public override void Create()
        {
            Clear(Matrix);
            Position = 1;
            for (int i = 0; i < 3; i++)
                Matrix[1, i] = true;
            Matrix[0, 1] = true;
        }
    }
    class LeftZ : Figure
    {
        public override void Create()
        {
            Clear(Matrix);
            Position = 1;
            Matrix[0, 0] = true;
            Matrix[1, 0] = true;
            Matrix[1, 1] = true;
            Matrix[2, 1] = true;
        }
    }
    class RightZ : Figure
    {
        public override void Create()
        {
            Position = 1;
            Matrix[0, 1] = true;
            Matrix[1, 0] = true;
            Matrix[1, 1] = true;
            Matrix[2, 0] = true;
        }
    }
}
//Field
namespace Tetris
{
    class Field : GameObject
    {
        private Figure f;
        public Field()
        {
            Symbol = ' ';
            Height = 20;
            Width = 12;
            FieldMatrix = new bool[Height, Width];
            Level = 0;
            Score = 0;
            Id = 0;
        }
        public int Id { private set; get; }
        public int X { set; get; }
        public int Y { set; get; }
        public int Score { private set; get; }
        public int Level { private set; get; }
        public bool[,] FieldMatrix { set; get; }
        public void DrawField()
        {
            for (int i = 0; i < Height - 1; i++)
            {
                for (int j = 1; j < Width - 1; j++)
                {
                    Console.CursorLeft = j;
                    Console.CursorTop = i;
                    if (FieldMatrix[i, j] == false)
                    {
                        Console.WriteLine(Symbol);//замальовує символом поля
                    }
                    else
                    {
                        Console.WriteLine(f.Symbol);//символом фігури
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n   Level " + Level);
            Console.WriteLine("\n  Scores " + Score);
        }
        public void Copy()
        {
            int x = this.X;
            int y = this.Y;
            for (int i = 0; i < 4; i++)
            {
                x = this.X;
                for (int j = 0; j < 4; j++)
                {
                    if (f.Matrix[i, j] == true)
                    {
                        FieldMatrix[y, x] = true;
                    }
                    x++;
                }
                y++;
            }
        }
        public void NewFigure()
        {
            FigureFactory fact;
            Random r = new Random();
            Y = 0;
            X = 5;
            Id = r.Next(0, 7);
            //Id = 6; //тест
            if (Id == 0)
            {
                fact = new LineFactory();
            }
            else if (Id == 1)
            {
                fact = new SquareFactory();
            }
            else if (Id == 2)
            {
                fact = new PyramideFactory();
            }
            else if (Id == 3)
            {
                fact = new LeftZFactory();
            }
            else if (Id == 4)
            {
                fact = new RightZFactory();
            }
            else if (Id == 5)
            {
                fact = new LeftLFactory();
            }
            else if (Id == 6)
            {
                fact = new RightLFactory();
            }
            else
            {
                Id = 0;
                fact = new LineFactory();
            }
            f = fact.CreateFigure();
            f.Create();
            Copy();
        }
        public void ClearPrevious()
        {
            int m = 0;
            int n = 0;

            for (int i = Y; i < Y + f.Height; i++)
            {
                for (int j = X; j < X + f.Width; j++)
                {
                    if (f.Matrix[m, n] == true)
                    {
                        FieldMatrix[i, j] = false;
                    }
                    n++;
                }
                m++;
                n = 0;
            }

        }
        public void Move()
        {
            ClearPrevious();
            Y++;
            Copy();
            DrawField();
        }
        public bool CheckRotation() { return false; }
        public bool CheckLine()
        {
            int counter = 0; //рахує кількість зайнятих клітинок
            int k = 0;
            for (int i = 0; i < Height; i++)
            {
                counter = 0;
                for (int j = 0; j < Width; j++)
                {
                    if (FieldMatrix[i, j] == true)
                        counter++;
                    if (counter == 10)
                    {
                        k = i; //запам"ятовує лінію
                        break;
                    }
                }
            }

            if (k == 0) return
                false;

            else
            {
                for (int i = 0; i < Width; i++)
                {
                    FieldMatrix[k, i] = false;
                }

                for (int i = k; i > 0; i--)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        FieldMatrix[i, j] = FieldMatrix[i - 1, j];
                    }
                }
                Score += 100;
                if (Score == 500)
                {
                    Level++;
                    Score = 0;
                }
                return true;
            }

        }
        //обробник події повороту фігури
        public void UpFig()
        {
            ClearPrevious();
            f.Rotate();
            Copy();
        }
        //обробник події падіння фігури
        public void DownFig()
        {
            while (CheckDown() == true)
            {
                Move();
            }
        }
        //обробник події зміщення фігури вліво
        public void LeftFig()
        {
            if (CheckLeft() == true)
            {
                ClearPrevious();
                X--;
                Copy();
            }
            else
                return;
        }
        //обробник події зміщення фігури вправо
        public void RightFig()
        {
            if (CheckRight() == true)
            {
                ClearPrevious();
                X++;
                Copy();
            }
            else return;
        }
        /*****************************/
        //перевірка виходів за межі поля
        public bool CheckLeft()
        {
            if (Id == 0)
            {
                return CheckLeftLine();
            }
            else if (Id == 1)
            {
                return CheckLeftSquare();
            }
            else if (Id == 2)
            {
                return CheckLeftPyramide();
            }
            else if (Id == 3)
            {
                return CheckLeftLeftZ();
            }
            else if (Id == 4)
            {
                return CheckLeftRightZ();
            }
            else if (Id == 5)
            {
                return CheckLeftLeftL();
            }
            else if (Id == 6)
            {
                return CheckLeftRightL();
            }
            else
                return false;
        }
        public bool CheckRight()
        {
            if (Id == 0)
            {
                return CheckRightLine();
            }
            else if (Id == 1)
            {
                return CheckRightSquare();
            }
            else if (Id == 2)
            {
                return CheckRightPyramide();
            }
            else if (Id == 3)
            {
                return CheckRightLeftZ();
            }
            else if (Id == 4)
            {
                return CheckRightRightZ();
            }
            else if (Id == 5)
            {
                return CheckRightLeftL();
            }
            else if (Id == 6)
            {
                return CheckRightRightL();
            }
            else
                return false;
        }
        public bool CheckDown()
        {
            if (Id == 0)
            {
                return CheckDownLine();
            }
            else if (Id == 1)
            {
                return CheckDownSquare();
            }
            else if (Id == 2)
            {
                return CheckDownPyramide();
            }
            else if (Id == 3)
            {
                return CheckDownLeftZ();
            }
            else if (Id == 4)
            {
                return CheckDownRightZ();
            }
            else if (Id == 5)
            {
                return CheckDownLeftL();
            }
            else if (Id == 6)
            {
                return CheckDownRightL();
            }
            else
                return false;
        }
        public bool IsAtBottom()
        {
            if (Id == 0)
            {
                return IsAtBottomLine();
            }
            else if (Id == 1)
            {
                return IsAtBottomSquare();
            }
            else if (Id == 2)
            {
                return IsAtBottomPyramide();
            }
            else if (Id == 3)
            {
                return IsAtBottomLeftZ();
            }
            else if (Id == 4)
            {
                return IsAtBottomRightZ();
            }
            else if (Id == 5)
            {
                return IsAtBottomLeftZ();
            }
            else if (Id == 6)
            {
                return IsABottomRightL();
            }
            else
                return false;
        }
        //Для кожної фігури свої методи перевірки
        //0
        #region LineMove
        private bool CheckLeftLine()
        {
            if (f.Position == 1 || f.Position == 3)
            {
                if (FieldMatrix[Y, X - 1] == true || X == 1)
                    return false;
                else return true;
            }

            else
            {
                if (f.Position == 2 || f.Position == 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (FieldMatrix[Y + i, X] || X == 0)
                            return false;
                    }
                    return true;
                }
            }
            return false;
        }
        private bool CheckRightLine()
        {
            if (f.Position == 1 || f.Position == 3)
            {
                if (FieldMatrix[Y, X + 4] == true || X == this.Width - 5)
                    return false;
                else return true;
            }

            else
            {
                if (f.Position == 2 || f.Position == 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (FieldMatrix[Y + i, X + 2] || X == this.Width - 3)
                            return false;
                    }
                    return true;
                }
                return false;
            }

        }
        private bool CheckDownLine()
        {
            if (f.Position == 1 || f.Position == 3)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (FieldMatrix[Y + 1, X + i] == true || (Y + 1) == this.Height - 1)
                        return false;
                }
                return true;
            }
            else
            {
                if (f.Position == 2 || f.Position == 4)
                {
                    if (FieldMatrix[Y + 4, X + 1] == true || (Y + 4) == this.Height - 1)
                        return false;
                }
                return true;
            }
        }
        private bool IsAtBottomLine()
        {
            for (int i = 0; i < 4; ++i)
            {
                if (FieldMatrix[1, X + i] == true)
                    return true;
            }
            return false;

        }
        #endregion
        //1
        #region SquareMove
        private bool CheckLeftSquare()
        {
            if (FieldMatrix[Y, X - 1] == true || FieldMatrix[Y + 1, X - 1] == true || X == 1)
                return false;
            else
                return true;
        }
        private bool CheckRightSquare()
        {
            if (FieldMatrix[Y, X + 2] == true || FieldMatrix[Y + 1, X + 2] == true || X == Width - 3)
                return false;
            else
                return true;
        }
        private bool CheckDownSquare()
        {
            if (FieldMatrix[Y + 2, X] == true || FieldMatrix[Y + 2, X + 1] == true || (Y + 2) == Height - 1)
                return false;
            return true;
        }
        private bool IsAtBottomSquare()
        {
            if (FieldMatrix[2, X] == true || FieldMatrix[2, X + 1] == true)
                return true;
            else
                return false;
        }
        #endregion
        //2
        #region PyramideMove
        private bool CheckLeftPyramide()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y, X] == true || FieldMatrix[Y + 1, X - 1] == true || X == 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                if (FieldMatrix[Y, X] == true || FieldMatrix[Y + 1, X - 1] == true || FieldMatrix[Y + 2, X] == true || X == 1)
                    return false;
                else
                    return true;
            }

            else if (f.Position == 3)
            {
                if (FieldMatrix[Y + 1, X - 1] == true || FieldMatrix[Y + 2, X] == true || X == 1)
                    return false;
                else
                    return true;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (FieldMatrix[Y + i, X] == true || X == 0)
                        return false;
                }
                return true;
            }
        }
        private bool CheckRightPyramide()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y, X + 2] == true || FieldMatrix[Y + 1, X + 3] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                if (FieldMatrix[Y, X + 2] == true || FieldMatrix[Y + 1, X + 2] == true || FieldMatrix[Y + 2, X + 2] == true || X == Width - 3)
                    return false;
                else
                    return true;
            }

            else if (f.Position == 3)
            {
                if (FieldMatrix[Y + 1, X + 3] == true || FieldMatrix[Y + 2, X + 2] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }

            else if (f.Position == 4)
            {
                if (FieldMatrix[Y, X + 2] == true || FieldMatrix[Y + 1, X + 3] == true || FieldMatrix[Y + 2, X + 2] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
            else return false;
        }
        private bool CheckDownPyramide()
        {
            if (f.Position == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (FieldMatrix[Y + 2, X + i] == true || (Y + 2) == Height - 1)
                        return false;
                }
                return true;
            }
            else
            {
                if (f.Position == 2)
                {
                    if (FieldMatrix[Y + 2, X] == true || FieldMatrix[Y + 3, X + 1] == true || (Y + 3) == Height - 1)
                        return false;
                    else
                        return true;
                }
                if (f.Position == 3)
                {
                    if (FieldMatrix[Y + 2, X] == true || FieldMatrix[Y + 3, X + 1] == true || FieldMatrix[Y + 2, X + 2] == true || (Y + 3) == Height - 1)
                        return false;
                    else
                        return true;
                }
                if (f.Position == 4)
                {
                    if (FieldMatrix[Y + 3, X + 1] == true || FieldMatrix[Y + 2, X + 2] == true || (Y + 3) == Height - 1)
                        return false;
                    else
                        return true;
                }
                return false;
            }

        }
        private bool IsAtBottomPyramide()
        {
            for (int i = 0; i < 3; i++)
            {
                if (FieldMatrix[2, X + i] == true)
                    return true;
            }
            return false;
        }
        #endregion
        //3
        #region LeftZMove
        private bool CheckLeftLeftZ()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y, X - 1] == true || FieldMatrix[Y + 1, X - 1] == true || FieldMatrix[Y + 2, X] == true || X == 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                if (FieldMatrix[Y + 1, X] == true || FieldMatrix[Y + 2, X - 1] == true || X == 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 3)
            {
                if (FieldMatrix[Y, X] == true || FieldMatrix[Y + 1, X] == true || FieldMatrix[Y + 2, X + 1] == true || X == 0) return false;
                else
                    return true;
            }
            else
            {
                if (FieldMatrix[Y, X] == true || FieldMatrix[Y + 1, X - 1] == true || X == 1)
                    return false;
                else
                    return true;
            }
        }
        private bool CheckRightLeftZ()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y, X + 1] == true || FieldMatrix[Y + 1, X + 2] == true || FieldMatrix[Y + 2, X + 2] == true || X == Width - 3)
                    return false;
                else
                    return true;
            }

            else if (f.Position == 2)
            {
                if (FieldMatrix[Y + 1, X + 3] == true || FieldMatrix[Y + 2, X + 2] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 3)
            {
                if (FieldMatrix[Y, X + 2] == true || FieldMatrix[Y + 1, X + 3] == true || FieldMatrix[Y + 2, X + 3] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
            else
            {
                if (FieldMatrix[Y, X + 3] == true || FieldMatrix[Y + 1, X + 2] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
        }
        private bool CheckDownLeftZ()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y + 2, X] == true || FieldMatrix[Y + 3, X + 1] == true || (Y + 3) == Height - 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                if (FieldMatrix[Y + 3, X] == true || FieldMatrix[Y + 3, X + 1] == true || FieldMatrix[Y + 2, X + 2] == true || (Y + 3) == Height - 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 3)
            {
                if (FieldMatrix[Y + 2, X + 1] == true || FieldMatrix[Y + 3, X + 2] == true || Y + 3 == Height - 1)
                    return false;
                else
                    return true;
            }

            else
            {
                if (FieldMatrix[Y + 2, X] == true || FieldMatrix[Y + 2, X + 1] == true || FieldMatrix[Y + 1, X + 2] == true || Y + 2 == Height - 1)
                    return false;
                else
                    return true;
            }
        }
        private bool IsAtBottomLeftZ()
        {
            if (FieldMatrix[2, X] == true || FieldMatrix[3, X + 1] == true)
                return true;
            else
                return false;
        }
        #endregion
        //4
        #region RightZMove
        private bool CheckLeftRightZ()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y, X] == true || FieldMatrix[Y + 1, X - 1] == true || FieldMatrix[Y + 2, X - 1] == true || X == 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                if (FieldMatrix[Y + 1, X - 1] == true || FieldMatrix[Y + 2, X] == true || X == 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 3)
            {
                if (FieldMatrix[Y, X + 1] == true || FieldMatrix[Y + 1, X] == true || FieldMatrix[Y + 2, X] == true || X == 0)
                    return false;
                else
                    return true;
            }
            else
            {
                if (FieldMatrix[Y, X - 1] == true || FieldMatrix[Y + 1, X] == true || X == 1)
                    return false;
                else
                    return true;
            }
        }
        private bool CheckRightRightZ()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y, X + 2] == true || FieldMatrix[Y + 1, X + 2] == true || FieldMatrix[Y + 2, X + 1] == true || X == Width - 3)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                if (FieldMatrix[Y + 1, X + 2] == true || FieldMatrix[Y + 2, X + 3] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 3)
            {
                if (FieldMatrix[Y, X + 3] == true || FieldMatrix[Y + 1, X + 3] == true || FieldMatrix[Y + 2, X + 2] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
            else
            {
                if (FieldMatrix[Y, X + 2] == true || FieldMatrix[Y + 1, X + 3] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
        }
        private bool CheckDownRightZ()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y + 3, X] == true || FieldMatrix[Y + 2, X + 1] == true || Y + 3 == Height - 1)
                    return false;
                else
                    return true;
            }

            else if (f.Position == 2)
            {
                if (FieldMatrix[Y + 2, X] == true || FieldMatrix[Y + 3, X + 1] == true || FieldMatrix[Y + 3, X + 2] == true || Y + 3 == Height - 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 3)
            {
                if (FieldMatrix[Y + 3, X + 1] == true || FieldMatrix[Y + 2, X + 2] == true || Y + 3 == Height - 1)
                    return false;
                else
                    return true;
            }
            else
            {
                if (FieldMatrix[Y + 1, X] == true || FieldMatrix[Y + 2, X + 1] == true || FieldMatrix[Y + 2, X + 2] == true || Y + 2 == Height - 1)
                    return false;
                else
                    return true;
            }
        }
        private bool IsAtBottomRightZ()
        {
            if (FieldMatrix[3, X] == true || FieldMatrix[2, X + 1] == true)
                return true;
            else
                return false;
        }
        #endregion
        //5
        #region LeftLMove
        private bool CheckLeftLeftL()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y, X - 1] == true || FieldMatrix[Y + 1, X + 1] == true || X == 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (FieldMatrix[Y + i, X - 1] == true || X == 1)
                        return false;
                }
                return true;
            }
            else if (f.Position == 3)
            {
                if (FieldMatrix[Y + 1, X - 1] == true || FieldMatrix[Y + 2, X - 1] == true || X == 1)
                    return false;
                else
                    return true;
            }
            else
            {
                if (FieldMatrix[Y + 2, X] == true || FieldMatrix[Y + 1, X + 1] == true || FieldMatrix[Y, X + 1] == true || X == 0)
                    return false;
                else
                    return true;
            }
        }
        private bool CheckRightLeftL()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y, X + 3] == true || FieldMatrix[Y + 1, X + 3] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                if (FieldMatrix[Y, X + 2] == true || FieldMatrix[Y + 1, X + 1] == true || FieldMatrix[Y + 2, X + 1] == true || X == Width - 3)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 3)
            {
                if (FieldMatrix[Y + 1, X + 1] == true || FieldMatrix[Y + 2, X + 3] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
            else
            {
                if (FieldMatrix[Y, X + 3] == true || FieldMatrix[Y + 1, X + 3] == true || FieldMatrix[Y + 2, X + 3] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
        }
        private bool CheckDownLeftL()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y + 1, X] == true || FieldMatrix[Y + 1, X + 1] == true || FieldMatrix[Y + 2, X + 2] || (Y + 2) == Height - 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                if (FieldMatrix[Y + 3, X] == true || FieldMatrix[Y + 1, X + 1] || Y + 3 == Height - 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (FieldMatrix[Y + 3, X + i] == true || (Y + 3) == Height - 1)
                        return false;
                }
                return true;
            }
            else
            {
                if (FieldMatrix[Y + 3, X + 1] == true || FieldMatrix[Y + 3, X + 2] == true || (Y + 3) == Height - 1)
                    return false;
                else
                    return true;
            }
        }
        private bool IsAtBottomLeftL()
        {
            if (FieldMatrix[1, X] == true || FieldMatrix[1, X + 1] == true || FieldMatrix[2, X + 2])
                return true;
            else
                return false;
        }
        #endregion
        //6
        #region RightLMove
        private bool CheckLeftRightL()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y, X - 1] == true || FieldMatrix[Y + 1, X - 1] == true || X == 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (FieldMatrix[Y + i, X - 1] == true || X == 1)
                        return false;
                }
                return true;
            }
            else if (f.Position == 3)
            {
                if (FieldMatrix[Y + 2, X - 1] == true || FieldMatrix[Y + 1, X + 1] == true || X == 1)
                    return false;
                else
                    return true;
            }
            else
            {
                if (FieldMatrix[Y, X] == true || FieldMatrix[Y + 1, X + 1] == true || FieldMatrix[Y + 2, X + 1] || X == 0)
                    return false;
                else return true;
            }

        }
        private bool CheckRightRightL()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y, X + 3] == true || FieldMatrix[Y + 1, X + 1] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                if (FieldMatrix[Y, X + 1] == true || FieldMatrix[Y + 1, X + 1] == true || FieldMatrix[Y + 2, X + 2] || X == Width - 3)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 3)
            {
                if (FieldMatrix[Y + 1, X + 3] == true || FieldMatrix[Y + 2, X + 3] == true || X == Width - 4)
                    return false;
                else
                    return true;
            }
            else
            {
                if (FieldMatrix[Y, X + 3] == true || FieldMatrix[Y + 1, X + 3] == true || FieldMatrix[Y + 2, X + 3] || X == Width - 4)
                    return false;
                else
                    return true;
            }
        }
        private bool CheckDownRightL()
        {
            if (f.Position == 1)
            {
                if (FieldMatrix[Y + 2, X] == true || FieldMatrix[Y + 1, X + 1] == true || FieldMatrix[Y + 1, X + 2] || (Y + 2) == Height - 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 2)
            {
                if (FieldMatrix[Y + 3, X] == true || FieldMatrix[Y + 3, X + 1] == true || (Y + 3) == Height - 1)
                    return false;
                else
                    return true;
            }
            else if (f.Position == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (FieldMatrix[Y + 3, X + i] || (Y + 3) == Height - 1)
                        return false;
                }
                return true;
            }
            else
            {
                if (FieldMatrix[Y + 1, X + 1] == true || FieldMatrix[Y + 3, X + 2] || (Y + 3) == Height - 1)
                    return false;
                else
                    return true;
            }
        }
        private bool IsABottomRightL()
        {
            if (FieldMatrix[2, X] == true || FieldMatrix[1, X + 1] == true || FieldMatrix[1, X + 2] == true)
                return true;
            else
                return false;
        }
        #endregion
    }
}
//Frame
namespace Tetris
{
    class Frame : GameObject
    {
        public Frame()
        {
            Symbol = '#';
            Height = 20;
            Width = 12;
        }
        public void SetFrame()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            for (int i = 0; i < Height; i++)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = i;
                Console.WriteLine(Symbol);
            }
            for (int i = 1; i < Width; i++)
            {
                Console.CursorLeft = i;
                Console.CursorTop = Height - 1;
                Console.WriteLine(Symbol);
            }
            for (int i = 0; i < Height; i++)
            {
                Console.CursorLeft = Width - 1;
                Console.CursorTop = i;
                Console.WriteLine(Symbol);
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
//Factory
namespace Tetris
{
    abstract class FigureFactory
    {
        public abstract Figure CreateFigure();
    }
    class LineFactory : FigureFactory
    {
        public override Figure CreateFigure()
        {
            return new Line();
        }
    }
    class SquareFactory : FigureFactory
    {
        public override Figure CreateFigure()
        {
            return new Square();
        }
    }
    class RightLFactory : FigureFactory
    {
        public override Figure CreateFigure()
        {
            return new RightL();
        }
    }
    class LeftLFactory : FigureFactory
    {
        public override Figure CreateFigure()
        {
            return new LefL();
        }
    }
    class PyramideFactory : FigureFactory
    {
        public override Figure CreateFigure()
        {
            return new Pyramide();
        }
    }
    class LeftZFactory : FigureFactory
    {
        public override Figure CreateFigure()
        {
            return new LeftZ();
        }
    }
    class RightZFactory : FigureFactory
    {
        public override Figure CreateFigure()
        {
            return new RightZ();
        }
    }
}
//Game
namespace Tetris
{
    class TetrisGame : Game
    {
        private static TetrisGame _instance;
        protected TetrisGame() { }
        public static TetrisGame Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TetrisGame();
                }
                return _instance;
            }
        }
        public override void Play()
        {
            Frame frame = new Frame();
            Field f = new Field();
            frame.SetFrame();
            f.NewFigure();
            f.DrawField();
            //створення обєктів подій для клавіатури
            EventUp up = new EventUp(); //вверх
            EventDown down = new EventDown(); //вниз
            EventLeft left = new EventLeft(); // вліво
            EventRight right = new EventRight(); //вправо
            //<КлассИлиОбъект>.<ИмяСобытия> += <КлассЧейМетодДолженЗапуститься>.<МетодПодходящийПоСигнатуре>
            up.UpEvent += f.UpFig;
            down.DownEvent += f.DownFig;
            left.LeftEvent += f.LeftFig;
            right.RightEvent += f.RightFig;
            ConsoleKeyInfo cki;
            while (true)
            {
                if (f.CheckDown() == true)
                    f.Move();
                else
                {
                    while (true)
                    {
                        bool flag = f.CheckLine();
                        if (flag == false)
                            break;
                    }
                    f.NewFigure();
                    if (f.IsAtBottom() == true)
                        break;
                }
                int dificulty = 11 - 2 * f.Level;
                for (int i = 0; i < dificulty; i++) //кількість ітерацій імітує швидкість
                {
                    System.Threading.Thread.Sleep(50);
                    if (Console.KeyAvailable)
                    {
                        cki = Console.ReadKey();
                        switch (cki.Key)
                        {
                            case ConsoleKey.UpArrow: //перевертає фігуру
                                {
                                    up.UpUserEvent(); //обробка події
                                    f.DrawField(); //перерисовує поле
                                    break;
                                }
                            case ConsoleKey.DownArrow:
                                {
                                    down.DownUserEvent();
                                    break;
                                }
                            case ConsoleKey.LeftArrow:
                                {
                                    left.LeftUserEvent();
                                    f.DrawField();
                                    break;
                                }
                            case ConsoleKey.RightArrow:
                                {
                                    right.RightUserEvent();
                                    f.DrawField();
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }

                    }
                }
            }
            Console.Clear();
            Console.WriteLine("\n\n\n\n\n      GAME OVER");
            Console.WriteLine("\n   TOTAL SCORES " + (f.Level * 1000 + f.Score) + "\n\n\n\n\n\n\n\n\n");
            Console.WriteLine("Press any key");
            Console.ReadLine();
        }
    }
}
//Menu
namespace Tetris
{
    sealed class TetrisMenu : IMenu
    {
        public TetrisMenu()
        {
            Console.Title = "Tetris";
            NumberOfItems = 3;
            Item = new string[NumberOfItems];
            Item[0] = "Start";
            Item[1] = "Game Information";
            Item[2] = "Exit";
        }
        public int NumberOfItems { get; set; }
        public string[] Item { get; set; }
        public void ShowMenu()
        {
            Console.Clear();
            for (int i = 0; i < Item.Length; ++i)
            {
                Console.WriteLine("{0}. {1}", i + 1, Item[i]);
            }
        }
        public void ChoiceHandler(string choice)
        {
            int userChoice = int.Parse(choice);
            try
            {
                if (userChoice < 1 || userChoice > 3)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    switch (userChoice)
                    {
                        case 1:
                            Console.Clear();
                            Game tg = TetrisGame.Instance;
                            tg.Play();
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("Up arrow key - to rotate figure");
                            Console.WriteLine("Down arrow key - to fall faster");
                            Console.WriteLine("Left arrow key - to move left");
                            Console.WriteLine("Right arrow key - to move right");
                            Console.WriteLine("\n\n\nPress any key");
                            Console.ReadLine();
                            break;
                        case 3://end == true
                            break;
                        default:
                            break;

                    }
                    return;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Wrong Choice!!!\nTry again\n{0}", e.Message);
            }
            return;
        }
    }
}