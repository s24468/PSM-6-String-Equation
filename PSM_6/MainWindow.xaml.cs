using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace PSM_6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //tworzenie wykresu
            var chart = new ChartValues<ObservablePoint>();
            var lineSeries = new LineSeries()
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
        }
    }
}