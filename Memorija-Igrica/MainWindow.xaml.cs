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
using System.Threading;
using System.Timers;

namespace Memorija_Igrica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        Tabla tabla = new Tabla();
        static int izabrano;
        int previous;
        public static TextBlock txt;
        public static TextBlock prevTxt;
        public static Rectangle rec;
        public static Rectangle prevRec;
        public MainWindow()
        {
            InitializeComponent();
            izabrano = 0;
            previous = -1;
            int counter = 0;
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    Rectangle rect = new Rectangle();
                    TextBlock text = new TextBlock();
                    Grid gr = new Grid();
                    Grid.SetColumn(gr, i);
                    Grid.SetRow(gr, j);
                    gr.MouseLeftButtonDown += Gr_MouseLeftButtonDown;
                    gr.Name = $"p{counter}";
                    RegisterName(gr.Name, gr);
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFromString(tabla.Polja[i].Boja);
                    rect.Fill = color;
                    rect.Stroke = Brushes.Black;
                    rect.StrokeThickness = 2;
                    rect.Name = $"r{counter}";
                    RegisterName(rect.Name, rect);
                    text.Text = "?";
                    text.Name = $"t{counter}";
                    RegisterName(text.Name, text);
                    counter++;
                    text.Margin = new Thickness(5);
                    text.HorizontalAlignment = HorizontalAlignment.Center;
                    text.VerticalAlignment = VerticalAlignment.Center;
                    text.FontSize = 20;
                    text.FontWeight = FontWeights.Bold;
                    gr.Children.Add(rect);
                    gr.Children.Add(text);
                    myGrid.Children.Add(gr);
                }
            }
        }

        private void Gr_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!tabla.Game) return;
            string gName = (sender as Grid).Name;
            int gNum = int.Parse(gName.Substring(1, gName.Length-1));
            if (tabla.Polja[gNum].Pogodjeno || gNum == previous) return;
            txt = this.FindName($"t{gNum}") as TextBlock;
            rec = this.FindName($"r{gNum}") as Rectangle;
            txt.Text = tabla.Polja[gNum].Broj.ToString();
            rec.Fill = Brushes.LightGreen;
            izabrano++;
            if(izabrano == 2)
            {
                prevRec = this.FindName($"r{previous}") as Rectangle;
                prevTxt = this.FindName($"t{previous}") as TextBlock;
                tabla.Game = false;
                if(tabla.Polja[previous].Broj == tabla.Polja[gNum].Broj)
                {
                    rec.Fill = Brushes.DarkGreen;
                    prevRec.Fill = Brushes.DarkGreen;
                    tabla.Polja[previous].Pogodjeno = true;
                    tabla.Polja[gNum].Pogodjeno = true;
                    tabla.Game = true;
                } else
                {
                    System.Timers.Timer aTimer = new System.Timers.Timer(1000);
                    aTimer.Elapsed += OnTimedEvent;
                    aTimer.Enabled = true;
                    aTimer.AutoReset = false;
                }
                previous = -1;
                izabrano = 0;
            } else
            {
                previous = gNum;
            }
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            tabla.Game = true;
            this.Dispatcher.Invoke(() =>
            {
                rec.Fill = Brushes.White;
                prevRec.Fill = Brushes.White;
                txt.Text = "?";
                prevTxt.Text = "?";
            });
            
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            tabla.Game = true;
            StartButton.IsEnabled = false;
            Random rand = new Random();
            List<int> nums = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                nums.Add(i % 10 + 1);
            }
            for(int i = 0; i < tabla.Polja.Length; i++)
            {
                int randomNum = rand.Next(nums.Count);
                tabla.Polja[i].Broj = nums[randomNum];
                nums.RemoveAt(randomNum);
            }
        }
    }
}
