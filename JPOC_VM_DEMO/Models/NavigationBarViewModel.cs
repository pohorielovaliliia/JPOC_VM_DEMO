namespace JPOC_VM_DEMO.Models
{
    public class NavigationBarViewModel
    {
        public bool IsAuthenticated { get; set; }
        public bool ShowShareLink { get; set; }
        public string ShareEmail { get; set; }
        public string ShareMessage { get; set; }
        public int NotificationCount { get; set; }
        public bool ShowMyPage { get; set; }
        public bool ShowUserMenu { get; set; }
        public string UserName { get; set; }
        public bool ShowSearchHistory { get; set; }
        public bool ShowIndividualAccountMenu { get; set; }
        public List<MenuItemModel> AccountMenuItems { get; set; }
        public bool ShowLogout { get; set; }
        public bool ShowAccountRegister { get; set; }
        public string FeedbackUrl { get; set; }
        public string GuidePageText { get; set; }
        public bool ShowContentListDownload { get; set; }
        public bool ShowVMPackageDownload { get; set; }
        //public AdminMenuViewModel AdminMenuData { get; set; }
        //public AnnouncementViewModel AnnouncementData { get; set; }
    }

    public class MenuItemModel
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }

}
