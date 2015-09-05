namespace Alienlab.Bloggr
{
  using System.Web.Security;

  using Sitecore;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Templates;
  using Sitecore.Diagnostics;
  using Sitecore.Events.Hooks;
  using Sitecore.Security.Accounts;
  using Sitecore.SecurityModel;

  [UsedImplicitly]
  public class BloggrInstallHook : IHook
  {
    public static readonly string[] RoleKinds =
      {
        "Administrators",
        "Authors",
        "Reviewers",
      };

    [NotNull]
    public static readonly string DefaultBlogSecurity = Settings.GetSetting("Bloggr.DefaultBlogSecurity", @"ar|sitecore\Bloggr Administrators - $name|pd|+item:rename|+item:admin|+item:delete|+item:create|+item:write|ar|sitecore\Bloggr Reviewers - $name|pd|+item:write|ar|sitecore\Bloggr Authors - $name|pd|+item:write|+item:create|");

    [NotNull]
    public static readonly string BlogsRoot = Settings.GetSetting("Bloggr.BlogsRoot", "/sitecore/content/blogs");

    [NotNull]
    public static Role EnsureAllRole([NotNull] string roleKind)
    {
      // ensure "sitecore\Bloggr All (role)" -> "sitecore\Bloggr All Users" -> "sitecore\Authors"
      Assert.ArgumentNotNull(roleKind, "roleKind");

      var all = "sitecore\\Bloggr All " + roleKind;
      var allExists = Role.Exists(all);
      if (!allExists)
      {
        Log.Info("Creating role: " + all, typeof(BloggrInstallHook));
        Roles.CreateRole(all);
      }

      var allRole = Role.FromName(all);
      Assert.IsNotNull(allRole, "Role does not exist: " + all);

      if (!allExists)
      {
        RolesInRolesManager.AddRoleToRole(allRole, EnsureAllUsersRole());
      }

      return allRole;
    }

    public static void EnsureBlogRoles([NotNull] string blogName)
    {
      Assert.ArgumentNotNull(blogName, "blogName");

      foreach (var roleKind in RoleKinds)
      {
        var globalRole = EnsureAllRole(roleKind);

        // ensure "sitecore\Bloggr (role) - (blog)" -> "sitecore\Bloggr All (role)" -> "sitecore\Bloggr All Users" -> "sitecore\Authors"
        var blogRoleName = "sitecore\\Bloggr " + roleKind + " - " + blogName;
        var blogRoleExists = Role.Exists(blogRoleName);
        if (!blogRoleExists)
        {
          Log.Info("Creating role: " + blogRoleName, typeof(BloggrInstallHook));
          Roles.CreateRole(blogRoleName);
          RolesInRolesManager.AddRoleToRole(Role.FromName(blogRoleName), globalRole);
        }
      }
    }

    public void Initialize()
    {
      EnsureRoles();

      EnsureItems();
    }

    private static void EnsureItems()
    {
      using (new SecurityDisabler())
      {
        var database = Factory.GetDatabase("master");
        var blogsItem = database.GetItem(BlogsRoot);
        if (blogsItem == null)
        {
          var content = database.GetItem(ItemIDs.ContentRoot);
          Assert.IsNotNull(content, "/sitecore/content item is missing");

          blogsItem = content.Add("Blogs", new TemplateID(TemplateIDs.Node));
          Assert.IsNotNull(blogsItem, "/sitecore/content/blogs item wasn't created");
        }

        var blogItems = blogsItem.Children;
        Assert.IsNotNull(blogItems, "blogItems");

        foreach (var blogName in BloggrFactory.GetBlogNames())
        {
          var blogItem = blogItems[blogName];
          if (blogItem == null)
          {
            blogItem = blogsItem.Add(blogName, new BranchId(ID.Parse("{15FA2775-5BFA-43C5-91E7-A3581448D0B2}")));
            Assert.IsNotNull(blogItem, "/sitecore/content/blogs/$name item wasn't created");
          }

          if (!string.IsNullOrEmpty(blogItem[FieldIDs.Security]))
          {
            continue;
          }

          using (new EditContext(blogItem))
          {
            blogItem[FieldIDs.Security] = DefaultBlogSecurity.Replace("$name", blogName);
          }
        }
      }
    }

    private static void EnsureRoles()
    {
      // ensure "sitecore\Bloggr All Users" -> "sitecore\Authors"
      var allUsersRole = EnsureAllUsersRole();

      // ensure "sitecore\Bloggr Global Administrators" -> "sitecore\Bloggr All Users" -> "sitecore\Author"
      Role globalAdminsRole;
      var globalAdmins = "sitecore\\Bloggr Global Administrators";
      var globalAdminsExists = Role.Exists(globalAdmins);
      if (!globalAdminsExists)
      {
        Log.Info("Creating role: " + globalAdmins, typeof(BloggrInstallHook));
        Roles.CreateRole(globalAdmins);
      }

      globalAdminsRole = Role.FromName(globalAdmins);
      Assert.IsNotNull(globalAdminsRole, "Role does not exist: " + globalAdmins);

      if (!globalAdminsExists)
      {
        RolesInRolesManager.AddRoleToRole(globalAdminsRole, allUsersRole);
      }

      var blogNames = BloggrFactory.GetBlogNames();

      foreach (var roleKind in RoleKinds)
      {
        EnsureAllRole(roleKind);
      }

      foreach (var blogName in blogNames)
      {
        EnsureBlogRoles(blogName);
      }
    }

    [NotNull]
    private static Role EnsureAllUsersRole()
    {
      Role allUsersRole;
      var allUsers = "sitecore\\Bloggr All Users";
      var allUsersExists = Role.Exists(allUsers);
      if (!allUsersExists)
      {
        Log.Info("Creating role: " + allUsers, typeof(BloggrInstallHook));
        Roles.CreateRole(allUsers);
      }

      allUsersRole = Role.FromName(allUsers);
      Assert.IsNotNull(allUsersRole, "Role does not exist: " + allUsers);

      if (!allUsersExists)
      {
        RolesInRolesManager.AddRoleToRole(allUsersRole, Role.FromName("sitecore\\Author"));
      }

      return allUsersRole;
    }
  }
}
