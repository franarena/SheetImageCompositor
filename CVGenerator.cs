using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenCvSharp;

namespace SheetImageCompositor
{
    public class CVGenerator
    {
        internal readonly static int CV_FILLED = 1;

        ILogger _logger;

        public CVGenerator(ILogger logger)
        {
            _logger = logger;
        }

        public string GenerateImage(string filePath, int rows, int columns)
        {
            var destFileName = string.Empty;
            try
            {
                destFileName = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath));
                if (File.Exists(destFileName))
                    File.Delete(destFileName);

                using var newImageRgb = Cv2.ImRead(filePath);
                using var newImage = new Mat();
                newImageRgb.CopyTo(newImage);
                Cv2.CvtColor(newImage, newImage, ColorConversionCodes.RGB2GRAY);

                var drawableRect = new Rect(0, 0, newImage.Cols, newImage.Rows);

                var sheet_w = columns * newImage.Cols + 0;
                var sheet_h = rows * newImage.Rows + 0;

                var notesInSheet = rows * columns;

                using Mat sheetImage = Mat.Zeros(new OpenCvSharp.Size(sheet_w, sheet_h), MatType.CV_32F);
                sheetImage.SetTo(Scalar.Black);

                Mat tileImage;

                for (var i = 0; i < rows; i++)
                {
                    for (var j = 0; j < columns; j++)
                    {
                        tileImage = newImage.Clone();

                        var tile = new Rect(newImage.Cols * j, newImage.Rows * i, newImage.Cols, newImage.Rows);

                        var aux = new Mat(sheetImage, tile);

                        //tileImage.CopyTo(new Mat(sheetImage, tile));
                        tileImage.CopyTo(aux);
                        aux.CopyTo(sheetImage);
                    }
                }

                Cv2.Rectangle(sheetImage, drawableRect, Scalar.White, CV_FILLED);

                Cv2.ImWrite(destFileName, sheetImage);

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
