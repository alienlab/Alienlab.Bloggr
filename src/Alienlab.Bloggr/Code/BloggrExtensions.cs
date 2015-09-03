namespace Alienlab.Bloggr
{
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  internal static class BloggrExtensions
  {
    [CanBeNull]
    public static Item GetChild([NotNull] this Item parent, [NotNull] string name)
    {
      Assert.ArgumentNotNull(parent, "parent");
      Assert.ArgumentNotNullOrEmpty(name, "name");

      var children = parent.Children;
      Assert.IsNotNull(children, "children");

      return children[name];
    }

    [CanBeNull]
    public static Item Get([NotNull] this Database db, [NotNull] string name)
    {
      Assert.ArgumentNotNull(db, "db");
      Assert.ArgumentNotNullOrEmpty(name, "name");

      return db.GetItem(name);
    }

    [NotNull]
    public static Item GetSure([NotNull] this Database db, [NotNull] string itemPath)
    {
      Assert.ArgumentNotNull(db, "db");
      Assert.ArgumentNotNullOrEmpty(itemPath, "itemPath");

      var item = Get(db, itemPath);
      Assert.IsNotNull(item, "The {0} item does not exist or context user ({1}) does not have an access to it", itemPath, Context.User.GetDomainName());

      return item;
    }

    [CanBeNull]
    public static Item GetItem([NotNull] this Item db, [NotNull] string itemPath)
    {
      Assert.ArgumentNotNull(db, "db");
      Assert.ArgumentNotNullOrEmpty(itemPath, "itemPath");

      var database = db.Database;
      Assert.IsNotNull(database, "database");

      return database.GetItem(itemPath);
    }

    [NotNull]
    public static Item GetItemSure([NotNull] this Item db, [NotNull] string itemPath)
    {
      Assert.ArgumentNotNull(db, "db");
      Assert.ArgumentNotNull(itemPath, "itemPath");
      
      var item = GetItem(db, itemPath);
      Assert.IsNotNull(item, "The {0} item does not exist or context user ({1}) does not have an access to it", itemPath, Context.User.GetDomainName());

      return item;
    }

    [CanBeNull]
    public static string Get([NotNull] this Item item, [NotNull] string name)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(name, "name");

      return item[name];
    }

    [NotNull]
    public static string GetSure([NotNull] this Item item, [NotNull] string itemPath)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNullOrEmpty(itemPath, "itemPath");

      var value = Get(item, itemPath);
      Assert.IsNotNull(value, "The {0} item does not have the {1} filled in", item.Paths.FullPath, itemPath);

      return value;
    }

    [CanBeNull]
    public static Field GetField([NotNull] this Item item, [NotNull] string name)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNullOrEmpty(name, "name");

      var fields = item.Fields;
      Assert.IsNotNull(fields, "fields");

      return fields[name];
    }

    [NotNull]
    public static Field GetFieldSure([NotNull] this Item item, [NotNull] string fieldName)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNullOrEmpty(fieldName, "fieldName");

      var field = GetField(item, fieldName);
      Assert.IsNotNull(field, "The {0} item does not have the {1} filled in", item.Paths.FullPath, fieldName);

      return field;
    }
  }
}
