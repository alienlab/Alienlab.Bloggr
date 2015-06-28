﻿namespace Alienlab.Bloggr 
{
  using System;
  using System.Collections.Specialized;
  using System.Linq;
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Text;
  using Sitecore.Web.UI.Sheer;

  [UsedImplicitly]
  public class BloggrCreatePostCommand : AddMaster
  {
    public override void Execute([NotNull] CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      var items = context.Items;
      Assert.IsNotNull(items, "items");

      var item = items.SingleOrDefault();
      Assert.IsNotNull(item, "item");

      var bloggrPostTemplate = item.GetItem("/sitecore/templates/Bloggr/Global/BloggrPost");
      var home = BloggrContext.GetHome(item);
      var structure = home != null ? BloggrContext.GetPostsStructureItem(home) : null;
      if (structure == null || structure.Get("BloggrPostAutoName") != "1")
      {
        context.Parameters.Add("master", bloggrPostTemplate.ID.ToString());

        base.Execute(context);
        return;
      }

      var template = (TemplateItem)bloggrPostTemplate;
      Context.Workflow.AddItem("BloggrPost", template, item);
    }

    /// <summary>
    /// Workaround for proper logic of base.Execute(context);
    /// </summary>
    [UsedImplicitly]
    protected new void Add([NotNull] ClientPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      base.Add(args);
    }
  }
}
