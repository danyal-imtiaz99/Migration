using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using migration.Models;

namespace migration.Services
{
    public class TemplateService
    {
        private readonly string _templatePath;

        public TemplateService()
        {
            _templatePath = "/Users/d.imtiaz/Desktop/2025/project25/migration/Templates/report_template.html";
        }

        public string GenerateReport(List<InventoryItem> data)
        {
            string template = File.ReadAllText(_templatePath);

            // Replace placeholders
            template = template.Replace("{{REPORT_DATE}}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            template = template.Replace("{{TOTAL_ITEMS}}", data.Count.ToString());
            template = template.Replace("{{TOTAL_VALUE}}", data.Sum(x => x.Quantity * x.Price).ToString("F2"));

            // Generate table rows
            string tableRows = GenerateTableRows(data);
            template = template.Replace("{{TABLE_ROWS}}", tableRows);

            return template;
        }

        private string GenerateTableRows(List<InventoryItem> data)
        {
            return string.Join("", data.Select(item => $@"
                <tr>
                    <td>{item.ItemName}</td>
                    <td style='text-align: center;'>{item.Quantity}</td>
                    <td class='currency'>${item.Price:F2}</td>
                    <td>{item.Category}</td>
                    <td class='currency'>${item.Quantity * item.Price:F2}</td>
                </tr>"));
        }
    }
}