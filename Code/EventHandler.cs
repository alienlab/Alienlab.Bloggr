namespace Alienlab.Bloggr
{
  using System;
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Events;

  public class EventHandler
  {
    [UsedImplicitly]
    protected void OnItemSaved([NotNull] object sender, [NotNull] EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull(args, "args");

      var item = Event.ExtractParameter(args, 0) as Item;
      Assert.IsNotNull(item, "item");

      if (item.TemplateName != "BloggrPost")
      {
        return;
      }

      throw new NotImplementedException("Use strategy, BloggrFlatStructure is ready and covered");
    }
  }
}
