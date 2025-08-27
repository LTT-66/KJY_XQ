using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MeasureMachine
{
    class NPOIHelper
    {
        IWorkbook workbookin = null;//import excel data to datatable 
        IWorkbook workbookout = null;//export data to excel file
        IWorkbook workbook_renewout = null;//renew data to excel file
        bool Is2007 = false; //.xlsx or .xls
        public string filename = "";
        public string str_NewFileName = "";

        public void InitWorkbookOpen(string path)
        {

            using (FileStream fsin = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (path.IndexOf(".xlsx") > 0) // 2007版本
                { workbookin = new XSSFWorkbook(fsin); Is2007 = true; }
                else if (path.IndexOf(".xls") > 0) // 2003版本
                { workbookin = new HSSFWorkbook(fsin); Is2007 = false; }
                fsin.Close();
            }
        }

        public void  InitWorkbookOut(string path)
        {
            if (path.IndexOf(".xlsx") > 0) // 2007版本
                workbookout = new XSSFWorkbook();
            else if (path.IndexOf(".xls") > 0) // 2003版本
                workbookout = new HSSFWorkbook();
        }

        public void InitWorkbookRenewOut(string path,string newPath)
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (path.IndexOf(".xlsx") > 0) // 2007版本
                    workbook_renewout = new XSSFWorkbook(fs);
                else if (path.IndexOf(".xls") > 0) // 2003版本
                    workbook_renewout = new HSSFWorkbook(fs);
                fs.Close();

                str_NewFileName = newPath;
            }
        }
        /**********************************************************************************************************
         * ExcelToDataTable 
         * 
         * 导出excel文件结果是全部字符串  
         * give the file path and provide column count                              从EXCEL导入到DataTable 返回都是字符串型
         ***********************************************************************************************************/
        public DataTable ExcelToDataTable(string path, string sheetname, int ColumCount, bool IgnoreFirstRow)
        {

            InitWorkbookOpen(path); //initialize workbook
            
            ISheet sheet =null;
            if(sheetname==null)
             sheet = workbookin.GetSheetAt(0);
            else
            sheet = workbookin.GetSheet(sheetname);

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            DataTable dt = new DataTable();

            if (ColumCount == 0)
            {
                IRow row0 = null;
                int max_coloumncount = 0;
                while (rows.MoveNext())
                {
                    if (Is2007)
                        row0 = (XSSFRow)rows.Current;
                    else
                        row0 = (HSSFRow)rows.Current;
                        if (row0.LastCellNum > max_coloumncount)
                            max_coloumncount = row0.LastCellNum;
                }
                rows.Reset();
              
                for (int j = 0; j < max_coloumncount; j++)
                {
                    dt.Columns.Add(( j+1).ToString());
                }
            }
            else
            for (int j = 0; j < ColumCount; j++)
            {
                dt.Columns.Add( (j+1).ToString());
            }

            if (IgnoreFirstRow) rows.MoveNext();

            while (rows.MoveNext())
            {

                IRow row = null;

                if (Is2007)
                    row = (XSSFRow)rows.Current;
                else
                    row = (HSSFRow)rows.Current;

                DataRow dr = dt.NewRow();
                if (ColumCount == 0)
                {
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        ICell cell = row.GetCell(i);

                        if (cell == null)
                        {
                            dr[i] = DBNull.Value;
                        }
                        else
                        {
                            dr[i] = cell.ToString();


                        }
                    }
                    
                }
               else
                for (int i = 0; i < ColumCount; i++)
                {
                    ICell cell = row.GetCell(i);

                    if (cell == null)
                    {
                        dr[i] = DBNull.Value;
                    }
                    else
                    {
                        dr[i] = cell.ToString();


                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /**********************************************************************************************************
        * ExcelRowToDataRow
        * 
        *  
        * give the file path and provide column count                              从EXCEL的一行导入到DataRow  返回的都是字符串型
        ***********************************************************************************************************/
        public DataRow ExcelRowToDataRow(string path, string sheetname, int ColumCount, int ExcelRowNum,ref int errnum)
        {
            DataTable dt = new DataTable();
            for (int j = 0; j < ColumCount; j++)
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
            }
            DataRow dtrow = dt.NewRow();
            InitWorkbookOpen(path); //initialize workbook

            ISheet sheet = null;
            if (sheetname == null)
                sheet = workbookin.GetSheetAt(0);
            else
                sheet = workbookin.GetSheet(sheetname);

            if (sheet == null) errnum = -1;//表不存在
            else
            {
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                if (ExcelRowNum == 1) { rows.MoveNext(); }
                else
                {
                    int k = 1;
                    while (rows.MoveNext())
                    {
                       
                        if (k == ExcelRowNum) break;
                        k++;
                    }
                }

                IRow row = null;

                if (Is2007)
                    row = (XSSFRow)rows.Current;
                else
                    row = (HSSFRow)rows.Current;


                for (int i = 0; i < ColumCount; i++)
                {
                    ICell cell = row.GetCell(i);

                    if (cell == null)
                    {
                        dtrow[i] = DBNull.Value;
                    }
                    else
                    {
                        dtrow[i] = cell.ToString();

                    }
                }
                errnum = 0;
            }
            return dtrow;
        }
        /**********************************************************************************************************
             *  ExcelToDataTable
             * 
             *                                                           从EXCEL导入到DataTable 返回具体类型 没有验证文件
             * 导出excel文件结果是对应类型 string[] ColumnsName, string[] ColumnsType
             ***********************************************************************************************************/
        public DataTable ExcelToDataTable(string path, string sheetname, int ColoumCount, string[] ColumnsName, string[] ColumnsType, bool IgnoreFirstRow,ref int  errnum)//列类型
        {
            errnum = 0;//返回出错行号
            InitWorkbookOpen(path);

            ISheet sheet = null;
            if (sheetname == null)
                sheet = workbookin.GetSheetAt(0);
            else
                sheet = workbookin.GetSheet(sheetname);

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            DataTable dt = new DataTable();

            for (int j = 0; j < ColoumCount; j++)
            {
                dt.Columns.Add(ColumnsName[j], Type.GetType(ColumnsType[j]));
            }
            if (IgnoreFirstRow) rows.MoveNext();
            while (rows.MoveNext())
            {
                errnum++;
                IRow row = null;

                if (Is2007)
                    row = (XSSFRow)rows.Current;
                else
                    row = (HSSFRow)rows.Current;

                DataRow dr = dt.NewRow();

                //for (int i = 0; i < row.LastCellNum; i++)
                for (int i = 0; i < ColoumCount; i++)
                {
                    ICell cell = row.GetCell(i);

                    if (cell == null)
                    {
                        dr[i] = DBNull.Value;
                    }
                    else
                    {
                        // dr[i] = cell.ToString();
                        switch (cell.CellType)
                        {
                            case CellType.Blank: //空
                                dr[i] = DBNull.Value;
                                break;
                            case CellType.String: //字符串
                                dr[i] = cell.StringCellValue;
                                break;
                            case CellType.Numeric: //数字                                

                                if (DateUtil.IsCellDateFormatted(cell))
                                {
                                    dr[i] = cell.DateCellValue;//时间

                                }
                                else
                                {
                                    dr[i] = cell.NumericCellValue;//数字
                                }
                                break;
                            case CellType.Formula://公式
                                {
                                    if (Is2007)
                                    {
                                        XSSFFormulaEvaluator e = new XSSFFormulaEvaluator(workbookin);
                                        dr[i] = e.Evaluate(cell).NumberValue;
                                    }
                                    else
                                    {
                                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(workbookin);
                                        dr[i] = e.Evaluate(cell).NumberValue;
                                    }

                                }
                                break;
                            default:
                                dr[i] = DBNull.Value;
                                break;
                        }

                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /**********************************************************************************************************
             *  ExcelToDataTable
             * 
             *                                                           从EXCEL导入到DataTable 返回具体类型 验证文件
             * 导出excel文件结果是对应类型 string[] ColumnsName, string[] ColumnsType
             ***********************************************************************************************************/
        public DataTable ExcelToDataTable2(string path, string sheetname, int ColumnCount, string[] ColumnsName, string[] ColumnsType, int verifycolumnnum,string verifycolumnname, ref int errnum)//列类型
        {
            DataTable dt = new DataTable();
            errnum = 0;//返回出错行号
            InitWorkbookOpen(path);
            bool checknull = true;//检查全行是否都是空null;
            // ISheet sheet = workbookin.GetSheetAt(sheetnumber);
            ISheet sheet = workbookin.GetSheet(sheetname);
            if (sheet == null) errnum = -1;//文件名称错误
            else
            {
              
                IRow row0 = sheet.GetRow(0);//取行
                if (row0 == null) errnum = -2;//文件内容不正确 空表
                else
                {
                    ICell cell0 = row0.GetCell(verifycolumnnum);//取列
                    string cellstring ="";
                    if (cell0 == null) errnum = -2;//文件内容不正确
                    else
                    cellstring = cell0.ToString();
                    if (cellstring != verifycolumnname) errnum = -2;//文件内容不正确
                    else
                    {
                        //验证正确后
                        errnum = 0;

                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                        for (int j = 0; j < ColumnCount; j++)
                        {
                            dt.Columns.Add(ColumnsName[j], Type.GetType(ColumnsType[j]));
                        }
                        rows.MoveNext();//忽略第一行
                        while (rows.MoveNext())
                        {
                            checknull = true;

                            errnum++;
                            IRow row = null;

                            if (Is2007)
                                row = (XSSFRow)rows.Current;
                            else
                                row = (HSSFRow)rows.Current;

                            DataRow dr = dt.NewRow();

                            //for (int i = 0; i < row.LastCellNum; i++)
                            for (int i = 0; i < ColumnCount; i++)
                            {
                                ICell cell = row.GetCell(i);

                                if ((cell == null)||(cell.CellType==CellType.Blank))
                                {
                                    dr[i] = DBNull.Value;
                                }
                                else
                                {
                                    checknull = false;
                                    // dr[i] = cell.ToString();
                                    switch (cell.CellType)
                                    {
                                       
                                        case CellType.String: //字符串
                                            dr[i] = cell.StringCellValue;
                                            break;
                                        case CellType.Numeric: //数字                                

                                            if (DateUtil.IsCellDateFormatted(cell))
                                            {
                                                dr[i] = cell.DateCellValue;//时间

                                            }
                                            else
                                            {
                                                dr[i] = cell.NumericCellValue;//数字
                                            }
                                            break;
                                        case CellType.Formula://公式
                                            {
                                                if (Is2007)
                                                {
                                                    XSSFFormulaEvaluator e = new XSSFFormulaEvaluator(workbookin);
                                                    dr[i] = e.Evaluate(cell).NumberValue;
                                                }
                                                else
                                                {
                                                    HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(workbookin);
                                                    dr[i] = e.Evaluate(cell).NumberValue;
                                                }

                                            }
                                            break;
                                        default:
                                            dr[i] = DBNull.Value;
                                            break;
                                    }

                                }
                            }
                           if(!checknull)//或略全空的行
                            dt.Rows.Add(dr);


                        }
                    }
                }
            }
            return dt;
        }
       
        /**********************************************************************************************************
         * DtToExcelWorkbook
         * next step，DtToExcelFsWrite();
         *                                           DataTable   导出到EXCEL
         ***********************************************************************************************************/
        public void DtToExcelWorkbook(string Path, DataTable dt)
        {

            filename = Path;
            InitWorkbookOut(Path);

            ICellStyle dateStyle = (ICellStyle)workbookout.CreateCellStyle();
            IDataFormat format = (IDataFormat)workbookout.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:SS");

            ISheet sheet = workbookout.CreateSheet("sheet1");


            int rowcount = 0;

            foreach (DataRow viewrow in dt.Rows)
            {
                IRow row = sheet.CreateRow(rowcount);

                foreach (DataColumn viewcolumn in dt.Columns)
                {
                    ICell cell = row.CreateCell(viewcolumn.Ordinal);

                    string drValue = viewrow[viewcolumn].ToString();

                    if (drValue == "") { }
                    else
                        switch (viewcolumn.DataType.ToString())
                        {
                            case "System.String"://字符串类型
                                cell.SetCellValue(drValue);
                                break;
                            case "System.DateTime"://日期类型
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                cell.SetCellValue(dateV);
                                cell.CellStyle = dateStyle;//格式化显示
                                break;
                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                cell.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                cell.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                cell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理
                                cell.SetCellValue("");
                                break;
                            default:
                                cell.SetCellValue("");
                                break;
                        }

                }
                rowcount++;

            }
        }
        /**********************************************************************************************************
          * DtToExcelFsWrite
          * 
          *                                                   到本地 workbookout.Write(fsout)
          ***********************************************************************************************************/
        public void DtToExcelFsWrite()
        {
            using (FileStream fsout = new FileStream(filename, FileMode.OpenOrCreate))
            {
                workbookout.Write(fsout);
                fsout.Close();
            }

        }
        /**********************************************************************************************************
            * DtToExcelWorkbook
            * next step，DtToExcelFsWrite();
            *                             DataTable 导出到EXCEL  int timecolumn
            ***********************************************************************************************************/
        public void DtToExcelWorkbook(string Path, DataTable dt,string sheetname, int timecolumn)
        {

            filename = Path;
            InitWorkbookOut(Path);

            ICellStyle dateStyle = (ICellStyle)workbookout.CreateCellStyle();
            dateStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            dateStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;

            IDataFormat format = (IDataFormat)workbookout.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:SS");



            ISheet sheet = workbookout.CreateSheet(sheetname);

            for(int i=0;i< dt.Columns.Count;i++)
                sheet.SetColumnWidth(i, 6000);//列宽
            //sheet.SetColumnWidth(timecolumn, 6000);//时间列宽

            int rowcount = 0;//导出标题
           
                IRow row0 = sheet.CreateRow(rowcount);

                foreach (DataColumn viewcolumn0 in dt.Columns)
                {
                    ICell cell0 = row0.CreateCell(viewcolumn0.Ordinal); 
                    string drValue = viewcolumn0.ColumnName.ToString();
                    cell0.SetCellValue(drValue);
                }
            
            rowcount = 1;//以下导出内容

            foreach (DataRow viewrow in dt.Rows)
            {
                IRow row = sheet.CreateRow(rowcount);

                foreach (DataColumn viewcolumn in dt.Columns)
                {
                    ICell cell = row.CreateCell(viewcolumn.Ordinal);

                    cell.CellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;

                    string drValue = viewrow[viewcolumn].ToString();
                   
                    if (drValue == "") { }
                    else
                        switch (viewcolumn.DataType.ToString())
                        {
                            case "System.String"://字符串类型
                                cell.SetCellValue(drValue);
                                break;
                            case "System.DateTime"://日期类型
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                cell.SetCellValue(dateV);
                                cell.CellStyle = dateStyle;//格式化显示
                                break;
                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                cell.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                cell.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                cell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理
                                cell.SetCellValue("");
                                break;
                            default:
                                cell.SetCellValue("");
                                break;
                        }

                }
                rowcount++;

            }
        }

        /**********************************************************************************************************
            * DtToExcelWorkbookM
            * next step，DtToExcelFsWrite();
            *                             DataTable 导出到EXCEL  int timecolumn  多页 先InitWorkbookOut(string path)
            ***********************************************************************************************************/

        public void DtToExcelWorkbookM(DataTable dt, string sheetname, int timecolumn)
        {
             
            ICellStyle dateStyle = (ICellStyle)workbookout.CreateCellStyle();
            dateStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            dateStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;

            IDataFormat format = (IDataFormat)workbookout.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:SS");



            ISheet sheet = workbookout.CreateSheet(sheetname);

            sheet.SetColumnWidth(timecolumn, 6000);//时间列宽

            int rowcount = 0;//导出标题

            IRow row0 = sheet.CreateRow(rowcount);

            foreach (DataColumn viewcolumn0 in dt.Columns)
            {
                ICell cell0 = row0.CreateCell(viewcolumn0.Ordinal);

                string drValue = viewcolumn0.ColumnName.ToString();
                cell0.SetCellValue(drValue);
            }

            rowcount = 1;//以下导出内容

            foreach (DataRow viewrow in dt.Rows)
            {
                IRow row = sheet.CreateRow(rowcount);

                foreach (DataColumn viewcolumn in dt.Columns)
                {
                    ICell cell = row.CreateCell(viewcolumn.Ordinal);

                    cell.CellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;

                    string drValue = viewrow[viewcolumn].ToString();

                    if (drValue == "") { }
                    else
                        switch (viewcolumn.DataType.ToString())
                        {
                            case "System.String"://字符串类型
                                cell.SetCellValue(drValue);
                                break;
                            case "System.DateTime"://日期类型
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                cell.SetCellValue(dateV);
                                cell.CellStyle = dateStyle;//格式化显示
                                break;
                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                cell.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                cell.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                cell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理
                                cell.SetCellValue("");
                                break;
                            default:
                                cell.SetCellValue("");
                                break;
                        }

                }
                rowcount++;

            }
           
        }
        /**********************************************************************************************************
          * DtToExcelWorkbook
          * next step，DtToExcelFsWrite();
          *                              DataTable 导出到EXCEL  int[] columnwidth,int columncount
          ***********************************************************************************************************/
        public void DtToExcelWorkbook(string Path, DataTable dt, int[] columnwidth, int columncount)//next step，DtToExcelFsWrite();
        {

            filename = Path;
            InitWorkbookOut(Path);

            ICellStyle dateStyle = (ICellStyle)workbookout.CreateCellStyle();
            IDataFormat format = (IDataFormat)workbookout.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:SS");

            ISheet sheet = workbookout.CreateSheet("sheet1");
            for (int k = 0; k < columncount; k++)
            {
                sheet.SetColumnWidth(k, 100 * columnwidth[k]);//时间列宽
            }
            int rowcount = 0;

            foreach (DataRow viewrow in dt.Rows)
            {
                IRow row = sheet.CreateRow(rowcount);

                foreach (DataColumn viewcolumn in dt.Columns)
                {
                    ICell cell = row.CreateCell(viewcolumn.Ordinal);

                    string drValue = viewrow[viewcolumn].ToString();

                    if (drValue == "") { }
                    else
                        switch (viewcolumn.DataType.ToString())
                        {
                            case "System.String"://字符串类型
                                cell.SetCellValue(drValue);
                                break;
                            case "System.DateTime"://日期类型
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                cell.SetCellValue(dateV);
                                cell.CellStyle = dateStyle;//格式化显示
                                break;
                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                cell.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                cell.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                cell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理
                                cell.SetCellValue("");
                                break;
                            default:
                                cell.SetCellValue("");
                                break;
                        }

                }
                rowcount++;

            }
        }
        /**********************************************************************************************************
         * DtToExcelWorkbook
         * next step，DtToExcelFsWrite();
         *                             DataGridView   导出到EXCEL  int timecolumn
         ***********************************************************************************************************/
        public void DtToExcelWorkbook(string Path, DataGridView dt, int timecolumn,int startcolumn)// next step，DtToExcelFsWrite();
        {
            filename = Path;
            InitWorkbookOut(Path);

            ICellStyle dateStyle = (ICellStyle)workbookout.CreateCellStyle();
            dateStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            dateStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
            IDataFormat format = (IDataFormat)workbookout.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:SS");

            ISheet sheet = workbookout.CreateSheet("sheet1");
            if(timecolumn>=0)
            sheet.SetColumnWidth(timecolumn, 35 * 160);//时间列宽
            //导出标题
             IRow row0 = sheet.CreateRow(0);

             for (int j = startcolumn; j < dt.Columns.Count; j++)
             {
                 ICell cell0 = row0.CreateCell(j - startcolumn);
                 cell0.CellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                 cell0.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                
                 string drValue = dt.Columns[j].HeaderText.ToString();
                 cell0.SetCellValue(drValue);

             }
           
            //导出内容

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i+1);

                for (int j = startcolumn; j < dt.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j - startcolumn);
                    cell.CellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;

                    string drValue = dt.Rows[i].Cells[j].Value.ToString();

                    if (drValue == "") { }
                    else
                        switch (dt.Rows[i].Cells[j].Value.GetType().ToString())
                        {
                            case "System.String"://字符串类型
                                cell.SetCellValue(drValue);
                                break;
                            case "System.DateTime"://日期类型
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                cell.SetCellValue(dateV);
                                cell.CellStyle = dateStyle;//格式化显示
                                break;
                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                cell.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                cell.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                cell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理
                                cell.SetCellValue("");
                                break;
                            default:
                                cell.SetCellValue("");
                                break;
                        }

                }


            }

        }

        /**********************************************************************************************************
        * getExcelCellValue  直接输入行号 从1起，输入列号字母"AZ" "CZ"   
        * 
        * 读取单元格内容
        * 需要外部初始化InitWorkbookOpen(path);
        ***********************************************************************************************************/
        public string getExcelCellValue( string sheetname,string str_Col,int row, bool isMerged)
        {
            //InitWorkbookOpen(path); //initialize workbook
            int column = 0;
            int column1 = 0;
            int column2 = 0;
            if(str_Col.Length==1)
            {
                column = str_Col[0] - 'A';
            }
            else if (str_Col.Length > 1 && str_Col.Length < 3)
            {
                column1 = str_Col[0];
                column2 = str_Col[1]; 
                 
                if (column1 == 'A')
                {
                    column = column2 - 'A' + 'Z' - 'A' + 1;
                }
                else if (column1 == 'B')
                {
                    column = column2 - 'A' + 2 * ('Z' - 'A') + 2;
                }
                else if (column1 == 'C')
                {
                    column = column2 - 'A' + 3 * ('Z' - 'A') +3;
                }
                else if (column1 == 'D')
                {
                    column = column2 - 'A' + 4 * ('Z' - 'A') + 4;
                }
            }

            string str_Rlt; 
            ISheet sheet = null;
            if (sheetname == null)
                sheet = workbookin.GetSheetAt(0);
            else
                sheet = workbookin.GetSheet(sheetname); 
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            
            if(isMerged)
            {
                str_Rlt = getMergedValue(ref sheet, row-1, column);
            }
            else
            {
                ICell cell = sheet.GetRow(row-1).GetCell(column);
                str_Rlt = cell.ToString();
            }
             
            return str_Rlt; 
        }

        /**********************************************************************************************************
        * setExcelCellValue  直接输入行号 从1起，输入列号字母"AZ" "CZ"   
        * 
        * 设置单元格内容
        * 需要外部初始化InitWorkbookRenewOut(path); 最后写入文件 doExcelFsWrite()
        ***********************************************************************************************************/
        public void setExcelCellValue(string sheetname, string str_Col, int row, bool isMerged,string value,int value_type)
        {
        
            int column = 0;
            int column1 = 0;
            int column2 = 0;
            if (str_Col.Length == 1)
            {
                column = str_Col[0] - 'A';
            }
            else if (str_Col.Length > 1 && str_Col.Length < 3)
            {
                column1 = str_Col[0];
                column2 = str_Col[1];

                if (column1 == 'A')
                {
                    column = column2 - 'A' + 'Z' - 'A' + 1;
                }
                else if (column1 == 'B')
                {
                    column = column2 - 'A' + 2 * ('Z' - 'A') + 2;
                }
                else if (column1 == 'C')
                {
                    column = column2 - 'A' + 3 * ('Z' - 'A') + 3;
                }
                else if (column1 == 'D')
                {
                    column = column2 - 'A' + 4 * ('Z' - 'A') + 4;
                }
            }
             
            ISheet sheet = null;
            if (sheetname == null)
                sheet = workbook_renewout.GetSheetAt(0);
            else
                sheet = workbook_renewout.GetSheet(sheetname);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            if (isMerged)
            {
                setMergedValue(ref sheet, row - 1, column, value,value_type);
            }
            else
            {
                ICell cell = sheet.GetRow(row - 1).GetCell(column);
                if (cell != null)
                {

                } 
                else
                {
                    IRow ptr_Row = sheet.GetRow(row - 1);
                    ptr_Row.CreateCell(column, CellType.Blank);
                    cell=sheet.GetRow(row - 1).GetCell(column);
                }

                if ((value == "") || (value == null))
                {
                    cell.SetCellValue("");
                    return;
                }
                switch (value_type)
                {
                    case 0:
                        // cell.SetCellType(CellType.String);
                        cell.SetCellValue(value);
                        break;
                    case 1:
                        // cell.SetCellType(CellType.Numeric);
                        double doubV = 0;
                        double.TryParse(value, out doubV);
                        cell.SetCellValue(doubV);
                        break;
                }

            }
             
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        public void doExcelFsWrite()
        {    //打开的文件只能是低版本 生成新的低版本 否则出现“部分内容有问题”；NPOI自身生成的xlsx没问题，存在于MS EXCEL兼容问题
            using (FileStream fsout = new FileStream(str_NewFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook_renewout.Write(fsout);
                fsout.Close();
            }
             
        }
        /// <summary>
        /// 获取合并单元格内容
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public string getMergedValue(ref ISheet sheet, int row, int column)
        {
            int sheetMergeCount = sheet.NumMergedRegions;
            for (int i = 0; i < sheetMergeCount; i++)
            {
                NPOI.SS.Util.CellRangeAddress range = sheet.GetMergedRegion(i);
                int firstColumn = range.FirstColumn;
                int lastColumn = range.LastColumn;
                int firstRow = range.FirstRow;
                int lastRow = range.LastRow;
                if (row >= firstRow && row <= lastRow)
                {
                    if (column >= firstColumn && column <= lastColumn)
                    { return sheet.GetRow(firstRow).GetCell(firstColumn).ToString(); }
                }
            }
            return null;
        }
        /// <summary>
        /// 设置合并单元格内容
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void setMergedValue(ref ISheet sheet, int row, int column,string value,int value_type)
        {
            int sheetMergeCount = sheet.NumMergedRegions;
            for (int i = 0; i < sheetMergeCount; i++)
            {
                NPOI.SS.Util.CellRangeAddress range = sheet.GetMergedRegion(i);
                int firstColumn = range.FirstColumn;
                int lastColumn = range.LastColumn;
                int firstRow = range.FirstRow;
                int lastRow = range.LastRow;
                if (row >= firstRow && row <= lastRow)
                {
                    if (column >= firstColumn && column <= lastColumn)
                    {
                        ICell cell = sheet.GetRow(firstRow).GetCell(firstColumn);

                        if (cell != null)
                        { 
                        } 
                        else
                        {
                            IRow ptr_Row = sheet.GetRow(firstRow);
                            ptr_Row.CreateCell(column, CellType.Blank);
                            cell = sheet.GetRow(firstRow).GetCell(firstColumn);
                        }
                        if((value=="")||(value==null))
                        {
                            cell.SetCellValue("");
                            return;
                        }
                        switch (value_type)
                        {
                            case 0:
                               // cell.SetCellType(CellType.String);
                                cell.SetCellValue(value);
                                break;
                            case 1:
                               // cell.SetCellType(CellType.Numeric);
                                double doubV = 0;
                                double.TryParse(value, out doubV);
                                cell.SetCellValue(doubV);
                                break;
                        }
                         
                        return;
                    }
                }
            }
           
        }

        public void sheetFormulaRecal(string sheetname)
        {
            ISheet sheet = null;
            if (sheetname == null)
                sheet = workbook_renewout.GetSheetAt(0);
            else
                sheet = workbook_renewout.GetSheet(sheetname);
            sheet.ForceFormulaRecalculation = true;
        }

    }
}
