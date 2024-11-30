using AForge.Video.DirectShow;
using AForge.Video;
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
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;

namespace Messe_Client
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        private VideoCaptureDevice _videoSource;

        public Window1()
        {
            InitializeComponent();
            StartWebcamPreview();
        }

        private void StartWebcamPreview()
        {
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("Keine Webcam gefunden");
                return;
            }
            _videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            _videoSource.NewFrame += VideoSource_NewFrame;
            _videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                // Freeze the image to make it thread-safe
                // Needed otherwise Thread Error
                bitmapImage.Freeze();
                UpdateImage(bitmapImage);
            }

            bitmap.Dispose();
        }

        private void UpdateImage(BitmapImage bitmapImage)
        {
            Dispatcher.Invoke(() =>
            {
                webcamPreview.Source = bitmapImage;
            });
        }

        private void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            if (_videoSource == null)
            {
                MessageBox.Show("Webcam not started.");
                return;
            }
            var frame = (BitmapImage)webcamPreview.Source;
            if (frame != null)
            {
                ((MainWindow)Application.Current.MainWindow).SetImageFromBase64(ConvertImageToBase64(frame));
                
            }
        }

        private string ConvertImageToBase64(BitmapImage image)
        {
            // Create a BitmapEncoder to encode the image to memory
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var memoryStream = new MemoryStream())
            {
                // Save the encoded image data to the memory stream
                encoder.Save(memoryStream);

                // Convert the memory stream to a byte array
                byte[] imageBytes = memoryStream.ToArray();

                // Convert the byte array to a Base64 string
                string base64String = Convert.ToBase64String(imageBytes);

                // Display or use the Base64 string as needed
                MessageBox.Show("Image converted to Base64 successfully!");
                Clipboard.SetText(base64String);

                return base64String;
            }
        }

        private void SaveImage(BitmapImage image)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "captured_image.png");

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }

            MessageBox.Show($"Image saved to {filePath}");
        }

        protected override async void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (_videoSource != null && _videoSource.IsRunning)
            {
                await Task.Run(() =>
                {
                    _videoSource.SignalToStop();
                    _videoSource.WaitForStop();
                });

                _videoSource = null;
            }
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                e.Cancel = true; // Cancel the closing event temporarily
                await Task.Run(() =>
                {
                    _videoSource.SignalToStop();
                    _videoSource.WaitForStop();
                });

                _videoSource = null; // Clean up
                e.Cancel = false; // Allow the window to close
                Close(); // Retry closing the window
            }

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Focus();
            }
        }
    }
}
