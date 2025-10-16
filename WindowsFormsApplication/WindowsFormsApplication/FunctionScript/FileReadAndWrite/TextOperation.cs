using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

public enum FileType
{ 
    Text,
    C_Code,
    C_Head,
    DBC,
    XML
}

public class TextOperation
{
    /// <summary>
    /// 写内容到选择文档中
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="type">文件类型-后缀</param>
    /// <param name="content">文件具体内容</param>
    static public void WriteData(string fileName, FileType type, string content)
    {
        StreamWriter writer;
        string suffix = ""; //文件名后缀
        if (type == FileType.Text)
        {
            suffix = ".txt";
        }
        else if (type == FileType.C_Code)
        {
            suffix = ".c";
        }
        else if (type == FileType.C_Head)
        {
            suffix = ".h";
        }
        else if (type == FileType.DBC)
        {
            suffix = ".dbc";
        }
        else if (type == FileType.XML)
        {
            suffix = ".xml";
        }
        else
        {
            suffix = ".txt";
        }

            //选取保存文件路径
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
        folderBrowser.SelectedPath = ".";
        folderBrowser.Description = "请选择保存目录";

        if (folderBrowser.ShowDialog() == DialogResult.OK)
        {
            string selectedPath = folderBrowser.SelectedPath;

            FileInfo file = new FileInfo(selectedPath + "\\" + fileName + suffix);
            if (!file.Exists)
            {
                writer = file.CreateText();//创建写入新文本文件的StreamWriter
            }
            else
            {
                //删除后新建
                file.Delete();
                file.Refresh();
                writer = file.CreateText();
            }
            writer.Write(content);
            writer.Flush();
            writer.Dispose();
            writer.Close();
        }
    }

    /// <summary>
    /// 写内容到指定路径文档中
    /// </summary>
    /// <param name="path">文件保存路径</param>
    /// <param name="fileName">文件名</param>
    /// <param name="type">文件类型-后缀</param>
    /// <param name="content">文件具体内容</param>
    static public void WriteData(string path, string fileName, FileType type, string content)
    {
        StreamWriter writer;
        string suffix = ""; //文件名后缀
        if (type == FileType.Text)
        {
            suffix = ".txt";
        }
        else if (type == FileType.C_Code)
        {
            suffix = ".c";
        }
        else if (type == FileType.C_Head)
        {
            suffix = ".h";
        }

        if (path != null)
        {
            string selectedPath = path;

            FileInfo file = new FileInfo(selectedPath + "\\" + fileName + suffix);
            if (!file.Exists)
            {
                writer = file.CreateText();//创建写入新文本文件的StreamWriter
            }
            else
            {
                //删除后新建
                file.Delete();
                file.Refresh();
                writer = file.CreateText();
            }
            writer.Write(content);
            writer.Flush();
            writer.Dispose();
            writer.Close();
        }
    }

    /// <summary>
    /// 读取text中的数据
    /// </summary>
    static public string ReadData()
    {
        string allData = "";
        //选取读取文件路径
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Txt Files (*.txt)|*.txt";
        openFileDialog.FilterIndex = 1;
        openFileDialog.Multiselect = false;

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string selectedFile = openFileDialog.FileName;
            //reader的获取方式有两种
            //第一种
            StreamReader reader = new StreamReader(selectedFile, Encoding.GetEncoding("gb2312"));

            //第二种
            //FileInfo file = new FileInfo(Application.dataPath + "/mytxt.txt");
            //reader = file.OpenText();//创建使用UTF8编码、从现有文本文件中进行读取的StreamReader

            allData = reader.ReadToEnd();
            if (allData == null)
            {
                MessageBox.Show("没有数据");
            }
            reader.Dispose();
            reader.Close();

        }

        return allData;
    }
}