using Jpoc.Common;
using JPOC_VM_DEMO.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Utilities = JPOC_VM_DEMO.Common.Utilities;
using GlobalVariables = JPOC_VM_DEMO.Common.GlobalVariables;
using JPOC_VM_DEMO.App_Code.Util;

namespace JPOC_VM_DEMO.App_Code.BaseClass
{
    public abstract class PageBase : PageModel
    {
        // Dependency-injected services
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly ILogger<PageBase> Logger;
        protected readonly JPoCUtility JpocUtil;

        // Access right enum
        protected PublicEnum.eAccsessRight AccessRight { get; set; } = PublicEnum.eAccsessRight.None;

        // Properties mapped from session or request
        protected bool IsPopUp { get; set; }
        protected string PageName => System.IO.Path.GetFileNameWithoutExtension(HttpContextAccessor.HttpContext.Request.Path).ToLower();
        protected UserModel UserInfo => HttpContextAccessor.HttpContext.Session.GetObject<UserModel>(GlobalVariables.Session.USER_INF);
        protected bool HasUserInfo => UserInfo != null;
        protected int UserId => HasUserInfo ? Utilities.N0Int(UserInfo.Id) : 0;
        protected string UserCode => HasUserInfo ? UserInfo.UserCode : string.Empty;
        protected string UserName => HasUserInfo ? Utilities.NZ(UserInfo.Name) : string.Empty;
        protected int InstitutionID => HasUserInfo ? Utilities.N0Int(UserInfo.InstitutionId) : 0;
        protected string InstitutionName => HasUserInfo ? Utilities.NZ(UserInfo.InstitutionName) : string.Empty;
        protected int RoleId => HasUserInfo ? Utilities.N0Int(UserInfo.RoleId) : 0;
        protected string Email => HasUserInfo ? Utilities.NZ(UserInfo.Email) : string.Empty;
        protected JPOC_VM_DEMO.Common.PublicEnum.eUserType UserType => HasUserInfo ? UserInfo.UserType : JPOC_VM_DEMO.Common.PublicEnum.eUserType.Undefined;
        protected DateTime LoginExpire => HasUserInfo ? UserInfo.LoginExpire : new DateTime(2099, 12, 31);
        protected bool IsAdmin => HasUserInfo && UserInfo.AdminFlag;
        protected bool IndTcAcceptance => HasUserInfo && UserInfo.IndTcAcceptance;
        protected bool IndRuacceptance => HasUserInfo && UserInfo.IndRuAcceptance;
        protected bool IndPpacceptance => HasUserInfo && UserInfo.IndPpAcceptance;
        protected JPOC_VM_DEMO.Common.PublicEnum.eLoginType LoginType => HasUserInfo ? UserInfo.LoginType : JPOC_VM_DEMO.Common.PublicEnum.eLoginType.Undefined;
        protected bool IsExpired => HasUserInfo && UserType == JPOC_VM_DEMO.Common.PublicEnum.eUserType.IND && LoginExpire < GlobalVariables.JpnNow.Date;
        protected bool IsElsevierManager => HasUserInfo && UserInfo.IsElsevierManager;
        protected string UserUniqueIdentity => LoginType == JPOC_VM_DEMO.Common.PublicEnum.eLoginType.IpAddress
            ? HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()
            : UserId.ToString();

        // Session-backed properties
        protected string SearchTerm
        {
            get => HttpContextAccessor.HttpContext.Session.GetString("SearchTerm") ?? string.Empty;
            set => HttpContextAccessor.HttpContext.Session.SetString("SearchTerm", value ?? string.Empty);
        }
        protected string SearchText
        {
            get => HttpContextAccessor.HttpContext.Session.GetString("SearchText") ?? string.Empty;
            set => HttpContextAccessor.HttpContext.Session.SetString("SearchText", value ?? string.Empty);
        }
        protected string SearchTab
        {
            get => HttpContextAccessor.HttpContext.Session.GetString("SearchTab") ?? string.Empty;
            set => HttpContextAccessor.HttpContext.Session.SetString("SearchTab", value ?? string.Empty);
        }
        protected string SearchType
        {
            get => HttpContextAccessor.HttpContext.Session.GetString("SearchType") ?? string.Empty;
            set => HttpContextAccessor.HttpContext.Session.SetString("SearchType", value ?? string.Empty);
        }
        protected string AllDrugFlag
        {
            get => HttpContextAccessor.HttpContext.Session.GetString("AllDrugFlag") ?? string.Empty;
            set => HttpContextAccessor.HttpContext.Session.SetString("AllDrugFlag", value ?? string.Empty);
        }

        protected List<string> DialogMessages
        {
            get => HttpContextAccessor.HttpContext.Session.GetObject<List<string>>("MESSAGE_LIST") ?? new List<string>();
            set => HttpContextAccessor.HttpContext.Session.SetObject("MESSAGE_LIST", value ?? new List<string>());
        }

        // Example: Use for feature flags
        protected bool EnableSolr => !string.IsNullOrEmpty(GlobalVariables.SolrUrl) && GlobalVariables.EnableSolr;

        // Example: Use for ban/credit/demo flags
        protected bool IsBan => HasUserInfo && UserInfo.IsBan;
        protected bool IsCreditError => HasUserInfo && UserInfo.IsCreditError;
        protected bool IsDemo
        {
            get
            {
                if (!HasUserInfo)
                    return GlobalVariables.IsDemo == PublicEnum.eDemoType.DEMO;
                if (GlobalVariables.IsDemo != PublicEnum.eDemoType.DEMO)
                    return false;
                if (GlobalVariables.DemoAdminIP == Utilities.GetClientIP() && UserInfo.RoleId == (int)JPOC_VM_DEMO.Common.PublicEnum.eRole.ElsevierAdmin)
                        return false;
                return true;
            }
        }

        // Constructor with DI
        protected PageBase(IHttpContextAccessor httpContextAccessor, ILogger<PageBase> logger, JPoCUtility jpocUtil)
        {
            HttpContextAccessor = httpContextAccessor;
            Logger = logger;
            JpocUtil = jpocUtil;
        }

        // Example: Message helpers
        protected string GetMessage(string code, params string[] values) => JpocUtil.GetMessage(code, values);
        protected MessageSet GetMessageSet(string code, params string[] values) => JpocUtil.GetMessageSet(code, values);

        protected void ExceptionManager(Exception ex) => JpocUtil.ExceptionManager(ex);

        protected void AddDialogMessage(string message)
        {
            var list = DialogMessages;
            list.Add(message);
            DialogMessages = list;
        }
        protected void ClearDialogMessage() => DialogMessages = new List<string>();
        protected string GetDialogMessage(bool isJavaScriptFormat = true) => JpocUtil.ConvMessage(DialogMessages, isJavaScriptFormat);

        // Example: Session helpers (replace WebMethod with API endpoints in controllers)
        public string GetSession(string key) => HttpContextAccessor.HttpContext.Session.GetString(key) ?? string.Empty;
        public void SetSession(string key, string value) => HttpContextAccessor.HttpContext.Session.SetString(key, value ?? string.Empty);
        public void RemoveSession(string key) => HttpContextAccessor.HttpContext.Session.Remove(key);

        // Example: Render logic (move to Razor view or middleware)
        // In ASP.NET Core, you would use TagHelpers, ViewComponents, or middleware for HTML rewriting.

        // Example: Culture initialization
        public override void OnPageHandlerExecuting(Microsoft.AspNetCore.Mvc.Filters.PageHandlerExecutingContext context)
        {
            if (RoleId == (int) JPOC_VM_DEMO.Common.PublicEnum.eRole.EntitlementSupport || RoleId == (int)JPOC_VM_DEMO.Common.PublicEnum.eRole.EntitlementAudit)
            {
                CultureInfo culture = new CultureInfo("en-US");
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }
            base.OnPageHandlerExecuting(context);
        }

        // Example: Access right logic and page init can be handled in OnGet/OnPost or via filters/middleware.

        // ... Additional logic and methods as needed ...
    }

    // Extension methods for session object serialization
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));
        }
        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }
}

