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

namespace Image_Viewer
{
    /// <summary>
    /// Interaction logic for EditPanel.xaml
    /// </summary>
    public partial class EditPanel : UserControl
    {
        private MainWindow wn;
        private string imageName;
        private int Angle = 0;
        private RotateTransform transformRotate = new RotateTransform(0);
        private ScaleTransform transformScale = new ScaleTransform();
        private bool SaveState = true;
        private double Number = 1.5;

        public EditPanel()
        {
            InitializeComponent();
        }

        public void Add_Window_Owner(MainWindow w)
        {
            this.wn = w;
            this.imageName = this.wn.imageOn.Source.ToString();
        }

        public void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            TransformedBitmap tb = new TransformedBitmap();
            BitmapImage bi = new BitmapImage(new Uri(this.imageName));
            tb.BeginInit();
            tb.Source = bi;
            // Set image rotation.
            Angle = Angle + 90;
            transformRotate = new RotateTransform(Angle);
                   
            tb.Transform = transformRotate;
            tb.EndInit();
            // Set the Image source.
            this.wn.imageOn.Source = tb;
        }

        public void LeftRight_Click(object sender, RoutedEventArgs e)
        {
            TransformedBitmap tb = new TransformedBitmap();
            BitmapImage bi = new BitmapImage(new Uri(this.imageName));

            tb.BeginInit();
            tb.Source = bi;
            // Set image rotation.
            Angle = Angle - 90;
            transformRotate = new RotateTransform(Angle);

            tb.Transform = transformRotate;
            tb.EndInit();
            // Set the Image source.
            this.wn.imageOn.Source = tb;
        }

        private void MirrorX_Click(object sender, RoutedEventArgs e)
        {
            TransformedBitmap tb = new TransformedBitmap();
            BitmapImage bi = new BitmapImage(new Uri(this.imageName));
            tb.BeginInit();
            tb.Source = bi;
            // Set image rotation.
            if (transformScale.ScaleY == -1)
            {
                transformScale = new ScaleTransform() { ScaleY = 1 };
            }
            else
            {
                transformScale = new ScaleTransform() { ScaleY = -1 };
            }
            tb.Transform = transformScale;
            tb.EndInit();
            // Set the Image source.
            this.wn.imageOn.Source = tb;
        }

        private void MirrorY_Click(object sender, RoutedEventArgs e)
        {
            TransformedBitmap tb = new TransformedBitmap();
            BitmapImage bi = new BitmapImage(new Uri(this.imageName));
            tb.BeginInit();
            tb.Source = bi;
            // Set image rotation.
            if (transformScale.ScaleX == -1)
            {
                transformScale = new ScaleTransform() { ScaleX = 1 };
            }
            else
            {
                transformScale = new ScaleTransform() { ScaleX = -1 };
            }
            tb.Transform = transformScale;
            tb.EndInit();
            // Set the Image source.
            this.wn.imageOn.Source = tb;
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            this.wn.PreviousHeight = this.wn.imageOn.ActualHeight;
            this.wn.PreviousWidth = this.wn.imageOn.ActualWidth;

            if((this.wn.PreviousHeight * 1.5 >= this.wn.HeightDef) || (this.wn.PreviousWidth * 1.5 >= this.wn.WidthDef))
            {
                this.wn.imageOn.Height = this.wn.imageOn.ActualHeight / Number;
                this.wn.imageOn.Width = this.wn.imageOn.ActualWidth / Number;
            }
            else
            {
                this.wn.Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                this.wn.Scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                this.wn.imageOn.Height = this.wn.imageOn.ActualHeight / Number;
                this.wn.imageOn.Width = this.wn.imageOn.ActualWidth / Number;
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            this.wn.PreviousHeight = this.wn.imageOn.ActualHeight;
            this.wn.PreviousWidth = this.wn.imageOn.ActualWidth;
            this.wn.imageOn.Height = this.wn.imageOn.ActualHeight * Number;
            this.wn.imageOn.Width = this.wn.imageOn.ActualWidth * Number;

            this.wn.Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            this.wn.Scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        private void AutoSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SaveState)
            {
                SaveState = false;
                this.ImageSave.Source = new BitmapImage(new Uri("pack://application:,,,/Image Viewer;component/image/p_autosave_off@2x.png"));
            }
            else
            {
                SaveState = true;
                this.ImageSave.Source = new BitmapImage(new Uri("pack://application:,,,/Image Viewer;component/image/p_autosave_on@2x.png"));
            }
        }

        private void OriginalSize_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(wn.imageOn.Source.ToString());
            BitmapImage img = new BitmapImage(new Uri(imageName));
            wn.imageOn.Source = img;
            wn.imageOn.Height = img.Height;
            wn.imageOn.Width = img.Width;
        }

        private void Fill_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage tmp= new BitmapImage(new Uri(imageName));
            wn.imageOn.Source = tmp;
            
            wn.imageOn.Height = wn.ContentPanel.ActualHeight;
            wn.imageOn.Width = wn.ContentPanel.ActualWidth;
            //wn.imageOn.Source=tmp;
        }

        private void AspectToFill_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
