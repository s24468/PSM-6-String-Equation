using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;


namespace PSM_6;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
///
///
public partial class MainWindow
{
    private readonly DispatcherTimer _timer;
    private readonly SeriesCollection _seriesCollection2;
    private readonly SeriesCollection _seriesCollection1;
    private CartesianChart cartesianChart; 


    private bool _isFirstChart;

    private ChartValues<double> chart;
    private ChartValues<double> chart2;
    const int N = 10;
    const double L = Math.PI;
    const double dx = L / N;
    const double dt = 0.2;

    public MainWindow()
    {
        InitializeComponent();
        chart = new ChartValues<double>()
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        chart2 = new ChartValues<double>()
            { 0, 0, 0 };
        _seriesCollection1 = new SeriesCollection()
        {
            new LineSeries()
            {
                Title = "Wave",
                Values = chart,
                PointGeometry = null
            }
        };
        _seriesCollection2 = new SeriesCollection()
        {
            new ColumnSeries()
            {
                Title = "Energy",
                Values = chart2,
                PointGeometry = null 
            }
        };


        _isFirstChart = true;
        Button switchButton = new Button
        {
            Content = "Change Chart",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(10)
        };
        switchButton.Click += SwitchButton_Click;
        cartesianChart = new CartesianChart()
        {
            Series = _seriesCollection1,
            AxisX = new AxesCollection
            {
                new Axis
                {
                    MinValue = 0,
                    MaxValue = 10 
                }
            },
            AxisY = new AxesCollection
            {
                new Axis
                {
                    MinValue = -1,
                    MaxValue = 1,
                    LabelFormatter = value => Math.Round(value, 2).ToString() // Zaokrąglij wartości do 2 miejsc po przecinku

                }
            },
            Margin = new Thickness(0, 50, 0, 0)
        };
        Grid grid = (Grid)Content;
        grid.Children.Add(switchButton);
        grid.Children.Add(cartesianChart);


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

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.6) };
        _timer.Tick += (sender, e) =>
        {
            for (var i = 1; i < N; i++)
            {
                a[i] = (y[i - 1] - 2 * y[i] + y[i + 1]) / (dx * dx);
            }

            for (var i = 1; i < N; i++)
            {
                yMid[i] = y[i] + v[i] * dt / 2;
                vMid[i] = v[i] + a[i] * dt / 2;
            }

            for (var i = 1; i < N; i++)
            {
                a[i] = (yMid[i - 1] - 2 * yMid[i] + yMid[i + 1]) / (dx * dx);
            }

            for (var i = 1; i < N; i++)
            {
                y[i] += vMid[i] * dt;
                v[i] += a[i] * dt;
            }

            double Ek = 0;
            double Ep = 0;
            for (int i = 1; i < N; i++)
            {
                Ek += dx * Math.Pow(v[i], 2) / 2;
                Ep += Math.Pow(y[i + 1] - y[i], 2) / (2 * dx);
            }

            double Etotal = Ek + Ep;

            chart2[0] = Ek;
            chart2[1] = Ep;
            chart2[2] = Etotal;
            for (var i = 0; i < N; i++)
            {
                chart[i] = y[i];
            }
        };
        _timer.Start();
    }

    private void SwitchButton_Click(object sender, RoutedEventArgs e)
    {
        _isFirstChart = !_isFirstChart;
        cartesianChart.Series = _isFirstChart ? _seriesCollection1 : _seriesCollection2;
        if (_isFirstChart)
        {
            cartesianChart.AxisX[0].MinValue = 0;
            cartesianChart.AxisX[0].MaxValue = 10;
            cartesianChart.AxisY[0].MinValue = -1;
            cartesianChart.AxisY[0].MaxValue = 1;
        }
        else
        {
            cartesianChart.AxisX[0].MinValue = 0;
            cartesianChart.AxisX[0].MaxValue = 3;
            cartesianChart.AxisY[0].MinValue = 0;
            cartesianChart.AxisY[0].MaxValue = 1;
        }

        if (_isFirstChart)
        {
            cartesianChart.AxisX[0].Labels = new List<string> { "1", "2", "3" };
        }
        else
        {
            cartesianChart.AxisX[0].Labels = new List<string> { "Ek", "Ep", "Etotal" };
        }
    }
}