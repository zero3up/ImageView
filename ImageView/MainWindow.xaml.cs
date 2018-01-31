using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace ImageView
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageOperation imgope = null;
        KeyBind bind = null;

        public MainWindow()
        {
            InitializeComponent();
            imgope = new ImageOperation(image);
            bind = new KeyBind(imgope);
        }

        ~MainWindow()
        {
            imgope.Close();
        }

        private void image_DragEnter(object sender, DragEventArgs e)
        {
            //コントロール内にドラッグされたとき実行される
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                //ドラッグされたデータ形式を調べ、ファイルのときはコピーアイコンとする
                e.Effects = System.Windows.DragDropEffects.Copy;
            }
            else
            {
                //ファイル以外は受け付けない
                e.Effects = System.Windows.DragDropEffects.None;
            }
        }

        private void image_Drop(object sender, DragEventArgs e)
        {
            string[] fileNames = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop, false);

            FileInfo fi = new FileInfo(fileNames[0]);
            if (fi.Extension == ".zip")
            {
                // ZIPファイルを開く
                imgope.Open(fi.FullName);
            }
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            //コントロール内にドラッグされたとき実行される
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                //ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
                e.Effects = System.Windows.DragDropEffects.Copy;
            }
            else
            {
                //ファイル以外は受け付けない
                e.Effects = System.Windows.DragDropEffects.None;
            }
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            string[] fileNames = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop, false);

            FileInfo fi = new FileInfo(fileNames[0]);
            if (fi.Extension == ".zip")
            {
                // ファイルを開く
                imgope.Open(fi.FullName);
            }
        }

        /// <summary>
        /// 画像コントロールでマウスホイール操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(e.Delta > 0)
            {
                bind.Action(KeyBind.KeyList.MouseScrollUp);
            }
            else
            {
                bind.Action(KeyBind.KeyList.MouseScrollDown);
            }
        }

        private void image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// マウス右クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ClickCount)
            {
                case 1:
                    bind.Action(KeyBind.KeyList.MouseRightClick);
                    break;
                case 2:
                    bind.Action(KeyBind.KeyList.MouseRightDoubleClick);
                    break;
            }
        }

        /// <summary>
        /// マウス左クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ClickCount)
            {
                case 1:
                    bind.Action(KeyBind.KeyList.MouseLeftClick);
                    break;
                case 2:
                    bind.Action(KeyBind.KeyList.MouseLeftDoubleClick);
                    break;
            }
        }
    }
}
