using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alienlab.Bloggr.Code.Templates
{
  using Sitecore;
  using Sitecore.Pipelines.RenderItemTile;

  public class BloggrRootItem : BloggrTemplate
  {
    public override string Icon
    {
      get
      {
        return "Network/16x16/server.png";
      }
    }
  }

  public class BloggrStructureItem : BloggrTemplate
  {
    
  }

  public class BloggrAuthorItem : BloggrPageItem
  {
    public override string Icon
    {
      get
      {
        return "Applications/16x16/photo_portrait.png";
      }
    }
  }

  public class BloggrPostItem : BloggrTemplate
  {
    public override string Icon
    {
      get
      {
        return "Business/16x16/note.png";
      }
    }
  }

  public class BloggrPageItem : BloggrTemplate
  {
    public override string Icon
    {
      get
      {
        return "Applications/16x16/document_plain.png";
      }
    }
  }

  public class BloggrHomeItem : BloggrTemplate
  {
    public override string Icon
    {
      get
      {
        return "network/16x16/home.png";
      }
    }
  }

  public class BloggrFeedItem : BloggrPageItem
  {
    public override string Icon
    {
      get
      {
        return "Business/16x16/index.png";
      }
    }
  }

  public class BloggrCreatedDateFolderStructureItem : BloggrTemplate
  {

  }

  public class BloggrPostFolderItem : BloggrTemplate
  {
    public override string Icon
    {
      get
      {
        return "Applications/16x16/folder.png";
      }
    }
  }

  public abstract class BloggrTemplate
  {
    [NotNull]
    public virtual string TemplateName
    {
      get
      {
        var name = this.GetType().Name;
        return name.EndsWith("Item") ? name.Substring(0, name.Length - "Item".Length) : name;
      }
    }

    [CanBeNull]
    public virtual string Icon
    {
      get
      {
        return null;
      }
    }
  }
}
