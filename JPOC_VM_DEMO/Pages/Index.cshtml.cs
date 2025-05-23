using JPOC_VM_DEMO.Models;
using JPOC_VM_DEMO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace JPOC_VM_DEMO.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDatabaseService _db;
        public List<AuditModel> AuditRecords { get; set; } = new();

        public IndexModel(IDatabaseService db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            var query = @"SELECT TOP (5) [id]
                        ,[entity_name]
                        ,[entity_id]
                        ,[action]
                        ,[created_by]
                        ,[created_date]
                    FROM [JPOC_IT].[dbo].[T_JP_Audit]";

            var result = await _db.ExecuteQueryAsync(query);

            foreach (DataRow row in result.Rows)
            {
                AuditRecords.Add(new AuditModel
                {
                    Id = Convert.ToInt32(row["id"]),
                    EntityName = row["entity_name"].ToString() ?? "",
                    EntityId = row["entity_id"].ToString() ?? "",
                    Action = row["action"].ToString() ?? "",
                    CreatedBy = row["created_by"].ToString() ?? "",
                    CreatedDate = Convert.ToDateTime(row["created_date"])
                });
            }
        }
    }
}
