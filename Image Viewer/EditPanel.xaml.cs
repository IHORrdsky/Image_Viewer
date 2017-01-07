using Microsoft.Win32;
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

namespace Image_Viewer
{
    /// <summary>
    /// Interaction logic for EditPanel.xaml
    /// </summary>
    public partial class EditPanel : UserControl
    {
        private MainWindow wn;
        private string imageName;
        private int mAngle = 0;
        private RotateTransform transformRotate = new RotateTransform(0);
        private ScaleTransform transformScale = new ScaleTransform();
        TransformedBitmap tb = new TransformedBitmap();
        private bool mSaveState = false;
        private double Number = 1.5;
        Stretch mStretch = Stretch.Uniform;
        
        public int Angle
        {
            set
            {
                this.mAngle = value;
            }
            get
            {
                return this.mAngle;
            }
        }
        public bool SaveState
        {
            get { return this.mSaveState; }
        }
        public string ImageName
        {
            get
            {
                return this.imageName;
            }
        }

        public EditPanel()
        {
            InitializeComponent();
        }

                /*BitmapSource img = (BitmapSource)(bi);
                CachedBitmap cache = new CachedBitmap(img, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                TransformedBitmap tb = new TransformedBitmap(cache, new RotateTransform(Angle));
                TiffBitmapEncoder encoder = new TiffBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(tb));
                using (FileStream file = File.OpenWrite(path))
                {
                    encoder.Save(file);
                }*/
        private void AutoSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mSaveState)
            {
                mSaveState = false;
                AutoSaveBtn.ToolTip = "Autosave off";
                this.ImageSave.Source = new BitmapImage(new Uri("pack://application:,,,/Image Viewer;component/image/p_autosave_off@2x.png"));
            }
            else
            {
                mSaveState = true;
                AutoSaveBtn.ToolTip = "Autosave on";
                this.ImageSave.Source = new BitmapImage(new Uri("pack://application:,,,/Image Viewer;component/image/p_autosave_on@2x.png"));
            }
        }

        public void Add_Window_Owner(MainWindow w)
        {
            this.wn = w;
            w.MySourceUpdated += W_SourceUpdated;
        }

        private void W_SourceUpdated(object sender, EventArgs e)
        {
            this.imageName = this.wn.imageOn.Source.ToString();
        }

        public void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            TransformedBitmap tb = new TransformedBitmap();
            BitmapImage bi = new BitmapImage(new Uri(this.wn.FileName));
            tb.BeginInit();
            tb.Source = bi;
            // Set image rotation.
            mAngle = mAngle + 90;
            transformRotate = new RotateTransform(mAngle);
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
            BitmapImage bi = new BitmapImage(new Uri(this.wn.FileName));

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

        private void MirrorY_Click(object sender, RoutedEventArgs e)
        {
            TransformedBitmap tb = new TransformedBitmap();
            BitmapImage bi = new BitmapImage(new Uri(this.wn.FileName));
            tb.BeginInit();
            tb.Source = bi;
            // Set image rotation.
            if (transformScale.ScaleX == -1)
            {
                transformScale = new ScaleTransform(1, transformScale.ScaleY);
            }
            else
            {
                transformScale = new ScaleTransform(-1, transformScale.ScaleY);
            }
            tb.Transform = transformScale;
            tb.EndInit();
            // Set the Image source.
            this.wn.imageOn.Source = tb;
        }

        private void MirrorX_Click(object sender, RoutedEventArgs e)
        {
            TransformedBitmap tb = new TransformedBitmap();
            BitmapImage bi = new BitmapImage(new Uri(this.wn.FileName));
            tb.BeginInit();
            tb.Source = bi;
            // Set image rotation.
            if (transformScale.ScaleY == -1)
            {
                transformScale = new ScaleTransform(transformScale.ScaleX, 1);
            }
            else
            {
                transformScale = new ScaleTransform(transformScale.ScaleX, -1);
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

        private void OriginalSize_Click(object sender, RoutedEventArgs e)
        {
            this.wn.imageOn.Stretch = mStretch;
            BitmapImage img = new BitmapImage(new Uri(wn.FileName));
            wn.imageOn.Source = img;
            wn.imageOn.Height = img.Height;
            wn.imageOn.Width = img.Width;

            this.wn.Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            this.wn.Scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            //if (this.wn.Scroll.HorizontalScrollBarVisibility != ScrollBarVisibility.Disabled || this.wn.Scroll.VerticalScrollBarVisibility != ScrollBarVisibility.Disabled)
            //{
            //    this.wn.Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            //    this.wn.Scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            //}
        }

        private void Fit_Click(object sender, RoutedEventArgs e)
        {
            this.wn.imageOn.Stretch = mStretch;
            BitmapImage tmp= new BitmapImage(new Uri(wn.FileName));
            wn.imageOn.Source = tmp;
            wn.imageOn.Width = wn.ContentPanel.ActualWidth;
            wn.imageOn.Height = wn.ContentPanel.ActualHeight;
            if (this.wn.Scroll.HorizontalScrollBarVisibility != ScrollBarVisibility.Disabled || this.wn.Scroll.VerticalScrollBarVisibility != ScrollBarVisibility.Disabled)
            {
                this.wn.Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                this.wn.Scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
        }

        private void AspectToFill_Click(object sender, RoutedEventArgs e)
        {
            this.wn.imageOn.Stretch = Stretch.UniformToFill;
            if (this.wn.Scroll.HorizontalScrollBarVisibility != ScrollBarVisibility.Disabled || this.wn.Scroll.VerticalScrollBarVisibility != ScrollBarVisibility.Disabled)
            {
                this.wn.Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                this.wn.Scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
            //OpenFileDialog dg = new OpenFileDialog();
            //dg.ShowDialog();
            //this.imageName = dg.FileName;
            //wn.imageOn.Source = new BitmapImage(new Uri(dg.FileName));
        }
    }
}
