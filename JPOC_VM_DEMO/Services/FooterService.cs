using JPOC_VM_DEMO.Models;

public interface IFooterService
{
    FooterViewModel GetFooterViewModel();
}

public class FooterService : IFooterService
{
    public FooterViewModel GetFooterViewModel()
    {
        return new FooterViewModel
        {
            VersionInfo = GetVersionInfo(),
            ShowSpVersion = ShouldShowSpVersion(),
            SpVersionUrl = GetSpVersionUrl(),
            ShowPc2Sp = ShouldShowPc2Sp()
        };
    }

    private string GetVersionInfo()
    {
        // Implement version info logic
        return "Version 1.0";
    }

    private bool ShouldShowSpVersion()
    {
        // Implement mobile detection logic
        return false;
    }

    private string GetSpVersionUrl()
    {
        // Implement SP version URL logic
        return "/sp";
    }

    private bool ShouldShowPc2Sp()
    {
        // Implement PC to SP switch logic
        return true;
    }
}
