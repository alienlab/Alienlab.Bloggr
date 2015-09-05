namespace Alienlab.Bloggr
{
  using System.Linq;

  using Sitecore;
  using Sitecore.Sites;

  public static class BloggrFactory
  {
    [NotNull]
    public static string[] GetBlogNames()
    {
      return SiteContextFactory.Sites.Select(x => x.Properties["BlogName"]).Where(x => !string.IsNullOrEmpty(x)).ToArray();
    }
  }
}