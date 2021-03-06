﻿namespace Alienlab.Bloggr
{
  using System;
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Events;
  using Sitecore.SecurityModel;

  public class BloggrPostEventHandler
  {
    [UsedImplicitly]
    protected void OnItemAdded([NotNull] object sender, [NotNull] EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

      var item = Event.ExtractParameter(args, 0) as Item;
      Assert.IsNotNull(item, "item");

      if (item.TemplateName != "Bloggr Post")
      {
        return;
      }

      try
      {
        using (new SecurityDisabler())
        {
          var structure = BloggrContext.GetPostsStructureSure(item);

          // fix post location first as post name may depend on it
          structure.FixPostLocation(item);

          // fix post name then
          structure.FixPostName(item);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Unhandled exception occurred in Alienlab.Bloggr.BloggrPostEventHandler.OnItemAdded(sender, args)", ex, this);
      }
    }

    [UsedImplicitly]
    protected void OnItemMoved([NotNull] object sender, [NotNull] EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

      var item = Event.ExtractParameter(args, 0) as Item;
      Assert.IsNotNull(item, "item");

      if (item.TemplateName != "Bloggr Post")
      {
        return;
      }

      try
      {
        using (new SecurityDisabler())
        {
          var structure = BloggrContext.GetPostsStructureSure(item);

          structure.FixPostLocation(item);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Unhandled exception occurred in Alienlab.Bloggr.EventHandler.OnItemMoved(sender, args)", ex, this);
      }
    }

    [UsedImplicitly]
    protected void OnItemRenamed([NotNull] object sender, [NotNull] EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

      var item = Event.ExtractParameter(args, 0) as Item;
      Assert.IsNotNull(item, "item");

      if (item.TemplateName != "Bloggr Post")
      {
        return;
      }

      try
      {
        using (new SecurityDisabler())
        {
          var structure = BloggrContext.GetPostsStructureSure(item);

          // fix post name because item was renamed and new name may be invalid according to current site IBloggrStructure rules
          structure.FixPostName(item);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Unhandled exception occurred in Alienlab.Bloggr.EventHandler.OnItemRenamed(sender, args)", ex, this);
      }
    }

    [UsedImplicitly]
    protected void OnItemSaved([NotNull] object sender, [NotNull] EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

      var item = Event.ExtractParameter(args, 0) as Item;
      Assert.IsNotNull(item, "item");

      if (item.TemplateName != "Bloggr Post")
      {
        return;
      }

      var changes = Event.ExtractParameter(args, 1) as ItemChanges;
      Assert.IsNotNull(changes, "changes");

      if (!changes.FieldChanges.Contains(FieldIDs.DisplayName))
      {
        return;
      }

      try
      {
        using (new SecurityDisabler())
        {
          var structure = BloggrContext.GetPostsStructureSure(item);

          // fix post name because item was chang es and new name may get obsolete and invalid according to current site IBloggrStructure rules
          structure.FixPostName(item);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Unhandled exception occurred in Alienlab.Bloggr.EventHandler.OnItemSaved(sender, args)", ex, this);
      }
    }
  }
}
