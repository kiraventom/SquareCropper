using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SquareCropper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            MainPB.AllowDrop = true;
            PathToImage = null;
        }

        private string _pathToImage;
        private string PathToImage 
        {
            get
            {
                return _pathToImage;
            }
            set
            {
                _pathToImage = value;
                if (_pathToImage != null)
                {
                    Bitmap orig = new Bitmap(_pathToImage);
                    Size newSize = AdjustImageSizeToSquareControl(orig.Size, MainPB.Size);
                    Bitmap thumb = new Bitmap(orig, newSize);
                    orig.Dispose();
                    MainPB.Image = thumb;
                }
            }
        }

        private void MainPB_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainPB_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] pathsToImages)
            {
                string pathToImage = pathsToImages[0];
                if (File.Exists(pathToImage))
                {
                    Bitmap image = new Bitmap(1, 1);
                    try
                    {
                        image = new Bitmap(pathToImage);
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("Некорректный тип файла!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        image.Dispose();
                    }

                    PathToImage = pathToImage;
                }
            }
        }

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
