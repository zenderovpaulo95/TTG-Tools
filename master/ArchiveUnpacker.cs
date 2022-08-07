using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BlowFishCS;

namespace TTG_Tools
{
    public partial class ArchiveUnpacker : Form
    {
        public ArchiveUnpacker()
        {
            InitializeComponent();
        }

        struct FileStruct
        {
            public string FileName;
            public uint Offset;
            public uint Size;
        }

        byte[] key = null;

        FileStruct[] FileList;

        private FileStruct[] GetFileListTTARCH(string FileName)
        {
            FileStruct[] Files = null;

            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                int version = br.ReadInt32();

                if(version < 2 || version > 9)
                {
                    MessageBox.Show("This archive doesn't support", "Error");
                    return null;
                }

                int encrypted = br.ReadInt32();
                int two = br.ReadInt32();

                if(two != 2)
                {
                    MessageBox.Show("Unknown archive", "Error");
                    return null;
                }

                int ArcSize = br.ReadInt32();

                byte[] block = br.ReadBytes(ArcSize);

                if(encrypted == 1)
                {
                    key = MainMenu.gamelist[encKeyListCB.SelectedIndex].key;
                    BlowFish blow = new BlowFish(key, version);
                    block = blow.Crypt_ECB(block, version, true);
                }

                MemoryStream ms = new MemoryStream(block);
                BinaryReader brms = new BinaryReader(ms);
                int dirCount = brms.ReadInt32();
                string[] dirs = new string[dirCount];

                for(int i = 0; i < dirCount; i++)
                {
                    int len = brms.ReadInt32();
                    byte[] tmp = brms.ReadBytes(len);
                    dirs[i] = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);
                }

                int fileCount = brms.ReadInt32();
                Files = new FileStruct[fileCount];

                for(int i = 0;i < fileCount; i++)
                {
                    int len = brms.ReadInt32();
                    byte[] tmp = brms.ReadBytes(len);
                    Files[i].FileName = Encoding.GetEncoding(MainMenu.settings.ASCII_N).GetString(tmp);
                    int zero = brms.ReadInt32();
                    Files[i].Offset = brms.ReadUInt32();
                    Files[i].Size = brms.ReadUInt32();
                }

                brms.Close();
                ms.Close();

                block = null;
                dirs = null;

                br.Close();
                fs.Close();

                GC.Collect();
            }
            catch
            {
                return null;
            }


            return Files;
        }

        private void FileBrowseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePathTB.Text = ofd.FileName;

                FileList = GetFileListTTARCH(ofd.FileName);

                if((FileList != null) && (FileList.Length > 0))
                {
                    dataGridView1.ColumnCount = 3;
                    dataGridView1.RowCount = FileList.Length;
                    dataGridView1.Columns[0].HeaderText = "File name";
                    dataGridView1.Columns[1].HeaderText = "File offset";
                    dataGridView1.Columns[2].HeaderText = "File size";

                    for(int i = 0; i < FileList.Length; i++)
                    {
                        dataGridView1[0, i].Value = FileList[i].FileName;
                        dataGridView1[1, i].Value = Convert.ToString(FileList[i].Offset);
                        dataGridView1[2, i].Value = Convert.ToString(FileList[i].Size);
                    }
                }
            }
        }

        private void ResourceDirBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if(fbd.ShowDialog() == DialogResult.OK)
            {
                dirPathTB.Text = fbd.SelectedPath;
            }
        }

        private void ArchiveUnpacker_Load(object sender, EventArgs e)
        {
            for(int i = 0; i < MainMenu.gamelist.Count; i++)
            {
                encKeyListCB.Items.Add((i + 1).ToString() + ". " + MainMenu.gamelist[i].gamename);
            }

            encKeyListCB.SelectedIndex = 0;
        }
    }
}
