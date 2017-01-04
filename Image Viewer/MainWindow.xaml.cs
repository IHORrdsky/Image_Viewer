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
using System.IO;

namespace Image_Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowState state = WindowState.Normal;
        private int number = 0;
        private string[] files;
        private string mFileName = "";
        //private Object data;

        private double mHeightDef;
        private double mWidthDef;
        private double mPreviousHeight;
        private double mPreviousWidth;

        public string FileName
        {
            get
            {
                return this.mFileName;
            }
            set
            {
                this.mFileName = value;
            }
        }
        public double HeightDef
        {
            get
            {
                return this.mHeightDef;
            }
            set
            {
                this.mHeightDef = value;
            }
        }
        public double WidthDef
        {
            get
            {
                return this.mWidthDef;
            }
            set
            {
                this.mWidthDef = value;
            }
        }
        public double PreviousHeight
        {
            get
            {
                return this.mPreviousHeight;
            }
            set
            {
                this.mPreviousHeight = value;
            }
        }
        public double PreviousWidth
        {
            get
            {
                return this.mPreviousWidth;
            }
            set
            {
                this.mPreviousWidth = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            this.EditPnl.Add_Window_Owner(this);

            //FileStream f = File.Open("фото.jpg", FileMode.Open);
            //BitmapDecoder decoder = JpegBitmapDecoder.Create(f, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
            //BitmapMetadata metadata = (BitmapMetadata)decoder.Frames[0].Metadata;
            //// Получаем заголовок через поле класса
            //string title = metadata.Title;
            //// Получаем заголовок из XMP
            //string xmptitle = (string)metadata.GetQuery(@"/xmp/<xmpalt>dc:title");
            //// Получаем заголовок из EXIF
            //string exiftitle = (string)metadata.GetQuery(@"/app1/ifd/{ushort=40091}");
            //// Получаем заголовок из IPTC
            //string iptctitle = (string)metadata.GetQuery(@"/app13/irb/8bimiptc/iptc/object name");

        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        //public void SaveChanges()
        //{
        //    if (FileName != "")
        //    {
        //        BitmapSource img = (BitmapSource)(bi);
        //        CachedBitmap cache = new CachedBitmap(img, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        //        TransformedBitmap tb = new TransformedBitmap(cache, new RotateTransform(Angle));
        //        TiffBitmapEncoder encoder = new TiffBitmapEncoder();
        //        encoder.Frames.Add(BitmapFrame.Create(tb));
        //        using (FileStream file = File.OpenWrite(path))
        //        {
        //            encoder.Save(file);
        //        }
        //    }
        //}

        //public static Bitmap LoadBitmap(string fileName)
        //{
        //    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        //        return new Bitmap(fs);
        //}

        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                files = (string[])e.Data.GetData(DataFormats.FileDrop);
                FileName = files[0];

                //data = e.Data.GetData(typeof(Object));
                //MessageBox.Show(files.Length.ToString());



                //MessageBox.Show(FileName);

                imageOn.Source = new BitmapImage(new Uri(FileName));
                firstBlock.Text = String.Empty;
                secondBlock.Text = String.Empty;
                this.EditPnl.Add_Window_Owner(this);
                this.HeightDef = imageOn.ActualHeight;
                this.PreviousHeight = imageOn.ActualHeight;
                this.WidthDef = imageOn.ActualWidth;
                this.PreviousWidth = imageOn.ActualWidth;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (this.EditPnl.SaveState/* && this.EditPnl.Angle != 0*/)
            {
                //MessageBox.Show(this.wn.FileName.Remove(this.wn.FileName.LastIndexOf('\\')+1));
                //string directory = this.FileName.Remove(this.FileName.LastIndexOf('\\') + 1);

                CachedBitmap cache = new CachedBitmap((BitmapSource)this.imageOn.Source, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                TransformedBitmap tb = new TransformedBitmap(cache, new RotateTransform(EditPnl.Angle));
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(tb));
                imageOn.Source = null;
                imageOn = null;
                using (FileStream file = File.Create(FileName))
                {
                    encoder.Save(file); 
                }

                //MessageBox.Show(this.imageOn.Source.ToString());
                //FileStream file = File.OpenWrite(this.imageOn.Source.ToString());
                //encoder.Save(file);
                //file.Close();
            }

            this.Close();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (number == 0)
            {
                number++;
                this.state = WindowState;
                this.WindowState = WindowState.Maximized;
                return;
            }
            if(this.WindowState == WindowState.Maximized)
            {
                this.WindowState = this.state;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
           // EditPnl.Opacity = 1;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
           // EditPnl.Opacity = 0;
        }

        private void ContentPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*if(imageOn.Source!=null)
            {
                BitmapImage img = new BitmapImage(new Uri(imageOn.Source.ToString()));
                imageOn.Source = img;
                imageOn.Height = ContentPanel.ActualHeight;
                imageOn.Width = ContentPanel.ActualWidth;
            }*/
           
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
