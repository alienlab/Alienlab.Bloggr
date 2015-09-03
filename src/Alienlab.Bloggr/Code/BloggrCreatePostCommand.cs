namespace Alienlab.Bloggr 
{
  using System.Linq;
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Framework.Commands;
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

      var home = BloggrContext.GetHome(item);
      var structure = home != null ? BloggrContext.GetPostsStructureItem(home) : null;
      var bloggrPostTemplate = item.GetItem("{2BDE036E-3503-4BF0-B978-213F46541B21}");
      if (structure == null || structure.Get("Bloggr Post AutoName") != "1")
      {
        context.Parameters.Add("master", bloggrPostTemplate.ID.ToString());

        base.Execute(context);
        return;
      }

      var template = (TemplateItem)bloggrPostTemplate;
      Context.Workflow.AddItem("Bloggr Post", template, item);
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
