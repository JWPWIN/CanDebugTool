using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

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
    static private readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    static private string GetSuffix(FileType type)
    {
        return type switch
        {
            FileType.Text => ".txt",
            FileType.C_Code => ".c",
            FileType.C_Head => ".h",
            FileType.DBC => ".dbc",
            FileType.XML => ".xml",
            _ => ".txt"
        };
    }

    /// <summary>
    /// 弹出目录选择并写入文件。成功返回 true；取消或失败返回 false。
    /// DBC/文本统一按 UTF-8（无 BOM）写入。
    /// </summary>
    static public bool WriteData(string fileName, FileType type, string content)
    {
        string suffix = GetSuffix(type);

        using FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
        folderBrowser.Description = "请选择保存目录";

        if (folderBrowser.ShowDialog() != DialogResult.OK)
            return false;

        string selectedPath = folderBrowser.SelectedPath;
        return WriteData(selectedPath, fileName, type, content);
    }

    /// <summary>
    /// 写内容到指定路径。成功返回 true。
    /// </summary>
    static public bool WriteData(string path, string fileName, FileType type, string content)
    {
        if (string.IsNullOrEmpty(path) || content is null)
            return false;

        try
        {
            string suffix = GetSuffix(type);
            string fullPath = Path.Combine(path, fileName + suffix);
            File.WriteAllText(fullPath, content, Utf8NoBom);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("写入文件失败：\n" + ex.Message, "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    /// <summary>弹出文件选择框，选择 DBC；取消返回 null。</summary>
    static public string PickDbcFile()
    {
        using OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "DBC Files (*.dbc)|*.dbc|All Files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.Multiselect = false;
        return openFileDialog.ShowDialog() == DialogResult.OK
            ? openFileDialog.FileName
            : null;
    }

    /// <summary>以 UTF-8 读取文本文件（可在后台线程调用）。</summary>
    static public string ReadFileUtf8(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            return null;
        return File.ReadAllText(filePath, Utf8NoBom);
    }

    /// <summary>读取 DBC（UTF-8 / BOM / GB18030 回退），可在后台线程调用。</summary>
    static public string ReadDbcText(string filePath) => DbcTextReader.ReadDbcFile(filePath);

    /// <summary>
    /// 弹出对话框读取文本（兼容旧入口）。DBC 建议用 PickDbcFile + ReadFileUtf8。
    /// </summary>
    static public string ReadData()
    {
        string path = PickDbcFile();
        if (path is null)
            return string.Empty;
        return ReadDbcText(path) ?? string.Empty;
    }
}
