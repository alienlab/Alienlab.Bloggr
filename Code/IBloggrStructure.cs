namespace Alienlab.Bloggr
{
  using Sitecore;
  using Sitecore.Data.Items;

  public interface IBloggrStructure
  {
    bool ValidatePostLocation([NotNull] Item bloggrPost);

    bool ValidatePostName([NotNull] Item bloggrPost);

    bool TryToFix([NotNull] Item bloggrPost);
  }
}
