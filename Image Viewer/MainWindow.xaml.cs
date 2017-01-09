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

        string title;
        string xmptitle;
        string exiftitle;
        string iptctitle;

        private double mHeightDef;
        private double mWidthDef;
        private double mPreviousHeight;
        private double mPreviousWidth;
        public event EventHandler MySourceUpdated;

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
            this.ContentPanel.SizeChanged += ContentPanel_SizeChanged;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {           
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                files = (string[])e.Data.GetData(DataFormats.FileDrop);
                FileName = files[0];
                //MessageBox.Show(FileName);

                /*using (FileStream f = File.Open(FileName, FileMode.Open))
                {
                    BitmapDecoder decoder = JpegBitmapDecoder.Create(f, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
                    BitmapMetadata metadata = (BitmapMetadata)decoder.Frames[0].Metadata;
                    // Получаем заголовок через поле класса
                    title = metadata.Title;
                    // Получаем заголовок из XMP
                    xmptitle = (string)metadata.GetQuery(@"/xmp/<xmpalt>dc:title");
                    // Получаем заголовок из EXIF
                    exiftitle = (string)metadata.GetQuery(@"/app1/ifd/{ushort=40091}");
                    // Получаем заголовок из IPTC
                    iptctitle = (string)metadata.GetQuery(@"/app13/irb/8bimiptc/iptc/object name");
                }*/
                //imageOn.Source = new BitmapImage(new Uri(FileName));
                imageOn.SetCurrentValue(Image.SourceProperty, new BitmapImage(new Uri(FileName))); 
                firstBlock.Text = String.Empty;
                secondBlock.Text = String.Empty;
                this.EditPnl.Add_Window_Owner(this);
                this.HeightDef = imageOn.ActualHeight;
                this.PreviousHeight = imageOn.ActualHeight;
                this.WidthDef = imageOn.ActualWidth;
                this.PreviousWidth = imageOn.ActualWidth;
                MessageBox.Show(e.Source.ToString());
                MySourceUpdated(this, EventArgs.Empty);
                //EditPnl.IsEnabled = true;
                //DataPnl.IsEnabled = true;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show(sender.ToString() + "    "+e.ToString());
            if (this.EditPnl.SaveState && this.imageOn.Source != null /* && this.EditPnl.Angle != 0*/)
            {
                BitmapEncoder encoder;
                string tmp = FileName;
                string imageFormat = tmp.Substring(tmp.LastIndexOf('.')+1);
                MessageBox.Show(imageFormat);
                for (int i=1; ;i++)
                {
                    if (!File.Exists(FileName))
                        break;
                    try
                    {
                        FileName = FileName.Remove(this.FileName.LastIndexOf('('));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        FileName = FileName.Remove(this.FileName.LastIndexOf('.'));
                    }
                    FileName += '('+ i.ToString()+ ")."+imageFormat;                    
                }
                switch (imageFormat)
                {
                    case "jpeg":
                        encoder = new JpegBitmapEncoder();
                        break;
                    case "png":
                        encoder = new PngBitmapEncoder();
                        break;
                    case "bmp":
                        encoder = new BmpBitmapEncoder();
                        break;
                    case "tiff":
                        encoder = new TiffBitmapEncoder();
                        break;
                    case "wmp":
                        encoder = new WmpBitmapEncoder();
                        break;
                    case "gif":
                        encoder = new GifBitmapEncoder();
                        break;
                    default:
                        encoder= new PngBitmapEncoder();
                        break;
                }
                CachedBitmap cache = new CachedBitmap((BitmapSource)this.imageOn.Source, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                TransformedBitmap tb = new TransformedBitmap(cache, new RotateTransform(EditPnl.Angle));
                encoder.Frames.Add(BitmapFrame.Create(tb));
                using (FileStream file = File.Create(this.FileName))
                {
                    encoder.Save(file);
                }
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
            /*if (imageOn.Source != null)
            {               
                EditPnl.Opacity = 1;
            }*/
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            /*if (imageOn.Source != null)
                EditPnl.Opacity = 0;*/
        }

        private void ContentPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //BitmapImage img = new BitmapImage(new Uri(imageOn.Source.ToString()));
            //imageOn.Source = img;
            //MySourceUpdated(this, EventArgs.Empty);
            imageOn.Height = ContentPanel.ActualHeight;
            imageOn.Width = ContentPanel.ActualWidth;
        } 
               
    }
}
