namespace Alienlab.Bloggr
{
  using Sitecore;
  using Sitecore.Data.Items;

  public interface IBloggrStructure
  {
    void FixPostName([NotNull] Item bloggrPost);

    void FixPostLocation([NotNull] Item bloggrPost);
  }
}
