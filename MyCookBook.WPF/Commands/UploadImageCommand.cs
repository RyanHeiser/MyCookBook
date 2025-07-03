using Microsoft.Win32;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Serialization;

namespace MyCookBook.WPF.Commands
{
    public class UploadImageCommand : AsyncCommandBase
    {
        private readonly CreateRecipeViewModel _viewModel;

        public UploadImageCommand(CreateRecipeViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select an image";
            dialog.Filter = "All supported file types|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "PNG (*.png)|*.png";

            if (dialog.ShowDialog() == true)
            {
                _viewModel.RawThumbnailData = ImageToByteArray(dialog.FileName, 128);
                _viewModel.RawImageData = ImageToByteArray(dialog.FileName, 0);
            }
        }

        private byte[] ImageToByteArray(string fileName, int decodeHeight)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(fileName);
            image.DecodePixelHeight = decodeHeight;
            image.EndInit();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
    }
}
