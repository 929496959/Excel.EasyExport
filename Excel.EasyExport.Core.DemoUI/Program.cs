using Microsoft.OpenApi.Models;

namespace Excel.EasyExport.Core.DemoUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NPOI.Export ����",
                    Version = "�ĵ��汾���",
                    Description = "�ĵ�����"
                });
                var file = Path.Combine(AppContext.BaseDirectory, "Muzi.ExcelExport.Core.DemoUI.xml");  // xml�ĵ�����·��
                var path = Path.Combine(AppContext.BaseDirectory, file); // xml�ĵ�����·��
                c.IncludeXmlComments(path, true); // true : ��ʾ��������ע��
                c.OrderActionsBy(o => o.RelativePath); // ��action�����ƽ�����������ж�����Ϳ��Կ���Ч���ˡ�

            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
