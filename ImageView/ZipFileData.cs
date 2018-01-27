using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace ImageView
{
    class ZipFileData
    {
        private ZipArchive item = null;

        
        private int pages = 0;

        private int index = 0;

        private BitmapImage data = null;

        private MemoryStream memstrem = new MemoryStream();

        private Dictionary<int, ZipArchiveEntry> dictionary = new Dictionary<int, ZipArchiveEntry>();

        private IEnumerable<ZipArchiveEntry> arclist;


        /// <summary>
        /// コンストラクタ ファイルオープンも行う
        /// </summary>
        /// <param name="path">ファイルパス</param>
        public ZipFileData(string path)
        {
            // 引数のパスのZIPファイルの情報を取得
            this.Open(path);

            this.GetItemData();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ZipFileData()
        {
        }

        /// <summary>
        /// デコンスタクラ
        /// </summary>
        ~ZipFileData()
        {
            this.Close();
        }

        /// <summary>
        /// ページ数
        /// </summary>
        public int Pages
        {
            get { return pages; }
        }

        /// <summary>
        /// 画像イメージ
        /// </summary>
        public BitmapImage Data
        {
            get { return data; }
        }

        /// <summary>
        /// 次の画像イメージ
        /// </summary>
        public BitmapImage Next
        {
            get
            {
                if (this.IsNext())
                {
                    index++;
                    GetItemData();
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 前の画像イメージ
        /// </summary>
        public BitmapImage Prev
        {
            get
            {
                if (this.IsPrev())
                {
                    index--;
                    GetItemData();
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 次の画像が存在するか判定
        /// </summary>
        /// <returns>判定結果 true:あり,false:なし</returns>
        public bool IsNext()
        {
            if (index + 1 == pages)
            {
                // 次の画像なし
                return false;
            }
            else
            {
                // 次の画像あり
                return true;
            }
        }

        /// <summary>
        /// 前の画像が存在するか判定
        /// </summary>
        /// <returns>判定結果 true:あり,false:なし</returns>
        public bool IsPrev()
        {
            if (index == 0)
            {
                // 前の画像なし
                return false;
            }
            else
            {
                // 前の画像あり
                return true;
            }
        }

        /// <summary>
        /// Zipファイルをオープンする
        /// </summary>
        /// <param name="path">ファイル名</param>
        public void Open(string path)
        {
            // 他のファイルを開いていたらクローズ
            this.Close();

            // 引数のパスのZIPファイルの情報を取得
            item = ZipFile.OpenRead(path);

            this.ItemSort();

            // ページ数を設定
            pages = arclist.Count();
        }

        /// <summary>
        /// Zipファイルと使用しいる画像を閉じる
        /// </summary>
        public void Close()
        {
            if(dictionary != null)
            {
                dictionary.Clear();
            }

            if (item != null)
            {
                item.Dispose();
            }

            if(memstrem != null)
            {
                memstrem.Close();
            }
        }

        private void ItemSort()
        {
            IEnumerable<ZipArchiveEntry> list = item.Entries.Where(file => file.Length != 0);
            arclist = list.OrderBy(file => file.Name);
            index = 0;
        }

        /// <summary>
        /// 画像を取得
        /// </summary>
        private void GetItemData()
        {
            memstrem.Close();
            memstrem = new MemoryStream();

            // 元の画像を消す
            Stream s = arclist.ElementAt(index).Open();

            s.CopyTo(memstrem);
            memstrem.Seek(0, 0);

            data = new BitmapImage();
            data.BeginInit();
            data.StreamSource = memstrem;
            data.EndInit();

            s.Close();
            s.Dispose();
        }
    }
}
