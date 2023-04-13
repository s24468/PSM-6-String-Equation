using System;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;


namespace PSM_6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow
    {
        private readonly DispatcherTimer _timer;

        // private ChartValues<ObservablePoint> chart;
        private ChartValues<double> chart;
        private LineSeries lineSeries;

        public MainWindow()
        {
            InitializeComponent();
            //tworzenie wykresu
            // chart = new ChartValues<ObservablePoint>()
            //     { new ObservablePoint(1, 1), new ObservablePoint(2, 2), new ObservablePoint(3, 3) };
            chart = new ChartValues<double>()
                { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };

            lineSeries = new LineSeries()
            {
                Title = "Moon",
                Values = chart,
                PointGeometry = null // Usuwa punkty z wykresu
            };

            var serial = new SeriesCollection() { lineSeries };
            var cartesianChart = new CartesianChart()
            {
                Series = serial,
            };
            Content = cartesianChart;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // // Dodaj nowy punkt do wykresu.
            // chart.Add(chart.Count);
            //
            // // Usuń stary punkt z wykresu, jeśli chcesz ograniczyć liczbę widocznych punktów.
            // if (chart.Count > 10)
            // {
            //     chart.RemoveAt(0);
            // }
            for (int i = 0; i < chart.Count; i++)
            {
                if (i % 2 == 0 &&chart[i]<=9)
                {
                    chart[i]++;
                }
                else if (i % 2 == 0)
                {
                    chart[i]--;
                }
                
            }
        }
    }
}