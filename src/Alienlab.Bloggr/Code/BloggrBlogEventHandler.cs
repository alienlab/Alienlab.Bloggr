namespace Alienlab.Bloggr
{
  using System;

  using Sitecore;
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Events;

  public class BloggrBlogEventHandler
  {
    [NotNull]
    private static readonly string DefaultBlogSecurity = Settings.GetSetting("Bloggr.DefaultBlogSecurity", @"ar|sitecore\Bloggr Administrators - $name|pd|+item:rename|+item:create|+item:delete|+item:admin|ar|sitecore\Bloggr Reviewers - $name|pd|+item:write|ar|sitecore\Bloggr Authors - $name|pd|+item:rename|+item:write|+item:create|");
          
    [UsedImplicitly]
    protected void OnItemAdded([NotNull] object sender, [NotNull] EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

      var item = Event.ExtractParameter(args, 0) as Item;
      Assert.IsNotNull(item, "item");

      if (item.TemplateName != "Bloggr Root")
      {
        return;
      }

      try
      {
        var name = item.Name;
        Assert.IsNotNullOrEmpty(name, "name");

        BloggrInstallHook.EnsureBlogRoles(name);

        using (new EditContext(item))
        {
          item[FieldIDs.Security] = DefaultBlogSecurity.Replace("$name", name);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Unhandled exception occurred in Alienlab.Bloggr.BloggrBlogEventHandler.OnItemAdded(sender, args)", ex, this);
      }
    }
  }
}