using JPOC_VM_DEMO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JPOC_VM_DEMO.Pages
{
    public class SearchModel : PageModel
    {
        private readonly ISearchService _searchService;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public string SearchText { get; set; }

        [BindProperty]
        public string SearchTerm { get; set; } = "すべて";

        public bool ShowVMMessage { get; set; }
        public bool ShowSpVersion { get; set; }
        public string VMUpdateDate { get; set; }
        public bool ShowTrialExpire { get; set; }
        public string Tab1Text { get; set; } = "症状・疾患";
        public List<CategoryItem> DiseaseCategories { get; set; }
        public List<CategoryItem> DrugCategories { get; set; }
        public List<CategoryItem> LabCategories { get; set; }
        public List<CategoryItem> HokenCategories { get; set; }
        public TreeViewModel CalculatorMenu { get; set; }

        public SearchModel(ISearchService searchService, IConfiguration configuration)
        {
            _searchService = searchService;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            // Initialize all the data
            //DiseaseCategories = await _searchService.GetDiseaseCategoriesAsync();
            //DrugCategories = await _searchService.GetDrugCategoriesAsync();
            //LabCategories = await _searchService.GetLabCategoriesAsync();
            //HokenCategories = await _searchService.GetHokenCategoriesAsync();
            //CalculatorMenu = await _searchService.GetCalculatorMenuAsync();

            VMUpdateDate = await _searchService.GetVMUpdateDateAsync();
            ShowVMMessage = _configuration.GetValue<bool>("ShowVMMessage");
            ShowSpVersion = _configuration.GetValue<bool>("ShowSpVersion");
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

