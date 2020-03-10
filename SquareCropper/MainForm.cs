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
            Model = new SCModel(this);
        }

        private SCModel Model { get; }

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

                    Model.PathToImage = pathToImage;
                }
            }
        }

        #region Window Movement
        private bool mouseDown;
        private Point lastLocation;

        private void MainPB_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void MainPB_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                Update();
            }
        }

        private void MainPB_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }
        #endregion
    }
}
