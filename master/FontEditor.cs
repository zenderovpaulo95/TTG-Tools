using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TTG_Tools.ClassesStructs;
using TTG_Tools.Graphics.Swizzles;

namespace TTG_Tools
{
    public partial class FontEditor : Form
    {
        [DllImport("kernel32.dll")]
        public static extern void SetProcessWorkingSetSize(IntPtr hWnd, int i, int j);

        public FontEditor()
        {
            InitializeComponent();
            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }

        OpenFileDialog ofd = new OpenFileDialog();
        bool edited; //Проверка на изменения в шрифте
        bool encrypted; //В случае, если шрифт был зашифрован
        byte[] encKey;
        int version;
        byte[] tmpHeader;
        byte[] check_header;
        bool someTexData;
        bool AddInfo;

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void FontEditor_Load(object sender, EventArgs e)
        {
            edited = false; //Tell a program about first launch window form so font is not modified.
            
            if(MainMenu.settings.swizzlePS4 || MainMenu.settings.swizzleNintendoSwitch)
            {
                if (MainMenu.settings.swizzlePS4) rbPS4Swizzle.Checked = true;
                else rbSwitchSwizzle.Checked = true;
            }
            else
            {
                rbNoSwizzle.Checked = true;
            }
        }

        public List<byte[]> head = new List<byte[]>();
        public ClassesStructs.FlagsClass fontFlags;
        FontClass.ClassFont font = null;

        private void ReplaceTexture(string DdsFile, ClassesStructs.TextureClass.OldT3Texture tex)
        {
            FileStream fs = new FileStream(DdsFile, FileMode.Open);
            byte[] temp = Methods.ReadFull(fs);
            fs.Close();

            tex.Content = new byte[temp.Length];
            Array.Copy(temp, 0, tex.Content, 0, temp.Length);

            MemoryStream ms = new MemoryStream(tex.Content);
            Graphics.TextureWorker.ReadDDSHeader(ms, ref tex.Width, ref tex.Height, ref tex.Mip, ref tex.TextureFormat, false);
            ms.Close();

            /*if (tex.isPS3)
            {
                int tmpPos = tex.block.Length;

                byte texFormat = 0;

                int texSize = tex.Content.Length;
                int paddedSize = Methods.pad_size(texSize, 128);

                //cut dds header and copy to padded block
                byte[] tmp = new byte[paddedSize - 128];
                Array.Copy(tex.Content, 128, tmp, 0, tex.Content.Length - 128);
                tex.Content = new byte[tmp.Length];
                Array.Copy(tmp, 0, tex.Content, 0, tmp.Length);

                switch (tex.TextureFormat)
                {
                    case (uint)ClassesStructs.TextureClass.OldTextureFormat.DX_DXT1:
                        texFormat = 0x86;
                        break;

                    case (uint)ClassesStructs.TextureClass.OldTextureFormat.DX_DXT5:
                        texFormat = 0x88;
                        break;
                }

                tmp = new byte[1];
                tmp[0] = Convert.ToByte(tex.Mip);
                Array.Copy(tmp, 0, tex.block, tmpPos - 103, tmp.Length);

                tmp = new byte[1];
                tmp[0] = texFormat;
                Array.Copy(tmp, 0, tex.block, tmpPos - 104, tmp.Length);

                tmp = new byte[1];
                tmp[0] = Convert.ToByte(tex.Mip);
                Array.Copy(tmp, 0, tex.block, tmpPos - 103, tmp.Length);

                tmp = BitConverter.GetBytes(tex.Width).Reverse().ToArray();
                Array.Copy(tmp, 2, tex.block, tmpPos - 96, 2);

                tmp = BitConverter.GetBytes(tex.Height).Reverse().ToArray();
                Array.Copy(tmp, 2, tex.block, tmpPos - 94, 2);


                tex.TexSize = texSize;

                tmp = BitConverter.GetBytes(texSize - 128).Reverse().ToArray();
                Array.Copy(tmp, 0, tex.block, tmpPos - 124, tmp.Length);

                tmp = BitConverter.GetBytes(paddedSize - 128).Reverse().ToArray();
                Array.Copy(tmp, 0, tex.block, tmpPos - 108, tmp.Length);

                paddedSize += 4; //Add 4 bytes for common size block
                tmp = BitConverter.GetBytes(paddedSize);
                Array.Copy(tmp, 0, tex.block, tmpPos - 132, tmp.Length);
            }*/

            tex.OriginalHeight = tex.Height;
            tex.OriginalWidth = tex.Width;
            font.BlockTexSize += tex.Content.Length - tex.TexSize;
            if(!tex.isPS3) tex.TexSize = tex.Content.Length;
        }

        private void ReplaceTexture(string DdsFile, ClassesStructs.TextureClass.NewT3Texture NewTex)
        {
            byte[] temp = File.ReadAllBytes(DdsFile);
            NewTex.Tex.Content = new byte[temp.Length];
            Array.Copy(temp, 0, NewTex.Tex.Content, 0, temp.Length);

            MemoryStream ms = new MemoryStream(NewTex.Tex.Content);

            FileInfo fi = new FileInfo(DdsFile);

            if (fi.Extension.ToLower() == ".dds")
            {
                Graphics.TextureWorker.ReadDDSHeader(ms, ref NewTex.Width, ref NewTex.Height, ref NewTex.Mip, ref NewTex.TextureFormat, true);
                NewTex.platform.platform = 2;

                if(MainMenu.settings.swizzleNintendoSwitch) NewTex.platform.platform = 15;
                if (MainMenu.settings.swizzlePS4) NewTex.platform.platform = 11;
            }
            else
            {
                Graphics.TextureWorker.ReadPvrHeader(ms, ref NewTex.Width, ref NewTex.Height, ref NewTex.Mip, ref NewTex.platform.platform, true);
                NewTex.platform.platform = (NewTex.platform.platform != 7) || (NewTex.platform.platform != 9) ? 7 : NewTex.platform.platform;
            }

            NewTex.Mip = 1; //There is no need more than one mip map!
            NewTex.Tex.MipCount = NewTex.Mip;
            NewTex.Tex.Textures = new ClassesStructs.TextureClass.NewT3Texture.TextureStruct[NewTex.Mip];

            int w = NewTex.Width;
            int h = NewTex.Height;

            int pos = (int)ms.Position;
            ms.Close();

            NewTex.Tex.TexSize = 0;

            int blockSize = NewTex.TextureFormat == 0x40 || NewTex.TextureFormat == 0x43 ? 8 : 16;

            for (int i = 0; i < NewTex.Tex.MipCount; i++)
            {
                NewTex.Tex.Textures[i].CurrentMip = i;
                Methods.getSizeAndKratnost(w, h, (int)NewTex.TextureFormat, ref NewTex.Tex.Textures[i].MipSize, ref NewTex.Tex.Textures[i].BlockSize);

                NewTex.Tex.Textures[i].Block = new byte[NewTex.Tex.Textures[i].MipSize];

                Array.Copy(NewTex.Tex.Content, pos, NewTex.Tex.Textures[i].Block, 0, NewTex.Tex.Textures[i].Block.Length);
                switch(NewTex.platform.platform)
                {
                    case 11:
                        if (NewTex.Tex.Textures[i].Block.Length < blockSize) blockSize = NewTex.Tex.Textures[i].Block.Length;
                        NewTex.Tex.Textures[i].Block = PS4.Swizzle(NewTex.Tex.Textures[i].Block, w, h, blockSize);
                        break;

                    case 15:
                        NewTex.Tex.Textures[i].Block = NintendoSwitch.NintendoSwizzle(NewTex.Tex.Textures[i].Block, w, h, (int)NewTex.TextureFormat, false);
                        break;
                } 


                pos += NewTex.Tex.Textures[i].MipSize;
                NewTex.Tex.TexSize += (uint)NewTex.Tex.Textures[i].MipSize;

                if (NewTex.SomeValue >= 5) NewTex.Tex.Textures[i].SubTexNum = 0;
                if (NewTex.HasOneValueTex) NewTex.Tex.Textures[i].One = 1;

                if (w > 1) w /= 2;
                if (h > 1) h /= 2;
            }
        }

        private void fillTableofCoordinates(FontClass.ClassFont font, bool Modified)
        {
            if (!font.NewFormat)
            {
                dataGridViewWithCoord.RowCount = font.glyph.CharCount;
                dataGridViewWithCoord.ColumnCount = 7;
                if (font.hasScaleValue)
                {
                    dataGridViewWithCoord.ColumnCount = 9;
                    dataGridViewWithCoord.Columns[7].HeaderText = "Width";
                    dataGridViewWithCoord.Columns[8].HeaderText = "Height";
                }

                for (int i = 0; i < font.glyph.CharCount; i++)
                {
                    dataGridViewWithCoord.Rows[i].HeaderCell.Value = Convert.ToString(i + 1);
                    dataGridViewWithCoord[0, i].Value = i;
                    dataGridViewWithCoord[1, i].Value = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(BitConverter.GetBytes(i)).Replace("\0", string.Empty);
                    dataGridViewWithCoord[2, i].Value = font.glyph.chars[i].XStart;
                    dataGridViewWithCoord[3, i].Value = font.glyph.chars[i].XEnd;
                    dataGridViewWithCoord[4, i].Value = font.glyph.chars[i].YStart;
                    dataGridViewWithCoord[5, i].Value = font.glyph.chars[i].YEnd;
                    dataGridViewWithCoord[6, i].Value = font.glyph.chars[i].TexNum;

                    if (font.hasScaleValue)
                    {
                        dataGridViewWithCoord[7, i].Value = font.glyph.chars[i].CharWidth;
                        dataGridViewWithCoord[8, i].Value = font.glyph.chars[i].CharHeight;
                    }
                }
            }
            else
            {
                dataGridViewWithCoord.RowCount = font.glyph.CharCount;
                dataGridViewWithCoord.ColumnCount = 13;
                dataGridViewWithCoord.Columns[7].HeaderText = "Width";
                dataGridViewWithCoord.Columns[8].HeaderText = "Height";
                dataGridViewWithCoord.Columns[9].HeaderText = "Offset by X";
                dataGridViewWithCoord.Columns[10].HeaderText = "Offset by Y";
                dataGridViewWithCoord.Columns[11].HeaderText = "X advance";
                dataGridViewWithCoord.Columns[12].HeaderText = "Channel";

                for (int i = 0; i < font.glyph.CharCount; i++)
                {
                    dataGridViewWithCoord.Rows[i].HeaderCell.Value = Convert.ToString(i + 1);
                    dataGridViewWithCoord[0, i].Value = font.glyph.charsNew[i].charId;

                    dataGridViewWithCoord[1, i].Value = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(BitConverter.GetBytes(font.glyph.charsNew[i].charId)).Replace("\0", string.Empty);
                    
                    if(MainMenu.settings.unicodeSettings == 0)
                    {
                        dataGridViewWithCoord[1, i].Value = Encoding.Unicode.GetString(BitConverter.GetBytes(font.glyph.charsNew[i].charId)).Replace("\0", string.Empty);
                    }

                    dataGridViewWithCoord[2, i].Value = font.glyph.charsNew[i].XStart;
                    dataGridViewWithCoord[3, i].Value = font.glyph.charsNew[i].XEnd;
                    dataGridViewWithCoord[4, i].Value = font.glyph.charsNew[i].YStart;
                    dataGridViewWithCoord[5, i].Value = font.glyph.charsNew[i].YEnd;
                    dataGridViewWithCoord[6, i].Value = font.glyph.charsNew[i].TexNum;
                    dataGridViewWithCoord[7, i].Value = font.glyph.charsNew[i].CharWidth;
                    dataGridViewWithCoord[8, i].Value = font.glyph.charsNew[i].CharHeight;
                    dataGridViewWithCoord[9, i].Value = font.glyph.charsNew[i].XOffset;
                    dataGridViewWithCoord[10, i].Value = font.glyph.charsNew[i].YOffset;
                    dataGridViewWithCoord[11, i].Value = font.glyph.charsNew[i].XAdvance;
                    dataGridViewWithCoord[12, i].Value = font.glyph.charsNew[i].Channel;
                }
            }

            for(int k = 0; k < dataGridViewWithCoord.RowCount; k++)
            {
                for(int l = 0; l < dataGridViewWithCoord.ColumnCount; l++)
                {
                    dataGridViewWithCoord[l, k].Style.BackColor = Modified ? Color.GreenYellow : Color.White;
                }
            }
        }

        private void fillTableofTextures(FontClass.ClassFont font)
        {
            dataGridViewWithTextures.RowCount = font.TexCount;

            if (!font.NewFormat)
            {
                for (int i = 0; i < font.TexCount; i++)
                {
                    dataGridViewWithTextures[0, i].Value = i;
                    dataGridViewWithTextures[1, i].Value = font.tex[i].Height;
                    dataGridViewWithTextures[2, i].Value = font.tex[i].Width;
                    dataGridViewWithTextures[3, i].Value = font.tex[i].TexSize;
                }
            }
            else
            {
                for (int i = 0; i < font.TexCount; i++)
                {
                    dataGridViewWithTextures[0, i].Value = i;
                    dataGridViewWithTextures[1, i].Value = font.NewTex[i].Height;
                    dataGridViewWithTextures[2, i].Value = font.NewTex[i].Width;
                    dataGridViewWithTextures[3, i].Value = font.NewTex[i].Tex.TexSize;
                }
            }
        }

        private string ConvertToString(byte[] mas)
        {
            string str = "";
            foreach (byte b in mas)
            { str += b.ToString("x") + " "; }

            return str;
        }

        public bool CompareArray(byte[] arr0, byte[] arr1)
        {
            int i = 0;
            while ((i < arr0.Length) && (arr0[i] == arr1[i])) i++;
            return (i == arr0.Length);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
                ofd.Filter = "Font files (*.font)|*.font";
                ofd.RestoreDirectory = true;
                ofd.Title = "Open font file";
                ofd.DereferenceLinks = false;
                byte[] binContent = new byte[0];
                string FileName = "";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    encrypted = false;
                    bool read = false;

                    FileStream fs;
                    try
                    {
                        FileName = ofd.FileName;
                        fs = new FileStream(ofd.FileName, FileMode.Open);
                        binContent = Methods.ReadFull(fs);
                        fs.Close();
                        read = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error!");
                        saveToolStripMenuItem.Enabled = false;
                        saveAsToolStripMenuItem.Enabled = false;
                        exportCoordinatesToolStripMenuItem1.Enabled = false;
                        Form.ActiveForm.Text = "Font Editor";
                    }


                    if (read)
                    {
                    try
                    {
                        fontFlags = null;

                        byte[] header = new byte[4];
                        Array.Copy(binContent, 0, header, 0, 4);

                        int poz = 0;

                        //Experiments with too old fonts
                        font = new FontClass.ClassFont();
                        font.hasOneFloatValue = false;
                        font.blockSize = false;
                        font.hasScaleValue = false;
                        AddInfo = false;

                        font.headerSize = 0;
                        font.texSize = 0;

                        poz = 4; //Begin position

                        check_header = new byte[4];
                        Array.Copy(binContent, 0, check_header, 0, check_header.Length);
                        encKey = null;
                        version = 2;

                        if ((Encoding.ASCII.GetString(check_header) != "5VSM") && (Encoding.ASCII.GetString(check_header) != "ERTM")
                        && (Encoding.ASCII.GetString(check_header) != "6VSM") && (Encoding.ASCII.GetString(check_header) != "NIBM")) //Supposed this font encrypted
                        {
                            //First trying decrypt probably encrypted font
                            try
                            {
                                string info = Methods.FindingDecrytKey(binContent, "font", ref encKey, ref version);
                                if (info != null)
                                {
                                    MessageBox.Show("Font was encrypted, but I decrypted.\r\n" + info);
                                    encrypted = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Maybe that font encrypted. Try to decrypt first.", "Error " + ex.Message);
                                poz = -1;
                                return;
                            }
                        }

                        if ((Encoding.ASCII.GetString(check_header) == "5VSM") || (Encoding.ASCII.GetString(check_header) == "6VSM"))
                        {
                            byte[] tmpBytes = new byte[4];
                            Array.Copy(binContent, 4, tmpBytes, 0, tmpBytes.Length);
                            font.NewFormat = true;
                            font.headerSize = BitConverter.ToInt32(tmpBytes, 0);

                            tmpBytes = new byte[4];
                            Array.Copy(binContent, 12, tmpBytes, 0, tmpBytes.Length);
                            font.texSize = BitConverter.ToUInt32(tmpBytes, 0);

                            poz = 16;
                        }

                        byte[] tmp = new byte[4];
                        Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                        poz += 4;
                        int countElements = BitConverter.ToInt32(tmp, 0);
                        font.elements = new string[countElements];
                        font.binElements = new byte[countElements][];
                        int lenStr;
                        someTexData = false;

                        tmp = new byte[8];
                        Array.Copy(binContent, poz, tmp, 0, tmp.Length);

                        if ((BitConverter.ToString(tmp) == "81-53-37-63-9E-4A-3A-9A") && (countElements == 1) && (Encoding.ASCII.GetString(check_header) == "ERTM"))
                        {
                            MessageBox.Show("This font is empty!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            font = null;
                            GC.Collect();
                            edited = false;
                            return;
                        }

                        if (BitConverter.ToString(tmp) == "81-53-37-63-9E-4A-3A-9A")
                        {
                            if((countElements == 1) && (Encoding.ASCII.GetString(check_header) == "6VSM"))
                            {
                                MessageBox.Show("This font is a vector font. Try use Auto (De)Packer.");
                                font = null;
                                GC.Collect();
                                edited = false;
                                return;
                            }

                            for (int i = 0; i < countElements; i++)
                            {
                                font.binElements[i] = new byte[8];
                                Array.Copy(binContent, poz, font.binElements[i], 0, font.binElements[i].Length);
                                poz += 12;

                                switch (BitConverter.ToString(font.binElements[i]))
                                {
                                    case "41-16-D7-79-B9-3C-28-84":
                                        fontFlags = new FlagsClass();
                                        break;

                                    case "E3-88-09-7A-48-5D-7F-93":
                                        someTexData = true;
                                        font.hasScaleValue = true;
                                        break;

                                    case "0F-F4-20-E6-20-BA-A1-EF":
                                        font.NewFormat = true;
                                        break;

                                    case "7A-BA-6E-87-89-88-6C-FA":
                                        AddInfo = true;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < countElements; i++)
                            {
                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                poz += 4;
                                lenStr = BitConverter.ToInt32(tmp, 0);
                                tmp = new byte[lenStr];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                poz += lenStr + 4; //Length element's name and 4 bytes data for Telltale Tool
                                font.elements[i] = Encoding.ASCII.GetString(tmp);

                                if (font.elements[i] == "class Flags")
                                {
                                    fontFlags = new FlagsClass();
                                }
                            }
                        }

                        tmpHeader = new byte[poz];
                        Array.Copy(binContent, 0, tmpHeader, 0, tmpHeader.Length);

                        tmp = new byte[4];
                        Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                        int nameLen = BitConverter.ToInt32(tmp, 0);
                        poz += 4;

                        tmp = new byte[4];
                        Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                        if (nameLen - BitConverter.ToInt32(tmp, 0) == 8)
                        {
                            nameLen = BitConverter.ToInt32(tmp, 0);
                            poz += 4;
                            font.blockSize = true;
                        }

                        tmp = new byte[nameLen];
                        Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                        font.FontName = Encoding.ASCII.GetString(tmp);
                        poz += nameLen;

                        font.One = binContent[poz];
                        poz++;

                        //Temporary solution
                        if ((font.One == 0x31 && (Encoding.ASCII.GetString(check_header) == "5VSM"))
                            || (Encoding.ASCII.GetString(check_header) == "6VSM"))
                        {
                            tmp = new byte[4];
                            Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                            poz += 4;

                            font.NewSomeValue = BitConverter.ToSingle(tmp, 0);
                        }

                        tmp = new byte[4];
                        Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                        poz += 4;
                        font.BaseSize = BitConverter.ToSingle(tmp, 0);

                        tmp = new byte[4];
                        Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                        font.halfValue = 0.0f;
                        font.lineHeight = 0.0f;
                        font.feedFace = null;
                        font.hasLineHeight = false;

                        if(BitConverter.ToString(tmp) == "CE-FA-ED-FE")
                        {
                            font.feedFace = new byte[4];
                            Array.Copy(binContent, poz, font.feedFace, 0, font.feedFace.Length);
                            poz += 4;
                            tmp = new byte[4];
                            Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                        }

                        if (font.hasScaleValue && Encoding.ASCII.GetString(header) == "5VSM")
                        {
                            //Check for Back to the Future for PS4

                            int tmpPos = poz;
                            tmp = new byte[4];
                            Array.Copy(binContent, tmpPos + 12, tmp, 0, tmp.Length);
                            int checkBlockSize = BitConverter.ToInt32(tmp, 0);

                            tmp = new byte[4];
                            Array.Copy(binContent, tmpPos + 16, tmp, 0, tmp.Length);
                            int checkCharCount = BitConverter.ToInt32(tmp, 0);

                            if ((checkCharCount * (4 * 12)) + 8 == checkBlockSize)
                            {
                                font.hasLineHeight = true;
                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                poz += 4;
                                font.lineHeight = BitConverter.ToSingle(tmp, 0);

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                            }
                            else
                            {
                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                            }
                        }

                        if ((BitConverter.ToSingle(tmp, 0) == 0.5)
                            || (BitConverter.ToSingle(tmp, 0) == 1.0))
                        {
                            font.halfValue = BitConverter.ToSingle(tmp, 0);
                            poz += 4;
                        }

                        if (font.hasScaleValue)
                        {
                            //very strange check method about 1.0f value 
                            int tmp_poz = poz;
                            tmp = new byte[4];
                            Array.Copy(binContent, tmp_poz, tmp, 0, tmp.Length);
                            font.glyph.BlockCoordSize = BitConverter.ToInt32(tmp, 0);
                            tmp_poz += 4;

                            tmp = new byte[4];
                            Array.Copy(binContent, tmp_poz, tmp, 0, tmp.Length);
                            font.glyph.CharCount = BitConverter.ToInt32(tmp, 0);
                            tmp_poz += 4;

                            //check if it size of chars + 8 bytes of block size and count of characters
                            if ((font.glyph.CharCount * (4 * 12)) + 8 != font.glyph.BlockCoordSize)
                            {
                                font.glyph.BlockCoordSize = 0;
                                font.glyph.CharCount = 0;
                                font.hasOneFloatValue = true;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);

                                font.oneValue = BitConverter.ToSingle(tmp, 0);
                                poz += 4;
                            }
                        }

                        font.glyph.BlockCoordSize = 0;

                        if (font.blockSize)
                        {
                            tmp = new byte[4];
                            Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                            font.glyph.BlockCoordSize = BitConverter.ToInt32(tmp, 0);
                            poz += 4;
                        }

                        tmp = new byte[4];
                        Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                        font.glyph.CharCount = BitConverter.ToInt32(tmp, 0);
                        poz += 4;

                        if (!font.NewFormat)
                        {
                            font.glyph.chars = new FontClass.ClassFont.TRect[font.glyph.CharCount];
                            font.glyph.charsNew = null;

                            for (int i = 0; i < font.glyph.CharCount; i++)
                            {
                                font.glyph.chars[i] = new FontClass.ClassFont.TRect();

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.chars[i].TexNum = BitConverter.ToInt32(tmp, 0);
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.chars[i].XStart = BitConverter.ToSingle(tmp, 0);
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.chars[i].XEnd = BitConverter.ToSingle(tmp, 0);
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.chars[i].YStart = BitConverter.ToSingle(tmp, 0);
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.chars[i].YEnd = BitConverter.ToSingle(tmp, 0);
                                poz += 4;

                                if (font.hasScaleValue)
                                {
                                    tmp = new byte[4];
                                    Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                    font.glyph.chars[i].CharWidth = (float)Math.Round(BitConverter.ToSingle(tmp, 0));
                                    poz += 4;

                                    tmp = new byte[4];
                                    Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                    font.glyph.chars[i].CharHeight = (float)Math.Round(BitConverter.ToSingle(tmp, 0));
                                    poz += 4;
                                }
                            }
                        }
                        else
                        {
                            font.glyph.chars = null;
                            font.glyph.charsNew = new ClassesStructs.FontClass.ClassFont.TRectNew[font.glyph.CharCount];

                            for (int i = 0; i < font.glyph.CharCount; i++)
                            {
                                font.glyph.charsNew[i] = new FontClass.ClassFont.TRectNew();

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].charId = BitConverter.ToUInt32(tmp, 0);
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].TexNum = BitConverter.ToInt32(tmp, 0);
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].Channel = BitConverter.ToInt32(tmp, 0);
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].XStart = BitConverter.ToSingle(tmp, 0);
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].XEnd = BitConverter.ToSingle(tmp, 0);
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].YStart = BitConverter.ToSingle(tmp, 0);
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].YEnd = BitConverter.ToSingle(tmp, 0);
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].CharWidth = (float)Math.Round(BitConverter.ToSingle(tmp, 0));
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].CharHeight = (float)Math.Round(BitConverter.ToSingle(tmp, 0));
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].XOffset = (float)Math.Round(BitConverter.ToSingle(tmp, 0));
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].YOffset = (float)Math.Round(BitConverter.ToSingle(tmp, 0));
                                poz += 4;

                                tmp = new byte[4];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.glyph.charsNew[i].XAdvance = (float)Math.Round(BitConverter.ToSingle(tmp, 0));
                                poz += 4;
                            }
                        }

                        if (font.blockSize)
                        {
                            tmp = new byte[4];
                            Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                            font.BlockTexSize = BitConverter.ToInt32(tmp, 0);
                            poz += 4;
                        }

                        tmp = new byte[4];
                        Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                        font.TexCount = BitConverter.ToInt32(tmp, 0);
                        poz += 4;

                        if (!font.NewFormat)
                        {
                            font.tex = new TextureClass.OldT3Texture[font.TexCount];
                            font.NewTex = null;

                            for (int i = 0; i < font.TexCount; i++)
                            {
                                font.tex[i] = Graphics.TextureWorker.GetOldTextures(binContent, ref poz, fontFlags != null, someTexData);
                                if (font.tex[i] == null)
                                {
                                    MessageBox.Show("Maybe unsupported font.", "Error");
                                    return;
                                }
                            }

                            for (int k = 0; k < font.glyph.CharCount; k++)
                            {
                                font.glyph.chars[k].XStart *= font.tex[font.glyph.chars[k].TexNum].Width;
                                font.glyph.chars[k].XStart = (float)Math.Round(font.glyph.chars[k].XStart);
                                font.glyph.chars[k].XEnd *= font.tex[font.glyph.chars[k].TexNum].Width;
                                font.glyph.chars[k].XEnd = (float)Math.Round(font.glyph.chars[k].XEnd);

                                font.glyph.chars[k].YStart *= font.tex[font.glyph.chars[k].TexNum].Height;
                                font.glyph.chars[k].YStart = (float)Math.Round(font.glyph.chars[k].YStart);
                                font.glyph.chars[k].YEnd *= font.tex[font.glyph.chars[k].TexNum].Height;
                                font.glyph.chars[k].YEnd = (float)Math.Round(font.glyph.chars[k].YEnd);
                            }
                        }
                        else
                        {
                            font.tex = null;
                            font.NewTex = new TextureClass.NewT3Texture[font.TexCount];
                            string format = "";
                            uint tmpPosition = 0;

                            if (font.headerSize != 0)
                            {
                                tmpPosition = (uint)font.headerSize + 16 + ((uint)countElements * 12) + 4;
                            }

                            for (int i = 0; i < font.TexCount; i++)
                            {
                                font.NewTex[i] = Graphics.TextureWorker.GetNewTextures(binContent, ref poz, ref tmpPosition, fontFlags != null, someTexData, true, ref format, AddInfo);

                                if (font.NewTex[i] == null)
                                {
                                    MessageBox.Show("Maybe unsupported font.", "Error");
                                    return;
                                }
                            }

                            if(font.NewTex[0].SomeValue > 4)
                            {
                                tmp = new byte[1];
                                Array.Copy(binContent, poz, tmp, 0, tmp.Length);
                                font.LastZero = tmp[0];
                                poz++;
                            }

                            for (int k = 0; k < font.glyph.CharCount; k++)
                            {
                                font.glyph.charsNew[k].XStart *= font.NewTex[font.glyph.charsNew[k].TexNum].Width;
                                font.glyph.charsNew[k].XStart = (float)Math.Round(font.glyph.charsNew[k].XStart);
                                font.glyph.charsNew[k].XEnd *= font.NewTex[font.glyph.charsNew[k].TexNum].Width;
                                font.glyph.charsNew[k].XEnd = (float)Math.Round(font.glyph.charsNew[k].XEnd);

                                font.glyph.charsNew[k].YStart *= font.NewTex[font.glyph.charsNew[k].TexNum].Height;
                                font.glyph.charsNew[k].YStart = (float)Math.Round(font.glyph.charsNew[k].YStart);
                                font.glyph.charsNew[k].YEnd *= font.NewTex[font.glyph.charsNew[k].TexNum].Height;
                                font.glyph.charsNew[k].YEnd = (float)Math.Round(font.glyph.charsNew[k].YEnd);
                            }
                        }

                        fillTableofCoordinates(font, false);
                        fillTableofTextures(font);

                        saveToolStripMenuItem.Enabled = true;
                        saveAsToolStripMenuItem.Enabled = true;
                        exportCoordinatesToolStripMenuItem1.Enabled = true;
                        rbKerning.Enabled = font.NewFormat;
                        rbNoKerning.Enabled = font.NewFormat;
                        edited = false;
                        FileInfo fi = new FileInfo(FileName);
                        if(Form.ActiveForm != null) Form.ActiveForm.Text = "Font Editor. Opened file " + fi.Name;

                    }
                    catch(Exception ex)
                    {
                        binContent = null;
                        GC.Collect();
                        MessageBox.Show("Unknown error: " + ex.Message);
                    }
                }
        }

}

        public int FindStartOfStringSomething(byte[] array, int offset, string string_something)
        {
            int poz = offset;
            while (Methods.ConvertHexToString(array, poz, string_something.Length, MainMenu.settings.ASCII_N, 1) != string_something)
            {
                poz++;
                if (Methods.ConvertHexToString(array, poz, string_something.Length, MainMenu.settings.ASCII_N, 1) == string_something)
                {
                    return poz;
                }
                if ((poz + string_something.Length + 1) > array.Length)
                {
                    break;
                }
            }
            return poz;
        }


        private void encFunc(string path) //Encrypts full font
        {
            if (encrypted == true) //Ask about a full enryption if you don't want build archive
            {
                if (MessageBox.Show("Do you want to make a full encryption?", "About encrypted font...",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FileStream fs = new FileStream(path, FileMode.Open);
                    byte[] fontContent = Methods.ReadFull(fs);
                    fs.Close();

                    Methods.meta_crypt(fontContent, encKey, version, false);

                    if (File.Exists(path)) File.Delete(path);
                    fs = new FileStream(path, FileMode.Create);
                    fs.Write(fontContent, 0, fontContent.Length);
                    fs.Close();
                }

            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!edited) return;
            Methods.DeleteCurrentFile(ofd.FileName);

            FileStream fs = new FileStream(ofd.FileName, FileMode.OpenOrCreate);
            SaveFont(fs, font);
            fs.Close();

            encFunc(ofd.FileName);
            fillTableofCoordinates(font, false);
            edited = false; //After saving return trigger to FALSE
        }

        private void SaveFont(Stream fs, ClassesStructs.FontClass.ClassFont font)
        {
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(tmpHeader);
            
            //First need check textures import
            font.texSize = 0;
            font.headerSize = 0;

            int len = Encoding.ASCII.GetBytes(font.FontName).Length;
            font.headerSize += 4;

            if (font.blockSize)
            {
                int subLen = len + 8;
                font.headerSize += 4;
                bw.Write(subLen);
            }

            bw.Write(len);
            bw.Write(Encoding.ASCII.GetBytes(font.FontName));
            font.headerSize += len;

            bw.Write(font.One);
            font.headerSize++;

            if ((font.One == 0x31 && (Encoding.ASCII.GetString(check_header) == "5VSM"))
                        || (Encoding.ASCII.GetString(check_header) == "6VSM"))
            {
                bw.Write(font.NewSomeValue);
                font.headerSize += 4;
            }

            bw.Write(font.BaseSize);
            font.headerSize += 4;

            if(font.feedFace != null)
            {
                bw.Write(font.feedFace);
                font.headerSize += 4;
            }

            if(Encoding.ASCII.GetString(check_header) == "5VSM"
                && font.hasLineHeight)
            {
                bw.Write(font.lineHeight);
                font.headerSize += 4;
            }

            if(font.halfValue == 0.5f || font.halfValue == 1.0f)
            {
                bw.Write(font.halfValue);
                font.headerSize += 4;
            }

            if (font.hasScaleValue && font.hasOneFloatValue)
            {
                bw.Write(font.oneValue);
                font.headerSize += 4;
            }

            if (font.blockSize)
            {
                if (!font.NewFormat)
                {
                    font.glyph.BlockCoordSize = font.glyph.CharCount * (5 * 4);

                    if (font.hasScaleValue) font.glyph.BlockCoordSize = font.glyph.CharCount * (7 * 4);

                    font.glyph.BlockCoordSize += 4; //Includes char count block
                }
                else
                {
                    font.glyph.BlockCoordSize = font.glyph.CharCount * (12 * 4);
                    font.glyph.BlockCoordSize += 4; //Includes char count block
                }

                font.glyph.BlockCoordSize += 4; //And block size itself

                bw.Write(font.glyph.BlockCoordSize);
                font.headerSize += 4;
            }

            bw.Write(font.glyph.CharCount);
            font.headerSize += 4;

            if (!font.NewFormat)
            {
                for (int i = 0; i < font.glyph.CharCount; i++)
                {
                    bw.Write(font.glyph.chars[i].TexNum);
                    bw.Write(font.glyph.chars[i].XStart / font.tex[font.glyph.chars[i].TexNum].OriginalWidth);
                    bw.Write(font.glyph.chars[i].XEnd / font.tex[font.glyph.chars[i].TexNum].OriginalWidth);
                    bw.Write(font.glyph.chars[i].YStart / font.tex[font.glyph.chars[i].TexNum].OriginalHeight);
                    bw.Write(font.glyph.chars[i].YEnd / font.tex[font.glyph.chars[i].TexNum].OriginalHeight);

                    if (font.hasScaleValue)
                    {
                        bw.Write(font.glyph.chars[i].CharWidth);
                        bw.Write(font.glyph.chars[i].CharHeight);
                    }
                }

                if (font.blockSize)
                {
                    font.BlockTexSize = 0;

                    for (int j = 0; j < font.TexCount; j++)
                    {
                        font.BlockTexSize += font.tex[j].BlockPos + font.tex[j].TexSize;
                    }

                    font.BlockTexSize += 8; //4 bytes of block size and 4 bytes of block (if it empty)

                    bw.Write(font.BlockTexSize);
                }

                bw.Write(font.TexCount);

                for (int i = 0; i < font.TexCount; i++)
                {
                    Graphics.TextureWorker.ReplaceOldTextures(fs, font.tex[i], someTexData, encrypted, encKey, version);
                }
            }
            else
            {
                for (int i = 0; i < font.glyph.CharCount; i++)
                {
                    bw.Write(font.glyph.charsNew[i].charId);
                    bw.Write(font.glyph.charsNew[i].TexNum);
                    bw.Write(font.glyph.charsNew[i].Channel);

                    var xSt = font.glyph.charsNew[i].XStart / font.NewTex[font.glyph.charsNew[i].TexNum].Width;
                    bw.Write(xSt);
                    var xEn = font.glyph.charsNew[i].XEnd / font.NewTex[font.glyph.charsNew[i].TexNum].Width;
                    bw.Write(xEn);
                    var ySt = font.glyph.charsNew[i].YStart / font.NewTex[font.glyph.charsNew[i].TexNum].Height;
                    bw.Write(ySt);
                    var yEn = font.glyph.charsNew[i].YEnd / font.NewTex[font.glyph.charsNew[i].TexNum].Height;
                    bw.Write(yEn);

                    bw.Write(font.glyph.charsNew[i].CharWidth);
                    bw.Write(font.glyph.charsNew[i].CharHeight);
                    bw.Write(font.glyph.charsNew[i].XOffset);
                    bw.Write(font.glyph.charsNew[i].YOffset);
                    bw.Write(font.glyph.charsNew[i].XAdvance);

                    font.headerSize += (4 * 12);
                }

                font.BlockTexSize = 0;
                font.texSize = 0;

                for (int i = 0; i < font.TexCount; i++)
                {
                    font.BlockTexSize += font.NewTex[i].Tex.headerSize;
                    font.headerSize += font.NewTex[i].Tex.headerSize;

                    for (int k = 0; k < font.NewTex[i].Mip; k++)
                    {
                        switch (Encoding.ASCII.GetString(check_header)) {
                            case "ERTM":
                                font.BlockTexSize += font.NewTex[i].Tex.Textures[k].MipSize;
                                break;

                            default:
                                font.texSize += (uint)font.NewTex[i].Tex.Textures[k].MipSize;
                                break;
                        }
                    }
                }

                font.BlockTexSize += 8; //4 bytes of block size and 4 bytes of block (if it empty)
                font.headerSize += 8;

                bw.Write(font.BlockTexSize);
                bw.Write(font.TexCount);

                int c = 1;

                if (Encoding.ASCII.GetString(check_header) == "ERTM")
                {
                    for (int i = 0; i < font.TexCount; i++) {
                        Graphics.TextureWorker.ReplaceNewTextures(fs, c, Encoding.ASCII.GetString(check_header), font.NewTex[i], true);
                    }
                }
                else
                {
                    if (font.NewTex[0].SomeValue > 4) font.headerSize++; //add 0x00 byte

                    for(c = 2; c < 4; c++)
                    {
                        for(int i = 0; i < font.TexCount; i++)
                        {
                            Graphics.TextureWorker.ReplaceNewTextures(fs, c, Encoding.ASCII.GetString(check_header), font.NewTex[i], true);
                        }

                        if (font.NewTex[0].SomeValue > 4 && c == 2) bw.Write(font.LastZero);
                    }

                    bw.BaseStream.Seek(4, SeekOrigin.Begin);
                    bw.Write(font.headerSize);
                    bw.BaseStream.Seek(12, SeekOrigin.Begin);
                    bw.Write(font.texSize);
                }
                
            }

            bw.Close();
            fs.Close();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "(" + textBox8.Text.Length.ToString() + ")";
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            label2.Text = "(" + textBox9.Text.Length.ToString() + ")";
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBox8.Text = "";
            textBox9.Text = "";
            label1.Text = "(0)";
            label2.Text = "(0)";
            checkBox1.Checked = true;
            checkBox2.Checked = true;

        }

        private void buttonCopyCoordinates_Click(object sender, EventArgs e)
        {
            string ch1 = textBox8.Text;
            string ch2 = textBox9.Text;
            if (ch1.Length == ch2.Length)
            {
                for (int i = 0; i < ch1.Length; i++)
                {
                    int f = Convert.ToInt32(ASCIIEncoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(ch1[i].ToString())[0]);
                    int s = Convert.ToInt32(ASCIIEncoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(ch2[i].ToString())[0]);
                    int first = 0;
                    int second = 0;
                    for (int j = 0; j < dataGridViewWithCoord.RowCount; j++)
                    {
                        if (Convert.ToInt32(dataGridViewWithCoord[0, j].Value) == f)
                        {
                            first = j;
                        }
                        if (Convert.ToInt32(dataGridViewWithCoord[0, j].Value) == s)
                        {
                            second = j;
                        }
                    }


                    CopyDataIndataGridViewWithCoord(6, first, second);
                    CopyDataIndataGridViewWithCoord(7, first, second);
                    CopyDataIndataGridViewWithCoord(8, first, second);
                    CopyDataIndataGridViewWithCoord(9, first, second);
                    CopyDataIndataGridViewWithCoord(10, first, second);
                    CopyDataIndataGridViewWithCoord(11, first, second);
                    CopyDataIndataGridViewWithCoord(12, first, second);

                    if (checkBox1.Checked == true)
                    {
                        CopyDataIndataGridViewWithCoord(2, first, second);
                        CopyDataIndataGridViewWithCoord(3, first, second);
                    }
                    if (checkBox2.Checked == true)
                    {
                        CopyDataIndataGridViewWithCoord(4, first, second);
                        CopyDataIndataGridViewWithCoord(5, first, second);
                    }
                }
            }
            else if (ch1.Length == 1)
            {
                for (int i = 0; i < ch2.Length; i++)
                {
                    int f = Convert.ToInt32(ASCIIEncoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(ch1[i].ToString())[0]);
                    int s = Convert.ToInt32(ASCIIEncoding.GetEncoding(MainMenu.settings.ASCII_N).GetBytes(ch2[i].ToString())[0]);
                    int first = 0;
                    int second = 0;
                    for (int j = 0; j < dataGridViewWithCoord.RowCount; j++)
                    {
                        if (Convert.ToInt32(dataGridViewWithCoord[0, j].Value) == f)
                        {
                            first = j;
                        }
                        if (Convert.ToInt32(dataGridViewWithCoord[0, j].Value) == s)
                        {
                            second = j;
                        }
                    }

                    CopyDataIndataGridViewWithCoord(6, first, second);
                    CopyDataIndataGridViewWithCoord(7, first, second);
                    CopyDataIndataGridViewWithCoord(8, first, second);
                    CopyDataIndataGridViewWithCoord(9, first, second);
                    CopyDataIndataGridViewWithCoord(10, first, second);
                    CopyDataIndataGridViewWithCoord(11, first, second);
                    CopyDataIndataGridViewWithCoord(12, first, second);

                    if (checkBox1.Checked == true)
                    {
                        CopyDataIndataGridViewWithCoord(2, first, second);
                        CopyDataIndataGridViewWithCoord(3, first, second);
                    }
                    if (checkBox2.Checked == true)
                    {
                        CopyDataIndataGridViewWithCoord(4, first, second);
                        CopyDataIndataGridViewWithCoord(5, first, second);
                    }
                }
            }
        }

        private void CopyDataIndataGridViewWithCoord(int column, int first, int second)
        {
            dataGridViewWithCoord[column, second].Value = dataGridViewWithCoord[column, first].Value;
            dataGridViewWithCoord[column, second].Style.BackColor = System.Drawing.Color.Green;
        }

        private void contextMenuStripExport_Import_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewWithTextures.Rows.Count > 0)
            {
                if (dataGridViewWithTextures.SelectedCells[0].RowIndex >= 0)
                {
                    exportToolStripMenuItem.Enabled = true;
                    importDDSToolStripMenuItem.Enabled = true;
                }
                else
                {
                    exportToolStripMenuItem.Enabled = false;
                    importDDSToolStripMenuItem.Enabled = false;
                    exportCoordinatesToolStripMenuItem1.Enabled = false;
                    toolStripImportFNT.Enabled = false;
                }
            }
        }

        private void dataGridViewWithTextures_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            dataGridViewWithTextures.Rows[e.RowIndex].Selected = true;
            MessageBox.Show(dataGridViewWithTextures.Rows[e.RowIndex].Selected.ToString());
        }

        private void dataGridViewWithTextures_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridViewWithTextures.Rows[e.RowIndex].Selected = true;
            }
            if (e.Button == MouseButtons.Left && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // получаем координаты
                Point pntCell = dataGridViewWithTextures.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                pntCell.X += e.Location.X;
                pntCell.Y += e.Location.Y;

                // вызываем менюшку
                contextMenuStripExport_Import.Show(dataGridViewWithTextures, pntCell);
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int file_n = dataGridViewWithTextures.SelectedCells[0].RowIndex;
            SaveFileDialog saveFD = new SaveFileDialog();
            if ((font.tex != null && font.tex[file_n].isIOS) || (font.NewTex != null && font.NewTex[file_n].isPVR))
            {
                saveFD.Filter = "PVR files (*.pvr)|*.pvr";
                saveFD.FileName = font.FontName + "_" + file_n.ToString() + ".pvr";
            }
            else
            {
                saveFD.Filter = "dds files (*.dds)|*.dds";
                saveFD.FileName = font.FontName + "_" + file_n.ToString() + ".dds";
            }

            if (saveFD.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(saveFD.FileName, FileMode.Create);
                Methods.DeleteCurrentFile(saveFD.FileName);

                switch (font.NewFormat)
                {
                    case true:
                        fs.Write(font.NewTex[file_n].Tex.Content, 0, font.NewTex[file_n].Tex.Content.Length);
                        break;

                    default:
                        fs.Write(font.tex[file_n].Content, 0, font.tex[file_n].Content.Length);
                        break;
                }

                fs.Close();
            }
        }

        private void importDDSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int file_n = dataGridViewWithTextures.SelectedCells[0].RowIndex;
            OpenFileDialog openFD = new OpenFileDialog();

            openFD.Filter = "dds files (*.dds)|*.dds";


            if (openFD.ShowDialog() == DialogResult.OK)
            {
                if (font.NewFormat) ReplaceTexture(openFD.FileName, font.NewTex[file_n]);
                else ReplaceTexture(openFD.FileName, font.tex[file_n]);

                fillTableofTextures(font);
                edited = true; //Отмечаем, что шрифт изменился
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFD = new SaveFileDialog();
            saveFD.Filter = "font files (*.font)|*.font";
            saveFD.FileName = ofd.SafeFileName.ToString();
            if (saveFD.ShowDialog() == DialogResult.OK)
            {
                Methods.DeleteCurrentFile((saveFD.FileName));
                FileStream fs = new FileStream((saveFD.FileName), FileMode.OpenOrCreate);
                SaveFont(fs, font);
                fs.Close();

                encFunc(saveFD.FileName);

                edited = false; //Файл сохранили, так что вернули флаг на ЛОЖЬ
            }
        }

        private void FontEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (edited == true)
            {
                DialogResult status = MessageBox.Show("Save font before closing Font Editor?", "Exit", MessageBoxButtons.YesNoCancel);
                if (status == DialogResult.Cancel)
                // если (состояние == DialogResult.Отмена) 
                {
                    e.Cancel = true; // Отмена = истина 
                }
                else if (status == DialogResult.Yes) //Если (состояние == DialogResult.Да)
                {
                    FileStream fs = new FileStream(ofd.SafeFileName, FileMode.Create); //Сохраняем в открытый файл.
                    SaveFont(fs, font);
                    //После соханения чистим списки
                }
                else //А иначе просто закрываем программу и чистим списки
                {
                }
            }
        }

        private void dataGridViewWithCoord_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int end_edit_column = e.ColumnIndex;
            int end_edit_row = e.RowIndex;
            bool success = false;
            if (old_data != "")
            {
                if ((end_edit_column >= 2 && end_edit_column <= dataGridViewWithCoord.ColumnCount) && Methods.IsNumeric(dataGridViewWithCoord[end_edit_column, end_edit_row].Value.ToString()))
                {
                    if (dataGridViewWithCoord[end_edit_column, end_edit_row].Value.ToString() != old_data)
                    {
                        if (end_edit_column == 2 || end_edit_column == 3) //X
                        {
                            dataGridViewWithCoord[7, end_edit_row].Value = (Convert.ToInt32(dataGridViewWithCoord[3, end_edit_row].Value) - Convert.ToInt32(dataGridViewWithCoord[2, end_edit_row].Value));
                            success = true;

                        }
                        else if (end_edit_column == 4 || end_edit_column == 5) //Y
                        {
                            dataGridViewWithCoord[8, end_edit_row].Value = (Convert.ToInt32(dataGridViewWithCoord[5, end_edit_row].Value) - Convert.ToInt32(dataGridViewWithCoord[4, end_edit_row].Value));
                            success = true;
                        }
                        else if (end_edit_column == 6) //dds
                        {
                            success = true;
                            if (Convert.ToInt32(dataGridViewWithCoord[end_edit_column, end_edit_row].Value) >= dataGridViewWithTextures.RowCount)
                            {
                                dataGridViewWithCoord[end_edit_column, end_edit_row].Value = old_data;
                                success = false;
                            }
                        }
                        else if (end_edit_column > 6 && end_edit_column < 8)
                        {
                            dataGridViewWithCoord[end_edit_column, end_edit_row].Value = old_data;
                        }
                    }
                }
                else
                {
                    dataGridViewWithCoord[end_edit_column, end_edit_row].Value = old_data;
                }
            }
            if(success)
            {
                dataGridViewWithCoord[end_edit_column,end_edit_row].Style.BackColor = Color.DarkCyan;
                if (!font.NewFormat) {
                    float.TryParse(dataGridViewWithCoord[2, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].XStart);
                    float.TryParse(dataGridViewWithCoord[3, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].XEnd);
                    float.TryParse(dataGridViewWithCoord[4, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].YStart);
                    float.TryParse(dataGridViewWithCoord[5, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].YEnd);
                    int.TryParse(dataGridViewWithCoord[6, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].TexNum);

                    if (font.hasScaleValue)
                    {
                        float.TryParse(dataGridViewWithCoord[7, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].CharWidth);
                        float.TryParse(dataGridViewWithCoord[8, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].CharHeight);
                    }
                }
                else
                {
                   
                    float.TryParse(dataGridViewWithCoord[4, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].YStart);
                    float.TryParse(dataGridViewWithCoord[5, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].YEnd);
                    int.TryParse(dataGridViewWithCoord[6, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].TexNum);
                    float.TryParse(dataGridViewWithCoord[7, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].CharWidth);
                    float.TryParse(dataGridViewWithCoord[8, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].CharHeight);
                    float.TryParse(dataGridViewWithCoord[9, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].XOffset);
                    float.TryParse(dataGridViewWithCoord[10, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].YOffset);
                    float.TryParse(dataGridViewWithCoord[11, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].XAdvance);
                    int.TryParse(dataGridViewWithCoord[12, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].Channel);
                }
            }
            if (!edited && success)
            {
                edited = success;
            }

        }
        public static string old_data;

        private void dataGridViewWithCoord_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int now_edit_column = e.ColumnIndex;
            int now_edit_row = e.RowIndex;
            old_data = dataGridViewWithCoord[now_edit_column, now_edit_row].Value.ToString();
        }

        private void dataGridViewWithCoord_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridViewWithCoord.Rows[e.RowIndex].Selected = true;
            }
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // получаем координаты
                Point pntCell = dataGridViewWithCoord.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                pntCell.X += e.Location.X;
                pntCell.Y += e.Location.Y;

                // вызываем менюшку
                contextMenuStripExp_imp_Coord.Show(dataGridViewWithCoord, pntCell);
            }
        }

        private void exportCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportCoordinatesToolStripMenuItem1_Click(sender, e);
        }

        private void exportCoordinatesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "FNT file (*.fnt) | *.fnt";
            sfd.FileName = font.FontName + ".fnt";

            if(sfd.ShowDialog() == DialogResult.OK)
            {
                string info = "info face=\"" + font.FontName + "\" size=" + font.BaseSize + " bold=0 italic=0 charset=\"\" unicode=";
                switch (font.NewFormat)
                {
                    case true:
                        info += "1\r\n";
                        break;

                    default:
                        info += "0\r\n";
                        break;
                }

                info += "common lineHeight=" + font.BaseSize;

                if ((font.One == 0x31 && (Encoding.ASCII.GetString(check_header) == "5VSM"))
                        || (Encoding.ASCII.GetString(check_header) == "6VSM"))
                {
                    info += " base=" + font.NewSomeValue;
                }
                else info += " base=" + font.BaseSize;
                
                info += " pages=" + font.TexCount + "\r\n";

                if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);
                FileStream fs = new FileStream(sfd.FileName, FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(info);
                info = "";

                for(int i = 0; i < font.TexCount; i++)
                {
                    info = "page id=" + i + " file=\"" + font.FontName + "_" + i + ".dds\"\r\n";
                    sw.Write(info);
                }

                info = "chars count=" + font.glyph.CharCount + "\r\n";
                sw.Write(info);

                if (!font.NewFormat)
                {
                    for(int i = 0; i < font.glyph.CharCount; i++)
                    {
                        info = "char id=" + i + " x=" + font.glyph.chars[i].XStart + " y=" + font.glyph.chars[i].YStart;
                        info += " width=";

                        if (font.hasScaleValue)
                        {
                            info += font.glyph.chars[i].CharWidth;
                        }
                        else
                        {
                            info += font.glyph.chars[i].XEnd - font.glyph.chars[i].XStart;
                        }

                        info += " height=";

                        if (font.hasScaleValue)
                        {
                            info += font.glyph.chars[i].CharHeight;
                        }
                        else
                        {
                            info += font.glyph.chars[i].YEnd - font.glyph.chars[i].YStart;
                        }

                        info += " xoffset=0 yoffset=0 xadvance=";

                        if (font.hasScaleValue)
                        {
                            info += font.glyph.chars[i].CharWidth;
                        }
                        else
                        {
                            info += font.glyph.chars[i].XEnd - font.glyph.chars[i].XStart;
                        }

                        info += " page=" + font.glyph.chars[i].TexNum + " chnl=15\r\n";

                        sw.Write(info);
                    }
                }
                else
                {
                    for (int i = 0; i < font.glyph.CharCount; i++)
                    {
                        info = "char id=" + font.glyph.charsNew[i].charId + " x=" + font.glyph.charsNew[i].XStart + " y=" + font.glyph.charsNew[i].YStart;
                        info += " width=" + font.glyph.charsNew[i].CharWidth + " height=" + font.glyph.charsNew[i].CharHeight;
                        info += " xoffset=" + font.glyph.charsNew[i].XOffset + " yoffset=" + font.glyph.charsNew[i].YOffset + " xadvance=";
                        info += font.glyph.charsNew[i].XAdvance + " page=" + font.glyph.charsNew[i].TexNum + " chnl=" + font.glyph.charsNew[i].Channel + "\r\n";

                        sw.Write(info);
                    }
                }

                sw.Close();
                fs.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Methods.IsNumeric(textBox1.Text))
            {
                int w = Convert.ToInt32(textBox1.Text);
                for (int i = 0; i < dataGridViewWithCoord.RowCount; i++)
                {
                    if (radioButtonXend.Checked)
                    {
                        dataGridViewWithCoord[3, i].Value = Convert.ToInt32(dataGridViewWithCoord[3, i].Value) + w;
                    }
                    else
                    {
                        dataGridViewWithCoord[2, i].Value = Convert.ToInt32(dataGridViewWithCoord[2, i].Value) + w;
                    }
                    dataGridViewWithCoord[7, i].Value = Convert.ToInt32(dataGridViewWithCoord[7, i].Value) + w;
                    dataGridViewWithCoord[12, i].Value = Convert.ToInt32(dataGridViewWithCoord[12, i].Value) + w;
                }
            }
        }

        private void toolStripImportFNT_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFD = new OpenFileDialog();
            openFD.Filter = "fnt files (*.fnt)|*.fnt";

            if (openFD.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(openFD.FileName);

                string[] strings = File.ReadAllLines(fi.FullName);

                int ch = -1;

                //Check for xml tags and removing it for comfortable searching needed data (useful for xml fnt files)
                for (int n = 0; n < strings.Length; n++)
                {
                    if ((strings[n].IndexOf('<') >= 0) || (strings[n].IndexOf('<') >= 0 && strings[n].IndexOf('/') > 0))
                    {
                        strings[n] = strings[n].Remove(strings[n].IndexOf('<'), 1);
                        if (strings[n].IndexOf('/') >= 0) strings[n] = strings[n].Remove(strings[n].IndexOf('/'), 1);
                    }
                    if (strings[n].IndexOf('>') >= 0 || (strings[n].IndexOf('/') >= 0 && strings[n + 1].IndexOf('>') > 0))
                    {
                        strings[n] = strings[n].Remove(strings[n].IndexOf('>'), 1);
                        if (strings[n].IndexOf('/') >= 0) strings[n] = strings[n].Remove(strings[n].IndexOf('/'), 1);
                    }
                    if (strings[n].IndexOf('"') >= 0)
                    {
                        while (strings[n].IndexOf('"') >= 0) strings[n] = strings[n].Remove(strings[n].IndexOf('"'), 1);
                    }
                }

                if (font.NewFormat)
                {
                    TextureClass.NewT3Texture[] tmpNewTex = null;

                    for (int m = 0; m < strings.Length; m++)
                    {
                        if (strings[m].ToLower().Contains("common lineheight"))
                        {
                            string[] splitted = strings[m].Split(new char[] { ' ', '=', '\"', ',' });
                            for (int k = 0; k < splitted.Length; k++)
                            {
                                switch (splitted[k].ToLower())
                                {
                                    case "lineheight":
                                        font.BaseSize = Convert.ToSingle(splitted[k + 1]);

                                        if(Encoding.ASCII.GetString(check_header) == "5VSM" && font.hasLineHeight)
                                        {
                                            font.lineHeight = Convert.ToSingle(splitted[k + 1]);
                                        }
                                        break;

                                    case "base":
                                        if ((font.One == 0x31 && (Encoding.ASCII.GetString(check_header) == "5VSM"))
                                            || (Encoding.ASCII.GetString(check_header) == "6VSM"))
                                        {
                                            font.NewSomeValue = Convert.ToSingle(splitted[k + 1]);
                                        }
                                        else font.BaseSize = Convert.ToSingle(splitted[k + 1]);
                                        break;

                                    case "pages":
                                        tmpNewTex = new TextureClass.NewT3Texture[Convert.ToInt32(splitted[k + 1])];

                                        if(Convert.ToInt32(splitted[k + 1]) > font.TexCount)
                                        {

                                            for(int j = 0; j < tmpNewTex.Length; j++)
                                            {
                                                tmpNewTex[j] = new TextureClass.NewT3Texture(font.NewTex[0]);
                                            }
                                        }
                                        else
                                        {
                                            for(int j = 0; j < tmpNewTex.Length; j++)
                                            {
                                                tmpNewTex[j] = new TextureClass.NewT3Texture(font.NewTex[j]);
                                            }
                                        }
                                        break;
                                }
                            }
                        }

                        if(strings[m].Contains("page id"))
                        {
                            string[] splitted = strings[m].Split(new char[] { ' ', '=', '\"', ',' });
                            int idNum = 0;

                            for (int k = 0; k < splitted.Length; k++)
                            {
                                switch (splitted[k].ToLower())
                                {
                                    case "id":
                                        idNum = Convert.ToInt32(splitted[k + 1]);
                                        break;

                                    case "file":
                                        string fileName = strings[m].Substring(strings[m].IndexOf("file=") + 5).Replace("\"", string.Empty);

                                        if (fileName.ToLower().Contains(".dds") && File.Exists(fi.DirectoryName + Path.DirectorySeparatorChar + fileName))
                                        {
                                            ReplaceTexture(fi.DirectoryName + Path.DirectorySeparatorChar + fileName, tmpNewTex[idNum]);
                                        }
                                        break;
                                }
                            }
                        }

                        if (strings[m].Contains("chars count"))
                        {
                            string[] splitted = strings[m].Split(new char[] { ' ', '=', '\"', ',' });
                            for (int k = 0; k < splitted.Length; k++)
                            {
                                switch (splitted[k].ToLower())
                                {
                                    case "count":
                                        font.glyph.CharCount = Convert.ToInt32(splitted[k + 1]);
                                        font.glyph.charsNew = new FontClass.ClassFont.TRectNew[font.glyph.CharCount];
                                        break;
                                }
                            }
                        }

                        if (strings[m].Contains("char id"))
                        {
                            string[] splitted = strings[m].Split(new char[] { ' ', '=', '\"', ',' });

                            for (int k = 0; k < splitted.Length; k++)
                            {
                                switch (splitted[k].ToLower())
                                {
                                    case "id":
                                        ch++;
                                        font.glyph.charsNew[ch] = new FontClass.ClassFont.TRectNew();

                                        if (Convert.ToInt32(splitted[k + 1]) < 0)
                                        {
                                            font.glyph.charsNew[ch].charId = 0;
                                        }
                                        else
                                        {
                                            font.glyph.charsNew[ch].charId = Convert.ToUInt32(splitted[k + 1]);
                                        }
                                        break;

                                    case "x":
                                        font.glyph.charsNew[ch].XStart = Convert.ToSingle(splitted[k + 1]);
                                        break;

                                    case "y":
                                        font.glyph.charsNew[ch].YStart = Convert.ToSingle(splitted[k + 1]);
                                        break;

                                    case "width":
                                        font.glyph.charsNew[ch].CharWidth = Convert.ToSingle(splitted[k + 1]);
                                        font.glyph.charsNew[ch].XEnd = font.glyph.charsNew[ch].XStart + font.glyph.charsNew[ch].CharWidth;
                                        break;

                                    case "height":
                                        font.glyph.charsNew[ch].CharHeight = Convert.ToSingle(splitted[k + 1]);
                                        font.glyph.charsNew[ch].YEnd = font.glyph.charsNew[ch].YStart + font.glyph.charsNew[ch].CharHeight;
                                        break;

                                    case "xoffset":
                                        font.glyph.charsNew[ch].XOffset = Convert.ToSingle(splitted[k + 1]);
                                        if (rbNoKerning.Checked) font.glyph.charsNew[ch].XOffset = 0;
                                        break;

                                    case "yoffset":
                                        font.glyph.charsNew[ch].YOffset = Convert.ToSingle(splitted[k + 1]);
                                        if (rbNoKerning.Checked) font.glyph.charsNew[ch].YOffset = 0;
                                        break;

                                    case "xadvance":
                                        font.glyph.charsNew[ch].XAdvance = Convert.ToSingle(splitted[k + 1]);
                                        if (rbNoKerning.Checked) font.glyph.charsNew[ch].XAdvance = font.glyph.charsNew[ch].CharWidth;
                                        break;

                                    case "page":
                                        font.glyph.charsNew[ch].TexNum = Convert.ToInt32(splitted[k + 1]);
                                        break;

                                    case "chnl":
                                        font.glyph.charsNew[ch].Channel = Convert.ToInt32(splitted[k + 1]);
                                        break;
                                }
                            }
                        }
                    }

                    if(tmpNewTex != null)
                    {
                        font.NewTex = tmpNewTex;
                        font.TexCount = font.NewTex.Length;
                        fillTableofTextures(font);
                    }
                }
                else
                {
                    TextureClass.OldT3Texture[] tmpOldTex = null;

                    //Make all characters as first texture due bug after saving font if font was with multi textures and saves as font with a 1 texture.
                    for(int i = 0; i < font.glyph.CharCount; i++)
                    {
                        font.glyph.chars[i].TexNum = 0;
                    }

                    bool isUnicodeFnt = false;

                    for (int m = 0; m < strings.Length; m++)
                    {
                        if (strings[m].ToLower().Contains("info face"))
                        {
                            string[] splitted = strings[m].Split(new char[] { ' ', '=', '\"', ',' });

                            for (int k = 0; k < splitted.Length; k++)
                            {
                                if (splitted[k].ToLower() == "unicode" && splitted[k + 1] != "")
                                {
                                    isUnicodeFnt = Convert.ToInt32(splitted[k + 1]) == 1;
                                }
                            }
                        }
                        if (strings[m].ToLower().Contains("common lineheight"))
                        {
                            string[] splitted = strings[m].Split(new char[] { ' ', '=', '\"', ',' });
                            for (int k = 0; k < splitted.Length; k++)
                            {
                                switch (splitted[k].ToLower())
                                {
                                    case "lineheight":
                                        font.BaseSize = Convert.ToSingle(splitted[k + 1]);
                                        break;

                                    case "pages":
                                        tmpOldTex = new TextureClass.OldT3Texture[Convert.ToInt32(splitted[k + 1])];

                                        if (Convert.ToInt32(splitted[k + 1]) > font.TexCount)
                                        {
                                            for(int c = 0; c < tmpOldTex.Length; c++)
                                            {
                                                tmpOldTex[c] = new TextureClass.OldT3Texture(font.tex[0]);
                                            }
                                        }
                                        else
                                        {
                                            for (int c = 0; c < tmpOldTex.Length; c++)
                                            {
                                                tmpOldTex[c] = new TextureClass.OldT3Texture(font.tex[c]);
                                            }
                                        }

                                        break;
                                }
                            }
                        }

                        if (strings[m].Contains("page id"))
                        {
                            string[] splitted = strings[m].Split(new char[] { ' ', '=', '\"', ',' });
                            int idNum = 0;

                            for (int k = 0; k < splitted.Length; k++)
                            {
                                switch (splitted[k].ToLower())
                                {
                                    case "id":
                                        idNum = Convert.ToInt32(splitted[k + 1]);
                                        break;

                                    case "file":

                                        string fileName = strings[m].Substring(strings[m].IndexOf("file=") + 5).Replace("\"", string.Empty);

                                        if (fileName.ToLower().Contains(".dds") && File.Exists(fi.DirectoryName + Path.DirectorySeparatorChar +  fileName))
                                        {
                                            ReplaceTexture(fi.DirectoryName + Path.DirectorySeparatorChar + fileName, tmpOldTex[idNum]);
                                        }
                                        break;
                                }
                            }
                        }

                        if (strings[m].Contains("char id"))
                        {
                            string[] splitted = strings[m].Split(new char[] { ' ', '=', '\"', ',' });

                            for (int k = 0; k < splitted.Length; k++)
                            {
                                switch (splitted[k].ToLower())
                                {
                                    case "id":
                                        uint tmpChar = 0;

                                        if (Convert.ToInt32(splitted[k + 1]) < 0)
                                        {
                                            tmpChar = 0;
                                        }
                                        else
                                        {
                                            tmpChar = Convert.ToUInt32(splitted[k + 1]);

                                            if (isUnicodeFnt)
                                            {
                                                if(tmpChar == 126)
                                                {
                                                    int puase = 1;
                                                }
                                                byte[] tmp_ch = BitConverter.GetBytes(Convert.ToUInt32(splitted[k + 1]));
                                                tmp_ch = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(MainMenu.settings.ASCII_N), tmp_ch);
                                                tmpChar = BitConverter.ToUInt16(tmp_ch, 0);
                                            }
                                        }

                                        for(int t = 0; t < font.glyph.CharCount; t++)
                                        {
                                            if(Convert.ToUInt32(dataGridViewWithCoord[0, t].Value) == tmpChar)
                                            {
                                                ch = t;
                                                break;
                                            }
                                        }

                                        break;

                                    case "x":
                                        font.glyph.chars[ch].XStart = Convert.ToSingle(splitted[k + 1]);
                                        break;

                                    case "y":
                                        font.glyph.chars[ch].YStart = Convert.ToSingle(splitted[k + 1]);
                                        break;

                                    case "width":
                                        if (font.hasScaleValue)
                                        {
                                            font.glyph.chars[ch].CharWidth = Convert.ToSingle(splitted[k + 1]);
                                            font.glyph.chars[ch].XEnd = font.glyph.chars[ch].XStart + font.glyph.chars[ch].CharWidth;
                                        }
                                        else
                                        {
                                            font.glyph.chars[ch].XEnd = font.glyph.chars[ch].XStart + Convert.ToSingle(splitted[k + 1]);
                                        }
                                        break;

                                    case "height":
                                        if (font.hasScaleValue)
                                        {
                                            font.glyph.chars[ch].CharHeight = Convert.ToSingle(splitted[k + 1]);
                                            font.glyph.chars[ch].YEnd = font.glyph.chars[ch].YStart + font.glyph.chars[ch].CharHeight;
                                        }
                                        else
                                        {
                                            font.glyph.chars[ch].YEnd = font.glyph.chars[ch].YStart + Convert.ToSingle(splitted[k + 1]);
                                        }
                                        break;

                                    case "page":
                                        font.glyph.chars[ch].TexNum = Convert.ToInt32(splitted[k + 1]);
                                        break;
                                }
                            }
                        }
                    }

                    if (tmpOldTex != null)
                    {
                        font.tex = new TextureClass.OldT3Texture[tmpOldTex.Length];

                        for(int i = 0; i < font.tex.Length; i++)
                        {
                            font.tex[i] = new TextureClass.OldT3Texture(tmpOldTex[i]);
                        }

                        tmpOldTex = null;
                        GC.Collect();

                        font.TexCount = font.tex.Length;
                        fillTableofTextures(font);
                    }
                }

                fillTableofCoordinates(font, true);
                edited = true;
            }

        }

        private void removeDuplicatesCharsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(font != null && font.glyph.charsNew.Length > 0)
            {

                Array.Sort(font.glyph.charsNew, (arr1, arr2) => arr1.charId.CompareTo(arr2.charId));
                font.glyph.charsNew = font.glyph.charsNew.GroupBy(i => i.charId).Select(g => g.Last()).ToArray();

                if (!edited) edited = true;
                fillTableofCoordinates(font, edited);
            }
        }

        private void importCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripImportFNT_Click(sender, e);
        }

        private void rbNoSwizzle_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.swizzlePS4 = false;
            MainMenu.settings.swizzleNintendoSwitch = false;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void rbPS4Swizzle_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.swizzlePS4 = true;
            MainMenu.settings.swizzleNintendoSwitch = true;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void rbSwitchSwizzle_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.swizzlePS4 = false;
            MainMenu.settings.swizzleNintendoSwitch = true;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void convertArgb8888CB_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}