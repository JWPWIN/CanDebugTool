using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OfficeOpenXml;
using System.IO;
using Excel;
using System.Data;

public class ExcelReadAndWrite
{
    /// <summary>
    /// 举例：创建一个excel表
    /// </summary>
    /// <param name="selPath">选择创建文件的路径</param>
    static public void CreatExcel(string selPath, string[,] data, uint dataRow, uint dataColumn)
    {
        //文件地址
        FileInfo newFile = new FileInfo(selPath + "/CanDbcCfg.xlsx");

        //如果文件存在删除重建
        if (newFile.Exists)
        {
            newFile.Delete();
            newFile = new FileInfo(selPath + "/CanDbcCfg.xlsx");
        }

        //数据操作
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            //初次创建增加数据操作（重点在于这条操作语句不同）
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("CanDbcCfg");

            for (int i = 0; i < dataRow; i++)
            {
                for (int j = 0; j < dataColumn; j++) 
                {
                    worksheet.Cells[i+1, j+1].Value = data[i,j];

                }

            }

            ////添加对应列名
            //worksheet.Cells[1, 1].Value = "列名1";
            //worksheet.Cells[1, 2].Value = "列名2";
            //worksheet.Cells[1, 3].Value = "列名3";

            //保存
            package.Save();
        }
    }

    /// <summary>
    /// 读取excel文件数据
    /// </summary>
    /// <param name="selPath">读取excel的路径</param>
    /// <returns>返回读取的excel的数据</returns>
    static public DataSet ReadExcel(string selPath)
    {
        //加载文件
        FileStream fileStream = File.Open(selPath, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        DataSet result = excelDataReader.AsDataSet();
        fileStream.Close();
        return result;
    }

    /// <summary>
    /// 举例：向一个sheet表中写入数据
    /// </summary>
    public void WriteExcel(string selPath)
    {
        //文件地址
        FileInfo newFile = new FileInfo(selPath + "/test.xlsx");
        //数据操作
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            //增加数据操作（重点在于这条操作语句与初次创建添加数据不同）
            ExcelWorksheet worksheet = package.Workbook.Worksheets["test"];

            //添加第二行数据
            worksheet.Cells[2, 1].Value = "名称1";
            worksheet.Cells[2, 2].Value = "价格1";
            worksheet.Cells[2, 3].Value = "销量1";

            //添加第三行数据
            worksheet.Cells[3, 1].Value = "名称2";
            worksheet.Cells[3, 2].Value = "价格2";
            worksheet.Cells[4, 3].Value = "销量2";

            //保存
            package.Save();
        }
    }

    /// <summary>
    /// 举例：修改表中数据
    /// </summary>
    public void ChangeExcel(string selPath)
    {
        //文件地址
        FileInfo newFile = new FileInfo(selPath + "/test.xlsx");

        //数据操作
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets["test"];

            //追加
            worksheet.Cells[4, 1].Value = "名称3";
            worksheet.Cells[4, 2].Value = "价格3";
            worksheet.Cells[4, 3].Value = "销量3";

            //删除某一列（参数：列的序号）
            worksheet.DeleteColumn(1);
            //删除某一行（参数：行的序号）
            worksheet.DeleteRow(1);

            //修改（和添加一样）
            worksheet.Cells[4, 1].Value = "修改名称";
            worksheet.Cells[4, 2].Value = "修改价格";
            worksheet.Cells[4, 3].Value = "修改销量";


            //保存
            package.Save();
        }
    }

}
