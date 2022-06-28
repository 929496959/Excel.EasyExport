using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Streaming;
using Excel.EasyExport;
using Excel.EasyExport.Core.DemoUI.Controllers;

namespace Excel.EasyExport.Core.DemoUI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DemoController : ControllerBase
    {

        private readonly ILogger<DemoController> _logger;

        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// ����ʹ�ã��Զ�ӳ���ֶ� ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Demo1()
        {
            IWorkbook wb = new XSSFWorkbook();
            //Prop ��Ӧʵ����ֶ��������ִ�Сд
            List<ExportColumn> columns = new()
            {
                new(){ Label="����", Prop="Name"},
                new(){ Label="��н", Prop="BaseSalary"},
                new(){ Label="�Ƿ�ȫ��", Prop="IsFullAttendance",},
                new(){ Label="��ͨ", Prop="Transportation"},
                new(){ Label="�Ͳ�", Prop="Meal",},
                new(){ Label="����ʱ��", Prop=nameof(Salary.CreateTime)},
            };

            List<Salary> data = new List<Salary>();
            data.Add(new Salary()
            {
                Name = "����",
                BaseSalary = 8000,
                IsFullAttendance = true,
                Meal = 300,
                Transportation = 300,
                CreateTime = DateTime.Now
            });

            var sheet = EasyExport.CreateSheet(wb, columns, data);

            using (MemoryStream ms = new())
            {
                wb.Write(ms);
                wb.Close();
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.xlsx");
            }
        }

        /// <summary>
        /// �ϲ���ͷ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Demo2()
        {
            IWorkbook wb = new XSSFWorkbook();

            List<ExportColumn> columns = new()
            {
                new(){ Label="����", Prop="Name",},
                new()
                {
                    Label = "����",
                    Children = new() {
                        new(){ Label="��н", Prop="BaseSalary"},
                        new(){ Label="ȫ��", Prop="FullAttendance"},
                        new(){ Label="����",
                            Children = new () {
                                new (){ Label="��ͨ", Prop="Transportation"},
                                new (){ Label="�Ͳ�", Prop="Meal"},
                            },
                        },
                    },
                },
                new(){ Label="����ʱ��",Prop = nameof(Salary.CreateTime)},
            };

            List<Salary> data = new() {
                new() { Name = "����",
                    BaseSalary = 8000,
                    FullAttendance = 200,
                    Meal = 300,
                    Transportation = 300,
                    CreateTime = DateTime.Now
                }
            };

            EasyExport.CreateSheet(wb, columns, data, "testSheet");

            using (MemoryStream ms = new())
            {
                wb.Write(ms);
                wb.Close();
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.xlsx");
            }
        }

        /// <summary>
        /// ָ������ʽ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Demo3()
        {
            IWorkbook wb = new XSSFWorkbook();

            List<ExportColumn> columns = new()
            {
                new(){
                    Label="����",
                    Prop="Name",
                    Width = 100,
                    Style = ()=>{
                        ICellStyle style = wb.CreateCellStyle();
                        style.Alignment = HorizontalAlignment.Left;
                        IFont font = wb.CreateFont();
                        font.IsBold=true;
                        style.SetFont(font);
                        return style;
                    }
                },
                new(){ Label="��н", Prop="BaseSalary"},
                new(){ Label="ȫ��", Prop="FullAttendance"},
                new(){ Label="��ͨ", Prop="Transportation"},
                new(){ Label="�Ͳ�", Prop="Meal"},
            };

            List<Salary> data = new List<Salary>();

            data.Add(new Salary() { Name = "����", BaseSalary = 8000, FullAttendance = 200, Meal = 300, Transportation = 300 });
            data.Add(new Salary() { Name = "����", BaseSalary = 9000, FullAttendance = 300, Meal = 300, Transportation = 300 });
            data.Add(new Salary() { Name = "����", BaseSalary = 1000, FullAttendance = 400, Meal = 300, Transportation = 300 });

            EasyExport.CreateSheet(wb, columns, data);

            using (MemoryStream ms = new())
            {
                wb.Write(ms);
                wb.Close();
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.xlsx");
            }
        }

        /// <summary>
        /// ��ʽ���У�д��һ�������磺������ӣ�ת��ʱ���ʽ����͵�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Demo4()
        {
            IWorkbook wb = new XSSFWorkbook();


            //���� Format ��ʽ����
            List<ExportColumn> columns = new()
            {
                new(){ Label="����", Prop="Name"},
                new(){ Label="��н", Prop="BaseSalary"},
                new(){ Label="ȫ��", Prop="FullAttendance"},
                new(){ Label="��ͨ", Prop="Transportation"},
                new(){ Label="�Ͳ�", Prop="Meal"},
                new(){
                    Label="�ϼ�",
                    //��һ�������ǵ�ǰ�����ݣ��ڶ��������ǵ�ǰ������
                    Format = (row,rowIndex)=>{
                        Salary salary = (Salary)row;
                        return (double)salary.BaseSalary+(double)salary.FullAttendance+(double)salary.Transportation+(double)salary.Meal;
                    }
                },
            };

            List<Salary> data = new List<Salary>();

            data.Add(new Salary() { Name = "����", BaseSalary = 8000, FullAttendance = 200, Meal = 300, Transportation = 300 });
            data.Add(new Salary() { Name = "����", BaseSalary = 9000, FullAttendance = 300, Meal = 300, Transportation = 300 });
            data.Add(new Salary() { Name = "����", BaseSalary = 1000, FullAttendance = 400, Meal = 300, Transportation = 300 });

            EasyExport.CreateSheet(wb, columns, data);

            //���format �Ƚϸ��ӣ����Ҹ����϶࣬����ʹ�÷��ͣ� ���Ե�������format�ֵ䣬 ��ʱ��ͷ�е� Prop �ֶα��keyΪ Prop��Ч��ͬ��

            //Dictionary<string, Func<Salary, int, string>> formatDic = new Dictionary<string, Func<Salary, int, string>>();
            //formatDic.Add("Total", (row, rowIndex) =>
            //{
            //    return (row.BaseSalary + row.FullAttendance + row.Transportation + row.Meal).ToString();
            //});
            //MuziExport.CreateSheet(wb, columns, data, formatDic);


            using (MemoryStream ms = new())
            {
                wb.Write(ms);
                wb.Close();
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.xlsx");
            }
        }

        /// <summary>
        /// ��ʽ���У�д��������ʹ�÷���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Demo5()
        {
            IWorkbook wb = new XSSFWorkbook();

            List<ExportColumn> columns = new()
            {
                new(){ Label="����", Prop="Name"},
                new(){ Label="��н", Prop="BaseSalary"},
                new(){ Label="ȫ��", Prop="FullAttendance"},
                new(){ Label="��ͨ", Prop="Transportation"},
                new(){ Label="�Ͳ�", Prop="Meal"},
                new(){ Label="�ϼ�", Prop="Total"},
            };

            List<Salary> data = new List<Salary>();

            data.Add(new Salary() { Name = "����", BaseSalary = 8000, FullAttendance = 200, Meal = 300, Transportation = 300 });
            data.Add(new Salary() { Name = "����", BaseSalary = 9000, FullAttendance = 300, Meal = 300, Transportation = 300 });
            data.Add(new Salary() { Name = "����", BaseSalary = 1000, FullAttendance = 400, Meal = 300, Transportation = 300 });

            //���format �Ƚϸ��ӣ����Ҹ����϶࣬����ʹ�÷��ͣ� ���Ե�������format�ֵ䣬 ��ʱ��ͷ�е� Prop �ֶα��keyΪ Prop
            //��ͷ��format ���ȼ����� �ֵ�format

            Dictionary<string, Func<Salary, int, object>> formatDic = new Dictionary<string, Func<Salary, int, object>>();
            formatDic.Add("Total", (row, rowIndex) =>
            {
                return ((double)row.BaseSalary + (double)row.FullAttendance + (double)row.Transportation + row.Meal).ToString();
            });

            EasyExport.CreateSheet(wb, columns, data, formatDic);


            using (MemoryStream ms = new())
            {
                wb.Write(ms);
                wb.Close();
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.xlsx");
            }
        }

        /// <summary>
        /// �������� 10 ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Demo6()
        {
            // Muzi.ExcelExport ֻ����� IWorkBook ��  ISheet �Ĵ���

            //��ͬ��������ѡ��ͬ�� IWorkBookʵ��

            //NPOI �ṩ����������

            //**HSSFWorkbook��XSSFWorkbook �� SXSSFWorkbook**

            //�����֮��

            //HSSFWorkbook ���Excel2003 ����65535����������

            //XSSFWorkbook ���Excel2007,     û���������ƣ������ڴ�ռ�ÿ��ܴ�

            //SXSSFWorkbook ��XSSFWorkbook ���ڴ�Ľ��棬�����в���������

            //������������в���

            //������10�������ݵ���Ϊ��

            IWorkbook wb = new SXSSFWorkbook();

            List<ExportColumn> columns = new()
            {
                new(){ Label="����", Prop="Name",Width=200},
                new(){ Label="��н", Prop="BaseSalary",Width=200},
                new(){ Label="ȫ��", Prop="FullAttendance",Width=200},
                new(){ Label="��ͨ", Prop="Transportation",Width=200},
                new(){ Label="�Ͳ�", Prop="Meal",Width=200},
                new(){ Label="ʱ��", Prop="CreateTime",Width=300},
            };

            List<Salary> data = new List<Salary>();

            for (int i = 0; i < 10000 * 10; i++)
            {
                data.Add(new Salary() { Name = "����", BaseSalary = 8000, FullAttendance = 200, Meal = 300, Transportation = 300, CreateTime = DateTime.Now });
            }

            EasyExport.CreateSheet(wb, columns, data);

            using (MemoryStream ms = new())
            {
                wb.Write(ms);
                wb.Close();
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.xlsx");
            }
        }

        /// <summary>
        /// ȫ������ �� �Զ��平��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Demo7()
        {
            IWorkbook wb = new XSSFWorkbook();

            List<ExportColumn> columns = new()
            {
                new(){ Label="����", Prop="Name"},
                new(){ Label="��н", Prop="BaseSalary"},
                new(){ Label="ȫ��", Prop="FullAttendance"},
                new(){ Label="��ͨ", Prop="Transportation"},
                new(){ Label="�Ͳ�", Prop="Meal"},
            };

            List<Salary> data = new List<Salary>();

            data.Add(new Salary() { Name = "����", BaseSalary = 8000, FullAttendance = 0, Meal = 300, Transportation = 300 });
            data.Add(new Salary() { Name = "����", BaseSalary = 8000, FullAttendance = 0, Meal = 300, Transportation = 300 });
            data.Add(new Salary() { Name = "����", BaseSalary = 8000, FullAttendance = 0, Meal = 300, Transportation = 300 });
            data.Add(new Salary() { Name = "����", BaseSalary = 8000, FullAttendance = 0, Meal = 300, Transportation = 300 });
            data.Add(new Salary() { Name = "����", BaseSalary = 8000, FullAttendance = 0, Meal = 300, Transportation = 300 });

            ICellStyle headerStyle = EasyExport.GetDefaultHeaderStyle(wb);

            ExportOptions exportOptions = new ExportOptions();
            //��ͷ�и�
            exportOptions.HeaderHeight = 400;
            //�����и�
            exportOptions.DataRowHeight = 300;
            //��ͷ��ʽ
            exportOptions.HeaderStyle = headerStyle;
            //������ʽ
            exportOptions.DataStyle = null;

            //�����ṩ�����Զ��平�ӣ��ֱ���  RowCreateAfter ��  CellCreateAfter

            //������Ϊ1���и�����Ϊ500��
            exportOptions.RowCreateAfter = (row, rowIndex) => { if (rowIndex == 1) row.Height = 800; };

            //��������ȵĵ�Ԫ�񱳾�ɫ
            exportOptions.CellCreateAfter = (cell, colIndex, row, rowIndex) =>
            {
                if (colIndex == rowIndex)
                {
                    ICellStyle cellStyle = wb.CreateCellStyle();

                    cellStyle.FillPattern = FillPattern.SolidForeground;
                    cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.RoyalBlue.Index;

                    cell.CellStyle = cellStyle;
                }

            };

            EasyExport.CreateSheet(wb, columns, data, exportOptions: exportOptions);

            using (MemoryStream ms = new())
            {
                wb.Write(ms);
                wb.Close();
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.xlsx");
            }
        }
    }

}
