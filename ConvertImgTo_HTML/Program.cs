using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;

namespace ConvertImgTo_HTML
{
    class Program
    {
        static void Main(string[] args)
        {
            var converter = new Program();
        }
        public Program()
        {
            var commandLine = Environment.GetCommandLineArgs();
            if (commandLine.Length > 1)
            {

                var setting = SettingInfo.SetOption();
                var args = new string[commandLine.Length - 1];
                Array.Copy(commandLine, 1, args, 0, args.Length);

                ConvertAsciiArt(args, setting);
            }
            else
                Console.WriteLine("コマンドライン引数に画像ファイルのパスが指定されていません。");

            Console.WriteLine("処理が完了しました。エンターを押すと終了します。");
            Console.Read();
        }

        public void ConvertAsciiArt(string[] CommandLine, SettingInfo setting)
        {
            IOutput output;
            for (int i = 0; i < CommandLine.Length; i++)
            {
                Console.WriteLine("\r\n" + System.IO.Path.GetFileName(CommandLine[i]));
                var imageNameWithoutEx = System.IO.Path.GetFileNameWithoutExtension(CommandLine[i]);
                var outputFileInfo = new System.IO.FileInfo(setting.Path.FullName + "\\" + imageNameWithoutEx + ".htm");

                Bitmap bmp;//解放処理が面倒なので投げた。
                switch (setting.Mode)
                {
                    case SettingInfo.ConvertMode.HtmlArt:
                        if (!System.IO.File.Exists(CommandLine[i]))
                        {
                            Console.WriteLine("指定されたファイルが存在しませんでした。\r\n" + CommandLine[i]);
                            continue;
                        }
                        bmp = new Bitmap(CommandLine[i]);
                        output = new Output_txt(bmp, setting.Chars);
                        break;
                    case SettingInfo.ConvertMode.AsciiArt:
                        if (!System.IO.File.Exists(CommandLine[i]))
                        {
                            Console.WriteLine("指定されたファイルが存在しませんでした。\r\n" + CommandLine[i]);
                            continue;
                        }
                        bmp = new Bitmap(CommandLine[i]);
                        output = new Output_aa(bmp, setting.Chars);
                        break;
                    case SettingInfo.ConvertMode.Movie:
                        if (!System.IO.Directory.Exists(CommandLine[i]))
                        {
                            Console.WriteLine("指定されたパスはフォルダとして認識されませんでした。\r\n" + CommandLine[i]);
                            continue;
                        }
                        output = new Output_movie(new System.IO.DirectoryInfo(CommandLine[i]), setting.Chars);
                        break;
                    default:
                        Console.WriteLine("予想外の状態が発生しました。画像の処理をスキップします。");
                        continue;
                }

                output.ReducingImage += output_Reducting;
                output.CreatingHtml += output_CreateHtml;
                output.Convert(outputFileInfo, setting.Size);
                Console.WriteLine("[処理完了 : {0}]", imageNameWithoutEx + ".htm");
            }
        }

        void output_Reducting(object sender, EventArgs e)
        {
            Console.WriteLine("[画像の縮小開始... ]");
        }
        void output_CreateHtml(object sender, EventArgs e)
        {
            Console.WriteLine("[HTMLの生成開始... ]");
        }
    }
    struct SettingInfo
    {
        public System.IO.DirectoryInfo Path
        {
            get;
            set;
        }
        public int Size
        {
            get;
            set;
        }
        public ConvertMode Mode
        {
            get;
            set;
        }
        public string Chars
        {
            get;
            set;
        }

        public static SettingInfo SetOption()
        {
            var setting = new SettingInfo();

            Console.WriteLine("使用するモードを入力してください");
            Console.WriteLine("   -txt\t\t指定された画像をHTMLで表現します。\r\n   -aa\t\t指定された画像からAAを生成します。\r\n   -movie\t\t指定された画像からAA動画を生成します。");
            switch (ExRead("^(-txt|-aa|-movie)$"))
            {
                case "-txt":
                    setting.Mode = SettingInfo.ConvertMode.HtmlArt;
                    break;
                case "-aa":
                    setting.Mode = SettingInfo.ConvertMode.AsciiArt;
                    break;
                case "-movie":
                    setting.Mode = SettingInfo.ConvertMode.Movie;
                    break;
            }

            Console.WriteLine("元画像のサイズを縮小します。縮小度数を整数で入力してください。");
            setting.Size = int.Parse(ExRead("^[0-9]+$"));

            Console.WriteLine("保存先を指定してください。");
            Console.WriteLine("   おまけ:-desktopと入力しておくとデスクトップに保存されます。");
            string input = ExRead(@"^[a-z]:\\.+|-desktop");
            setting.Path = new System.IO.DirectoryInfo(
                input == "-desktop" ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : input
                );

            switch (setting.Mode)
            {
                case ConvertMode.HtmlArt:
                    Console.WriteLine("画像の表現に使用する文字を指定してください。");
                    setting.Chars = ExRead(".+");
                    break;
                case ConvertMode.AsciiArt | ConvertMode.Movie:
                    Console.WriteLine("階調を表現する文字を指定してください。");
                    Console.WriteLine("   (-defaultと入力するとあらかじめ用意された文字列が使われます。)");

                    string AA_String = ExRead(".*|-default");
                    setting.Chars = AA_String == "-default" ? null : AA_String;
                    break;
            }

            return setting;
        }
        public static string ExRead(string matchPattern)
        {
            string mode;
            bool inputed;
            do
            {
                Console.Write(">");
                mode = Console.ReadLine();

                if (mode == "" || !System.Text.RegularExpressions.Regex.IsMatch(mode, matchPattern))
                {
                    Console.WriteLine("入力に間違いがあります。打ち直してください。");
                    inputed = true;
                }
                else
                {
                    inputed = false;
                }
            }
            while (inputed);
            return mode;
        }
        public enum ConvertMode
        {
            AsciiArt,
            HtmlArt,
            Movie,
        }
    }

    abstract class ConverterBase : IOutput
    {
        protected System.Drawing.Bitmap _bmp;

        protected Bitmap GetResizeBmg(Bitmap bmp, int BlockSize)
        {
            System.Drawing.Bitmap resizeBmp = new Bitmap(bmp.Width / BlockSize, bmp.Height / BlockSize);

            for (int l = 0; l < bmp.Height / BlockSize; l++)
            {
                //画像にブロックが横に何個はいるかを出し、その数だけ下のブロックを取得するコードを実行する。
                for (int i = 0; i < bmp.Width / BlockSize; i++)
                {
                    int Ave_R = 0;
                    int Ave_G = 0;
                    int Ave_B = 0;

                    //モザイクの横の流れ。このfor文のループ一回がモザイクブロック一個分になります。
                    for (int j = 0; j < BlockSize; j++)
                    {
                        //モザイクの縦の流れ
                        for (int k = 0; k < BlockSize; k++)
                        {
                            /*iは何個目のピースかを示しすため、ピースのサイズを
                             * かければi番目のブロックはじめの座標が入る。
                             * lは行を示すため何行目かが分かる。そこにピースのサイズを
                             * 入れれば縦の座標も出る。
                             */
                            Ave_R += int.Parse(bmp.GetPixel(i * BlockSize + j, l * BlockSize + k).R.ToString());
                            Ave_G += int.Parse(bmp.GetPixel(i * BlockSize + j, l * BlockSize + k).G.ToString());
                            Ave_B += int.Parse(bmp.GetPixel(i * BlockSize + j, l * BlockSize + k).B.ToString());
                        }
                    }
                    Ave_R = Ave_R / (BlockSize * BlockSize);
                    Ave_G = Ave_G / (BlockSize * BlockSize);
                    Ave_B = Ave_B / (BlockSize * BlockSize);

                    //平均値を書き込みループを抜ける。このとき平均値は初期化され次の処理に影響はない。
                    resizeBmp.SetPixel(i, l, Color.FromArgb(Ave_R, Ave_G, Ave_B));
                }
            }
            return resizeBmp;
        }
        public virtual XmlDocument GetXml(int size)
        {
            OnReducingImage(new EventArgs());
            var bmp = GetResizeBmg(_bmp, size);

            OnCreatingHtml(new EventArgs());
            var xmlDoc = ToAsciiArt(bmp);

            return xmlDoc;
        }
        public virtual void Transform(int size, System.IO.Stream output, XmlReader xslt, System.Xml.Xsl.XsltArgumentList args)
        {
            var xmlDoc = GetXml(size);

            //Xml変換用のxslとxsltのストリームを用意
            var xsl = new System.Xml.Xsl.XslCompiledTransform();
            xsl.Load(xslt);
            xsl.Transform(xmlDoc, args, output);
        }
        public virtual void Transform(int size, System.IO.Stream output, XmlDocument xslt, System.Xml.Xsl.XsltArgumentList args)
        {
            var xmlDoc = GetXml(size);

            //Xml変換用のxslとxsltのストリームを用意
            var xsl = new System.Xml.Xsl.XslCompiledTransform(true);
            xsl.Load(xslt);
            xsl.Transform(xmlDoc, args, output);
        }
        public abstract void Convert(System.IO.FileInfo output, int size);
        public abstract XmlDocument ToAsciiArt(Bitmap bmp);
        protected static string To16(int n)
        {
            string output = "";

            int sinhou = 16;
            int division = n;
            int surplus;
            do
            {
                surplus = division % sinhou;
                division = (int)(division / sinhou);
                //division = division > sinhou ? (int)(division / sinhou) : 1;

                switch (surplus)
                {
                    case 10:
                        output = "A" + output;
                        break;
                    case 11:
                        output = "B" + output;
                        break;
                    case 12:
                        output = "C" + output;
                        break;
                    case 13:
                        output = "D" + output;
                        break;
                    case 14:
                        output = "E" + output;
                        break;
                    case 15:
                        output = "F" + output;
                        break;
                    default:
                        output = surplus.ToString() + output;
                        break;
                }
            }
            while (division > 0);

            if (output.Length < 2)
            {
                output = "0" + output;
            }
            return output;
        }

        //イベント発動メソッド
        protected virtual void OnReducingImage(EventArgs e)
        {
            if (ReducingImage != null)
                ReducingImage(this, e);
        }
        protected virtual void OnCreatingHtml(EventArgs e)
        {
            if (CreatingHtml != null)
                CreatingHtml(this, e);
        }
        //イベント本体
        public event EventHandler ReducingImage;
        public event EventHandler CreatingHtml;
    }
    interface IOutput
    {
        void Convert(System.IO.FileInfo output, int size);

        event EventHandler ReducingImage;
        event EventHandler CreatingHtml;
    }

    class Output_txt : ConverterBase
    {
        public Output_txt(Bitmap bmp_source, string chars)
        {
            _bmp = bmp_source;
            _chars = chars;
        }
        string _chars;

        public override void Convert(System.IO.FileInfo output, int size)
        {
            using (var fileStrm = new System.IO.FileStream(
                output.FullName,
                System.IO.FileMode.Create,
                System.IO.FileAccess.Write))
            {
                var cssPath = output.Directory.FullName
                    + "\\" + System.IO.Path.GetFileNameWithoutExtension(output.FullName) + ".css";
                var xsltDoc = new XmlDocument();
                var args_xsl = new System.Xml.Xsl.XsltArgumentList();

                //Transform前に引数やxslt等を準備
                args_xsl.AddParam("Mode", "", "HtmlArt");
                args_xsl.AddParam("StyleSheetHref", "", System.IO.Path.GetFileName(cssPath));
                xsltDoc.LoadXml(Properties.Resources.Xslt);

                //_bmpを変換し、出力結果をfileStrmへ書き出す
                Transform(size, fileStrm, xsltDoc, args_xsl);
                //CSSを書き出す
                System.IO.File.WriteAllText(cssPath, Properties.Resources.Style_txt);
            }
        }
        public override XmlDocument ToAsciiArt(Bitmap bmp)
        {
            //出力用xml
            var xml = new XmlDocument();

            //本書きの際はXmlDocumentでは処理が遅いためWriterを使う
            var memory = new System.IO.MemoryStream();
            var xmlWriter = XmlTextWriter.Create(memory);
            xmlWriter.WriteStartDocument();
            try
            {
                //描画用のエリアを生成<AsciiArt>
                xmlWriter.WriteStartElement("AsciiArt");

                //char型では書き込めないのでStringに変換。
                var aaString = new string[_chars.Length];
                for (int i = 0; i < aaString.Length; i++)
                {
                    aaString[i] = _chars[i].ToString();
                }

                #region Xml書き出し
                int int_nextTurn = 0;
                for (int i = 0; i <= bmp.Height - 1; i++)
                {
                    //<line>
                    xmlWriter.WriteStartElement("line");
                    for (int j = 0; j <= bmp.Width - 1; j++)
                    {
                        //16進法に変換する。
                        string red = System.Convert.ToString(int.Parse(bmp.GetPixel(j, i).R.ToString()), 16);
                        string gre = System.Convert.ToString(int.Parse(bmp.GetPixel(j, i).G.ToString()), 16);
                        string blu = System.Convert.ToString(int.Parse(bmp.GetPixel(j, i).B.ToString()), 16);

                        //<char color="#ffffff">■</char>
                        xmlWriter.WriteStartElement("char");
                        xmlWriter.WriteAttributeString("color", "#" + red + gre + blu);

                        //本書き
                        xmlWriter.WriteString(aaString[int_nextTurn].ToString());
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteString("\r\n");

                        int_nextTurn++;
                        if (int_nextTurn >= aaString.Length)
                            int_nextTurn = 0;
                    }
                    //</line>
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteString("\r\n");
                }
                #endregion

                //</AsciiArt>
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();

                xmlWriter.Flush();
                memory.Position = 0;
                xml.Load(memory);
            }
            finally
            {
                xmlWriter.Close();
                memory.Dispose();
            }
            return xml;
        }
    }
    class Output_aa : ConverterBase
    {
        public Output_aa(Bitmap bmp_source, string chars)
        {
            _bmp = bmp_source;
            _chars = chars;
        }
        string _chars;

        public override void Convert(System.IO.FileInfo output, int size)
        {
            using (var memory = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(Properties.Resources.Xslt)))
            using (var fileStrm = new System.IO.FileStream(
                output.FullName,
                System.IO.FileMode.Create,
                System.IO.FileAccess.Write))
            {
                memory.Position = 0;
                var xsltReader = new XmlTextReader(memory);
                var cssPath = output.Directory.FullName
                    + "\\" + System.IO.Path.GetFileNameWithoutExtension(output.FullName) + ".css";
                var args_xsl = new System.Xml.Xsl.XsltArgumentList();

                //Xslへの引数を用意
                args_xsl.AddParam("Mode", "", "AsciiArt");
                args_xsl.AddParam("StyleSheetHref", "", System.IO.Path.GetFileName(cssPath));

                //_bmpを変換し、出力結果をfileStrmに書き込む
                Transform(size, fileStrm, xsltReader, args_xsl);
                //CSSを書き出す
                System.IO.File.WriteAllText(cssPath, Properties.Resources.Style_AA);
            }
        }
        public override XmlDocument ToAsciiArt(Bitmap bmp)
        {
            //最終的な戻り値用にXmlDocumentを生成
            var xml = new XmlDocument();

            //AA用のString[]型の文字(aaString)を用意する。(_charがnullの場合に備え既定の文字列も用意した。)
            var aaChars = _chars == null ? "@#WBRQ0hexc!;:. " : _chars;
            var aaString = new string[aaChars.Length];
            for (int i = 0; i < aaString.Length; i++)
                //空白文字対策として事前に空白はHTML用に変換する
                aaString[i] = //aaChars[i].ToString();
                    (aaChars[i].ToString() == " ") ? "&nbsp;" : aaChars[i].ToString();

            /*文字の使い分けを指定。明るさマックスの際に不具合が
             * あるためマックスは後から手動で256と指定した。*/
            List<int> AA_threshold = new List<int>();
            for (int i = 0; i < aaString.Length - 1; i++)
            {
                AA_threshold.Add((256 / aaString.Length) * (i + 1));
            }
            AA_threshold.Add(256);

            //XmlWriterとそれの生成用にメモリ空間を確保
            //XmlDocumentでは速度に難有のためXmlWriterを利用した
            using (var memory = new System.IO.MemoryStream())
            {
                //本書き用のWriter
                var xmlWriter = XmlTextWriter.Create(memory);
                try
                {
                    //<div id="aaArea">
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("AsciiArt");
                    xmlWriter.WriteAttributeString("id", "aaArea");
                    xmlWriter.WriteAttributeString("width", bmp.Width.ToString());
                    xmlWriter.WriteAttributeString("height", bmp.Height.ToString());

                    #region XML書き出し
                    //<![CDATA[ <テスト> ]]>
                    for (int h = 0; h < bmp.Height; h++)
                    {
                        //行の開始を示す
                        xmlWriter.WriteStartElement("line");
                        for (int w = 0; w < bmp.Width; w++)
                        {
                            Color pixel_source = bmp.GetPixel(w, h);
                            //明るさを算出
                            int Ave_color = 0;
                            Ave_color += int.Parse(pixel_source.R.ToString());
                            Ave_color += int.Parse(pixel_source.G.ToString());
                            Ave_color += int.Parse(pixel_source.B.ToString());
                            Ave_color = Ave_color / 3;

                            //画像の明度に応じて文字を選別
                            for (int i = 0; i < AA_threshold.Count; i++)
                                if (Ave_color < AA_threshold[i])
                                {
                                    xmlWriter.WriteString(aaString[i]);
                                    break;
                                }
                        }
                        //x行目の<line>を閉じる
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteString("\r\n");
                    }
                    #endregion

                    //</AsciiArt>
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();

                    //バッファ内のものをすべて書き込みXmlDocumentへ変換する
                    xmlWriter.Flush();
                    memory.Flush();
                    memory.Position = 0;

                    //xmlWriterで作成したAAをxmlDocumentに変換
                    xml.Load(memory);
                }
                finally
                {
                    xmlWriter.Close();
                }
            }
            return xml;
        }
    }
    class Output_movie : IOutput
    {
        public Output_movie(System.IO.DirectoryInfo input, string chars)
        {
            _bmpFiles = input.GetFiles("*.bmp");
            _chars = chars;
        }

        string _chars;
        string _jsName = "caption";
        System.IO.FileInfo[] _bmpFiles;

        public void Convert(System.IO.FileInfo output, int size)
        {
            var converters = new Output_aa[_bmpFiles.Length];
            var captionDire = new System.IO.DirectoryInfo(output.Directory.FullName + "\\Captions");
            var cssFileInfo = new System.IO.FileInfo(output.Directory.FullName + "\\" + System.IO.Path.GetFileNameWithoutExtension(output.Name) + ".css");
            var jsFileInfo = new System.IO.FileInfo(output.Directory.FullName + "\\" + System.IO.Path.GetFileNameWithoutExtension(output.Name) + ".js");
            captionDire.Create();

            //Jsファイルを生成
            OutputCaptions(captionDire, converters, size);

            //作品本体であるhtmlを生成する
            using (var fileStrm = new System.IO.FileStream(output.FullName, System.IO.FileMode.Create))
            {
                var htmlDocBase = new XmlDocument();
                var importList = htmlDocBase.CreateElement("import-files");

                //動画制御用のスクリプトの参照
                var main_js = htmlDocBase.CreateElement("file");
                main_js.AppendChild(htmlDocBase.CreateTextNode(jsFileInfo.Name));
                importList.AppendChild(main_js);
                //各フレームの文字列を格納したスクリプトの参照
                for (int i = 0; i < converters.Length; i++)
                {
                    var item = htmlDocBase.CreateElement("file");
                    item.AppendChild(htmlDocBase.CreateTextNode(captionDire.Name + "/" + _jsName + i + ".js"));
                    importList.AppendChild(item);
                }
                htmlDocBase.AppendChild(importList);

                var xsltDoc = new XmlDocument();
                var xsl = new System.Xml.Xsl.XslCompiledTransform(true);
                var xsl_args = new System.Xml.Xsl.XsltArgumentList();
                xsltDoc.LoadXml(Properties.Resources.Xslt);

                xsl_args.AddParam("StyleSheetHref", "", cssFileInfo.Name);
                xsl_args.AddParam("Mode", "", "MoviePlayer");
                xsl.Load(xsltDoc);
                xsl.Transform(htmlDocBase, xsl_args, fileStrm);
            }

            //css &JavaScriptの書き出し
            System.IO.File.WriteAllText(cssFileInfo.FullName, Properties.Resources.Style_AA);
            System.IO.File.WriteAllText(jsFileInfo.FullName, Properties.Resources.JScript);
        }
        void OutputCaptions(System.IO.DirectoryInfo output, Output_aa[] converters, int size)
        {
            //xsltを読み込む
            var xsltDoc = new XmlDocument();
            var xslt_args = new System.Xml.Xsl.XsltArgumentList();
            xsltDoc.LoadXml(Properties.Resources.Xslt);
            xslt_args.AddParam("Mode", "", "Movie");

            for (int i = 0; i < converters.Length; i++)
                using (var fileStrm = new System.IO.FileStream(output.FullName + "\\" + _jsName + i + ".js", System.IO.FileMode.Create))
                using (var bmp = new System.Drawing.Bitmap(_bmpFiles[i].OpenRead()))
                {
                    xslt_args.AddParam("captionIndex", "", i);
                    converters[i] = new Output_aa(bmp, _chars);
                    converters[i].ReducingImage += delegate(object sender, EventArgs e)
                    {
                        Console.WriteLine(i + "番目開始");
                    };
                    converters[i].CreatingHtml += delegate(object sender, EventArgs e)
                    {
                        Console.WriteLine(i + "番目完了");
                    };

                    converters[i].Transform(size, fileStrm, xsltDoc, xslt_args);
                    xslt_args.RemoveParam("captionIndex", "");
                }
        }

        //イベント発動メソッド
        protected virtual void OnReducingImage(EventArgs e)
        {
            if (ReducingImage != null)
                ReducingImage(this, e);
        }
        protected virtual void OnCreatingHtml(EventArgs e)
        {
            if (CreatingHtml != null)
                CreatingHtml(this, e);
        }

        public event EventHandler ReducingImage;
        public event EventHandler CreatingHtml;
    }

}