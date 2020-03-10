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
            Frame.MouseWheel += this.Frame_MouseWheel;
            MainForm.Controls.Add(Frame);
            Frame.BringToFront();
            PathToImage = null;
        }

        private TransparentButton Frame { get; }
        private Form MainForm { get; }
        private readonly Size DefaultSize = new Size(600, 600);
        private Size MaxFrameSize = new Size(1, 1);

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
                var mainPB = MainForm.Controls.OfType<PictureBox>().First();
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
                    var dragNDropTip = MainForm.Controls.OfType<Label>().First();
                    dragNDropTip.Visible = false;
                    Frame.Visible = true;
                    if (thumb.Width <= thumb.Height)
                    {
                        MaxFrameSize = new Size(thumb.Width, thumb.Width);
                    }
                    else
                    {
                        MaxFrameSize = new Size(thumb.Height, thumb.Height);
                    }
                    Frame.Size = MaxFrameSize;
                    Frame.ImageBehind = thumb;
                }
            }
        }

        public void Save()
        {
            if (Frame.ImageBehind != null)
            {
                var sfd = new SaveFileDialog
                {
                    Filter = "PNG изображение|*.png"
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap output = new Bitmap(Frame.Width, Frame.Height);
                    using (Graphics g = Graphics.FromImage(output))
                    {
                        g.DrawImage(Frame.ImageBehind, new Rectangle(new Point(0, 0), Frame.Size), new Rectangle(Frame.Location, Frame.Size), GraphicsUnit.Pixel);
                    }
                    output.Save(sfd.FileName);
                    output.Dispose();
                }
            }
        }

        #region Frame Movement and Resizing
        private bool mouseDown;
        private Point lastLocation;
        private double sizeMod = 1.0;

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

        private void Frame_MouseWheel(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (e.Delta > 0 && sizeMod < 1.0)
                {
                    sizeMod += 0.05;
                    if (sizeMod > 1.0)
                    {
                        sizeMod = 1.0;
                    }
                }
                else if (e.Delta < 0 && sizeMod > 0.5)
                {
                    sizeMod -= 0.05;
                    if (sizeMod < 0.5)
                    {
                        sizeMod = 0.5;
                    }
                }
                Frame.Point = Frame.Location;
                Frame.Size = new Size((int)(MaxFrameSize.Width * sizeMod), (int)(MaxFrameSize.Height * sizeMod));
            }
        }
        #endregion
    }
}
