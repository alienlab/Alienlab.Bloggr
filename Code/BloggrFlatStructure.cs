namespace Alienlab.Bloggr
{
  using System.Globalization;
  using System.Linq;
  using System.Text.RegularExpressions;
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.SecurityModel;
  using UnidecodeSharpFork;

  public class BloggrFlatStructure : IBloggrStructure
  {
    [NotNull]
    protected readonly Regex BloggrPostNameRegex = new Regex(@"^(\d+)?(\-)?([\w\d\-]+)?$", RegexOptions.Compiled);

    [NotNull]
    protected readonly Regex UnsupportedSymbolsRegex = new Regex(@"[^\w\d\(\)]+", RegexOptions.Compiled);

    [NotNull]
    protected readonly object SyncRoot = new object();

    public virtual bool ValidatePostLocation(Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");

      var parent = bloggrPost.Parent;
      Assert.IsNotNull(parent, "parent");

      return parent.TemplateName == "BloggrHome";
    }

    public virtual bool ValidatePostName(Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");
      var name = bloggrPost.Name;
      Assert.IsNotNull(name, "name");

      var match = this.BloggrPostNameRegex.Match(name);

      return !string.IsNullOrEmpty(match.Groups[1].Value) && !string.IsNullOrEmpty(match.Groups[2].Value) && !string.IsNullOrEmpty(match.Groups[3].Value);
    }

    public virtual bool TryToFix(Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");

      var validLocation = this.ValidatePostLocation(bloggrPost);
      var validName = this.ValidatePostName(bloggrPost);
      if (validLocation && validName)
      {
        return true;
      }

      var home = BloggrContext.GetBloggrHome(bloggrPost);
      if (home == null)
      {
        return false;
      }

      if (!validLocation)
      {
        var itemPathOriginal = bloggrPost.Paths.FullPath;
        using (new SecurityDisabler())
        {
          bloggrPost.MoveTo(home); 
        }

        validLocation = this.ValidatePostLocation(bloggrPost);
        Assert.IsTrue(validLocation, "After an attempt of fixing location of the blog post ({0}) it still remains invalid ({1})", itemPathOriginal, bloggrPost.Paths.FullPath);
      }

      var nameOriginal = bloggrPost.Name;
      Assert.IsNotNull(nameOriginal, "name");

      if (!validName)
      {
        var match = this.BloggrPostNameRegex.Match(nameOriginal);
        var text = this.FixName(bloggrPost);

        // check number last  as if it is not okay we need to lock to compute the value
        var number = match.Groups[1].Value;
        if (string.IsNullOrEmpty(number))
        {
          lock (this.SyncRoot)
          {
            number = BloggrContext.GetBloggrPosts(home)
              .Count(x => this.ValidatePostName(bloggrPost)).ToString();

            using (new EditContext(bloggrPost, SecurityCheck.Disable))
            {
              bloggrPost.Name = number + "-" + text;
            }
          }
        }
        else
        {
          using (new EditContext(bloggrPost, SecurityCheck.Disable))
          {
            bloggrPost.Name = number + "-" + text;
          }
        }

        validName = this.ValidatePostName(bloggrPost);
        Assert.IsTrue(validName, "After an attempt of fixing name of the blog post ({0}) it still remains invalid ({1})", nameOriginal, bloggrPost.Name);
      }

      return true;
    }

    [NotNull]
    internal virtual string FixName([NotNull] Item bloggrPost)
    {
      Assert.ArgumentNotNull(bloggrPost, "bloggrPost");

      var displayName = bloggrPost.DisplayName;
      Assert.IsNotNullOrEmpty(displayName, "displayName");

      var latinTitle = displayName.Unidecode();
      Assert.IsNotNull(latinTitle, "latinTitle");

      latinTitle = this.UnsupportedSymbolsRegex.Replace(latinTitle, "-").Replace("-)", ")").Replace("(-", "(").Trim("-".ToCharArray());

      return char.ToUpper(latinTitle[0]) + (latinTitle.Length > 1 ? latinTitle.Substring(1).ToLower() : string.Empty);
    }
  }
}