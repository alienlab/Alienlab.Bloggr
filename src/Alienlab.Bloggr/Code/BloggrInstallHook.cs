namespace Alienlab.Bloggr
{
  using System.Web.Security;

  using Sitecore;
  using Sitecore.Diagnostics;
  using Sitecore.Events.Hooks;
  using Sitecore.Security.Accounts;

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
      // ensure "sitecore\Bloggr All Users" -> "sitecore\Authors"
      var allUsersRole = EnsureAllUsersRole();

      // ensure "sitecore\Bloggr Global Administrators" -> "sitecore\Bloggr All Users" -> "sitecore\Author"
      Role globalAdminsRole;
      var globalAdmins = "sitecore\\Bloggr Global Administrators";
      var globalAdminsExists = Role.Exists(globalAdmins);
      if (!globalAdminsExists)
      {
        Log.Info("Creating role: " + globalAdmins, this);
        Roles.CreateRole(globalAdmins);
      }

      globalAdminsRole = Role.FromName(globalAdmins);
      Assert.IsNotNull(globalAdminsRole, "Role does not exist: " + globalAdmins);

      if (!globalAdminsExists)
      {
        RolesInRolesManager.AddRoleToRole(globalAdminsRole, allUsersRole);
      }

      var blogNames = BloggrFactory.GetBlogs();

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
