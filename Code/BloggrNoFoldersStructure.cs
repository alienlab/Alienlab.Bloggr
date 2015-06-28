namespace Alienlab.Bloggr
{
  using System.Text.RegularExpressions;
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.SecurityModel;

  public class BloggrNoFoldersStructure : IBloggrStructure
  {
    [NotNull]
    protected readonly Regex BloggrPostNameRegex = new Regex(@"^(\w[\w\-]+\w+)?$", RegexOptions.Compiled);

    public virtual bool ValidatePostName(Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");
      
      var name = bloggrPost.Name;
      Assert.IsNotNull(name, "name");

      return this.BloggrPostNameRegex.IsMatch(name);
    }

    public virtual void FixPostName(Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");
    }

    public virtual bool ValidatePostLocation(Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");

      var parent = bloggrPost.Parent;
      Assert.IsNotNull(parent, "parent");

      return parent.TemplateName == "BloggrHome";
    }

    public void FixPostLocation(Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");

      if (this.ValidatePostLocation(bloggrPost))
      {
        return;
      }

      var home = BloggrContext.GetHomeSure(bloggrPost);
      var itemPathOriginal = bloggrPost.Paths.FullPath;
      using (new SecurityDisabler())
      {
        bloggrPost.MoveTo(home);
      }

      var validLocation = this.ValidatePostLocation(bloggrPost);
      Assert.IsTrue(validLocation, "After an attempt of fixing location of the blog post ({0}) it still remains invalid ({1})", itemPathOriginal, bloggrPost.Paths.FullPath);
    }
  }
}