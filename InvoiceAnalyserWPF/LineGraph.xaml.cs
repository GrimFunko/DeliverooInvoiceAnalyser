using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Windows.Threading;

namespace InvoiceAnalyserWPF
{
    /// <summary>
    /// Interaction logic for LineGraph.xaml
    /// </summary>
    public partial class LineGraph : UserControl
    {
        public LineGraph()
        {
            InitializeComponent();

            Loaded += LineGraph_Loaded;
            DataChanged += LineGraph_DataChanged;
            
        }

        private void LineGraph_DataChanged(object sender, RoutedPropertyChangedEventArgs<List<KeyValuePair<object, double>>> e)
        {
            dataDependentLabels.Children.Clear();

            if (graphCanvas.ActualHeight <= 1)
            {
                Dispatcher.BeginInvoke(new Action(() => VisualiseData()), DispatcherPriority.ContextIdle, null);
                return;
            }
                           
            VisualiseData();
        }

        private void LineGraph_Loaded(object sender, RoutedEventArgs e)
        {
            // Loaded occurs before user control is rendered, this check waits for elements to be rendered
            if (graphCanvas.ActualHeight <= 1)
                return;
            Loaded -= LineGraph_Loaded;

            DrawAxis();
        }

        private Point zero;
        private Point endX;
        private Point endY;

        private void DrawAxis()
        {
            zero = new Point(GraphMargin, graphCanvas.ActualHeight - GraphMargin);
            endX = new Point(graphCanvas.ActualWidth - GraphMargin, graphCanvas.ActualHeight - GraphMargin);
            endY = new Point(GraphMargin, GraphMargin);

            //X - Axis
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(zero, endX));
            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            graphCanvas.Children.Add(xaxis_path);

            //Y - Axis
            GeometryGroup yaxis_geom = new GeometryGroup();
            
            yaxis_geom.Children.Add(new LineGeometry(zero, endY));
            var yStep = (zero.Y - endY.Y) / 10; 
            for(double y = endY.Y; y < zero.Y; y += yStep)
            {
                yaxis_geom.Children.Add(new LineGeometry(new Point(zero.X - 5, y), new Point(zero.X, y)));
            }
            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;
            
            graphCanvas.Children.Add(yaxis_path);
        }

        private void VisualiseData()
        {
            if (GraphData == null)
                return;
            //Data deets
            int xPartitions = GraphData.Count;
            if (xPartitions == 0)
                return;
            double valueMax = GraphData.Max(x => x.Value);

            //Y plot points
            int yMax = valueMax == 0 ? 10 : (int)(Math.Ceiling(valueMax / 10) * 10);
            double yValueInteravals = yMax / 10;

            //Label Y - Axis
            for(int i = 0; i <= 10; i++)
            {
                TextBlock yLabel = new TextBlock { Text = $"{i * yValueInteravals}", Background = Brushes.Transparent, TextAlignment = TextAlignment.Justify, FontSize = GraphDataLabelFontSize};
                var aw = yLabel.ActualWidth;
                var ah = yLabel.ActualHeight;

                yLabel.Margin = new Thickness(GraphMargin - 25 - aw, (zero.Y - endY.Y) - (i * ((zero.Y - endY.Y) / 10)) + (2 * GraphMargin/3), 0, 0);
                dataDependentLabels.Children.Add(yLabel);
            }

            //Label X-Axis
            GeometryGroup xaxis_geom = new GeometryGroup();
            var xStep = (endX.X - zero.X) / GraphData.Count;

            for (int j = 0; j < GraphData.Count; j++)
            {
                //Create Axis data mark
                xaxis_geom.Children.Add(new LineGeometry(new Point(GraphMargin + ((j+1) * xStep), zero.Y), new Point(GraphMargin + ((j+1) * xStep), zero.Y + 5)));
                
                //Create and position label
                TextBlock xLabel = new TextBlock { Text = GraphData[j].Key.ToString(), Background = Brushes.Transparent, FontSize = GraphDataLabelFontSize };

                xLabel.Margin = new Thickness((j+1)*xStep + (2* GraphMargin / 3), zero.Y, 0, 0);
                dataDependentLabels.Children.Add(xLabel);
            }
            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = xaxis_geom;
            dataDependentLabels.Children.Add(yaxis_path);


            //Get data Coordinates
            PointCollection points = new PointCollection();
            for (int p = 0; p < GraphData.Count; p++)
            {
                points.Add(new Point(GraphMargin + (xStep * (p+1)), zero.Y - ((GraphData[p].Value / yMax) * (zero.Y - endY.Y))));
            }

            //Draw on points
            foreach (Point p in points)
            {
                Ellipse dot = new Ellipse() { Fill = DataPointColour, Width = 6, Height = 6 };
                Canvas.SetTop(dot, p.Y - (dot.Height / 2));
                Canvas.SetLeft(dot, p.X - (dot.Width / 2));
                ToolTip pointToolTip = new ToolTip();
                pointToolTip.Content = GraphData[points.IndexOf(p)].Value;
                dot.ToolTip = pointToolTip;
                dataDependentLabels.Children.Add(dot);
            }

            //Draw line
            Polyline dataLine = new Polyline();
            dataLine.StrokeThickness = 1;
            dataLine.Stroke = DataLineColour;
            dataLine.Points = points;
            dataDependentLabels.Children.Add(dataLine);
        }


        public SolidColorBrush DataLineColour
        {
            get { return (SolidColorBrush)GetValue(DataLineColourProperty); }
            set { SetValue(DataLineColourProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataLineColour.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataLineColourProperty =
            DependencyProperty.Register("DataLineColour", typeof(SolidColorBrush), typeof(LineGraph), new FrameworkPropertyMetadata(Brushes.Red));




        public SolidColorBrush DataPointColour
        {
            get { return (SolidColorBrush)GetValue(DataPointColourProperty); }
            set { SetValue(DataPointColourProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataPointColour.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataPointColourProperty =
            DependencyProperty.Register("DataPointColour", typeof(SolidColorBrush), typeof(LineGraph), new FrameworkPropertyMetadata(Brushes.Red));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LineGraph), new FrameworkPropertyMetadata(""));

        public string xAxisTitle
        {
            get { return (string)GetValue(xAxisTitleProperty); }
            set { SetValue(xAxisTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for xAxisTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty xAxisTitleProperty =
            DependencyProperty.Register("xAxisTitle", typeof(string), typeof(LineGraph), new FrameworkPropertyMetadata("X-Axis"));



        public string yAxisTitle
        {
            get { return (string)GetValue(yAxisTitleProperty); }
            set { SetValue(yAxisTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for yAxisTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty yAxisTitleProperty =
            DependencyProperty.Register("yAxisTitle", typeof(string), typeof(LineGraph), new FrameworkPropertyMetadata("Y-Axis"));

        public int MaxY { get; set; }
        public int MaxX { get; set; }

        public double GraphMargin
        {
            get { return (double)GetValue(GraphMarginProperty); }
            set
            {
                if (value >= graphCanvas.Height - 5 || value >= graphCanvas.Width - 5)
                {
                    if (Height > Width)
                        SetValue(GraphMarginProperty, graphCanvas.Width - 5);
                    else
                        SetValue(GraphMarginProperty, graphCanvas.Height - 5);
                }
                else
                    SetValue(GraphMarginProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for GraphMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphMarginProperty =
            DependencyProperty.Register("GraphMargin", typeof(double), typeof(LineGraph), new FrameworkPropertyMetadata((double)8));


        public double GraphDataLabelFontSize
        {
            get { return (double)GetValue(GraphDataLabelFontSizeProperty); }
            set { SetValue(GraphDataLabelFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GraphDataLabelFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphDataLabelFontSizeProperty =
            DependencyProperty.Register("GraphDataLabelFontSize", typeof(double), typeof(LineGraph), new FrameworkPropertyMetadata((double)11));



        public List<KeyValuePair<object, double>> GraphData
        {
            get { return (List<KeyValuePair<object, double>>)GetValue(GraphDataProperty); }
            set { SetValue(GraphDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GraphData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GraphDataProperty =
            DependencyProperty.Register("GraphData", typeof(List<KeyValuePair<object, double>>), typeof(LineGraph), new FrameworkPropertyMetadata(new List<KeyValuePair<object, double>>(), new PropertyChangedCallback(OnDataChanged)));

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            LineGraph control = (LineGraph)d;

            RoutedPropertyChangedEventArgs<List<KeyValuePair<object, double>>> e = new RoutedPropertyChangedEventArgs<List<KeyValuePair<object, double>>>(
                (List<KeyValuePair<object, double>>)args.OldValue, (List<KeyValuePair<object, double>>)args.NewValue, DataChangedEvent);

            control.OnDataChanged(e);
        }

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent DataChangedEvent = EventManager.RegisterRoutedEvent(
            "DataChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<List<KeyValuePair<object, double>>>), typeof(LineGraph));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<List<KeyValuePair<object, double>>> DataChanged
        {
            add { AddHandler(DataChangedEvent, value); }
            remove { RemoveHandler(DataChangedEvent, value); }
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnDataChanged(RoutedPropertyChangedEventArgs<List<KeyValuePair<object, double>>> args)
        {
            RaiseEvent(args);
        }
    }
}
