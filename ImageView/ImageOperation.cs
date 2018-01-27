﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;

namespace ImageView
{
    class ImageOperation
    {
        System.Windows.Controls.Image imgctrl = null;
        
        private ZipFileData data = null;

        private FileInfo fi;

        private IEnumerable<FileInfo> filist;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageOperation(System.Windows.Controls.Image imgctrl)
        {
            this.imgctrl = imgctrl;
        }

        /// <summary>
        /// デストラクター
        /// </summary>
        ~ImageOperation()
        {
            this.Close();
        }

        /// <summary>
        /// ファイルオープン
        /// </summary>
        /// <param name="path"></param>
        public void Open(string path)
        {
            fi = new FileInfo(path);

            filist =  fi.Directory.EnumerateFiles("*.zip", SearchOption.TopDirectoryOnly);
            
            data = new ZipFileData(path);
            if(data != null)
            {
                imgctrl.Source = data.Data;
            }
        }

        public void Close()
        {
            if (data != null)
            {
                data.Close();
                data = null;
            }
        }

        public void Next()
        {
            if (data.IsNext())
            {
                // 次の画像表示
                imgctrl.Source = data.Next;
            }
            else
            {
                // 次のファイルを取得
                OpenNextFile();
            }
        }

        public void Prev()
        {
            if(data.IsPrev())
            {
                imgctrl.Source = data.Prev;
            }
            else
            {
                // 前のファイルを取得
                OpenPrevFile();
            }
        }

        public void NextFile()
        {
            OpenNextFile();
        }

        public void PrevFile()
        {
            OpenPrevFile();
        }

        private void OpenNextFile()
        {
            IEnumerable<FileInfo>  nextlist = filist.Where(file => file.CreationTime > fi.CreationTime);
            FileInfo nextfi = nextlist.OrderBy(file => file.CreationTime).First();
            data.Close();
            this.Open(nextfi.FullName);
        }

        private void OpenPrevFile()
        {
            IEnumerable<FileInfo> nextlist = filist.Where(file => file.CreationTime < fi.CreationTime);
            FileInfo nextfi = nextlist.OrderByDescending(file => file.CreationTime).First();
            data.Close();
            this.Open(nextfi.FullName);
        }
    }
}
