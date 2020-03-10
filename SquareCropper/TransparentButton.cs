using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SquareCropper
{
    public partial class TransparentButton : Button
    {
        public TransparentButton()
        {
            ImageBehind = null;
            InitializeComponent();
        }

        private Bitmap _imageBehind;
        public Bitmap ImageBehind
        {
            get
            {
                return _imageBehind;
            }
            set
            {
                if (value != null)
                {
                    if (_imageBehind != null)
                    {
                        _imageBehind.Dispose();
                    }
                    _imageBehind = new Bitmap(value);
                }
                
                this.Refresh();
            }
        }

        private Point _point;
        public Point Point
        {
            get
            {
                return _point;
            }
            set
            {
                _point = value;
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (ImageBehind != null)
            {
                pe.Graphics.DrawImage(ImageBehind, new Rectangle(new Point(0, 0), this.Size), new Rectangle(this.Location, this.Size), GraphicsUnit.Pixel);
            }
            pe.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 0)), 0, 0, this.Width, this.Height);
        }
    }
}
