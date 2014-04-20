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
                Console.WriteLine("�R�}���h���C�������ɉ摜�t�@�C���̃p�X���w�肳��Ă��܂���B");

            Console.WriteLine("�������������܂����B�G���^�[�������ƏI�����܂��B");
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

                Bitmap bmp;//����������ʓ|�Ȃ̂œ������B
                switch (setting.Mode)
                {
                    case SettingInfo.ConvertMode.HtmlArt:
                        if (!System.IO.File.Exists(CommandLine[i]))
                        {
                            Console.WriteLine("�w�肳�ꂽ�t�@�C�������݂��܂���ł����B\r\n" + CommandLine[i]);
                            continue;
                        }
                        bmp = new Bitmap(CommandLine[i]);
                        output = new Output_txt(bmp, setting.Chars);
                        break;
                    case SettingInfo.ConvertMode.AsciiArt:
                        if (!System.IO.File.Exists(CommandLine[i]))
                        {
                            Console.WriteLine("�w�肳�ꂽ�t�@�C�������݂��܂���ł����B\r\n" + CommandLine[i]);
                            continue;
                        }
                        bmp = new Bitmap(CommandLine[i]);
                        output = new Output_aa(bmp, setting.Chars);
                        break;
                    case SettingInfo.ConvertMode.Movie:
                        if (!System.IO.Directory.Exists(CommandLine[i]))
                        {
                            Console.WriteLine("�w�肳�ꂽ�p�X�̓t�H���_�Ƃ��ĔF������܂���ł����B\r\n" + CommandLine[i]);
                            continue;
                        }
                        output = new Output_movie(new System.IO.DirectoryInfo(CommandLine[i]), setting.Chars);
                        break;
                    default:
                        Console.WriteLine("�\�z�O�̏�Ԃ��������܂����B�摜�̏������X�L�b�v���܂��B");
                        continue;
                }

                output.ReducingImage += output_Reducting;
                output.CreatingHtml += output_CreateHtml;
                output.Convert(outputFileInfo, setting.Size);
                Console.WriteLine("[�������� : {0}]", imageNameWithoutEx + ".htm");
            }
        }

        void output_Reducting(object sender, EventArgs e)
        {
            Console.WriteLine("[�摜�̏k���J�n... ]");
        }
        void output_CreateHtml(object sender, EventArgs e)
        {
            Console.WriteLine("[HTML�̐����J�n... ]");
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

            Console.WriteLine("�g�p���郂�[�h����͂��Ă�������");
            Console.WriteLine("   -txt\t\t�w�肳�ꂽ�摜��HTML�ŕ\�����܂��B\r\n   -aa\t\t�w�肳�ꂽ�摜����AA�𐶐����܂��B\r\n   -movie\t\t�w�肳�ꂽ�摜����AA����𐶐����܂��B");
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

            Console.WriteLine("���摜�̃T�C�Y���k�����܂��B�k���x���𐮐��œ��͂��Ă��������B");
            setting.Size = int.Parse(ExRead("^[0-9]+$"));

            Console.WriteLine("�ۑ�����w�肵�Ă��������B");
            Console.WriteLine("   ���܂�:-desktop�Ɠ��͂��Ă����ƃf�X�N�g�b�v�ɕۑ�����܂��B");
            string input = ExRead(@"^[a-z]:\\.+|-desktop");
            setting.Path = new System.IO.DirectoryInfo(
                input == "-desktop" ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : input
                );

            switch (setting.Mode)
            {
                case ConvertMode.HtmlArt:
                    Console.WriteLine("�摜�̕\���Ɏg�p���镶�����w�肵�Ă��������B");
                    setting.Chars = ExRead(".+");
                    break;
                case ConvertMode.AsciiArt | ConvertMode.Movie:
                    Console.WriteLine("�K����\�����镶�����w�肵�Ă��������B");
                    Console.WriteLine("   (-default�Ɠ��͂���Ƃ��炩���ߗp�ӂ��ꂽ�����񂪎g���܂��B)");

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
                    Console.WriteLine("���͂ɊԈႢ������܂��B�ł������Ă��������B");
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
                //�摜�Ƀu���b�N�����ɉ��͂��邩���o���A���̐��������̃u���b�N���擾����R�[�h�����s����B
                for (int i = 0; i < bmp.Width / BlockSize; i++)
                {
                    int Ave_R = 0;
                    int Ave_G = 0;
                    int Ave_B = 0;

                    //���U�C�N�̉��̗���B����for���̃��[�v��񂪃��U�C�N�u���b�N����ɂȂ�܂��B
                    for (int j = 0; j < BlockSize; j++)
                    {
                        //���U�C�N�̏c�̗���
                        for (int k = 0; k < BlockSize; k++)
                        {
                            /*i�͉��ڂ̃s�[�X�������������߁A�s�[�X�̃T�C�Y��
                             * �������i�Ԗڂ̃u���b�N�͂��߂̍��W������B
                             * l�͍s���������߉��s�ڂ���������B�����Ƀs�[�X�̃T�C�Y��
                             * �����Ώc�̍��W���o��B
                             */
                            Ave_R += int.Parse(bmp.GetPixel(i * BlockSize + j, l * BlockSize + k).R.ToString());
                            Ave_G += int.Parse(bmp.GetPixel(i * BlockSize + j, l * BlockSize + k).G.ToString());
                            Ave_B += int.Parse(bmp.GetPixel(i * BlockSize + j, l * BlockSize + k).B.ToString());
                        }
                    }
                    Ave_R = Ave_R / (BlockSize * BlockSize);
                    Ave_G = Ave_G / (BlockSize * BlockSize);
                    Ave_B = Ave_B / (BlockSize * BlockSize);

                    //���ϒl���������݃��[�v�𔲂���B���̂Ƃ����ϒl�͏��������ꎟ�̏����ɉe���͂Ȃ��B
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

            //Xml�ϊ��p��xsl��xslt�̃X�g���[����p��
            var xsl = new System.Xml.Xsl.XslCompiledTransform();
            xsl.Load(xslt);
            xsl.Transform(xmlDoc, args, output);
        }
        public virtual void Transform(int size, System.IO.Stream output, XmlDocument xslt, System.Xml.Xsl.XsltArgumentList args)
        {
            var xmlDoc = GetXml(size);

            //Xml�ϊ��p��xsl��xslt�̃X�g���[����p��
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

        //�C�x���g�������\�b�h
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
        //�C�x���g�{��
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

                //Transform�O�Ɉ�����xslt��������
                args_xsl.AddParam("Mode", "", "HtmlArt");
                args_xsl.AddParam("StyleSheetHref", "", System.IO.Path.GetFileName(cssPath));
                xsltDoc.LoadXml(Properties.Resources.Xslt);

                //_bmp��ϊ����A�o�͌��ʂ�fileStrm�֏����o��
                Transform(size, fileStrm, xsltDoc, args_xsl);
                //CSS�������o��
                System.IO.File.WriteAllText(cssPath, Properties.Resources.Style_txt);
            }
        }
        public override XmlDocument ToAsciiArt(Bitmap bmp)
        {
            //�o�͗pxml
            var xml = new XmlDocument();

            //�{�����̍ۂ�XmlDocument�ł͏������x������Writer���g��
            var memory = new System.IO.MemoryStream();
            var xmlWriter = XmlTextWriter.Create(memory);
            xmlWriter.WriteStartDocument();
            try
            {
                //�`��p�̃G���A�𐶐�<AsciiArt>
                xmlWriter.WriteStartElement("AsciiArt");

                //char�^�ł͏������߂Ȃ��̂�String�ɕϊ��B
                var aaString = new string[_chars.Length];
                for (int i = 0; i < aaString.Length; i++)
                {
                    aaString[i] = _chars[i].ToString();
                }

                #region Xml�����o��
                int int_nextTurn = 0;
                for (int i = 0; i <= bmp.Height - 1; i++)
                {
                    //<line>
                    xmlWriter.WriteStartElement("line");
                    for (int j = 0; j <= bmp.Width - 1; j++)
                    {
                        //16�i�@�ɕϊ�����B
                        string red = System.Convert.ToString(int.Parse(bmp.GetPixel(j, i).R.ToString()), 16);
                        string gre = System.Convert.ToString(int.Parse(bmp.GetPixel(j, i).G.ToString()), 16);
                        string blu = System.Convert.ToString(int.Parse(bmp.GetPixel(j, i).B.ToString()), 16);

                        //<char color="#ffffff">��</char>
                        xmlWriter.WriteStartElement("char");
                        xmlWriter.WriteAttributeString("color", "#" + red + gre + blu);

                        //�{����
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

                //Xsl�ւ̈�����p��
                args_xsl.AddParam("Mode", "", "AsciiArt");
                args_xsl.AddParam("StyleSheetHref", "", System.IO.Path.GetFileName(cssPath));

                //_bmp��ϊ����A�o�͌��ʂ�fileStrm�ɏ�������
                Transform(size, fileStrm, xsltReader, args_xsl);
                //CSS�������o��
                System.IO.File.WriteAllText(cssPath, Properties.Resources.Style_AA);
            }
        }
        public override XmlDocument ToAsciiArt(Bitmap bmp)
        {
            //�ŏI�I�Ȗ߂�l�p��XmlDocument�𐶐�
            var xml = new XmlDocument();

            //AA�p��String[]�^�̕���(aaString)��p�ӂ���B(_char��null�̏ꍇ�ɔ�������̕�������p�ӂ����B)
            var aaChars = _chars == null ? "@#WBRQ0hexc!;:. " : _chars;
            var aaString = new string[aaChars.Length];
            for (int i = 0; i < aaString.Length; i++)
                //�󔒕����΍�Ƃ��Ď��O�ɋ󔒂�HTML�p�ɕϊ�����
                aaString[i] = //aaChars[i].ToString();
                    (aaChars[i].ToString() == " ") ? "&nbsp;" : aaChars[i].ToString();

            /*�����̎g���������w��B���邳�}�b�N�X�̍ۂɕs���
             * ���邽�߃}�b�N�X�͌ォ��蓮��256�Ǝw�肵���B*/
            List<int> AA_threshold = new List<int>();
            for (int i = 0; i < aaString.Length - 1; i++)
            {
                AA_threshold.Add((256 / aaString.Length) * (i + 1));
            }
            AA_threshold.Add(256);

            //XmlWriter�Ƃ���̐����p�Ƀ�������Ԃ��m��
            //XmlDocument�ł͑��x�ɓ�L�̂���XmlWriter�𗘗p����
            using (var memory = new System.IO.MemoryStream())
            {
                //�{�����p��Writer
                var xmlWriter = XmlTextWriter.Create(memory);
                try
                {
                    //<div id="aaArea">
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("AsciiArt");
                    xmlWriter.WriteAttributeString("id", "aaArea");
                    xmlWriter.WriteAttributeString("width", bmp.Width.ToString());
                    xmlWriter.WriteAttributeString("height", bmp.Height.ToString());

                    #region XML�����o��
                    //<![CDATA[ <�e�X�g> ]]>
                    for (int h = 0; h < bmp.Height; h++)
                    {
                        //�s�̊J�n������
                        xmlWriter.WriteStartElement("line");
                        for (int w = 0; w < bmp.Width; w++)
                        {
                            Color pixel_source = bmp.GetPixel(w, h);
                            //���邳���Z�o
                            int Ave_color = 0;
                            Ave_color += int.Parse(pixel_source.R.ToString());
                            Ave_color += int.Parse(pixel_source.G.ToString());
                            Ave_color += int.Parse(pixel_source.B.ToString());
                            Ave_color = Ave_color / 3;

                            //�摜�̖��x�ɉ����ĕ�����I��
                            for (int i = 0; i < AA_threshold.Count; i++)
                                if (Ave_color < AA_threshold[i])
                                {
                                    xmlWriter.WriteString(aaString[i]);
                                    break;
                                }
                        }
                        //x�s�ڂ�<line>�����
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteString("\r\n");
                    }
                    #endregion

                    //</AsciiArt>
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();

                    //�o�b�t�@���̂��̂����ׂď�������XmlDocument�֕ϊ�����
                    xmlWriter.Flush();
                    memory.Flush();
                    memory.Position = 0;

                    //xmlWriter�ō쐬����AA��xmlDocument�ɕϊ�
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

            //Js�t�@�C���𐶐�
            OutputCaptions(captionDire, converters, size);

            //��i�{�̂ł���html�𐶐�����
            using (var fileStrm = new System.IO.FileStream(output.FullName, System.IO.FileMode.Create))
            {
                var htmlDocBase = new XmlDocument();
                var importList = htmlDocBase.CreateElement("import-files");

                //���搧��p�̃X�N���v�g�̎Q��
                var main_js = htmlDocBase.CreateElement("file");
                main_js.AppendChild(htmlDocBase.CreateTextNode(jsFileInfo.Name));
                importList.AppendChild(main_js);
                //�e�t���[���̕�������i�[�����X�N���v�g�̎Q��
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

            //css &JavaScript�̏����o��
            System.IO.File.WriteAllText(cssFileInfo.FullName, Properties.Resources.Style_AA);
            System.IO.File.WriteAllText(jsFileInfo.FullName, Properties.Resources.JScript);
        }
        void OutputCaptions(System.IO.DirectoryInfo output, Output_aa[] converters, int size)
        {
            //xslt��ǂݍ���
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
                        Console.WriteLine(i + "�ԖڊJ�n");
                    };
                    converters[i].CreatingHtml += delegate(object sender, EventArgs e)
                    {
                        Console.WriteLine(i + "�Ԗڊ���");
                    };

                    converters[i].Transform(size, fileStrm, xsltDoc, xslt_args);
                    xslt_args.RemoveParam("captionIndex", "");
                }
        }

        //�C�x���g�������\�b�h
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