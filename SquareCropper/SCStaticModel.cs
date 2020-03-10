using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareCropper
{
    public static class SCStaticModel
    {
        public static Size AdjustImageSizeToSquareControl(Size imageSize, Size controlSize)
        {
            double ratio = imageSize.Width / (double)imageSize.Height;

            //if pic is horizontal
            if (ratio > 1)
            {
                int newWidth = controlSize.Width;
                int newHeight = (int)(imageSize.Height * (newWidth / (double)imageSize.Width));
                return new Size(newWidth, newHeight);
            }
            else
            //if pic is vertical
            if (ratio < 1)
            {
                int newHeight = controlSize.Height;
                int newWidth = (int)(imageSize.Width * (newHeight / (double)imageSize.Height));
                return new Size(newWidth, newHeight);
            }
            else
            //if pic is square
            {
                return controlSize;
            }
        }
    }
}
