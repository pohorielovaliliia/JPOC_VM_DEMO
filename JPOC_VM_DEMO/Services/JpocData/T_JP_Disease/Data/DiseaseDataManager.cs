using System.Data;
using Microsoft.Data.SqlClient;
using JPOC_VM_DEMO.Services.JpocData.T_JP_Disease.Model;
using JPOC_VM_DEMO.Services.JpocData.Utilities;

namespace JPOC_VM_DEMO.Services.JpocData.T_JP_Disease.Data
{
    public interface IDiseaseDataManager
    {
        //Task<Disease> ReturnObjectAsync(string storeProc);
        //Task<Disease> ReturnObjectAsync(string storeProc, List<SqlParameter> sqlParameters);
        //Task<List<Disease>> ReturnListAsync(string storeProc);
        //Task<List<Disease>> ReturnListAsync(string storeProc, List<SqlParameter> sqlParameters);
        Task<DataTable> ReturnDTAsync(string storeProc);
        DataTable ReturnDT(string storeProc);
        //Task<DataTable> ReturnDTAsync(string storeProc, List<SqlParameter> sqlParameters);
        //Task<DataTable> ReturnDTSqlQueryAsync(string sqlQuery, List<SqlParameter> sqlParameters);
        //Task<DataSet> ReturnSetAsync(string storeProc);
        //Task<DataSet> ReturnSetAsync(string storeProc, List<SqlParameter> sqlParameters);
        //Task<string> ReturnDTStatusAsync(string storeProc, List<SqlParameter> sqlParameters);
    }

    public class DiseaseDataManager : IDiseaseDataManager
    {
        private readonly IDatabaseFactory _databaseFactory;
        //private readonly IDatabaseService _db;

        public DiseaseDataManager(
            IDatabaseFactory databaseFactory
            //IDatabaseService db
            )
        {
            _databaseFactory = databaseFactory;
            //_db = db;
        }

        private Disease SetObject(DataTable dt)
        {
            try
            {
                if (dt?.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    return new Disease
                    {

                        Id = Convert.ToInt32(row["id"]),
                        Guid = Guid.Parse(row["guid"].ToString()),
                        Title = row["title"] as string ?? "",
                        Type = row["type"] as string ?? "",
                        Synonyms = row["synonyms"] as string ?? "",
                        Importance = row["importance"] == DBNull.Value ? 0 : Convert.ToInt32(row["importance"]),
                        Frequency = row["frequency"] == DBNull.Value ? 0 : Convert.ToInt32(row["frequency"]),
                        Urgency = row["urgency"] == DBNull.Value ? 0 : Convert.ToInt32(row["urgency"]),
                        Sequence = row["sequence"] == DBNull.Value ? 0 : Convert.ToDouble(row["sequence"]),
                        Information = row["information"] as string ?? "",
                        Status = row["status"] as string ?? "",
                        IsCurrentVersion = (bool)(row["is_current_version"] == DBNull.Value ? null : (bool?)row["is_current_version"]),
                        Version = row["version"] == DBNull.Value ? 0 : Convert.ToInt32(row["version"]),
                        VersionString = row["version_string"] as string ?? "",
                        CreatedBy = row["created_by"] == DBNull.Value ? 0 : Convert.ToInt32(row["created_by"]),
                        ModifiedBy = row["modified_by"] == DBNull.Value ? 0 : Convert.ToInt32(row["modified_by"]),
                        CheckoutBy = row["checkout_by"] == DBNull.Value ? 0 : Convert.ToInt32(row["checkout_by"]),
                        CreatedDate = row["created_date"] == DBNull.Value ? null : (DateTime?)row["created_date"],
                        ModifiedDate = row["modified_date"] == DBNull.Value ? null : (DateTime?)row["modified_date"],
                        Defunct = (bool)(row["defunct"] == DBNull.Value ? null : (bool?)row["defunct"]),
                        ExternalLink = row["external_link"] as string ?? "",
                        AuthorName = row["author_name"] as string ?? "",
                        AuthorInstitution = row["author_institution"] as string ?? "",
                        CoauthorName = row["coauthor_name"] as string ?? "",
                        CoauthorInstitution = row["coauthor_institution"] as string ?? "",
                        ExternalLink2 = row["external_link2"] as string ?? "",
                        ExternalLink3 = row["external_link3"] as string ?? "",
                        SubcategoryId = row["subcategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(row["subcategoryId"]),
                        SortId = row["sortid"] == DBNull.Value ? 0 : Convert.ToInt32(row["sortid"]),
                        ProhibitedImport = (bool)(row["prohibited_import"] == DBNull.Value ? null : (bool?)row["prohibited_import"]),
                        LatestEdittingDate = row["latest_editting_date"] as string ?? "",
                        WideTitle = row["WideTitle"] == DBNull.Value ? 0 : Convert.ToInt32(row["WideTitle"]),
                        AuthorIntroducation = row["AuthorIntroducation"] as string ?? "",
                        InactiveMessage = row["inactive_message"] as string ?? "",
                        PublishProd = (bool)(row["publish_prod"] == DBNull.Value ? null : (bool?)row["publish_prod"]),
                        HistoryText = row["history_text"] as string ?? "",
                        IsWip = (bool)(row["is_wip"] == DBNull.Value ? null : (bool?)row["is_wip"]),
                        IntroductionAuthor = row["introduction_author"] as string ?? "",
                        IntroductionAuthorInstitution = row["introduction_author_institution"] as string ?? "",
                        IntroductionExpertise = row["introduction_expertise"] as string ?? "",
                        IntroductionAcademy = row["introduction_academy"] as string ?? "",
                        IntroductionSpecialist = row["introduction_specialist"] as string ?? "",
                        IntroductionResume = row["introduction_resume"] as string ?? "",
                        IntroductionAdvice = row["introduction_advice"] as string ?? "",
                        IntroductionExternal = row["introduction_external"] as string ?? "",
                        IntroductionAuthorMessage = row["introduction_author_message"] as string ?? "",
                        IntroductionAuthorPhoto = row["introduction_author_photo"] as string ?? ""
                    };
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private List<Disease> SetList(DataTable dt)
        {
            try
            {
                var list = new List<Disease>();
                if (dt?.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var disease = new Disease
                        {
                            // Same mapping as SetObject method
                            // Properties mapping omitted for brevity - use same mapping as above
                        };
                        list.Add(disease);
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Disease> ReturnObjectAsync(string storeProc)
        {
            try
            {
                var dt = (await _databaseFactory.ExecuteAsync(storeProc)).Tables[0];
                return dt?.Rows.Count > 0 ? SetObject(dt) : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Disease> ReturnObjectAsync(string storeProc, List<SqlParameter> sqlParameters)
        {
            try
            {
                var dt = (await _databaseFactory.ExecuteAsync(storeProc, sqlParameters)).Tables[0];
                return dt?.Rows.Count > 0 ? SetObject(dt) : null;
            }
            catch
            {
                return null;
            }
        }

        // TODO: Implement other interface methods similarly with async/await pattern
        // ReturnListAsync, ReturnDTAsync, ReturnDTSqlQueryAsync, ReturnSetAsync, ReturnDTStatusAsync
        public async Task<DataTable> ReturnDTAsync(string storeProc)
        {
            try
            {
                var ds = (await _databaseFactory.ExecuteAsync(storeProc));
                //var ds = (await _db.ExecuteQueryAsync(storeProc));
                var dt = ds?.Tables[0];
                return dt?.Rows.Count > 0 ? dt : null;
            }
            catch
            {
                return null;
            }
        }

        public DataTable ReturnDT(string storeProc)
        {
            try
            {
                
                var ds = _databaseFactory.Execute(storeProc);
                //var ds = (await _db.ExecuteQueryAsync(storeProc));
                var dt = ds?.Tables[0];
                return dt?.Rows.Count > 0 ? dt : null;
            }
            catch
            {
                return null;
            }
        }
    }
}

