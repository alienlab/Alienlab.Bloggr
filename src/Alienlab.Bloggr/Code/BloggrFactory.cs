namespace Alienlab.Bloggr
{
  using System;
  using System.Linq;

  using Sitecore;
  using Sitecore.Diagnostics;
  using Sitecore.Sites;
  using Sitecore.StringExtensions;

  public static class BloggrFactory
  {
    public static void CreateBlog([NotNull] string name)
    {
      Assert.ArgumentNotNull(name, "name");
      Assert.ArgumentCondition(GetBlogs().All(x => !x.Equals(name, StringComparison.OrdinalIgnoreCase)), "name", "Blog {0} already exists".FormatWith(name));
    
      
    }

    internal static string[] GetBlogs()
    {
      return SiteContextFactory.Sites
        .Select(x => x.Properties["BlogName"])
        .Where(x => !string.IsNullOrEmpty(x))
        .ToArray();
    }
  }
}