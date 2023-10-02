using System;
using System.Collections.Generic;
using System.IO;
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

namespace ERDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Line line;
        private TableThumb object1;
        private TableThumb object2;
        public MainWindow()
        {
            InitializeComponent();
            line = new Line();
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 2;
            canvas.Children.Add(line);
            canvas.MouseMove += Canvas_MouseMove;
            object1 = CreateTablePart(10);
            object2 = CreateTablePart(11);

        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //UpdateLines();
            canvas.Children.OfType<Line>().ToList().ForEach(line => canvas.Children.Remove(line));
            DrawLineBetweenElements(object1, object2);
        }

        private void TableThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            TableThumb tableThumb = (TableThumb)sender;
            Canvas.SetLeft(tableThumb, Canvas.GetLeft(tableThumb) + e.HorizontalChange);
            Canvas.SetTop(tableThumb, Canvas.GetTop(tableThumb) + e.VerticalChange);
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void savePDFButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void savePNGButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCanvasToPng(canvas, $@"C:\Users\Gold\Desktop\test.png");
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTablePart();
        }

        private TableThumb CreateTablePart(int initialPosition = 0)
        {
            TableThumb tableThumb = new TableThumb();
            Canvas.SetLeft(tableThumb, initialPosition * 5);
            Canvas.SetTop(tableThumb, initialPosition * 5);
            tableThumb.SetUserControl(new TableControl { Width = 300, Height = 200 });
            tableThumb.DragDelta += TableThumb_DragDelta;
            canvas.Children.Add(tableThumb);
            return tableThumb;
        }
        private void SaveCanvasToPng(Canvas canvas, string filePath)
        {
            // Canvasの幅と高さを取得
            double width = canvas.ActualWidth;
            double height = canvas.ActualHeight;

            // 描画用のRenderTargetBitmapを作成
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Pbgra32);

            // RenderTargetBitmapにCanvasを描画
            renderTargetBitmap.Render(canvas);

            // BitmapEncoderを作成してPNG形式でエンコード
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            // ファイルに保存
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fs);
            }
        }

        //private void UpdateLines()
        //{ 
        //    // すべての線を削除
        //    canvas.Children.OfType<Line>().ToList().ForEach(line => canvas.Children.Remove(line));

        //    // すべてのオブジェクト同士を線でつなぐ
        //    foreach (UIElement element1 in canvas.Children.OfType<TableThumb>().ToList())
        //    {
        //        foreach (UIElement element2 in canvas.Children.OfType<TableThumb>().ToList())
        //        {
        //            if (element1 != element2 && element1 is FrameworkElement && element2 is FrameworkElement)
        //            {
        //                DrawLineBetweenElements(element1 as FrameworkElement, element2 as FrameworkElement);
        //            }
        //        }
        //    }
        //}

        private void DrawLineBetweenElements(FrameworkElement element1, FrameworkElement element2)
        {
            double horizontalX1 = 0;
            double horizontalX2 = 0;
            double horizontalY = 0;
            double verticalY1 = 0;
            double verticalY2 = 0;
            double verticalX = 0;
            double x1 = Canvas.GetLeft(element1);
            double y1 = Canvas.GetTop(element1);

            double x2 = Canvas.GetLeft(element2);
            double y2 = Canvas.GetTop(element2);

            //horizontal line
            if (x1 + element1.ActualWidth < x2)
            {
                horizontalX1 = x1 + element1.ActualWidth;
                horizontalY = y1 + (element1.ActualHeight / 2);
                double midY1 = y1 + (element1.ActualHeight / 2);
                if (midY1 > y2 && midY1 < y2 + element2.ActualHeight)
                {
                    horizontalX2 = x2;
                }
                if (midY1 < y2 || midY1 > y2 + element2.ActualHeight)
                {
                    horizontalX2 = x2 + (element2.ActualWidth / 2);
                }
            }
            if (x1 > x2)
            {
                horizontalX1 = x1;
                horizontalY = y1 + (element1.ActualHeight / 2);
                double midY1 = y1 + (element1.ActualHeight / 2);
                if (midY1 > y2 && midY1 < y2 + element2.ActualHeight)
                {
                    horizontalX2 = x2 + element2.ActualWidth;
                }
                if (midY1 < y2 || midY1 > y2 + element2.ActualHeight)
                {
                    horizontalX2 = x2 + (element2.ActualWidth / 2);
                }
            }
            Line horizontalLine = CreateLine();
            horizontalLine.X1 = horizontalX1;
            horizontalLine.Y1 = horizontalY;
            horizontalLine.X2 = horizontalX2;
            horizontalLine.Y2 = horizontalY;
            canvas.Children.Add(horizontalLine);

            //vertical line
            if (y1 + element1.ActualHeight < y2)
            {
                verticalY1 = y1 + element1.ActualHeight;
                verticalX = x1 + (element1.ActualWidth / 2);
                double midX1 = x1 + (element1.ActualWidth / 2);
                if (midX1 > x2 && midX1 < x2 + element2.ActualWidth)
                {
                    verticalY2 = y2;
                }
                if (midX1 < x2 || midX1 > x2 + element2.ActualWidth)
                {
                    verticalY2 = y2 + (element2.ActualHeight / 2);
                }
            }
            if (y1 > y2)
            {
                verticalY1 = y1;
                verticalX = x1 + (element1.ActualWidth / 2);
                double midX1 = x1 + (element1.ActualWidth / 2);
                if (midX1 > x2 && midX1 < x2 + element2.ActualWidth)
                {
                    verticalY2 = y2 + element2.ActualHeight;
                }
                if (midX1 < x2 || midX1 > x2 + element2.ActualWidth)
                {
                    verticalY2 = y2 + (element2.ActualHeight / 2);
                }
            }
            Line verticalLine = CreateLine();
            verticalLine.X1 = verticalX;
            verticalLine.Y1 = verticalY1;
            verticalLine.X2 = verticalX;
            verticalLine.Y2 = verticalY2;
            canvas.Children.Add(verticalLine);
        }

        private Line CreateLine()
        {
            return new Line
            {
                StrokeThickness = 2,
                Stroke = Brushes.Black
            };
        }
    }
}
