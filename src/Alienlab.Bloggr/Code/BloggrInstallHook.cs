namespace Alienlab.Bloggr
{
  using Sitecore;
  using Sitecore.Events.Hooks;
  using Sitecore.Security.Accounts;

  [UsedImplicitly]
  public class BloggrInstallHook : IHook
  {
    public readonly string[] Roles =
      {
        "sitecore\\Bloggr Author",
        "sitecore\\Bloggr Reviewer",
      };

    public void Initialize()
    {
      foreach (var role in this.Roles)
      {
        if (Role.Exists(role))
        {
          continue;
        }
        System.Web.Security.Roles.CreateRole(role);
        RolesInRolesManager.AddRoleToRole(Role.FromName(role), Role.FromName("sitecore\\Author"));
      }
    }
  }
}
