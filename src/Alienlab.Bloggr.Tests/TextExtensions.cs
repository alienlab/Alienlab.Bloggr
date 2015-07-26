namespace Alienlab.Bloggr.Tests
{
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb;

  internal static class TextExtensions
  {
    [NotNull]
    internal static Item Get([NotNull] this Db db, [NotNull] string path)
    {
      Assert.ArgumentNotNull(db, "db");
      Assert.ArgumentNotNull(path, "path");

      var item = db.GetItem(path);
      Assert.IsNotNull(item, "item");

      return item;
    }
  }
}
