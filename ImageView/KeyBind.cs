using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageView
{
    delegate void Next();
    delegate void Prev();
    delegate void NextFile();
    delegate void PrevFile();

    class KeyBind
    {
        private Next next;
        private Prev prev;
        private NextFile nextfile;
        private PrevFile prevfile;

        /// <summary>
        /// 使用できるキーのリスト
        /// </summary>
        public enum KeyList
        {
            // マウス キー
            /// <summary>
            /// マウス 左クリック
            /// </summary>
            MouseLeftClick,

            /// <summary>
            /// マウス 左クリック
            /// </summary>
            MouseLeftDoubleClick,

            /// <summary>
            /// マウス右クリック
            /// </summary>
            MouseRightClick,

            /// <summary>
            /// マウス右クリック
            /// </summary>
            MouseRightDoubleClick,

            /// <summary>
            /// マウス スクロールアップ
            /// </summary>
            MouseScrollUp,

            /// <summary>
            /// マウス スクロールダウン
            /// </summary>
            MouseScrollDown,

        };

        /// <summary>
        /// 使用できるアクション
        /// </summary>
        public enum ActionList
        {
            /// <summary>
            /// アクションなし
            /// </summary>
            None,
            /// <summary>
            /// 次の画像
            /// </summary>
            Next,
            /// <summary>
            /// 前の画像
            /// </summary>
            Prev,
            /// <summary>
            /// 次のファイル
            /// </summary>
            NextFile,
            /// <summary>
            /// 前のファイル
            /// </summary>
            PrevFile
        }

        private Dictionary<KeyList, ActionList> dictionary = new Dictionary<KeyList, ActionList>();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KeyBind(ImageOperation imgope)
        {
            // TODO:仮のキーバインド
            dictionary.Add(KeyList.MouseLeftClick, ActionList.None);
            dictionary.Add(KeyList.MouseRightClick, ActionList.None);
            dictionary.Add(KeyList.MouseLeftDoubleClick, ActionList.NextFile);
            dictionary.Add(KeyList.MouseRightDoubleClick, ActionList.PrevFile);
            dictionary.Add(KeyList.MouseScrollUp, ActionList.Prev);
            dictionary.Add(KeyList.MouseScrollDown, ActionList.Next);

            next = new Next(imgope.Next);
            prev = new Prev(imgope.Prev);
            nextfile = new NextFile(imgope.NextFile);
            prevfile = new PrevFile(imgope.PrevFile);

        }

        public void Action(KeyList input)
        {
            switch(dictionary[input])
            {
                case ActionList.Next:
                    this.next();
                    break;
                case ActionList.Prev:
                    this.prev();
                    break;
                case ActionList.NextFile:
                    this.nextfile();
                    break;
                case ActionList.PrevFile:
                    this.prevfile();
                    break;
            }
        }
    }
}
