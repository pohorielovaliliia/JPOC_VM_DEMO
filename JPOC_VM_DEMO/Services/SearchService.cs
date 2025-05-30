using Jpoc.Tools.Core;
using JPOC_VM_DEMO.Common;
using JPOC_VM_DEMO.Models;
using JPOC_VM_DEMO.Pages;
using JPOC_VM_DEMO.Services.JpocData.T_JP_Disease.Logic;
using JPOC_VM_DEMO.Services.JpocData.Utilities;
using System.Data;
using PublicEnum = JPOC_VM_DEMO.Common.PublicEnum;
using GlobalVariables = JPOC_VM_DEMO.Common.GlobalVariables;
using JPOC_VM_DEMO.App_Code.CTRL;

namespace JPOC_VM_DEMO.Services
{
    public class SearchService : ISearchService
    {
        private readonly ILogger<SearchService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IDatabaseService _db;
        private readonly IDatabaseFactory _databaseFactory;
        private readonly IDiseaseBusinessManager _diseaseBusinessManager;
        private readonly CtrlSearch _ctrlSearch;
        //private readonly IDiseaseRepository _diseaseRepository;
        //private readonly ISystemRepository _systemRepository;
        //private readonly IEnqueteRepository _enqueteRepository;
        //private readonly IUtilities _utilities;

        private CtrlSearch Ctrl => _ctrlSearch;

        public SearchService(
            ILogger<SearchService> logger,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IDatabaseService db,
            IDatabaseFactory databaseFactory,
            IDiseaseBusinessManager diseaseBusinessManager,
            CtrlSearch ctrlSearch)
            //IDiseaseRepository diseaseRepository,
            //ISystemRepository systemRepository,
            //IEnqueteRepository enqueteRepository,
            //IUtilities utilities)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _db = db;
            _databaseFactory = databaseFactory;
            _diseaseBusinessManager = diseaseBusinessManager;
            _ctrlSearch = ctrlSearch;
            //_diseaseRepository = diseaseRepository;
            //_systemRepository = systemRepository;
            //_enqueteRepository = enqueteRepository;
            //_utilities = utilities;
        }

        public async Task<SearchViewModel> InitializeSearchPageAsync(UserModel currentUser)
        {
            try
            {
                var viewModel = new SearchViewModel
                {
                    CurrentUser = currentUser,
                    ShowBanMessage = await CheckBanStatusAsync(currentUser),
                    //ImeEnabled = await GetIMESettingAsync(),
                    //ShowSubscriptionExpiry = await CheckSubscriptionExpiryAsync(currentUser),
                    //ShowFirstTimeDialog = await CheckFirstTimeLoginAsync(),
                    //ShowTrialAlert = await CheckTrialAlertAsync(currentUser)
                };

                //// Load all required data
                //await LoadCategoriesAsync(viewModel);
                //await LoadMedicalCalculatorDataAsync(viewModel);

                //viewModel.HeaderImageSettings = await GetHeaderImageSettingsAsync();

                //if (currentUser?.RoleId == (int) Role.Individual)
                //{
                //    viewModel.TopInformation = await LoadTopInformationAsync();
                //}

                //if (await ShouldShowBannerAsync(currentUser))
                //{
                //    viewModel.BannerHtml = await LoadBannerHtmlAsync();
                //}

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing search page for user {UserId}", currentUser?.Id);
                throw;
            }
        }

        //public async Task<DataView> GetDiseaseCategoriesAsync()
        //{
        //    var diseaseCategories = await _diseaseRepository.GetDiseaseCategoriesAsync();
        //    return diseaseCategories?.DefaultView ?? new DataView();
        //}

        //public async Task<DataView> GetTreeCategoriesAsync(string categoryType)
        //{
        //    var filter = $"category_type = '{categoryType}'";
        //    var treeCategories = await _systemRepository.GetTreeCategoriesAsync();
        //    return new DataView(treeCategories, filter, "sequence", DataViewRowState.CurrentRows);
        //}

        //public async Task<DataView> GetMedicalCalculatorViewAsync(string category = "")
        //{
        //    var filter = string.IsNullOrEmpty(category) ? "" : $"category = '{category}'";
        //    var medicalCalculators = await _systemRepository.GetMedicalCalculatorsAsync();
        //    return new DataView(medicalCalculators, filter, "category_sequence, sequence", DataViewRowState.CurrentRows);
        //}

        //public async Task<bool> ProcessSearchAsync(string searchText, string searchTerm, string userIdentity)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(searchText))
        //        {
        //            return false;
        //        }

        //        // Save search history
        //        await SaveSearchHistoryAsync(userIdentity, searchText);

        //        // Get redirect URL for search results
        //        var redirectUrl = await GetSearchRedirectUrlAsync(searchText, searchTerm);

        //        // Set redirect URL in TempData for the page to handle
        //        _httpContextAccessor.HttpContext.Items["RedirectUrl"] = redirectUrl;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error processing search. SearchText: {SearchText}, SearchTerm: {SearchTerm}",
        //            searchText, searchTerm);
        //        throw;
        //    }
        //}

        //public async Task<EnqueteInfo> GetEnqueteInfoAsync(UserModel currentUser)
        //{
        //    if (currentUser == null)
        //    {
        //        return null;
        //    }

        //    try
        //    {
        //        var activeEnquetes = await _enqueteRepository.GetActiveEnquetesAsync(currentUser.InstitutionId);
        //        if (activeEnquetes?.Any() != true)
        //        {
        //            return null;
        //        }

        //        var enqueteInfo = new EnqueteInfo();

        //        if (IsIpOrVmUser(currentUser))
        //        {
        //            // IP/VM users always see first active enquete
        //            enqueteInfo.Description = activeEnquetes[0].MainDescription;
        //            enqueteInfo.Id = activeEnquetes[0].Id;
        //            enqueteInfo.ShouldDisplay = true;
        //        }
        //        else
        //        {
        //            // Regular users see first incomplete enquete
        //            foreach (var enquete in activeEnquetes)
        //            {
        //                if (!await _enqueteRepository.IsEnqueteCompletedAsync(
        //                    enquete.Id, currentUser.Id, currentUser.InstitutionId))
        //                {
        //                    enqueteInfo.Description = enquete.MainDescription;
        //                    enqueteInfo.Id = enquete.Id;
        //                    enqueteInfo.ShouldDisplay = true;
        //                    break;
        //                }
        //            }
        //        }

        //        return enqueteInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error getting enquete info for user {UserId}", currentUser.Id);
        //        return null;
        //    }
        //}

        private bool IsIpOrVmUser(UserModel user)
        {
            if (user == null)
            {
                return false;
            }

            return user.RoleId == (int)PublicEnum.eRole.InstitutionIP ||
                   user.RoleId == (int)PublicEnum.eRole.VMUser;
        }

        private async Task<bool> CheckBanStatusAsync(UserModel user)
        {
            // Implementation of ban status check
            return false; // Placeholder
        }

        //TODO: REMOVE, THIS WAS JUST AN EXAMPLE OF CALLING STORED PROCEDURES DIRECTLY
        public async Task<string> GetVMUpdateDateAsync()
        {
            try
            {
                const string query = "EXEC [P_JP_DiseaseGetLatestEditingDate]";
                var result = await _db.ExecuteQueryAsync(query);

                if (result == null || result.Rows.Count == 0)
                    return string.Empty;

                return result.Rows[0][0]?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting VM update date");
                throw;
            }

        }
        public string GetVMUpdateDate()
        {
            try
            {
                //var dt = _diseaseBusinessManager.View.GetLatestEditingDate(); //TODO: ADD VIEW

                // Await the task to get the DataTable result


                //var dtTask = _diseaseBusinessManager.GetLatestEditingDateAsync();
                //dtTask.Wait(); // Ensure the task is completed
                //var dt = dtTask.Result;

                var dt = _diseaseBusinessManager.GetLatestEditingDate();
                if (dt != null && dt.Rows.Count > 0)
                {
                    string lDate = dt.Rows[0]["latest_editting_date"].ToString();
                    if (DateTime.TryParse(lDate, out DateTime dLastEditDate))
                    {
                        return $"{dLastEditDate.Year}年{dLastEditDate.Month}月{dLastEditDate.Day}日";
                    }
                }
                return GlobalVariables.VMUpdateDate;
            }
            catch (Exception)
            {
                return GlobalVariables.VMUpdateDate;
            }
        }

        public DataView GetDiseaseCategories()
        {

            if (Ctrl.Dto.DiseaseDataSet.T_JP_DiseaseCategory == null || Ctrl.Dto.DiseaseDataSet.T_JP_DiseaseCategory.Count == 0)
            {
                return new DataView();
            }
            return new DataView(Ctrl.Dto.DiseaseDataSet.T_JP_DiseaseCategory, string.Empty, "sequence", DataViewRowState.CurrentRows);
        }


        //private async Task<bool> GetIMESettingAsync()
        //{
        //    var imeSetting = await _systemRepository.GetSettingAsync("IMESetting");
        //    return string.IsNullOrEmpty(imeSetting) || bool.Parse(imeSetting);
        //}

        // Additional private helper methods would go here...
    }

    public interface ISearchService
    {
        Task<SearchViewModel> InitializeSearchPageAsync(UserModel currentUser);
        Task<string> GetVMUpdateDateAsync();
        string GetVMUpdateDate();
        DataView GetDiseaseCategories();
        //Task<DataView> GetTreeCategoriesAsync(string categoryType);
        //Task<DataView> GetMedicalCalculatorViewAsync(string category = "");
        //Task<bool> ProcessSearchAsync(string searchText, string searchTerm, string userIdentity);
        //Task<EnqueteInfo> GetEnqueteInfoAsync(UserModel currentUser);
    }

    public class SearchViewModel
    {
        public UserModel CurrentUser { get; set; }
        public bool ShowBanMessage { get; set; }
        public string BanMessageText { get; set; }
        public bool ImeEnabled { get; set; }
        public bool ShowSubscriptionExpiry { get; set; }
        public bool ShowFirstTimeDialog { get; set; }
        public bool ShowTrialAlert { get; set; }
        public int TrialDaysRemaining { get; set; }
        public HeaderImageSettings HeaderImageSettings { get; set; }
        public string TopInformation { get; set; }
        public string BannerHtml { get; set; }
        public DataView DiseaseCategories { get; set; }
        public DataView DrugCategories { get; set; }
        public DataView LabCategories { get; set; }
        public DataView InsuranceCategories { get; set; }
        public List<TreeNode> MedicalCalculatorMenu { get; set; }
    }

    public class HeaderImageSettings
    {
        public string ImageUrl { get; set; }
        public string AlternateText { get; set; }
        public string ToolTip { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public class EnqueteInfo
    {
        public bool ShouldDisplay { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
    }
}

