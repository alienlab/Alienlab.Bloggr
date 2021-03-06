﻿namespace Alienlab.Bloggr
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public static class BloggrContext
  {
    public const string BloggrStructure = "Bloggr Structure";
    public const string BloggrStructureType = "Bloggr Structure Type";

    [CanBeNull]
    public static Item GetHome()
    {
      var item = Context.Item;
      if (item == null)
      {
        return null;
      }

      return GetHome(item);
    }

    [CanBeNull]
    public static Item GetHome([NotNull] Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      return item.Axes.SelectSingleItem("ancestor-or-self::*[@@templatekey='bloggr home']");
    }

    [NotNull]
    public static Item GetHomeSure()
    {
      return GetHomeSure(Sitecore.Context.Item);
    }

    [NotNull]
    public static Item GetHomeSure([NotNull] Item item)
    {
      Assert.ArgumentNotNull(item, "item");
      
      var bloggrHome = GetHome(item);
      Assert.IsNotNull(bloggrHome, "Cannot find Bloggr Home item for {0}", item.Paths.FullPath);

      return bloggrHome;
    }

    [NotNull]
    public static IReadOnlyCollection<Item> GetPosts()
    {
      var homeItem = GetHomeSure();
      return GetPosts(homeItem);
    }

    [NotNull]
    public static IReadOnlyCollection<Item> GetPosts([NotNull] Item bloggrHome)
    {
      Assert.ArgumentNotNull(bloggrHome, "bloggrHome");

      return bloggrHome.Axes.GetDescendants().Where(x => x.TemplateName == "Bloggr Post").ToArray();
    }

    [NotNull]
    public static IBloggrStructure GetPostsStructureSure([NotNull] Item item)
    {
      Assert.ArgumentNotNull(item, "item");

      var bloggrHome = GetHomeSure(item);
      var bloggrStructure = GetPostsStructureItem(bloggrHome);
      var typeName = bloggrStructure.GetSure(BloggrStructureType);

      var type = Type.GetType(typeName);
      Assert.IsNotNull(type, "The {0} type cannot be found", typeName);

      var instance = Activator.CreateInstance(type);
      Assert.IsNotNull(instance, "Cannot create an instance of {0}", typeName);

      return (IBloggrStructure)instance;
    }

    [NotNull]
    public static Item GetPostsStructureItem([NotNull] Item bloggrHome)
    {
      Assert.ArgumentNotNull(bloggrHome, "bloggrHome");

      var bloggrStructureId = bloggrHome.GetSure(BloggrStructure);
      var bloggrStructure = bloggrHome.GetItemSure(bloggrStructureId);

      return bloggrStructure;
    }
  }
}
