namespace BryceCampainResume.Tests;

public class HomePageTests : BunitContext
{
    [Fact]
    public void RendersNameAndTitle()
    {
        var cut = Render<Pages.Home>();

        Assert.Contains("Bryce Campain", cut.Markup);
        Assert.Contains("Information Technology Leader", cut.Markup);
    }

    [Fact]
    public void RendersResumeDownloadLink()
    {
        var cut = Render<Pages.Home>();

        var link = cut.Find("a.download-resume-button");
        Assert.Equal("/Bryce-Campain-Resume-Nov2025.pdf", link.GetAttribute("href"));
        Assert.Equal("Download PDF Résumé", link.TextContent.Trim());
    }

    [Fact]
    public void FunModeIconIsHiddenByDefault()
    {
        var cut = Render<Pages.Home>();

        Assert.Throws<ElementNotFoundException>(() => cut.Find("i.fa-face-laugh-beam"));
    }

    [Fact]
    public void FunModeIconAppearsWhenEnabled()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = Render<Pages.Home>();

        var toggle = cut.Find("input[type=checkbox]");
        toggle.Change(true);

        cut.WaitForAssertion(() =>
        {
            var icon = cut.Find("i.fa-face-laugh-beam");
            Assert.Contains("fa-regular", icon.ClassList);
        });
    }
}
