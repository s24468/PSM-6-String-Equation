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
        const int N = 10;
        const double L = Math.PI;
        const double dx = L / N;
        const double dt = 0.2;
        const int timesteps = 200;

        public MainWindow()
        {
            InitializeComponent();
            //tworzenie wykresu
            // chart = new ChartValues<ObservablePoint>()
            //     { new ObservablePoint(1, 1), new ObservablePoint(2, 2), new ObservablePoint(3, 3) };
            chart = new ChartValues<double>()
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            lineSeries = new LineSeries()
            {
                Title = "Wave",
                Values = chart,
                PointGeometry = null // Usuwa punkty z wykresu
            };

            var serial = new SeriesCollection() { lineSeries };
            var cartesianChart = new CartesianChart()
            {
                Series = serial,
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        MinValue = 0,
                        MaxValue = 10 // Ustaw maksymalną wartość dla osi X
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        MinValue = -1,
                        MaxValue = 1 // Ustaw maksymalną wartość dla osi Y
                    }
                }
            };
            Content = cartesianChart;
            double[] y = new double[N + 1];
            double[] v = new double[N + 1];
            double[] a = new double[N + 1];
            double[] yMid = new double[N + 1];
            double[] vMid = new double[N + 1];

            for (int i = 0; i <= N; i++)
            {
                y[i] = Math.Sin(i * dx);
                v[i] = 0;
            }


            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.1) };
            _timer.Tick += (sender, e) =>
            {
                for (int i = 1; i < N; i++)
                {
                    a[i] = (y[i - 1] - 2 * y[i] + y[i + 1]) / (dx * dx);
                }

                for (int i = 1; i < N; i++)
                {
                    yMid[i] = y[i] + v[i] * dt / 2;
                    vMid[i] = v[i] + a[i] * dt / 2;
                }

                for (int i = 1; i < N; i++)
                {
                    a[i] = (yMid[i - 1] - 2 * yMid[i] + yMid[i + 1]) / (dx * dx);
                }

                for (int i = 1; i < N; i++)
                {
                    y[i] = y[i] + vMid[i] * dt;
                    v[i] = v[i] + a[i] * dt;
                }

                for (int i = 0; i < N; i++)
                {
                    chart[i] = y[i];
                }
            };
            _timer.Start();
        }

        // private void Timer_Tick(object sender, EventArgs e)
        // {
        //  }
    }
}


// // Dodaj nowy punkt do wykresu.
// chart.Add(chart.Count);
//
// // Usuń stary punkt z wykresu, jeśli chcesz ograniczyć liczbę widocznych punktów.
// if (chart.Count > 10)
// {
//     chart.RemoveAt(0);
// }