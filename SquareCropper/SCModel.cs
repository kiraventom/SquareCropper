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
                Location = new Point(0, 0),
                Visible = false
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
                    MainForm.Size = mainPB.Image.Size;
                    var dragNDropTip = MainForm.Controls.OfType<Label>().FirstOrDefault();
                    if (dragNDropTip != null)
                    {
                        dragNDropTip.Visible = false;
                    }
                    Frame.Visible = true;
                    if (thumb.Width <= thumb.Height)
                    {
                        Frame.Size = new Size(thumb.Width, thumb.Width);
                    }
                    else
                    {
                        Frame.Size = new Size(thumb.Height, thumb.Height);
                    }
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
                Point newLocation = new Point(
                    (Frame.Location.X - lastLocation.X) + e.X, (Frame.Location.Y - lastLocation.Y) + e.Y);
                if (newLocation.X <= 0)
                {
                    newLocation.X = 0;
                }
                if (newLocation.Y <= 0)
                {
                    newLocation.Y = 0;
                }
                if (newLocation.X >= MainForm.Width - Frame.Width)
                {
                    newLocation.X = MainForm.Width - Frame.Width;
                }
                if (newLocation.Y >= MainForm.Height - Frame.Height)
                {
                    newLocation.Y = MainForm.Height - Frame.Height;
                }
                Frame.Location = newLocation;
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
