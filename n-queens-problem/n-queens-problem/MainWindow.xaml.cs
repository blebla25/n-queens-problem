

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lokalno_Iskalni_Algoritmi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            int size = Size.SelectedIndex + 4;
            InitializeArray(size);
            DrawChessboard(size);
        }


        Grid ChessBoard = new Grid() { Name = "myGrid" };
        int[,] array;
        int counter = 0;
        int temp = 0;
        int deltaTemp = 0;


        private void InitializeArray(int size)
        {
            array = new int[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    array[i, j] = 0;
                }
        }

        private void DrawChessboard(int size)
        {

            GridLengthConverter myGridLengthConverter = new GridLengthConverter();
            GridLength side = (GridLength)myGridLengthConverter.ConvertFromString("Auto");
            for (int i = 0; i < size; i++)
            {
                ChessBoard.ColumnDefinitions.Add(new ColumnDefinition());
                ChessBoard.ColumnDefinitions[i].Width = side;
                ChessBoard.RowDefinitions.Add(new RowDefinition());
                ChessBoard.RowDefinitions[i].Height = side;
            }
            Border[,] square = new Border[size, size];
            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                {

                    square[row, col] = new Border();
                    square[row, col].Height = 50 * (12 / (float)size);
                    square[row, col].Width = 50 * (12 / (float)size);
                    Grid.SetColumn(square[row, col], col);
                    Grid.SetRow(square[row, col], row);
                    if ((row + col) % 2 == 0)
                    {
                        square[row, col].Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                    }
                    else
                    {
                        square[row, col].Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    }
                    if (array[row, col] == 1)
                    {
                        ImageBrush test = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri(@"Resources/Quenn.png", UriKind.Relative))
                        };

                        if ((row + col) % 2 == 0)
                        {
                            square[row, col].BorderBrush = Brushes.White;
                        }
                        else
                        {
                            square[row, col].BorderBrush = Brushes.Black;
                        }

                        square[row, col].BorderThickness = new Thickness(10 * (12 / (float)size), 10 * (12 / (float)size), 10 * (12 / (float)size), 10 * (12 / (float)size));
                        square[row, col].Background = test;
                        square[row, col].Padding = new Thickness(5);

                    }


                    ChessBoard.Children.Add(square[row, col]);
                }

            Test.Children.Add(ChessBoard);
        }

        private void InitializeQuenn(int row, int column)
        {
            array[row, column] = 1;

        }
        private void InitializeQuenns(List<Point> queens)
        {
            InitializeArray(queens.Count);
            for(int i = 0; i< queens.Count; i++)
            {
                array[(int)queens[i].X, (int)queens[i].Y] = 1;
            }
            

        }
        private void RandomQuenns(int size)
        {
            InitializeArray(size);
            Random rnd = new Random();
            for (int i = 0; i < size; i++)
            {
                InitializeQuenn(i, rnd.Next(size));
            }


        }
        private void Change(object sender, RoutedEventArgs e)
        {
            int size = Size.SelectedIndex + 4;
            InitializeArray(size);
            ClearBoard();
            DrawChessboard(size);
            Heuristic.Content = 0;
            NumOfIterations.Content = "";
            NarisiParametre();
        }
        private void ClearBoard()
        {
            Test.Children.Clear();
            Test.ColumnDefinitions.Clear();
            Test.RowDefinitions.Clear();
            ChessBoard.Children.Clear();
            ChessBoard.ColumnDefinitions.Clear();
            ChessBoard.RowDefinitions.Clear();
        }

        private void NarisiParametre()
        {
            if (SearchAlgorithm.SelectedIndex != 0)
            {
                NumOfMoves.Visibility = System.Windows.Visibility.Hidden;
                movesSameDirection.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                NumOfMoves.Visibility = System.Windows.Visibility.Visible;
                movesSameDirection.Visibility = System.Windows.Visibility.Visible;
            }
            if (SearchAlgorithm.SelectedIndex != 1)
            {
                Temp.Visibility = System.Windows.Visibility.Hidden;
                TempText.Visibility = System.Windows.Visibility.Hidden;

            }
            else
            {
                Temp.Visibility = System.Windows.Visibility.Visible;
                TempText.Visibility = System.Windows.Visibility.Visible;

            }
            if (SearchAlgorithm.SelectedIndex != 2)
            {
                Models.Visibility = System.Windows.Visibility.Hidden;
                ModelsText.Visibility = System.Windows.Visibility.Hidden;

            }
            else
            {
                Models.Visibility = System.Windows.Visibility.Visible;
                ModelsText.Visibility = System.Windows.Visibility.Visible;

            }
            if (SearchAlgorithm.SelectedIndex != 3)
            {
                Population.Visibility = System.Windows.Visibility.Hidden;
                PopulationText.Visibility = System.Windows.Visibility.Hidden;

            }
            else
            {
                Population.Visibility = System.Windows.Visibility.Visible;
                PopulationText.Visibility = System.Windows.Visibility.Visible;

            }
        }
        private void Generate(object sender, RoutedEventArgs e)
        {

            InitializeArray(Size.SelectedIndex + 4);
            RandomQuenns(Size.SelectedIndex + 4);


            ClearBoard();
            DrawChessboard(Size.SelectedIndex + 4);

            List<Point> seznam = GetQueens();
            int tmp = GetHeuristic(seznam);
            Heuristic.Content = tmp;
            NumOfIterations.Content = "";

        }

        private int GetHeuristic(List<Point> seznam)
        {
            int count = 0;
            for (int k = 0; k < seznam.Count; k++)
            {
                if (!IsSafe((int)seznam[k].X, (int)seznam[k].Y))
                {
                    for (int i = 0; i < Size.SelectedIndex + 4; i++)
                    {
                        if (array[(int)seznam[k].X, i] == 1)
                        {
                            if (i != (int)seznam[k].Y) count++;
                        }
                        if (array[i, (int)seznam[k].Y] == 1)
                        {
                            if (i != (int)seznam[k].X) count++;
                        }
                        for (int j = 0; j < Size.SelectedIndex + 4; j++)
                        {
                            int difRow = Math.Abs(i - (int)seznam[k].X);
                            int difCol = Math.Abs(j - (int)seznam[k].Y);
                            if (difRow == difCol && array[i, j] == 1)
                            {
                                if (!(i == (int)seznam[k].X && j == (int)seznam[k].Y)) count++;
                            }
                        }
                    }

                }
            }
            
            return count/2;

        }
        
        private List<Point> GetQueens()
        {
            List<Point> vector = new List<Point>();

            for (int i = 0; i < Size.SelectedIndex + 4; i++)
                for (int j = 0; j < Size.SelectedIndex + 4; j++)
                {
                    if (array[i, j] == 1)
                    {
                        vector.Add(new Point(i, j));
                    }
                }
            return vector;
        }



        private bool IsSafe(int row, int col)
        {
            for (int i = 0; i < Size.SelectedIndex + 4; i++)
            {
                if (array[row, i] == 1)
                {
                    if (i != col) return false;
                }
                if (array[i, col] == 1)
                {
                    if (i != row) return false;
                }
                for (int j = 0; j < Size.SelectedIndex + 4; j++)
                {
                    int difRow = Math.Abs(i - row);
                    int difCol = Math.Abs(j - col);
                    if (difRow == difCol && array[i, j] == 1)
                    {
                        if (!(i == row && j == col)) return false;
                    }
                }
            }
            return true;
        }


        private List<Point> Kopiraj(List<Point> queens)
        {
            List<Point> myList = new List<Point>();
            for (int i = 0; i < queens.Count; i++)
            {
                myList.Add(queens.ElementAt(i));
            }
            return myList;
        }

        private List<Point> Razvij(List<Point> queens) {
            int bestHeuristic;
            bool check = true;
        
            List<Point> bestQueens = new List<Point>();
            List<Point> reserveQueens = new List<Point>();
            bestQueens = Kopiraj(queens);
            InitializeQuenns(queens);
            bestHeuristic = GetHeuristic(queens);
            for(int i  = 0; i < queens.Count; i++) { 
                for(int j = 0; j < queens.Count; j++)
                {
                    if (!((int)queens[i].X == i && (int)queens[i].Y == j))
                    {

                        if (SearchAlgorithm.SelectedIndex == 0)                   // razvij za Hill Climbing
                        {
                            int pomiki = Int32.Parse(movesSameDirection.Text);

                            Point tmp = new Point(i, j);
                            queens[i] = tmp;
                            InitializeQuenns(queens);
                            int tHeuristic = GetHeuristic(queens);
                            if (bestHeuristic >= tHeuristic)
                            {
                                check = false;
                                bestQueens = Kopiraj(queens);
                                bestHeuristic = tHeuristic;
                                if (bestHeuristic == tHeuristic)
                                {
                                    counter++;
                                }
                                break;
                            }
 


                            queens = Kopiraj(bestQueens);
                            if (counter > pomiki + bestQueens.Count -1)
                                return bestQueens;
                        }
                        else if(SearchAlgorithm.SelectedIndex == 1)                   // razvij za Simulated Annealing
                        {

                            Point tmp = new Point(i, j);
                            queens[i] = tmp;
                            InitializeQuenns(queens);
                            int tHeuristic = GetHeuristic(queens);
                            if(bestHeuristic > tHeuristic)
                            {
                                bestQueens = Kopiraj(queens);
                                bestHeuristic = tHeuristic;
                                
                            }
                            else
                            {
                                int diff = tHeuristic - bestHeuristic;
                                Random rnd = new Random();
                                double proba = rnd.NextDouble();
                                if((double)Math.Pow(Math.E, -(diff/temp)) < proba) {
                                    bestQueens = Kopiraj(queens);
                                    bestHeuristic = tHeuristic;
                                }
                                
                            }
                            temp -= deltaTemp;
                            queens = Kopiraj(bestQueens);
                            counter++;
                            if (temp <= 0) return bestQueens;
            
                        }
                    }
                   
                }
            }

           
            return bestQueens;
        }
        private bool IsEqual(List<Point> first, List<Point> second)
        {
            bool status = true;
            for(int i = 0; i< first.Count; i++)
            {
                if (first[i].X != second[i].X) status = false;
                if (first[i].Y != second[i].Y) status = false;
            }
            return status;
        }

        private void VzpenjanjeNaHrib(List<Point> queens)
        {
            counter = 0;
            InitializeQuenns(queens);
            List<Point> rezerva = new List<Point>();
            List<Point> prejsni = Kopiraj(queens);
            int pomiki = Int32.Parse(movesSameDirection.Text);
            while (counter < pomiki)
            {
                
                List<Point> tmp = Razvij(queens);
                if (IsEqual(prejsni, tmp))
                {
                    rezerva = Kopiraj(tmp);
                    RandomQuenns(queens.Count);
                    tmp = GetQueens();
                }

                InitializeQuenns(tmp);
                
                queens = Kopiraj(tmp);
                if (GetHeuristic(tmp) == 0)
                    break;
                prejsni = Kopiraj(tmp);

            }
            
            if (rezerva.Count > 0)
                if (GetHeuristic(rezerva) < GetHeuristic(queens))
                    queens = Kopiraj(rezerva);
            ClearBoard();
            InitializeQuenns(queens);
            DrawChessboard(queens.Count);
            Heuristic.Content = GetHeuristic(queens);
            NumOfIterations.Content = counter.ToString();
        }

        private void SimuliranoOhlajanje(List<Point> queens)
        {
            counter = 0;
            InitializeQuenns(queens);
            List<Point> prejsni = Kopiraj(queens);
            List<Point> rezerva = new List<Point>();
            temp = Int32.Parse(startTemp.Text);
            deltaTemp = Int32.Parse(changeTemp.Text);

            while (temp > 0)
            {
                int t = temp;
                List<Point> tmp = Razvij(queens);
                if (IsEqual(prejsni, tmp))
                {
                    rezerva = Kopiraj(tmp);
                    RandomQuenns(queens.Count);
                    tmp = GetQueens();
                }
                InitializeQuenns(tmp);
                queens = Kopiraj(tmp);
                if (GetHeuristic(tmp) == 0)
                    break;
                prejsni = Kopiraj(tmp);

            }
            if (rezerva.Count > 0)
            {
                InitializeQuenns(rezerva);
                int a = GetHeuristic(rezerva);
                InitializeQuenns(queens);
                int b = GetHeuristic(queens);
                if (a < b)
                    queens = Kopiraj(rezerva);
            }
            InitializeQuenns(queens);
            ClearBoard();
            DrawChessboard(queens.Count);

            Heuristic.Content = GetHeuristic(queens);
            NumOfIterations.Content = counter.ToString();
        }

        private List<Point> LokalniZarek(List<Point> queens)
        {
            counter = 0;
            InitializeQuenns(queens);
            int maxNumberIterations = Int32.Parse(maxNumOfIterations.Text);
            int states = Int32.Parse(numOfModels.Text);
            List<int> heuristics = new List<int>();
            List<List<Point>> statesList = new List<List<Point>>();
            Random rand = new Random();
            int bestHeuristic = 100;
            for (int i = 0; i < states; i++)   
            {
                List<Point> queenArray = new List<Point>();
                for (int k = 0; k < queens.Count; k++)
                {
                    int row = (int)queens[k].X;
                    int col = rand.Next(0, queens.Count);
                    Point q = new Point(row, col);
                    queenArray.Add(q);
                }
                InitializeQuenns(queenArray);
                int t = GetHeuristic(queenArray);
                if (t < bestHeuristic) bestHeuristic = t;
                statesList.Add(queenArray);
            }

            List<List<Point>> NewStatesArray = new List<List<Point>>();

            List<List<Point>> tmp = new List<List<Point>>();
            for (int k = 0; k < maxNumberIterations; k++) 
            {

                tmp.Clear();
                for (int i = 0; i < statesList.Count; i++)
                {
                    tmp.Add(Kopiraj(statesList[i]));
                }
                counter++;
                


                statesList = Razvij_List(statesList, bestHeuristic);

                for (int i = 0; i < statesList.Count; i++)
                {
                    if(IsEqual(statesList[i], tmp[i])) {
                        RandomQuenns(statesList[i].Count);
                        statesList[i] = GetQueens();
                    }
                }

                for (int i = 0; i < statesList.Count; i++)
                {
                   
                    
                    InitializeQuenns(statesList[i]);
                    int h = GetHeuristic(statesList[i]);
                    if (h == 0)
                        return statesList[i];
                    heuristics.Add(h);



                }

                NewStatesArray = sortQueens(statesList, heuristics);
                statesList.Clear();
                heuristics.Clear();

                for (int l = 0; l < states; l++)  //for desired number of states elected
                {
                    statesList.Add(NewStatesArray[l]);
                }
               
            }
           
            return statesList[0];
        }

        public List<List<Point>> Razvij_List (List<List<Point>> statesList, int heuristic)
        {
            bool status = false;
            for (int i = 0; i < statesList.Count; i++)     
            {
                for (int j = 0; j < statesList[i].Count; j++)         
                {
                    int tmpX = (int)statesList[i][j].X;
                    for (int k = 0; k < statesList[i].Count; k++)        
                    {
                        Point rezerva = statesList[i][j];
                        Point tmp = new Point(j, k);
                        
                        InitializeQuenns(statesList[i]);
                        int a = GetHeuristic(statesList[i]);
                        statesList[i][j] = tmp;
                        InitializeQuenns(statesList[i]);
                        int b = GetHeuristic(statesList[i]);
                        if (a < b)
                            statesList[i][j] = rezerva;
                        InitializeQuenns(statesList[i]);
                        if (GetHeuristic(statesList[i]) < a)
                        {
                            Point t = new Point(tmpX, statesList[i][j].Y);
                            statesList[i][j] = t;
                            status = true;
                            break;

                          
                        }
                        
                        
                    }
                    if (status) break;
                }
            }
            return statesList;
        }


        private List<Point> GenetskiAlgoritem(List<Point> queens)
        {
            counter = 0;
            int population = Int32.Parse(sizePop.Text);
            population = population - (population % 2);

            float elitizem = (float)(Int32.Parse(elitePercent.Text) * population / 100);
            float krizanje = float.Parse(crossProba.Text);
            float mutacija = float.Parse(mutaProba.Text);
            int generacije = Int32.Parse(numOfGenerations.Text);

         
            List<List<Point>> queensList = new List<List<Point>>();
            List<List<Point>> tmpQueensList = new List<List<Point>>();
            List<int> heuristics = new List<int>();
            for (int i = 0; i < population; i++)
            {
                RandomQuenns(queens.Count);
                queensList.Add(GetQueens());
            }

            for (int i = 0; i < generacije; i++)
            {


                for (int j = 0; j < population; j++)        //Heuristika vseh možnosti
                {
                    int h;
                    InitializeQuenns(queensList[j]);
                    h = GetHeuristic(queensList[j]);
                    if (h == 0)                             //Našli smo rešitev 
                    {
                        return queensList[j];
                       
                    }
                    heuristics.Add(h);
                }

                queensList = sortQueens(queensList, heuristics);

                for (int z = 0; z < elitizem; z++)
                {
                    tmpQueensList.Add(Kopiraj(queensList[z]));
                }

                queensList = Krizaj(queensList, krizanje);
                queensList = Mutiraj(queensList, mutacija);

                queensList.InsertRange(0, tmpQueensList);
                queensList.RemoveRange(population, queensList.Count - population);

                heuristics.Clear();
                counter++;
            }
            for (int j = 0; j < population; j++)        //Heuristika vseh možnosti
            {
                int h;
                InitializeQuenns(queensList[j]);
                h = GetHeuristic(queensList[j]);
                if (h == 0)                             //Našli smo rešitev 
                {
                    return queensList[j];

                }
                heuristics.Add(h);
            }

            queensList = sortQueens(queensList, heuristics);

            return queensList[0];
        }

        public List<List<Point>> Krizaj(List<List<Point>> statesList, float krizanje)
        {
            Random rand = new Random();
            for (int i = 0; i < statesList.Count; i+=2)
            {
                
                double proba = rand.NextDouble();
                if(proba < krizanje)
                {
                    int indexStanja = rand.Next(0, statesList[i].Count);
                    for(int j = 0; j < indexStanja; j++)
                    {
                        int tmp = (int)statesList[i][j].X;
                        Point a = new Point(statesList[i + 1][j].X, statesList[i][j].Y);
                        statesList[i][j] = a;
                        Point b = new Point(tmp, statesList[i+1][j].Y);
                        statesList[i + 1][j] = b;
                    
                    }
                }
            }
            return statesList;
        }

        public List<List<Point>> Mutiraj(List<List<Point>> statesList, float mutiranje)
        {
            Random rand = new Random();
            for (int i = 0; i < statesList.Count; i += 2)
            {
               
                double proba = rand.NextDouble();
                if (proba < mutiranje)
                {
                    int row = rand.Next(0, statesList[i].Count);
                    int col = rand.Next(0, statesList[i].Count);
                    Point a = new Point(row, col);
                    statesList[i][row] = a;
                }
            }
            return statesList;
        }


        private void StartAlgorithm(object sender, RoutedEventArgs e)
        {
            if (SearchAlgorithm.SelectedIndex == 0)
            {
                if (Heuristic.Content == null || Heuristic.Content.ToString() == "0" || movesSameDirection.Text == "")
                    MessageBox.Show("You dont have all parameters for algorithm.");
                else
                    VzpenjanjeNaHrib(GetQueens());
            }
            else if(SearchAlgorithm.SelectedIndex == 1)
            {
                if (Heuristic.Content == null || Heuristic.Content.ToString() == "0" || startTemp.Text == "" || changeTemp.Text == "")
                    MessageBox.Show("You dont have all parameters for algorithm.");
                else
                    SimuliranoOhlajanje(GetQueens());
            }
            else if (SearchAlgorithm.SelectedIndex == 2)
            {
                if (Heuristic.Content == null || Heuristic.Content.ToString() == "0" || numOfModels.Text == "" || maxNumOfIterations.Text == "")
                    MessageBox.Show("You dont have all parameters for algorithm.");
                else
                {
                    List<Point> queens = new List<Point>();
                    queens = LokalniZarek(GetQueens());
                    InitializeQuenns(queens);
                    ClearBoard();
                    DrawChessboard(queens.Count);
                    NumOfIterations.Content = counter.ToString();
                    Heuristic.Content = GetHeuristic(queens);
                    
                }
            }
            else if (SearchAlgorithm.SelectedIndex == 3)
            {
                if (Heuristic.Content == null || Heuristic.Content.ToString() == "0" || sizePop.Text == "" || elitePercent.Text == "" || crossProba.Text == "" || mutaProba.Text == "" || numOfGenerations.Text == "")
                    MessageBox.Show("You dont have all parameters for algorithm.");
                else
                {
                    
                    List<Point> queens = new List<Point>();
                    queens = GenetskiAlgoritem(GetQueens());
                    InitializeQuenns(queens);
                    ClearBoard();
                    DrawChessboard(queens.Count);
                    NumOfIterations.Content = counter.ToString();
                    Heuristic.Content = GetHeuristic(queens);
                }
            }
        }


        public List<List<Point>> sortQueens(List<List<Point>> statesArray, List<int> heuristics)
        {
            List<List<Point>> statesListSorted = new List<List<Point>>();
            List<int> heuristicsCopy = new List<int>();

            statesListSorted.AddRange(statesArray);
            heuristicsCopy.AddRange(heuristics);
            heuristics.Sort();

            List<int> index = new List<int>();
            for (int i = 0; i < heuristicsCopy.Count; i++)
            {
                for (int j = 0; j < heuristics.Count; j++)
                {
                    if (heuristicsCopy[i] == heuristics[j])
                    {
                        if (index.Contains(j))
                            continue;
                        statesListSorted[j] = statesArray[i];
                        index.Add(j);
                        break;
                    }
                }
            }
            return statesListSorted;
        }

    }
}
