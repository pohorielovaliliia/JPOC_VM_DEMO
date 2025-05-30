using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using JPOC_VM_DEMO.Common;
using JPOC_VM_DEMO.App_Code.CTRL;
using JPOC_VM_DEMO.App_Code.CTRL.DTO;
using JPOC_VM_DEMO.App_Code.Util;


namespace JPOC_VM_DEMO.App_Code.BaseClass
{

    public abstract class PageBaseModel : PageBase
    {
        #region Fields
        private PublicEnum.AccessRight _AccessRight;
        private string _ReturnValue;
        private string _EnableSolr;
        protected AbstractCtrl mCtrl;
        private AbstractDto mDto;
        #endregion

        #region Properties
        protected string ObjTypeName
        {
            get
            {
                string s = this.GetType().ToString();
                if (string.IsNullOrEmpty(s)) return string.Empty;
                s = s.Replace("Pages.", string.Empty).Replace("Page", string.Empty);
                return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
            }
        }

        // TopMasterPage is removed as Layouts replace MasterPages in .NET Core
        #endregion

        #region Constructor
        protected PageBaseModel(IHttpContextAccessor httpContextAccessor, ILogger<PageBase> logger, JPoCUtility jpocUtility)
            : base(httpContextAccessor, logger, jpocUtility)
        {
            try
            {
                _AccessRight = PublicEnum.AccessRight.None;

                // Culture is now handled through middleware or RequestLocalizationOptions
                var culture = GlobalVariables.AppCultureInfo;
                if (culture != null)
                {
                    Thread.CurrentThread.CurrentUICulture = culture;
                }

                string dtoName = "jpoc.Dto" + this.ObjTypeName;
                Type dtoType = Type.GetType(dtoName);
                if (dtoType != null)
                {
                    object obj = Activator.CreateInstance(dtoType);
                    if (obj != null) mDto = (AbstractDto)obj;
                }

                string ctrlName = "jpoc.Ctrl" + this.ObjTypeName;
                Type ctrlType = Type.GetType(ctrlName);
                if (ctrlType != null)
                {
                    object obj = Activator.CreateInstance(ctrlType, mDto);
                    if (obj != null) mCtrl = (AbstractCtrl)obj;
                }
            }
            catch (Exception ex)
            {
                this.ExceptionManager(ex);
            }
        }
        #endregion
    }
}
