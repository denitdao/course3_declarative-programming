using System;
using Microsoft.Win32;
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

namespace lab01a
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Ink.DrawingAttributes inkDA = new System.Windows.Ink.DrawingAttributes();

        public MainWindow()
        {
            InitializeComponent();
            thickness_slider.Value = 2;
            thickness_slider.Minimum = 1;
            thickness_slider.Maximum = 12;

            color_picker.SelectedColor = Colors.Black;

            inkCanvas.DefaultDrawingAttributes = inkDA;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png)|*.png";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(openFileDialog.FileName)),
                    MaxWidth = inkCanvas.ActualWidth,
                    MaxHeight = inkCanvas.ActualHeight
                };
                inkCanvas.Children.Add(image);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files (*.png)|*.png";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (saveFileDialog.ShowDialog() == true)
            {
                Size size = new Size(inkCanvas.ActualWidth * 1.5, inkCanvas.ActualHeight * 1.5);
                double dpi = 96d;

                RenderTargetBitmap rtb = new RenderTargetBitmap((int)size.Width, (int)size.Height,
                                                                    dpi, dpi, PixelFormats.Default);
                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(inkCanvas);
                    dc.DrawRectangle(vb, null, new Rect(new Point(), size));
                }
                rtb.Render(dv);

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    pngEncoder.Save(ms);
                    System.IO.File.WriteAllBytes(saveFileDialog.FileName, ms.ToArray());
                }
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Remove(inkCanvas.Strokes.LastOrDefault());
        }

        private void Eraser_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void Pencil_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.Select;
        }

        private void Thickness_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            inkDA.Height = thickness_slider.Value;
            inkDA.Width = thickness_slider.Value;

            inkCanvas.DefaultDrawingAttributes = inkDA;
        }

        private void Color_Change(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            inkDA.Color = (Color) e.NewValue;

            inkCanvas.DefaultDrawingAttributes = inkDA;
        }
    }
}
