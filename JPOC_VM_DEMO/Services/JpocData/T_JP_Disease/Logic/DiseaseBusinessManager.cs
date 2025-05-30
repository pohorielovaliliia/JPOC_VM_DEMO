using JPOC_VM_DEMO.Services.JpocData.T_JP_Disease.Data;
using System.Data;

namespace JPOC_VM_DEMO.Services.JpocData.T_JP_Disease.Logic
{
    public interface IDiseaseBusinessManager
    {
        Task<DataTable> GetLatestEditingDateAsync();
        DataTable GetLatestEditingDate();
        //Task<T_JP_Disease> GetByIdAsync(int diseaseId);
        //Task<IEnumerable<T_JP_Disease>> GetAllAsync();
        //Task<DataTable> GetAllDatatableAsync();
        //Task<DataTable> GetActiveDatatableAsync();
        //Task<DataTable> GetDiseaseForCoiAsync();
        //Task<DataTable> GetDiseaseSectionsCountAsync(int diseaseId);
        //Task<DataTable> GetDiseaseImageByKeywordAsync(string searchKeyword);
        //Task<DataTable> GetDiseaseForNewContentSummaryAsync();
        //Task<DataTable> GetDiseaseForDrugApprovalInformationAsync();
        //Task<DataTable> GetDiseaseSynonymsAsync(string searchKeyword);
        //Task<bool> UploadExcelAsync(DataTable dt, int userId);
        //Task<bool> UploadSynonymsExcelAsync(DataTable dt, int userId);
    }

    public class DiseaseBusinessManager : IDiseaseBusinessManager
    {
        private readonly string _connectionString;
        private readonly IDiseaseDataManager _dataManager;

        public DiseaseBusinessManager(IConfiguration configuration, IDiseaseDataManager dataManager)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _dataManager = dataManager;
        }

        public async Task<DataTable> GetLatestEditingDateAsync()
        {
            return await _dataManager.ReturnDTAsync("[P_JP_DiseaseGetLatestEditingDate]");
        }

        public DataTable GetLatestEditingDate()
        {
            return _dataManager.ReturnDT("[P_JP_DiseaseGetLatestEditingDate]");
        }

        //public async Task<T_JP_Disease> GetByIdAsync(int diseaseId)
        //{
        //    var parameters = new List<SqlParameter>
        //    {
        //        new SqlParameter("@disease_id", diseaseId)
        //    };
        //    return await _dataManager.ReturnObjectAsync("[P_JP_DiseaseGetAll]", parameters);
        //}

        //public async Task<IEnumerable<T_JP_Disease>> GetAllAsync()
        //{
        //    return await _dataManager.ReturnListAsync("[P_JP_DiseaseGetAll]");
        //}

        //public async Task<DataTable> GetAllDatatableAsync()
        //{
        //    return await _dataManager.ReturnDTAsync("[P_JP_DiseaseGetAll]");
        //}

        //public async Task<DataTable> GetActiveDatatableAsync()
        //{
        //    return await _dataManager.ReturnDTAsync("[P_JP_DiseaseGetActive]");
        //}

        //public async Task<DataTable> GetDiseaseForCoiAsync()
        //{
        //    return await _dataManager.ReturnDTAsync("[P_JP_COIDiseaseGetNew]");
        //}

        //public async Task<DataTable> GetDiseaseSectionsCountAsync(int diseaseId)
        //{
        //    var parameters = new List<SqlParameter>
        //    {
        //        new SqlParameter("@id", diseaseId)
        //    };
        //    return await _dataManager.ReturnDTAsync("[P_JP_Disease_GetDiseaseSectionsCount]", parameters);
        //}

        //public async Task<DataTable> GetDiseaseImageByKeywordAsync(string searchKeyword)
        //{
        //    var parameters = new List<SqlParameter>
        //    {
        //        new SqlParameter("@searchKeyword", searchKeyword)
        //    };
        //    return await _dataManager.ReturnDTAsync("[P_JP_Disease_GetDiseaseImageByKeyword]", parameters);
        //}

        //public async Task<DataTable> GetDiseaseForNewContentSummaryAsync()
        //{
        //    return await _dataManager.ReturnDTAsync("[P_JP_DiseaseNewContentSummaryGetNewDisease]");
        //}

        //public async Task<DataTable> GetDiseaseForDrugApprovalInformationAsync()
        //{
        //    return await _dataManager.ReturnDTAsync("[P_JP_DiseaseDrugApprovalInformationGetNewDisease]");
        //}

        //public async Task<DataTable> GetDiseaseSynonymsAsync(string searchKeyword)
        //{
        //    var sql = "SELECT [id], [synonyms] FROM T_JP_Disease";
        //    var parameters = new List<SqlParameter>();

        //    if (!string.IsNullOrEmpty(searchKeyword))
        //    {
        //        var keywords = searchKeyword.Split(',');
        //        var searchFilter = string.Join(",", keywords.Select((k, i) => $"@key{i}"));

        //        for (int i = 0; i < keywords.Length; i++)
        //        {
        //            parameters.Add(new SqlParameter($"@key{i}", keywords[i]));
        //        }

        //        sql += $" WHERE id in ({searchFilter})";
        //    }

        //    return await _dataManager.ReturnDTSqlQueryAsync(sql, parameters);
        //}

        //public async Task<bool> UploadExcelAsync(DataTable dt, int userId)
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    await connection.OpenAsync();
        //    using var transaction = await connection.BeginTransactionAsync();

        //    try
        //    {
        //        using var command = new SqlCommand("[P_JP_TempDisease_Delete]", connection, transaction)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };
        //        command.Parameters.AddWithValue("@UserID", userId);
        //        await command.ExecuteNonQueryAsync();

        //        using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
        //        {
        //            bulkCopy.DestinationTableName = "T_JP_TempDisease";
        //            bulkCopy.ColumnMappings.Add("id", "id");
        //            bulkCopy.ColumnMappings.Add("coauthor_name", "coauthor_name");
        //            bulkCopy.ColumnMappings.Add("coauthor_institution", "coauthor_institution");
        //            bulkCopy.ColumnMappings.Add("is_wip", "is_wip");
        //            bulkCopy.ColumnMappings.Add("latest_editting_date", "latest_editting_date");
        //            bulkCopy.ColumnMappings.Add("created_by", "created_by");
        //            bulkCopy.ColumnMappings.Add("created_date", "created_date");
        //            await bulkCopy.WriteToServerAsync(dt);
        //        }

        //        command.Parameters.Clear();
        //        command.CommandText = "[P_JP_TempDisease_Update]";
        //        command.Parameters.AddWithValue("@UserID", userId);
        //        await command.ExecuteNonQueryAsync();

        //        await transaction.CommitAsync();
        //        return true;
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync();
        //        return false;
        //    }
        //}

        //public async Task<bool> UploadSynonymsExcelAsync(DataTable dt, int userId)
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    await connection.OpenAsync();
        //    using var transaction = await connection.BeginTransactionAsync();

        //    try
        //    {
        //        using var command = new SqlCommand("[P_JP_Disease_Synonyms_Temp_Delete]", connection, transaction)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };
        //        command.Parameters.AddWithValue("@createby", userId);
        //        await command.ExecuteNonQueryAsync();

        //        using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
        //        {
        //            bulkCopy.DestinationTableName = "T_JP_Disease_Synonyms_Temp";
        //            bulkCopy.ColumnMappings.Add("id", "id");
        //            bulkCopy.ColumnMappings.Add("synonyms", "synonyms");
        //            bulkCopy.ColumnMappings.Add("created_by", "created_by");
        //            bulkCopy.ColumnMappings.Add("created_date", "created_date");
        //            await bulkCopy.WriteToServerAsync(dt);
        //        }

        //        command.Parameters.Clear();
        //        command.CommandText = "[P_JP_Disease_Synonyms_BatchUpdate]";
        //        command.Parameters.AddWithValue("@createby", userId);
        //        await command.ExecuteNonQueryAsync();

        //        await transaction.CommitAsync();
        //        return true;
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync();
        //        return false;
        //    }
        //}
    }
}
