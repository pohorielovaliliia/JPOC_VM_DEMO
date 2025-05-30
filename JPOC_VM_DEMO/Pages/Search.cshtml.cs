using Jpoc.Dac;
using JPOC_VM_DEMO.App_Code.BaseClass;
using JPOC_VM_DEMO.App_Code.CTRL;
using JPOC_VM_DEMO.App_Code.Util;
using JPOC_VM_DEMO.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace JPOC_VM_DEMO.Pages
{
    // search.aspx.vb -> MainContentsPageBase -> PageBaseModel -> PageBase
    public class SearchModel : MainContentsPageBase //PageModel
    {
        private readonly ISearchService _searchService;
        private readonly IConfiguration _configuration;
        private readonly CtrlSearch _ctrlSearch;

        [BindProperty]
        public string SearchText { get; set; }

        [BindProperty]
        public string SearchTerm { get; set; } = "すべて";

        public bool ShowVMMessage { get; set; }
        public bool ShowSpVersion { get; set; }
        public string VMUpdateDate { get; set; }
        public bool ShowTrialExpire { get; set; }

        // Keep the same functionality as the original DiseaseCategoryView
        private DataView DiseaseCategoryView
        {
            get
            {
                return new DataView();
            }
            //get
            //{
            //    if (_searchService.DiseaseDataSet?.T_JP_DiseaseCategory == null ||
            //        _searchService.DiseaseDataSet.T_JP_DiseaseCategory.Count == 0)
            //    {
            //        return new DataView();
            //    }

            //    return new DataView(
            //        _searchService.DiseaseDataSet.T_JP_DiseaseCategory,
            //        string.Empty,  // No filter
            //        "sequence",    // Sort by sequence
            //        DataViewRowState.CurrentRows
            //    );
            //}
        }
        public List<CategoryItem> DiseaseCategories { get; set; }
        public List<CategoryItem> DrugCategories { get; set; }
        public List<CategoryItem> LabCategories { get; set; }
        public List<CategoryItem> HokenCategories { get; set; }
        public TreeViewModel CalculatorMenu { get; set; }

        private CtrlSearch Ctrl => _ctrlSearch;

        public SearchModel(
            ISearchService searchService,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PageBase> logger,
            JPoCUtility jpocUtility,
            CtrlSearch ctrlSearch
        ) : base(httpContextAccessor, logger, jpocUtility)
        {
            _searchService = searchService;
            _configuration = configuration;
            _ctrlSearch = ctrlSearch;
        }

        public void OnGet()
        {
            // Initialize all the data
            //DiseaseCategories = await _searchService.GetDiseaseCategoriesAsync();
            //DrugCategories = await _searchService.GetDrugCategoriesAsync();
            //LabCategories = await _searchService.GetLabCategoriesAsync();
            //HokenCategories = await _searchService.GetHokenCategoriesAsync();
            //CalculatorMenu = await _searchService.GetCalculatorMenuAsync();

            //VMUpdateDate = await _searchService.GetVMUpdateDateAsync();

            var currentUserInfo = UserInfo;

            //Ctrl.GetData(currentUserInfo.InstitutionId);
            Ctrl.GetData("997");

            VMUpdateDate = _searchService.GetVMUpdateDate();
            ShowVMMessage = _configuration.GetValue<bool>("ShowVMMessage");
            ShowSpVersion = _configuration.GetValue<bool>("ShowSpVersion");
            DiseaseCategories = getDiseaseCategory(_searchService.GetDiseaseCategories());
        }
        public string GetTab1Text()
        {
            if (GlobalVariables.isFoundation)
                return "掲載コンテンツ一覧";
            else
                return "症状・疾患";
        }
        public List<CategoryItem> getDiseaseCategory(DataView dataView)
        {
            const string url = "menuDetails.aspx?category={0}&currentpage={1}";

            List<CategoryItem> diseaseCategories = new List<CategoryItem>();
            foreach (DataRowView drv in dataView)
            {
                if (drv != null)
                {
                    // Cast the DataRow to the strongly-typed row if possible
                    var dr = drv.Row as Jpoc.Dac.DS_DISEASE.T_JP_DiseaseCategoryRow;
                    if (dr != null)
                    {
                        CategoryItem item = new CategoryItem();
                        item.Title = Utilities.HtmlEncodeHtmlTag(dr.category);
                        item.Url = Utilities.HtmlEncodeHtmlTag(
                            string.Format(url, dr.id, "PageName"));
                        diseaseCategories.Add(item);
                    }
                }
            }
            return diseaseCategories;
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                ModelState.AddModelError(string.Empty, "検索キーワードを入れてください");
                return Page();
            }

            // Redirect to search results
            return RedirectToPage("/SearchResults", new { q = SearchText, term = SearchTerm });
        }
    }

    public class CategoryItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsHeader { get; set; }
        public List<CategoryItem> Children { get; set; }
    }

    public class TreeViewModel
    {
        public List<TreeNode> Menu1 { get; set; }
        public List<TreeNode> Menu2 { get; set; }
        public List<TreeNode> Menu3 { get; set; }
    }

    public class TreeNode
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public List<TreeNode> Children { get; set; }
    }
}

