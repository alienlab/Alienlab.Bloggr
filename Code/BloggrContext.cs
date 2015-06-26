namespace Alienlab.Bloggr
{
  using System.Collections.Generic;
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public static class BloggrContext
  {
    [CanBeNull]
    public static Item GetBloggrHome()
    {
      var item = Context.Item;
      if (item == null)
      {
        return null;
      }

      return GetBloggrHome(item);
    }

    [CanBeNull]
    public static Item GetBloggrHome([NotNull] Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      return item.Axes.SelectSingleItem("ancestor-or-self::*[@@templatekey='bloggrhome']");
    }

    [NotNull]
    public static IReadOnlyCollection<Item> GetBloggrPosts([NotNull] Item bloggrHome)
    {
      Assert.ArgumentNotNull(bloggrHome, "bloggrHome");

      return bloggrHome.Axes
        .SelectItems("descendant::*[@templatekey='bloggrPost']") ?? new Item[0];
    }
  }
}
