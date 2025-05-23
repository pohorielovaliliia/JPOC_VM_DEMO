using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace JPOC_VM_DEMO.Pages
{
    // Pages/Login.cshtml.cs
    //public class LoginModel : PageModel
    //{
    //    //private readonly ICredentialHelper _credentialHelper;
    //    //private readonly SignInManager<ApplicationUser> _signInManager;
    //    private readonly ILogger<LoginModel> _logger;

    //    public LoginModel(
    //        //ICredentialHelper credentialHelper,
    //        //SignInManager<ApplicationUser> signInManager,
    //        ILogger<LoginModel> logger)
    //    {
    //        //_credentialHelper = credentialHelper;
    //        //_signInManager = signInManager;
    //        _logger = logger;
    //    }

    //    [BindProperty]
    //    [Required(ErrorMessage = "???????????????")]
    //    public string LoginID { get; set; }

    //    [BindProperty]
    //    [Required(ErrorMessage = "???????????????")]
    //    public string Password { get; set; }

    //    [BindProperty]
    //    public string Institution { get; set; }

    //    [BindProperty]
    //    public bool RememberMe { get; set; }

    //    public bool ShowVMMessage { get; set; }

    //    public void OnGet()
    //    {
    //        // Check if VM message should be shown based on query parameters
    //        ShowVMMessage = Request.Query["LoginType"].ToString() == "1";
    //    }

    //    public async Task<IActionResult> OnPostAsync()
    //    {
    //        if (!ModelState.IsValid)
    //            return Page();

    //        try
    //        {
    //            //// Authenticate user
    //            //var result = await _credentialHelper.ValidateCredentialsAsync(
    //            //    LoginID,
    //            //    Password,
    //            //    Institution);

    //            //if (result.Succeeded)
    //            //{
    //            //    // Set authentication cookie
    //            //    await _signInManager.SignInAsync(result.User, RememberMe);

    //            //    // Store credentials in cookie if remember me is checked
    //            //    if (RememberMe)
    //            //    {
    //            //        Response.Cookies.Append("LoginID", LoginID);
    //            //        Response.Cookies.Append("Institution", Institution);
    //            //    }

    //            //    _logger.LogInformation($"User {LoginID} logged in successfully");
    //            //    return RedirectToPage("/Index");
    //            //}

    //            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    //            return Page();
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError($"Login error for user {LoginID}: {ex.Message}");
    //            ModelState.AddModelError(string.Empty, "An error occurred during login.");
    //            return Page();
    //        }
    //    }
    //}

    public class LoginModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public string ErrorMessage { get; set; }
        public string MessageVM { get; set; }
        public bool ShowVMMessage { get; set; }
        public bool ShowSiteDescription { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "ユーザー名を入力してください。")]
            public string LoginID { get; set; }

            [Required(ErrorMessage = "パスワードを入力してください。")]
            public string Password { get; set; }

            public string Institution { get; set; }
            public bool RememberMe { get; set; }
        }

        public void OnGet()
        {
            // Set flags/messages as needed
            ShowVMMessage = false;
            ShowSiteDescription = false;
            // TODO: Set MessageVM, etc. from query or service
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "入力内容に誤りがあります。";
                return Page();
            }

            // TODO: Authenticate user here
            // If success:
            // return RedirectToPage("/Index");
            // If failure:
            ErrorMessage = "ログインに失敗しました。";
            return Page();
        }
    }

}
