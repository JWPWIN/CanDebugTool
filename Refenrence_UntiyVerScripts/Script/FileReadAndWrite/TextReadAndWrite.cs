using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public enum FileType
{ 
    Text,
    C_Code,
    C_Head
}

public class TextReadAndWrite
{
    //StreamWriter writer;
    //StreamReader reader;
    //List<string> allData;
    //public void Example()
    //{
    //    FileInfo file = new FileInfo(Application.dataPath + "/mytxt.txt");
    //    if (file.Exists)//存在txt，删除以前的内容
    //    {
    //        file.Delete();
    //        file.Refresh();//刷新对象的状态
    //    }
    //    for (int i = 0; i < 4; i++)
    //    {
    //        WriteData("记录数据：" + i);
    //    }
    //    allData = new List<string>();
    //    //ReadData();
    //    for (int i = 0; i < allData.Count; i++)
    //    {
    //        Debug.Log("读取数据：" + allData[i]);
    //    }
    //}

    /// <summary>
    /// 写内容到文档中
    /// </summary>
    /// <param name="path">文件保存路径</param>
    /// <param name="fileName">文件名</param>
    /// <param name="type">文件类型-后缀</param>
    /// <param name="content">文件具体内容</param>
    static public void WriteData(string path,string fileName, FileType type, string content)
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

        FileInfo file = new FileInfo(path + "\\" + fileName + suffix);
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
    /// <summary>
    /// 读取text中的数据
    /// </summary>
    static public string ReadData(string path)
    {
        //reader的获取方式有两种
        //第一种
        StreamReader reader = new StreamReader(path, Encoding.GetEncoding("gb2312"));
        string allData;
        //第二种
        //FileInfo file = new FileInfo(Application.dataPath + "/mytxt.txt");
        //reader = file.OpenText();//创建使用UTF8编码、从现有文本文件中进行读取的StreamReader
         
        allData = reader.ReadToEnd();
        if (allData == null)
        {
            Debug.Log("没有数据");
        }

        reader.Dispose();
        reader.Close();

        return allData;
    }
}