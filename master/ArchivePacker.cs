﻿using Microsoft.Win32;
using OodleTools;
using System;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TTG_Tools
{
    public partial class ArchivePacker : Form
    {
        FolderBrowserDialog fbd = new FolderBrowserDialog(); //Для выбора папки
        SaveFileDialog sfd = new SaveFileDialog(); //Для сохранения архива

        public static FileInfo[] fi; //Получение списка файлов
        int archiveVersion;
        

        public ArchivePacker()
        {
            InitializeComponent();
        }

        void AddNewReport(string report)
        {
            if (messageListBox.InvokeRequired)
            {
                messageListBox.Invoke(new ReportHandler(AddNewReport), report);
            }
            else
            {
                messageListBox.Items.Add(report);
                messageListBox.SelectedIndex = messageListBox.Items.Count - 1;
                messageListBox.SelectedIndex = -1;
            }
        }

        void Progress(int i)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new ProgressHandler(Progress), i);
            }
            else
            {
                progressBar1.Value = i;
            }
        }


        public static UInt64 pad_it(UInt64 num, UInt64 pad)
        {
            UInt64 t;
            t = num % pad;

            if (Convert.ToBoolean(t)) num += pad - t;
            return (num);
        }

        public static Int32 pad_size(Int32 num, Int32 pad)
        {
            while (num % pad != 0) num++;

            return num;
        }
        
        private bool CheckDll(string filePath)
        {
            if (File.Exists(filePath) == true)
            {
                try
                {
                    byte[] testBlock = { 0x43, 0x48, 0x45, 0x43, 0x4B, 0x20, 0x54, 0x45, 0x53, 0x54, 0x20, 0x42, 0x4C, 0x4F, 0x43, 0x4B };
                    testBlock = ZlibCompressor(testBlock);
                    
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else return false;
        }

        private static byte[] ZlibCompressor(byte[] bytes) //Для старых архивов (с версии 3 по 7)
        {
            byte[] retBytes = new byte[bytes.Length];

            using (MemoryStream outMemoryStream = new MemoryStream())
            {
                using (Joveler.ZLibWrapper.ZLibStream outZStream = new Joveler.ZLibWrapper.ZLibStream(outMemoryStream, Joveler.ZLibWrapper.ZLibMode.Compress, Joveler.ZLibWrapper.ZLibCompLevel.Level9))
                {
                    using (Stream inMemoryStream = new MemoryStream(bytes))
                    {

                        Methods.CopyStream(inMemoryStream, outZStream);
                        outZStream.Flush();
                        retBytes = outMemoryStream.ToArray();
                    }
                }
            }

            return retBytes;
        }

        private static byte[] OodleCompress(byte[] bytes)
        {
            byte[] retVal = new byte[bytes.Length];
            long bufSize = bytes.Length;
            int compressedSize = OodleTools.Imports.OodleLZ_Compress(OodleTools.OodleFormat.LZHLW, bytes, bufSize, retVal, OodleTools.OodleCompressionLevel.Optimal5, 0, 0, 0);
            bytes = new byte[compressedSize];
            Array.Copy(retVal, 0, bytes, 0, bytes.Length);
            retVal = null;
            return bytes;
        }

        private static byte[] DeflateCompressor(byte[] bytes) //Для старых (версии 8 и 9) и новых архивов
        {
            byte[] retVal;
            using (MemoryStream compressedMemoryStream = new MemoryStream())
            {
                using (System.IO.Compression.DeflateStream compressStream = new System.IO.Compression.DeflateStream(compressedMemoryStream, System.IO.Compression.CompressionMode.Compress))
                {
                    using(MemoryStream inMemStream = new MemoryStream(bytes))
                    {
                        inMemStream.CopyTo(compressStream);
                        compressStream.Close();
                        retVal = compressedMemoryStream.ToArray();
                    }
                }
            }
            return retVal;
        }

        

        public byte[] encryptFunction(byte[] bytes, byte[] key, int archiveVersion)
        {
            BlowFishCS.BlowFish enc = new BlowFishCS.BlowFish(key, archiveVersion);
            //Methods.meta_crypt(bytes, key, archiveVersion, false);
            return enc.Crypt_ECB(bytes, archiveVersion, false);
        }


        public void builder_ttarch2(string input_folder, string output_path, bool compression, byte[] key, bool encLua, int version_archive, bool newEngine, int compressAlgorithm)
        {                           
            DirectoryInfo di = new DirectoryInfo(input_folder);
            fi = di.GetFiles("*", SearchOption.AllDirectories);
            UInt64[] name_crc = new UInt64[fi.Length];
            string[] name = new string[fi.Length];
            UInt64 offset = 0;

            for (int i = 0; i < fi.Length; i++) //Get CRC64 checksum
            {
                name[i] = null;
                if ((fi[i].Extension == ".lua") && (MainMenu.settings.encryptLuaInArchive == false))
                {
                    if (!newEngine) name[i] = fi[i].Name.Replace(".lua", ".lenc");
                    else name[i] = fi[i].Name;
                }
                else name[i] = fi[i].Name;
                name_crc[i] = CRCs.CRC64(0, name[i].ToLower()); //Calculate crc64 file name with lower characters
            }

            for (int k = 0; k < fi.Length - 1; k++) //Sort file names by less crc64
            {
                for (int l = k + 1; l < fi.Length; l++)
                {
                    if (name_crc[l] < name_crc[k])
                    {
                        FileInfo temp = fi[k];
                        fi[k] = fi[l];
                        fi[l] = temp;
                        
                        string temp_str = name[k];
                        name[k] = name[l];
                        name[l] = temp_str;

                        UInt64 temp_crc = name_crc[k];
                        name_crc[k] = name_crc[l];
                        name_crc[l] = temp_crc;
                    }
                }
            }

            UInt32 info_size = (uint)fi.Length * (8 + 8 + 4 + 4 + 2 + 2); //Prepare header size...
            UInt32 data_size = 0; //archive's size
            UInt32 name_size = 0; //and size of file name table

            for (int j = 0; j < fi.Length; j++) //Calculate archive's size and size of file name table
            {
                name_size += Convert.ToUInt32(name[j].Length) + 1;
                data_size += Convert.ToUInt32(fi[j].Length);
            }

            name_size = Convert.ToUInt32(pad_it((UInt64)name_size, (UInt64)0x10000)); //Pad size of file name's block by 64KB
            byte[] info_table = new byte[info_size];
            byte[] names_table = new byte[name_size];

            UInt32 name_offset = 0;
            for (int d = 0; d < fi.Length; d++) //Calculate name's offset and add zero byte after file name
            {
                name[d] += "\0";
                Array.Copy(Encoding.ASCII.GetBytes(name[d]), 0, names_table, name_offset, name[d].Length);
                name_offset += Convert.ToUInt32(name[d].Length);
            }

            byte[] nctt_header = { 0x4E, 0x43, 0x54, 0x54 }; //Prepare non-compressed header (NCTT)
            byte[] att = new byte[4];

            //And prepare compressed header
            if (version_archive == 1) att = BitConverter.GetBytes(0x33415454); //Before Minecraft:Story mode - 3ATT
            else if (version_archive == 2) att = BitConverter.GetBytes(0x34415454); //After Minecraft Story mode - 4ATT

            Array.Reverse(att);

            UInt64 common_size = data_size + info_size + name_size + 4 + 4 + 4 + 4; //Common archive's size

            byte[] cz_bin = new byte[8];
            cz_bin = BitConverter.GetBytes(common_size);


            UInt32 ns = name_size;
            UInt32 tmp;
            UInt64 file_offset = 0;

            progressBar1.Maximum = fi.Length;
            
            for (int k = 0; k < fi.Length; k++) //Making header with file information
            {
                byte[] crc64_hash = BitConverter.GetBytes(name_crc[k]); //CRC64 file name
                Array.Copy(crc64_hash, 0, info_table, Convert.ToInt64(offset), crc64_hash.Length);
                offset += 8;
                byte[] fo_bin = BitConverter.GetBytes(file_offset); //Offset to file
                Array.Copy(fo_bin, 0, info_table, Convert.ToInt64(offset), fo_bin.Length);
                offset += 8;
                byte[] fs_bin = BitConverter.GetBytes((int)fi[k].Length); //Size of file
                Array.Copy(fs_bin, 0, info_table, Convert.ToInt64(offset), fs_bin.Length);
                offset += 4;
                Array.Copy(BitConverter.GetBytes(0), 0, info_table, Convert.ToInt64(offset), 4);
                offset += 4;
                tmp = ns - name_size;
                byte[] tmp_bin1 = new byte[2]; //Get offset to file name
                tmp_bin1 = BitConverter.GetBytes(tmp / 0x10000);
                Array.Copy(tmp_bin1, 0, info_table, Convert.ToInt64(offset), 2);
                offset += 2;
                byte[] tmp_bin2 = new byte[2]; //The same thing but calculates another
                tmp_bin2 = BitConverter.GetBytes(tmp % 0x10000);
                Array.Copy(tmp_bin2, 0, info_table, Convert.ToInt64(offset), 2);
                offset += 2;
                ns += Convert.ToUInt32(name[k].Length);
                file_offset += Convert.ToUInt32(fi[k].Length);

                Progress(k + 1);
            } 

            string format = ".ttarch2";
            if (output_path.IndexOf(".obb") > 0) format = ".obb";
            string temp_path = output_path.Replace(format, ".tmp");

            FileStream fs = new FileStream(temp_path, FileMode.Create);

            fs.Write(nctt_header, 0, 4);
            fs.Write(cz_bin, 0, 8);
            fs.Write(att, 0, 4);

            if (version_archive == 1) //In older versions I had to write 02 00 00 00
            {
                fs.Write(BitConverter.GetBytes(2), 0, 4);
            }

            fs.Write(BitConverter.GetBytes(name_size), 0, 4);
            fs.Write(BitConverter.GetBytes(fi.Length), 0, 4);
            fs.Write(info_table, 0, Convert.ToInt32(info_size));
            fs.Write(names_table, 0, Convert.ToInt32(name_size));

            progressBar1.Maximum = fi.Length;

            for (int l = 0; l < fi.Length; l++)
            {
                FileStream fr = new FileStream(fi[l].FullName, FileMode.Open);
                byte[] file = new byte[fr.Length];
                fr.Read(file, 0, file.Length);

                //Encrypt lua if it needs
                if ((fi[l].Extension == ".lua") && !MainMenu.settings.encryptLuaInArchive)
                {
                    file = Methods.encryptLua(file, key, newEngine, 7);
                }

                fs.Write(file, 0, file.Length);
                fr.Close();
                Progress(l + 1);
            }
            fs.Close();

            if (compression == false)
            {
                if (File.Exists(output_path) == true) File.Delete(output_path);
                File.Move(temp_path, output_path); //If we won't compress archive just rename a temporary file
            }
            else
            {
                fs = new FileStream(output_path, FileMode.Create);

                UInt64 offset_table = 0;//Calculate offsets compressed blocks

                UInt64 full_it = pad_it(common_size, 0x10000); //Pad size from 3ATT/4ATT header

                UInt32 blocks_count = Convert.ToUInt32(full_it) / 0x10000; //Get count of compressed blocks
                byte[] bin_blocks_count = new byte[4];
                bin_blocks_count = BitConverter.GetBytes(blocks_count);

                //ZCTT - compressed archive with zlib/deflate libraries
                //ECTT - encrypted compressed archive with zlib/deflate libraries
                //zCTT - compressed archive with oo2core library
                //eCTT - encrypted compressed archive with oo2core library

                byte[] compressedHeader = { 0x5A, 0x43, 0x54, 0x54 }; //Compressed archive's header (ZCTT)

                if (compressAlgorithm == 1) compressedHeader[0] = 0x7a; //If oodle algorithm then change Z to z char

                byte[] encCompressedHeader = { 0x45, 0x43, 0x54, 0x54 }; //Encrypted compressed archive's header (ECTT)

                if (compressAlgorithm == 1) encCompressedHeader[0] = 0x65; //If oodle algorithm then change E to e char

                byte[] chunk_size = { 0x00, 0x00, 0x01, 0x00 }; //Write chunk size (64KB)
                UInt64 chunk_table_size = 8 * blocks_count + 8;
                offset = compressAlgorithm == 0 ? chunk_table_size + 4 + 4 + 4 : chunk_table_size + 4 + 4 + 4 + 4;

                byte[] chunk_table = new byte[chunk_table_size];
                byte[] bin_offset = BitConverter.GetBytes(offset);

                Array.Copy(bin_offset, 0, chunk_table, (uint)offset_table, 8);
                offset_table += 8;

                if (MainMenu.settings.encArchive) fs.Write(encCompressedHeader, 0, encCompressedHeader.Length); //If compressed archive encrypts then write encrypted header
                else fs.Write(compressedHeader, 0, compressedHeader.Length); //else write compressed header

                if(compressAlgorithm == 1)
                {
                    byte[] val = BitConverter.GetBytes(1);
                    fs.Write(val, 0, val.Length);
                }
                fs.Write(chunk_size, 0, 4);
                fs.Write(bin_blocks_count, 0, 4);
                fs.Write(chunk_table, 0, chunk_table.Length);

                FileStream temp_fr = new FileStream(temp_path, FileMode.Open);
                temp_fr.Seek(12, SeekOrigin.Begin);

                progressBar1.Maximum = (int)blocks_count;

                for (int i = 0; i < blocks_count; i++) //Read 64KB blocks and compress it
                {
                    byte[] temp = new byte[0x10000];
                    temp_fr.Read(temp, 0, temp.Length);

                    byte[] compressed_block = new byte[temp.Length];

                    switch (compressAlgorithm)
                    {
                        case 0:
                            compressed_block = DeflateCompressor(temp);
                            break;

                        case 1:
                            compressed_block = OodleCompress(temp);
                            break;
                    }
                    

                    if (MainMenu.settings.encArchive) //Encrypt compressed block if user select encrypt archive
                    {
                        int num = comboGameList.SelectedIndex;
                        temp = encryptFunction(compressed_block, key, 7);
                    }

                    offset += (uint)compressed_block.Length;
                    bin_offset = BitConverter.GetBytes(offset);
                    Array.Copy(bin_offset, 0, chunk_table, (uint)offset_table, 8);
                    offset_table += 8;
                    fs.Write(compressed_block, 0, compressed_block.Length);

                    Progress(i + 1);
                }

                int off = 12;
                if (compressAlgorithm == 1) off += 4;

                fs.Seek(off, SeekOrigin.Begin); //Return to chunk table position
                fs.Write(chunk_table, 0, chunk_table.Length); //and record our data

                temp_fr.Close();
                fs.Close();
                File.Delete(temp_path); //Remove temporary file
            }
            
        }

        public void builder_ttarch(string inputFolder, string outputPath, byte[] key, bool compression, int versionArchive, bool encryptCheck, bool DontEncLua, int compressAlgorithm) //Функция сборки
        {
            DirectoryInfo di = new DirectoryInfo(inputFolder);
            DirectoryInfo[] di1 = di.GetDirectories("*", SearchOption.AllDirectories); //Just for fun if files were in different directories for Telltale Tool engine this optional

            FileInfo[] fi = di.GetFiles("*", SearchOption.AllDirectories);
            int chunkSize = versionArchive == 7 ? 0x20000 : 0x10000;
            int headerChunkSize = versionArchive == 7 ? 128 : 64;

            if(fi.GroupBy(f => f.Name).Where(group => group.Count() > 1).Select(group => group.Key).Count() > 0)
            {
                fi = fi.Distinct(new FileNameComparer()).ToArray();
                AddNewReport("Found duplicated files in directories. Successfully removed duplicated files.");
            }

            uint fileOffset = 0;
            uint tableSize = 0;
            uint[] compressedChunks = null;

            if (versionArchive == 2 && compression) compression = false;

            MemoryStream ms = new MemoryStream();
            BinaryWriter mbw = new BinaryWriter(ms);

            bool WithoutParentFolders = false;
            int directories = di1.Length;

            if (directories == 0)
            {
                directories = 1;
                WithoutParentFolders = true;
            }

            mbw.Write(directories);
            tableSize += 4;

            for (int i = 0; i < directories; i++) //Get directories' name
            {
                byte[] dirName = !WithoutParentFolders ? Encoding.ASCII.GetBytes(di1[i].Parent.Name + Path.PathSeparator + di1[i].Name) : Encoding.ASCII.GetBytes(di.FullName);
                int dirNameSize = (int)dirName.Length;
                mbw.Write(dirNameSize);
                mbw.Write(dirName);

                tableSize += 4 + (uint)dirName.Length;
            }

            int filesCount = (int)fi.Length;
            mbw.Write(filesCount);
            tableSize += 4;

            for (int i = 0; i < fi.Length; i++)
            {
                string name = (fi[i].Extension.ToLower() == ".lua") && !DontEncLua ? fi[i].Name.Remove(fi[i].Name.Length - 3, 3) + "lenc" : fi[i].Name;

                if (Methods.meta_check(fi[i]) && compression)
                {
                    compression = false;
                    AddNewReport("Found file with meta encryption. Compression flag disabled.");
                }

                byte[] binFileName = Encoding.ASCII.GetBytes(name);
                int fileNameLen = binFileName.Length;
                int fileSize = (int)fi[i].Length;
                int zeroVal = 0;

                //Record file table with MemoryStream
                mbw.Write(fileNameLen);
                mbw.Write(binFileName);
                mbw.Write(zeroVal);
                mbw.Write(fileOffset);
                mbw.Write(fileSize);

                fileOffset += (uint)fileSize;

                tableSize += 4 + 4 + 4 + 4 + (uint)binFileName.Length;
            }

            if(versionArchive == 4)
            {
                int zero = 0;
                byte val = 0;

                mbw.Write(zero);
                mbw.Write(val);
            }

            if(versionArchive == 2)
            {
                tableSize += 8 + 8 + 4; //Add file offset and archive size + 0xFEEDFACE
                mbw.Write(tableSize);
                mbw.Write(fileOffset);
                uint feedface = 0xfeedface;
                mbw.Write(feedface);
            }

            byte[] table = ms.ToArray();

            byte[] crc32Header = CRCs.CRC32_generator(table);

            tableSize = (uint)table.Length;

            if (versionArchive <= 2 || encryptCheck) table = encryptFunction(table, key, versionArchive);

            if (versionArchive >= 7 && compression)
            {
                switch (versionArchive)
                {
                    case 7:
                    case 8:
                        table = compressAlgorithm == 0 ? ZlibCompressor(table) : DeflateCompressor(table);
                        break;
                    default:
                        table = DeflateCompressor(table);
                        break;
                }
            }

            mbw.Close();
            ms.Close();

            int chunksCount = pad_size((int)fileOffset, (int)chunkSize) / chunkSize;

            compressedChunks = new uint[chunksCount];
            int encrypted = (versionArchive <= 2) || encryptCheck ? 1 : 0; //Set flag for encrypt archive
            int unknown1 = 2;
            int unknown2 = compression ? 2 : 1;

            if (File.Exists(outputPath)) File.Delete(outputPath);

            FileStream fs = new FileStream(outputPath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(versionArchive);
            bw.Write(encrypted);
            bw.Write(unknown1);
            uint pos = 0;

            if(archiveVersion >= 3)
            {
                bw.Write(unknown2);
                int headerChunksCount = compression ? chunksCount : 0;
                bw.Write(headerChunksCount);

                if (compression)
                {
                    for(int i = 0; i < chunksCount; i++)
                    {
                        bw.Write(compressedChunks[i]);
                    }
                }

                pos = (uint)bw.BaseStream.Position;
                bw.Write(fileOffset); //Archive size

                if (archiveVersion == 4 || archiveVersion >= 7)
                {
                    int priority = 0;
                    int unknownValue = checkXmode.Checked ? 1 : 0;
                    byte[] binChunkSize = BitConverter.GetBytes(headerChunkSize);

                    bw.Write(priority);
                    bw.Write(priority);

                    if (archiveVersion > 4)
                    {
                        bw.Write(unknownValue);
                        bw.Write(unknownValue);
                        bw.Write(binChunkSize[0]);

                        byte[] zeros = versionArchive == 7 ? new byte[] { 0, 0, 0 } : new byte[] { 0, 0, 0, 0 };

                        bw.Write(zeros);

                        if (versionArchive == 9)
                        {
                            bw.Write(crc32Header);
                        }
                    }
                }
            }

            bw.Write(tableSize);
            bw.Write(table);

            progressBar1.Maximum = fi.Length > 1 ? fi.Length : 1;

            int ch = 0;
            int a = 0;

            int chunkOff = 0;
            byte[] chunk = new byte[chunkSize];
            uint compressedArchive = 0;

            //TODO: Подумать над нормальным кодом запаковки архивов

            while(ch < chunksCount && a < fi.Length)
            {
                string name = (fi[a].Extension.ToLower() == ".lua") && !DontEncLua ? fi[a].Name.Remove(fi[a].Name.Length - 3, 3) + "lenc" : fi[a].Name;
                byte[] file = File.ReadAllBytes(fi[a].FullName);

                int res = Methods.meta_crypt(file, key, archiveVersion, false);

                if ((!DontEncLua && fi[a].Extension.ToLower() == ".lua") || fi[a].Extension.ToLower() == ".lenc") file = Methods.encryptLua(file, key, false, archiveVersion);

                int fileChunkCount = pad_size(chunkOff + file.Length, chunkSize) / chunkSize;
                int chunkFile = file.Length;
                int chunkFileOff = 0;

                for(int c = 0; c < fileChunkCount; c++)
                {
                    int chSize = chunkFile;
                    
                    if(chunkOff + chSize > chunkSize)
                    {
                        chSize = chunkOff == 0 ? chunkSize : chunkSize - chunkOff;
                    }

                    Array.Copy(file, chunkFileOff, chunk, chunkOff, chSize);
                    
                    chunkOff += chSize;
                    chunkFile -= chSize;
                    chunkFileOff += chSize;

                    if ((chunkOff >= chunkSize) || ((ch + 1 == chunksCount) && (a + 1 == fi.Length)))
                    {
                        if((ch + 1 == chunksCount) && (chunkOff < chunkSize))
                        {
                            byte[] tmp = new byte[chunkOff];
                            Array.Copy(chunk, 0, tmp, 0, tmp.Length);
                            chunk = new byte[tmp.Length];
                            Array.Copy(tmp, 0, chunk, 0, tmp.Length);
                        }

                        if (compression)
                        {
                            byte[] check = new byte[chunk.Length];
                            if (versionArchive >= 8) check = versionArchive == 8 && compressAlgorithm == 0 ? ZlibCompressor(chunk) : DeflateCompressor(chunk);
                            else check = ZlibCompressor(chunk);

                            if(check.Length < chunk.Length)
                            {
                                chunk = new byte[check.Length];
                                Array.Copy(check, 0, chunk, 0, chunk.Length);
                                check = null;
                            }

                            if (encryptCheck)
                            {
                                chunk = encryptFunction(chunk, key, versionArchive);
                            }
                        }

                        bw.Write(chunk);

                        if (compression)
                        {
                            compressedChunks[ch] = (uint)chunk.Length;
                            compressedArchive += (uint)chunk.Length;
                        }

                        chunkOff = 0;
                        chunk = new byte[chunkSize];
                        ch++;
                    }
                }

                AddNewReport("File " + fi[a].Name + " packed");
                Progress(a + 1);
                a++;
            }

            if (compression)
            {
                bw.BaseStream.Seek(20, SeekOrigin.Begin);
                
                for(int c = 0; c < chunksCount; c++)
                {
                    bw.Write(compressedChunks[c]);
                }

                bw.BaseStream.Seek(pos, SeekOrigin.Begin);
                bw.Write(compressedArchive);
            }

            bw.Close();
            fs.Close();

            AddNewReport("Packing archive complete");

            /*for (int i = 0; i < chunksCount; i++)
            {
                string name = (fi[i].Extension.ToLower() == ".lua") && !DontEncLua ? fi[i].Name.Remove(fi[i].Name.Length - 3, 3) + "lenc" : fi[i].Name;
                byte[] file = File.ReadAllBytes(fi[i].FullName);

                int res = Methods.meta_crypt(file, key, archiveVersion, false);

                if ((!DontEncLua && fi[i].Extension.ToLower() == ".lua") || fi[i].Extension.ToLower() == ".lenc") file = Methods.encryptLua(file, key, false, archiveVersion);

                if(compression)
                {
                    int fileChunksCount = pad_size(file.Length, chunkSize) / chunkSize;


                }
                else
                {
                    bw.Write(file);
                }

                AddNewReport("Packed file: " + name);
                Progress(i + 1);
            }*/

            bw.Close();
            fs.Close();
            #region Old (somehow) worked code
            /*bool WithoutParentFolders = false;

            int directories = di1.Length;

            if ((directories == 0))
            {
                directories = 1;
                WithoutParentFolders = true;
            }

            byte[] dir_name_count = BitConverter.GetBytes(directories);
            ms.Write(dir_name_count, 0, dir_name_count.Length);

            byte[] empty_bytes = { 0x00, 0x00, 0x00, 0x00 }; //Unknown 00 00 00 00 bytes

            for (int i = 0; i < directories; i++) //Get directories' name
            {
                byte[] dir_name = !WithoutParentFolders ? Encoding.ASCII.GetBytes(di1[i].Parent.Name + "\\" + di1[i].Name) : Encoding.ASCII.GetBytes(di.FullName);
                byte[] dir_name_size = BitConverter.GetBytes(dir_name.Length);
                ms.Write(dir_name_size, 0, dir_name_size.Length);
                ms.Write(dir_name, 0, dir_name.Length);
            }

            fi = di.GetFiles("*", SearchOption.AllDirectories);

            byte[] files_count = BitConverter.GetBytes(fi.Length); //Get count of file
            ms.Write(files_count, 0, files_count.Length);
           
            uint file_offset = 0; //Calculate file's offset

            for (int j = 0; j < fi.Length; j++)
            {
                string name;
                if ((fi[j].Extension == ".lua") && (DontEncLua == false))
                {
                    name = fi[j].Name.Replace(".lua", ".lenc");
                }
                else name = fi[j].Name;

                int file_name_length = name.Length;
                byte[] bin_length = BitConverter.GetBytes(file_name_length);

                byte[] bin_file_name = new byte[name.Length];
                bin_file_name = Encoding.ASCII.GetBytes(name);

                int file_size;
                file_size = (int)fi[j].Length;

                byte[] bin_file_size = new byte[4];
                bin_file_size = BitConverter.GetBytes(file_size);

                byte[] bin_file_offset = new byte[4];
                bin_file_offset = BitConverter.GetBytes(file_offset);

                //Record file table with MemoryStream
                ms.Write(bin_length, 0, bin_length.Length);
                ms.Write(bin_file_name, 0, bin_file_name.Length);
                ms.Write(empty_bytes, 0, empty_bytes.Length);
                ms.Write(bin_file_offset, 0, bin_file_offset.Length);
                ms.Write(bin_file_size, 0, bin_file_size.Length);

                file_offset += (uint)file_size;
            }

            if (version_archive == 4)
            {
                ms.Write(empty_bytes, 0, 4);
                ms.Write(empty_bytes, 0, 1);
            }

            byte[] table_files = ms.ToArray(); //Converto to byte array from MemoryStream
            ms.Close(); //and close MemoryStream

            if (version_archive <= 2) //Special for archives version 2 (oldest telltale's games)
            {
                byte[] tempHeader = new byte[table_files.Length + 4 + 4 + 4 + 4];
                Array.Copy(table_files, 0, tempHeader, 0, table_files.Length);

                uint temp_table_size = (uint)tempHeader.Length;
                temp_table_size += 4;

                byte[] binTable = BitConverter.GetBytes(temp_table_size);
                Array.Copy(binTable, 0, tempHeader, table_files.Length, binTable.Length);

                byte[] archive_size = BitConverter.GetBytes(file_offset);
                Array.Copy(archive_size, 0, tempHeader, table_files.Length + 4, archive_size.Length);

                byte[] bin_feedface = {0xCE, 0xFA, 0xED, 0xFE};
                
                Array.Copy(bin_feedface, 0, tempHeader, table_files.Length + 4 + 4, 4);
                Array.Copy(bin_feedface, 0, tempHeader, table_files.Length + 4 + 4 + 4, 4);

                table_files = new byte[tempHeader.Length];
                Array.Copy(tempHeader, 0, table_files, 0, tempHeader.Length);
            }

            uint table_size = (uint)table_files.Length;

            byte[] header_crc32 = CRCs.CRC32_generator(table_files);

            long header_size = table_size;

            if ((version_archive == 2) || (encryptCheck == true))
            {
                table_files = encryptFunction(table_files, key, version_archive);
            }

            byte[] archive_version = BitConverter.GetBytes(version_archive); //Get archive's version

            byte[] encrypt = (version_archive <= 2) || encryptCheck ? BitConverter.GetBytes(1) : BitConverter.GetBytes(0); //Set flag for encrypt archive

            byte[] hz1 = { 0x02, 0x00, 0x00, 0x00 }; //Unknown data
            byte[] hz2 = { 0x01, 0x00, 0x00, 0x00 }; //Unknown data (1)

            byte[] priority = BitConverter.GetBytes(0); //Set default archive's priority 0 (this since version 7)
            int pos_header = 0;

            //Remove temporary file if it exists
            if (File.Exists(Application.StartupPath + "\\temp.file")) File.Delete(Application.StartupPath + "\\temp.file");

            FileStream fs = new FileStream(Application.StartupPath + "\\temp.file", FileMode.CreateNew);

            fs.Write(archive_version, 0, archive_version.Length);
            pos_header += 4;
            fs.Write(encrypt, 0, encrypt.Length);
            pos_header += 4;
            fs.Write(hz1, 0, hz1.Length);
            pos_header += 4;

            if (version_archive >= 3)
            {
                fs.Write(hz2, 0, hz2.Length);
                pos_header += 4;
                fs.Write(empty_bytes, 0, empty_bytes.Length);
                pos_header += 4;

                long archive_size = file_offset; //Archive's size
                byte[] bin_arch_size = BitConverter.GetBytes(archive_size);
                fs.Write(bin_arch_size, 0, 4);
                pos_header += 4;
            }

            if ((version_archive == 4) || (version_archive >= 7))
            {
                if (version_archive == 4)
                {
                    fs.Write(priority, 0, 4);
                    pos_header += 4;
                    fs.Write(empty_bytes, 0, empty_bytes.Length);
                    pos_header += 4;
                }
                else
                {
                    fs.Write(priority, 0, 4);
                    pos_header += 4;
                    fs.Write(empty_bytes, 0, empty_bytes.Length);
                    pos_header += 4;

                    if (checkXmode.Checked == true)
                    {
                        empty_bytes = BitConverter.GetBytes(1);

                        fs.Write(empty_bytes, 0, empty_bytes.Length);
                        fs.Write(empty_bytes, 0, empty_bytes.Length);
                        pos_header += 4;
                        pos_header += 4;
                    }
                    else
                    {
                        empty_bytes = BitConverter.GetBytes(0);

                        fs.Write(empty_bytes, 0, empty_bytes.Length);
                        fs.Write(empty_bytes, 0, empty_bytes.Length);
                        pos_header += 4;
                        pos_header += 4;
                    }

                    empty_bytes = BitConverter.GetBytes(0);

                    byte[] block_sz = { 0x40 }; //Set block size 64KB for versions since 7
                    fs.Write(block_sz, 0, 1);
                    pos_header += 1;

                    if (version_archive == 7)
                    {
                        fs.Write(empty_bytes, 0, empty_bytes.Length - 1);
                        pos_header += 3;
                    }
                    else
                    {
                        fs.Write(empty_bytes, 0, empty_bytes.Length);
                        pos_header += 4;
                    }
                }


                if (version_archive == 9)
                {
                    fs.Write(header_crc32, 0, header_crc32.Length);
                    pos_header += 4;
                }
            }

            byte[] bin_header_size = BitConverter.GetBytes(header_size); //Set table size

            fs.Write(bin_header_size, 0, 4);
            pos_header += 4;
            fs.Write(table_files, 0, table_files.Length);
            pos_header += table_files.Length;

            progressBar1.Maximum = fi.Length;

            //Record files in archive
            for (int j = 0; j < fi.Length; j++)
            {
                FileStream fr = new FileStream(fi[j].FullName, FileMode.Open);
                byte[] file = new byte[fi[j].Length];
                fr.Read(file, 0, file.Length);

                if ((fi[j].Extension == ".lenc") || ((fi[j].Extension == ".lua") && (DontEncLua == false)))
                {
                    file = Methods.encryptLua(file, key, false, version_archive); //false - for oldest telltale's games. No need add some additional data
                }

                int meta = Methods.meta_crypt(file, key, version_archive, false); //Needs for oldest telltale's games

                if(meta != 0) compression = false;

                fs.Write(file, 0, file.Length);
                fr.Close();
                Progress(j + 1);
            }
            fs.Close();
            

            //Just rename temporary file if archive is not compressed
                if (compression == false)
                {
                    if (File.Exists(Application.StartupPath + "\\temp.file"))
                    {
                        if (File.Exists(output_path)) File.Delete(output_path);
                        File.Move(Application.StartupPath + "\\temp.file", output_path);
                    }
                }
                else //Else begin compress archive
                {
                    progressBar1.Maximum = fi.Length;

                    int hz3 = 2;
                    hz2 = BitConverter.GetBytes(hz3); //Change unknown data from 1 by 2

                    uint ca_size = 0; //Size of compressed archive
                    int sct = 0; //start compressed table

                    byte[] binCompressedTS = new byte[4];

                    if (version_archive >= 7)
                    {
                        switch (version_archive)
                        {
                            case 7:
                            case 8:
                                table_files = compressAlgorithm == 0 ? ZlibCompressor(table_files) : DeflateCompressor(table_files);
                                binCompressedTS = BitConverter.GetBytes(table_files.Length);
                                break;
                            default:
                                table_files = DeflateCompressor(table_files);
                                binCompressedTS = BitConverter.GetBytes(table_files.Length);
                                break;
                        }
                    }

                    FileStream fa = new FileStream(MainMenu.settings.archivePath, FileMode.Create);
                    fa.Write(archive_version, 0, 4);
                    sct += 4;
                    fa.Write(encrypt, 0, 4);
                    sct += 4;
                    fa.Write(hz1, 0, 4);
                    sct += 4;
                    fa.Write(hz2, 0, 4);
                    sct += 4;


                    FileStream file_reader = new FileStream(Application.StartupPath + "\\temp.file", FileMode.Open);
                    int blocks = Convert.ToInt32(pad_it(Convert.ToUInt64(file_reader.Length - pos_header), Convert.ToUInt64(0x10000))) / 0x10000;

                    byte[] binBlocks = BitConverter.GetBytes(blocks);

                    int compressed_block_size = blocks * 4 + 4 + 4;
                    byte[] compressed_blocks_header = new byte[compressed_block_size];
                    Array.Copy(binBlocks, 0, compressed_blocks_header, 0, 4);
                    int poz = 4;

                    byte[] binTableSize = new byte[4];
                    binTableSize = BitConverter.GetBytes(table_size);
                        

                    fa.Write(compressed_blocks_header, 0, compressed_blocks_header.Length);

                    if (version_archive >= 4)
                    {
                        fa.Write(priority, 0, 4);
                        fa.Write(empty_bytes, 0, 4);
                    }

                    if (version_archive >= 7)
                    {
                        if (checkXmode.Checked == true)
                        {
                            empty_bytes = new byte[4];
                            empty_bytes = BitConverter.GetBytes(1);

                            fa.Write(empty_bytes, 0, empty_bytes.Length);
                            fa.Write(empty_bytes, 0, empty_bytes.Length);
                            empty_bytes = new byte[4];
                            empty_bytes = BitConverter.GetBytes(0);
                        }
                        else
                        {
                            empty_bytes = new byte[4];
                            empty_bytes = BitConverter.GetBytes(0);

                            fa.Write(empty_bytes, 0, empty_bytes.Length);
                            fa.Write(empty_bytes, 0, empty_bytes.Length);
                        }

                        byte[] block_sz = version_archive == 7 ? BitConverter.GetBytes(0x80) : BitConverter.GetBytes(0x40);
                        fa.Write(block_sz, 0, 1);

                        if (version_archive == 7)
                        {
                            fa.Write(empty_bytes, 0, 3);
                        }
                        else fa.Write(empty_bytes, 0, empty_bytes.Length);
                    }

                    if (version_archive == 9)
                    {
                        fa.Write(header_crc32, 0, header_crc32.Length);
                    }

                    fa.Write(binTableSize, 0, 4);

                    if (version_archive >= 7)
                    {
                        fa.Write(binCompressedTS, 0, 4);
                        fa.Write(table_files, 0, table_files.Length);
                    }
                    else fa.Write(table_files, 0, table_files.Length);


                    progressBar1.Maximum = blocks;
                    file_reader.Seek(pos_header, SeekOrigin.Begin);

                    for (int j = 0; j < blocks; j++)
                    {
                        byte[] blockArchive = version_archive == 7 ? new byte[0x20000] : new byte[0x10000];

                        file_reader.Read(blockArchive, 0, blockArchive.Length);
                        byte[] compressed_block;
                        if (version_archive >= 8) compressed_block = version_archive == 8 && compressAlgorithm == 0 ? ZlibCompressor(blockArchive) : DeflateCompressor(blockArchive);
                        else compressed_block = ZlibCompressor(blockArchive);

                        if (EncryptIt.Checked == true)
                        {
                            compressed_block = encryptFunction(compressed_block, key, version_archive);
                        }

                        fa.Write(compressed_block, 0, compressed_block.Length);

                        uint cbs = Convert.ToUInt32(compressed_block.Length);
                        ca_size += cbs;

                        byte[] binSize = BitConverter.GetBytes(cbs);
                        Array.Copy(binSize, 0, compressed_blocks_header, poz, 4);
                        poz += 4;
                        Progress(j + 1);
                    }

                    byte[] binCASize = new byte[4];
                    binCASize = BitConverter.GetBytes(ca_size);
                    Array.Copy(binCASize, 0, compressed_blocks_header, poz, 4);
                    fa.Seek(sct, SeekOrigin.Begin);
                    fa.Write(compressed_blocks_header, 0, compressed_blocks_header.Length);
                    fa.Close();
                    file_reader.Close();
                    if (File.Exists(Application.StartupPath + "\\temp.file") == true) File.Delete(Application.StartupPath + "\\temp.file");
                }*/
            #endregion
        }
    

        private void ArchivePacker_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < MainMenu.gamelist.Count(); i++)
            {
                comboGameList.Items.Add(i + ". " + MainMenu.gamelist[i].gamename);
            }

            compressionCB.Items.Add("Deflate");
            compressionCB.Items.Add("Oodle LZ");
            compressionCB.SelectedIndex = 0;
            
            newEngineLua.Checked = MainMenu.settings.encNewLua;
            checkCompress.Checked = MainMenu.settings.compressArchive;
            checkXmode.Checked = MainMenu.settings.oldXmode;
            DontEncLuaCheck.Checked = MainMenu.settings.encryptLuaInArchive;
            EncryptIt.Checked = MainMenu.settings.encArchive;
            if (MainMenu.settings.inputDirPath != "") textBox1.Text = MainMenu.settings.inputDirPath;
            if (MainMenu.settings.archivePath != "") textBox2.Text = MainMenu.settings.archivePath;

            textBox3.Enabled = MainMenu.settings.customKey;
            int encKeyIndex = MainMenu.settings.versionArchiveIndex;
            if (MainMenu.settings.archiveFormat == 0) ttarchRB.Checked = true;
            else ttarch2RB.Checked = true;

            comboGameList.SelectedIndex = MainMenu.settings.encKeyIndex;
            versionSelection.SelectedIndex = encKeyIndex;


            if (MainMenu.settings.customKey && Methods.stringToKey(MainMenu.settings.encCustomKey) != null)
            {
                CheckCustomKey.Checked = MainMenu.settings.customKey;
                textBox3.Text = MainMenu.settings.encCustomKey;
            }
        }

        private void ttarchRB_CheckedChanged(object sender, EventArgs e)
        {
            versionSelection.Items.Clear();
            versionSelection.Items.Add("2");
            versionSelection.Items.Add("3");
            versionSelection.Items.Add("4");
            versionSelection.Items.Add("5");
            versionSelection.Items.Add("6");
            versionSelection.Items.Add("7");
            versionSelection.Items.Add("8");
            versionSelection.Items.Add("9");
            versionSelection.SelectedIndex = 0;

            checkXmode.Visible = true;
            checkXmode.Checked = MainMenu.settings.oldXmode;
            
            MainMenu.settings.archiveFormat = 0;
            MainMenu.settings.versionArchiveIndex = versionSelection.SelectedIndex;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void ttarch2RB_CheckedChanged(object sender, EventArgs e)
        {
            versionSelection.Items.Clear();
            versionSelection.Items.Add("1");
            versionSelection.Items.Add("2");
            versionSelection.SelectedIndex = 0;

            checkXmode.Visible = false;

            MainMenu.settings.archiveFormat = 1;
            MainMenu.settings.versionArchiveIndex = versionSelection.SelectedIndex;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;

                MainMenu.settings.inputDirPath = textBox1.Text;
                Settings.SaveConfig(MainMenu.settings);
            }
            else
            {
                textBox1.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ttarchRB.Checked)
            {
                sfd.Filter = "TTARCH archive (*.ttarch) | *.ttarch";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = sfd.FileName;
                    
                    MainMenu.settings.archivePath = textBox2.Text;
                    Settings.SaveConfig(MainMenu.settings);
                }
                else
                {
                    textBox2.Clear();
                }
            }
            else
            {
                sfd.Filter = "TTARCH2 archive (*.ttarch2) | *.ttarch2| Android archive (*.obb) | *.obb";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = sfd.FileName;

                    MainMenu.settings.archivePath = textBox2.Text;
                    Settings.SaveConfig(MainMenu.settings);
                }
                else
                {
                    textBox2.Clear();
                }
            }
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            if ((MainMenu.settings.inputDirPath != "") && (MainMenu.settings.archivePath != ""))
            {
                DirectoryInfo checkDI = new DirectoryInfo(MainMenu.settings.inputDirPath);
                if (checkDI.Exists)
                {
                    string example = "96CA99A085CF988AE4DBE2CDA6968388C08B99E39ED89BB6D790DCBEAD9D9165B6A69EBBC2C69EB3E7E3E5D5AB6382A09CC4929FD1D5A4";
                    
                    archiveVersion = Convert.ToInt32(versionSelection.SelectedItem);

                    byte[] keyEnc;
                    if(CheckCustomKey.Checked) keyEnc = Methods.stringToKey(textBox3.Text);
                    else keyEnc = MainMenu.gamelist[comboGameList.SelectedIndex].key;
                    
                    if((keyEnc == null) && (MainMenu.settings.encArchive || (MainMenu.settings.encryptLuaInArchive == false)))
                    {
                        if (!CheckCustomKey.Checked)
                        {
                            MainMenu.settings.encArchive = false;
                            MainMenu.settings.encryptLuaInArchive = true;
                        }
                        else
                        {
                            MessageBox.Show("Check string for correctly. Here's example of encryption key:\r\n" + example, "Error");
                            return;
                        }
                    }

                    if (Methods.GetExtension(MainMenu.settings.archivePath).ToLower() == ".obb") MainMenu.settings.compressArchive = false;

                    int algorithmCompress = 0;

                    if (ttarch2RB.Checked && (versionSelection.SelectedIndex == 1) && (compressionCB.SelectedIndex == 1)) algorithmCompress = 1;
                    else if (ttarchRB.Checked && (versionSelection.SelectedIndex == 5 || versionSelection.SelectedIndex == 6) && (compressionCB.SelectedIndex == 1)) algorithmCompress = 1;
                    else if (ttarchRB.Checked && versionSelection.SelectedIndex == 7) algorithmCompress = 1; //Latest version uses deflate algorithm

                    if (ttarchRB.Checked == true) builder_ttarch(MainMenu.settings.inputDirPath, MainMenu.settings.archivePath, keyEnc, MainMenu.settings.compressArchive, archiveVersion, MainMenu.settings.encArchive, MainMenu.settings.encryptLuaInArchive, algorithmCompress);
                    else builder_ttarch2(MainMenu.settings.inputDirPath, MainMenu.settings.archivePath, MainMenu.settings.compressArchive, keyEnc, MainMenu.settings.encryptLuaInArchive, archiveVersion, MainMenu.settings.encNewLua, algorithmCompress);
                }
                else MessageBox.Show("This folder doesn't exist!", "Error");
                
            }
            else MessageBox.Show("Check paths!", "Error");
        }

        private void ArchivePacker_FormClosing(object sender, FormClosingEventArgs e)
        {
            comboGameList.Items.Clear();
        }

        private void comboGameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainMenu.settings.encKeyIndex = comboGameList.SelectedIndex;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void newEngineLua_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.encNewLua = newEngineLua.Checked;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void CheckCustomKey_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.customKey = CheckCustomKey.Checked;
            textBox3.Enabled = MainMenu.settings.customKey;
            Settings.SaveConfig(MainMenu.settings);

            if ((MainMenu.settings.customKey == true) &&
               ((MainMenu.settings.encCustomKey != "") && (MainMenu.settings.encCustomKey != null)))
            {
                textBox3.Text = MainMenu.settings.encCustomKey;
            }
            else
            {
                textBox3.Text = "";
            }
        }

        private void DontEncLuaCheck_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.encryptLuaInArchive = DontEncLuaCheck.Checked;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void EncryptIt_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.encArchive = EncryptIt.Checked;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void checkCompress_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.compressArchive = checkCompress.Checked;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void checkXmode_CheckedChanged(object sender, EventArgs e)
        {
            MainMenu.settings.oldXmode = checkXmode.Checked;
            Settings.SaveConfig(MainMenu.settings);
        }

        private void versionSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ttarchRB.Checked)
            {
                compressionCB.Items.Clear();

                compressionCB.Items.Add("Zlib");
                compressionCB.Items.Add("Deflate");
                compressionCB.SelectedIndex = 0;

                compressionCB.Visible = versionSelection.SelectedIndex == 5 || versionSelection.SelectedIndex == 6;
                compressionLabel.Visible = versionSelection.SelectedIndex == 5 || versionSelection.SelectedIndex == 6;

                checkXmode.Visible = versionSelection.SelectedIndex >= 5;
                checkCompress.Visible = versionSelection.SelectedIndex >= 1;
                newEngineLua.Visible = false;
            }
            else
            {
                compressionCB.Items.Clear();
                compressionCB.Items.Add("Deflate");
                compressionCB.Items.Add("Oodle LZ");
                compressionCB.SelectedIndex = 0;

                compressionLabel.Visible = ttarch2RB.Checked && versionSelection.SelectedIndex == 1;
                compressionCB.Visible = ttarch2RB.Checked && versionSelection.SelectedIndex == 1;
                newEngineLua.Visible = true;
                checkXmode.Visible = false;
            }

            MainMenu.settings.versionArchiveIndex = versionSelection.SelectedIndex;
            Settings.SaveConfig(MainMenu.settings);
        }
    }
}
