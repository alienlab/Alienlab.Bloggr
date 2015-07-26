namespace Alienlab.Bloggr
{
  using System;
  using System.Linq;
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.SecurityModel;

  [UsedImplicitly]
  public class BloggrCreatedDateFoldersStructure : IBloggrStructure
  {
    public virtual bool ValidatePostName(Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");
      
      var name = bloggrPost.Name;
      Assert.IsNotNull(name, "name");

      int num;
      if (!int.TryParse(name, out num))
      {
        return false;
      }

      var children = bloggrPost.Parent.Children;
      Assert.IsNotNull(children, "children");

      return children.Count(x => x != null && x.Name == name) == 1;
    }

    public virtual void FixPostName(Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");
      
      var children = bloggrPost.Parent.Children;
      Assert.IsNotNull(children, "children");

      int tmp;
      var childNumbers = children
        .Select(x => x != null && int.TryParse(x.Name, out tmp) ? (int?)tmp : null)
        .Where(x => x != null)
        .Select(x => (int)x)
        .ToArray();

      var maxNum = 0;
      if (childNumbers.Any())
      {
        maxNum = childNumbers.Max();
        int intName;
        if (int.TryParse(bloggrPost.Name, out intName))
        {

          if (intName == maxNum && childNumbers.Count(x => x == maxNum) == 1)
          {
            return;
          }
        }
      }

      var newName = (maxNum + 1).ToString("D2");
      Assert.IsNotNull(newName, "newName");

      this.RenameItem(bloggrPost, newName);
    }

    public virtual bool ValidatePostLocation([NotNull] Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");

      var statistics = bloggrPost.Statistics;
      Assert.IsNotNull(statistics, "statistics");

      var created = statistics.Created;
      if (created == DateTime.MinValue)
      {
        // in Sitecore 8.0 is used UTC, in previous versions - local time.
        created = DateTime.UtcNow;
      }

      var day = bloggrPost.Parent;
      if (day == null || day.Name != created.ToString("dd"))
      {
        return false;
      }

      var month = day.Parent;
      if (month == null || month.Name != created.ToString("MM"))
      {
        return false;
      }

      var year = month.Parent;
      if (year == null || year.Name != created.ToString("yyyy"))
      {
        return false;
      }

      return BloggrContext.GetHome(year) != null;
    }

    public virtual void FixPostLocation(Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");

      if (this.ValidatePostLocation(bloggrPost))
      {
        return;
      }

      var statistics = bloggrPost.Statistics;
      Assert.IsNotNull(statistics, "statistics");

      var created = statistics.Created;
      if (created == DateTime.MinValue)
      {
        // in Sitecore 8.0 is used UTC, in previous versions - local time.
        created = DateTime.UtcNow;
      }

      var createdYear = created.ToString("yyyy");
      var createdMonth = created.ToString("MM");
      var createdDay = created.ToString("dd");

      var home = BloggrContext.GetHomeSure(bloggrPost);
      var year = home.GetChild(createdYear) ?? this.CreateDateFolder(home, home, createdYear);
      var month = year.GetChild(createdMonth) ?? this.CreateDateFolder(year, home, createdMonth);
      var day = month.GetChild(createdDay) ?? this.CreateDateFolder(month, home, createdDay);
      bloggrPost.MoveTo(day);
    }
    
    [NotNull]
    internal virtual Item CreateDateFolder([NotNull] Item parent, [NotNull] Item home, [NotNull] string folderName)
    {
      Assert.ArgumentNotNull(parent, "parent");
      Assert.ArgumentNotNull(home, "home");
      Assert.ArgumentNotNull(folderName, "folderName");
      var bloggrStructure = BloggrContext.GetPostsStructureItem(home);
      var templateField = bloggrStructure.GetSure("BloggrFolderTemplate");

      var dateFolder = parent.Add(folderName, new TemplateID(new ID(templateField)));
      Assert.IsNotNull(dateFolder, "dateFolder");

      return dateFolder;
    }

    internal virtual void RenameItem([NotNull] Item bloggrPost, [NotNull] string newName)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");
      Assert.ArgumentNotNull(newName, "newName");

      using (new EditContext(bloggrPost, SecurityCheck.Disable))
      {
        bloggrPost.Name = newName;
      }
    }
  }
}
