namespace Alienlab.Bloggr
{
  using Sitecore.Events.Hooks;

  public class BloggrInstallHook : IHook
  {
    public void Initialize()
    {/*
      var trees = new[]
      {
        // Content
        new Item("/sitecore/content/Bloggr", TemplateIDs.Node)
        {
            new Field(FieldIDs.Branches, "$BloggrRoot")
        },

        // Layout
        new Item("/sitecore/layout/Layouts/Bloggr/Layout", TemplateIDs.Layout)
        {
          new Field("Path", "/Views/Bloggr/Layout.cshtml")
        },

        new Item("/sitecore/layout/Renderings/Bloggr", "{7EE0975B-0698-493E-B3A2-0B2EF33D0522}")
        {
          new Item("Feed", Sitecore.Mvc.Names.TemplateIds.ViewRendering)
          {
            new Field("Path", "/Views/Bloggr/Components/Feed.cshtml")
          }, 
          new Item("Footer", Sitecore.Mvc.Names.TemplateIds.ViewRendering)
          {
            new Field("Path", "/Views/Bloggr/Components/Footer.cshtml")
          }, 
          new Item("Header", Sitecore.Mvc.Names.TemplateIds.ViewRendering)
          {
            new Field("Path", "/Views/Bloggr/Components/Header.cshtml")
          }, 
          new Item("Post", Sitecore.Mvc.Names.TemplateIds.ViewRendering)
          {
            new Field("Path", "/Views/Bloggr/Components/Post.cshtml")
          }, 
        },

        // System
        new Item("/sitecore/system/Modules/Bloggr/Structures/Created-Date Folders", "$BloggrCreatedDateFolderStructure")
        {
          new Field("BloggrFolderTemplate", "$BloggrPostFolder"),
          new Field(BloggrContext.BloggrStructureType, typeof(BloggrCreatedDateFoldersStructure).AssemblyQualifiedName),
          new Field("BloggrPostAutoName", true)
        },
        new Item("/sitecore/system/Modules/Bloggr/Structures/No Folders", "$BloggrStructure")
        {
          new Field(BloggrContext.BloggrStructureType, typeof(BloggrNoFoldersStructure).AssemblyQualifiedName)
        },
        new Item("/sitecore/system/Workflows/BloggrWorkflow", TemplateIDs.Workflow)
        {
          new Field(FieldIDs.WorkflowState, "$BloggrDraft")
        },
        new Item("/sitecore/system/Workflows/BloggrWorkflow/BloggrDraft", TemplateIDs.WorkflowState)
        {
          new Field(FieldIDs.Icon, "/sitecore/shell/themes/standard/applications/16x16/document_edit.png") 
        },
        new Item("/sitecore/system/Workflows/BloggrWorkflow/BloggrDraft/For Review", TemplateIDs.WorkflowCommand)
        {
          new Field(FieldIDs.NextState, "$BloggrAwaitingApproval")
        },
        new Item("/sitecore/system/Workflows/BloggrWorkflow/BloggrAwaitingApproval", TemplateIDs.WorkflowState)
        {
          new Field(FieldIDs.Icon, "applications/16x16/document_ok.png") 
        },
        new Item("/sitecore/system/Workflows/BloggrWorkflow/BloggrApproved", TemplateIDs.WorkflowState)
        {
          new Field(FieldIDs.Icon, "Network/16x16/earth.png") 
        }
      };*/
    }
    /*
    private class Template
    {
      private readonly ICollection<object> subobjects = new List<object>();

      public void Add(Field field)
      {
        this.subobjects.Add(field);
      }
    }*/
  }
}
