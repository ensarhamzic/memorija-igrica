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
using System.Diagnostics;

/*
 * Igra memorije
 Ako je razlika izmedju pogodaka:
 <5s -> 10 poena
 5-15s -> 5 poena
 15-30s -> 3 poena
 >30s -> 1 poen
 (Prvi pogodak uvek nosi 1 poen
 ********************************
 Igra traje max 90 sekundi, i moze se u bilo kojem trenutku prekinuti.
*/

namespace Memorija_Igrica
{

    public partial class MainWindow : Window
    {
        Tabla tabla = new Tabla();
        static int izabrano; // broj poteza
        int previous; // index kvadrata proslog poteza
        int score;
        int hits; // Broj pogodaka
        int time; // Vreme do kraja igre
        System.Timers.Timer timer; // Tajmer za time
        TextBlock txt; // textblock za trenutni potez
        TextBlock prevTxt; // textblock proslog poteza
        Rectangle rec; // kvadrat trenutnog poteza
        Rectangle prevRec; // kvadrat proslog poteza
        Stopwatch stopWatch; // Brojac vremena izmedju pogodaka


        public MainWindow()
        {
            InitializeComponent();
            izabrano = 0;
            previous = -1;
            score = 0;
            hits = 0;
            time = 0;
            int counter = 0;
            // Pravimo plocice 5*4
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
                    rect.Fill = Brushes.White;
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

        // Click event za grid na koji stisnemo za potez
        private void Gr_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!tabla.Game) return;
            string gName = (sender as Grid).Name;
            int gNum = int.Parse(gName.Substring(1, gName.Length-1));
            if (tabla.Polja[gNum].Pogodjeno || gNum == previous) return;
            txt = this.FindName($"t{gNum}") as TextBlock; // Lociramo textblock koji se nalazi u gridu na koji smo kliknuli
            rec = this.FindName($"r{gNum}") as Rectangle; // Lociramo rectangle koji se nalazi u gridu na koji smo kliknuli
            txt.Text = tabla.Polja[gNum].Broj.ToString();
            rec.Fill = Brushes.LightGreen;
            izabrano++;
            if(izabrano == 2)
            {
                prevRec = this.FindName($"r{previous}") as Rectangle;
                prevTxt = this.FindName($"t{previous}") as TextBlock;
                tabla.Game = false;
                // Da li je u pitanju pogodak
                if(tabla.Polja[previous].Broj == tabla.Polja[gNum].Broj)
                {
                    // Bodovanje
                    if(stopWatch != null)
                    {
                        stopWatch.Stop();
                        int duration = (int) stopWatch.ElapsedMilliseconds / 1000;
                        if(duration < 5)
                        {
                            score += 10;
                        } else if(duration < 15)
                        {
                            score += 5;
                        } else if(duration < 30)
                        {
                            score += 3;
                        } else
                        {
                            score++;
                        }
                        
                    } else
                    {
                        score++; // Prvi pogodatak nosi 1 bod
                    }
                    stopWatch = new Stopwatch();
                    stopWatch.Start();
                    rec.Fill = Brushes.DarkGreen;
                    prevRec.Fill = Brushes.DarkGreen;
                    tabla.Polja[previous].Pogodjeno = true;
                    tabla.Polja[gNum].Pogodjeno = true;
                    tabla.Game = true;
                    Score.Text = score.ToString();
                    hits++;
                    // Pobeda
                    if (hits == 10)
                    {
                        MessageBox.Show($"Pobedili ste! Osvojili ste {score} poena", "Pobeda");
                        EndButton.IsEnabled = false;
                        StartButton.IsEnabled = true;
                        timer.Enabled = false;
                    }
                } else
                {
                    // Nakon 1s, "okreni" plocice
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
            // Ovako mora ako iz neke druge f-je menjamo style
            this.Dispatcher.Invoke(() =>
            {
                rec.Fill = Brushes.White;
                prevRec.Fill = Brushes.White;
                txt.Text = "?";
                prevTxt.Text = "?";
            });
            
        }

        // Funkcija koja svake sekunde smanjuje time za 1 i po isteku vremena zavrsava igru
        private void TimerTick(object source, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                Time.Text = Convert.ToString((--time));
            });
            
            if(time == 0)
            {
                timer.Enabled = false;
                tabla.Game = false;
                MessageBox.Show("Vreme je isteklo. Izgubili ste!", "Vreme isteklo");
                this.Dispatcher.Invoke(() =>
                {
                    EndButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); // Simulacija klika
                });
                
            }
            
        }

        // Start igre - resetujemo sve i random rasporedimo brojeve
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            timer = new System.Timers.Timer(1000); // Na svakih 1000ms tj 1s
            timer.Elapsed += TimerTick;
            timer.AutoReset = true;
            timer.Enabled = true;
            time = 90; // Trajanje igre
            izabrano = 0;
            previous = -1;
            score = 0;
            hits = 0;
            stopWatch = null;
            for (int i = 0; i < 20; i++)
            {
                tabla.Polja[i].Pogodjeno = false;
                this.Dispatcher.Invoke(() =>
                {
                    (this.FindName($"r{i}") as Rectangle).Fill = Brushes.White;
                    (this.FindName($"t{i}") as TextBlock).Text = "?";
                });
            }
            Score.Text = score.ToString();
            Time.Text = time.ToString();
            
            tabla.Game = true;
            StartButton.IsEnabled = false;
            EndButton.IsEnabled = true;
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

        // Manuelno zavrsimo igru (zabranimo dalje igranje sve do ponovnog zapocinjanja igre
        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            stopWatch = null;
            tabla.Game = false;
            EndButton.IsEnabled = false;
            StartButton.IsEnabled = true;
            timer.Enabled = false;
        }
    }
}
