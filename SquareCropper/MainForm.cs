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
            dropDown = new ToolStripDropDown();
            dropDown.Items.Add("Сохранить");
            dropDown.Items.Add("Помощь");
            dropDown.Items.Add("Выйти");
            dropDown.ItemClicked += this.DropDown_ItemClicked;
            MainPB.AllowDrop = true;
            Model = new SCModel(this);
        }

        private SCModel Model { get; }
        private ToolStripDropDown dropDown;

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

        private void MainPB_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dropDown.Show(MainPB, e.Location);
            }
        }

        private void DropDown_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Equals("Сохранить"))
            {
                dropDown.Hide();
                Model.Save();
            }
            if (e.ClickedItem.Text.Equals("Помощь"))
            {
                dropDown.Hide();
                MessageBox.Show(
                    "Чтобы добавить изображение, перетащите его на место, не занятое рамкой.\n\n" +
                    "Для перемещения рамки перетаскивайте её левой кнопкой мыши.\n\n" +
                    "Для изменения размера рамки используйте колесо мыши, зажав левую кнопку мыши на рамке.\n\n" +
                    "Для сохранения результата нажмите правой кнопкой в месте, не занятом рамкой, и выберите пункт \"Сохранить\".\n\n" +
                    "Для выхода из программы нажмите правой кнопкой в месте, не занятом рамкой, и выберите пункт \"Выйти\".",
                    "Помощь",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            if (e.ClickedItem.Text.Equals("Выйти"))
            {
                this.Close();
            }
        }
    }
}
