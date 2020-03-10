using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SquareCropper.SCStaticModel;

namespace SquareCropper
{
    public class SCModel
    {
        public SCModel(Form mainForm)
        {
            MainForm = mainForm;
            Frame = new TransparentButton()
            {
                Size = new Size(300, 300),
                Point = new Point(0, 0)
            };
            Frame.MouseDown += Frame_MouseDown;
            Frame.MouseUp += Frame_MouseUp;
            Frame.MouseMove += Frame_MouseMove;
            MainForm.Controls.Add(Frame);
            Frame.BringToFront();
            PathToImage = null;
        }

        private TransparentButton Frame { get; }
        private Form MainForm { get; }
        private readonly Size DefaultSize = new Size(600, 600);

        private string _pathToImage;
        public string PathToImage
        {
            get
            {
                return _pathToImage;
            }
            set
            {
                _pathToImage = value;
                var mainPB = MainForm.Controls.OfType<PictureBox>().FirstOrDefault();
                if (mainPB is null)
                {
                    throw new Exception("No PictureBox on the form");
                }
                if (_pathToImage != null)
                {
                    Bitmap orig = new Bitmap(_pathToImage);
                    Size newSize = AdjustImageSizeToSquareControl(orig.Size, DefaultSize);
                    Bitmap thumb = new Bitmap(orig, newSize);
                    orig.Dispose();
                    if (mainPB.Image != null)
                    {
                        mainPB.Image.Dispose();
                    }
                    mainPB.Image = thumb;

                    Frame.ImageBehind = thumb;
                }
            }
        }

        #region Frame Movement
        private bool mouseDown;
        private Point lastLocation;

        private void Frame_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Frame_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Frame.Location = new Point(
                    (Frame.Location.X - lastLocation.X) + e.X, (Frame.Location.Y - lastLocation.Y) + e.Y);
                Frame.Point = Frame.Location;
            }
        }

        private void Frame_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        #endregion
    }
}
