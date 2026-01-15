using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

public class ExcelManager
{
    public ExcelManager()
    {
        //construction
    }

    static public Dictionary<string, List<List<string>>> ImportData()
    {
        //excel总数据信息: Dictionary<sheet表名, sheet表内容>
        Dictionary<string, List<List<string>>> excelAllData = new Dictionary<string, List<List<string>>>();

        //选取读取文件路径
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Excel Files (*.xlsx; *.xls)|*.xlsx; *.xls";
        openFileDialog.FilterIndex = 1;
        openFileDialog.Multiselect = false;

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string selectedFile = openFileDialog.FileName;
            //MessageBox.Show($"您选中的文件路径为：{selectedFile}");
            ExcelPackage.License.SetNonCommercialPersonal("My Name");

            using (var package = new ExcelPackage(new System.IO.FileInfo(selectedFile)))
            {
                for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                {
                    // 获取工作簿的工作表
                    var worksheet = package.Workbook.Worksheets[i];

                    //如果sheet表名第一个字符为#，则跳过数据不读取
                    if (worksheet.Name[0] == '#')
                    {
                        continue;
                    }

                    //excel单个sheet数据信息:
                    List<List<string>> excelData = new List<List<string>>();

                    // 获取工作表的行数和列数
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    // 循环遍历工作表中的数据
                    for (int row = 0; row < rowCount; row++)
                    {
                        List<string> rowData = new List<string>();
                        for (int col = 0; col < colCount; col++)
                        {
                            if (worksheet.Cells[row + 1, col + 1].Value != null)
                            {
                                rowData.Add(worksheet.Cells[row + 1, col + 1].Value.ToString());
                            }
                            else
                            {
                                rowData.Add("");
                            }
                        }
                        excelData.Add(rowData);
                    }

                    excelAllData.Add(worksheet.Name, excelData);
                }

            }
            return excelAllData;
        }


        return null;
    }

    static public void ExportData(List<List<string>> dataList, List<string> titleList)
    {
        if (dataList == null || titleList == null) { return; }//表头或数据为空 退出
        if (dataList[0].Count != titleList.Count) { return; }//表头和数据列不一致 退出

        // 创建 SaveFileDialog 对象
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        // 设置对话框标题
        saveFileDialog.Title = "保存文件";

        // 设置文件过滤器
        saveFileDialog.Filter = "Excel Files (*.xlsx; *.xls)|*.xlsx; *.xls";

        // 显示对话框并检查用户是否点击了“保存”
        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            string filePath = saveFileDialog.FileName;

            // 创建一个新的Excel包
            FileInfo file = new FileInfo(filePath);
            ExcelPackage.License.SetNonCommercialPersonal("My Name");

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                //写入第一行表头
                for (int col = 0; col < titleList.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = titleList[col];
                }

                // 写入每行数据
                int rowCount = dataList.Count;
                int colCount = dataList[0].Count;

                for (int row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < colCount; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = dataList[row][col];
                    }
                }
                // 保存Excel文件
                package.Save();

                MessageBox.Show("写入到Excel文件成功！！！");
            }
        }

    }
}

