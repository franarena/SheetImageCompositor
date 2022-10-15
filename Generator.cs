using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SheetImageCompositor
{
    public class Generator
    {
        Image? _image;
        ILogger _logger;
        int _width = 0;
        int _height = 0;

        public Generator(ILogger logger)
        {
            _logger = logger;
        }

        public string GenerateImage (string filePath, int rows, int columns)
        {
            var destFileName = string.Empty;
            try
            {
                destFileName = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath));
                _image = Image.FromFile(filePath);
                
                using var bitMap = new Bitmap(_image.Width * columns, _image.Height * rows);
                using var resultImage = Graphics.FromImage(bitMap);

                for (var i = 0; i < rows; i++)
                {
                    _height = i * _image.Height;
                    for (var j = 0; j < columns; j++)
                    {
                        _width = j * _image.Width;
                        resultImage.DrawImage(_image, _width, _height);
                    }
                }

                resultImage.Save();
                if (File.Exists(destFileName))
                    File.Delete(destFileName);
                bitMap.Save(destFileName, System.Drawing.Imaging.ImageFormat.Tiff);
            }
            catch (Exception ex)
            {
                destFileName = string.Empty;
                _logger.LogError(ex, ex.Message);
            }

            return destFileName;
        }
    }
}
