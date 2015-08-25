namespace Alienlab.Bloggr.Tests
{
  using FluentAssertions;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.FakeDb;

  [TestClass]
  public class BloggrNoFoldersStructureTests
  {
    #region BloggrNoFoldersStructure.ValidatePostLocation(bloggrPost)

    [TestMethod]
    public void ValidatePostLocation_Home_Post()
    {
      var bloggrNoFoldersStructure = new BloggrNoFoldersStructure();
      var db = CreateHomePost();

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/01-Post");

        // Test Call
        bloggrNoFoldersStructure
          .ValidatePostLocation(post)
          .Should()
          .Be(true);
      }
    }

    [TestMethod]
    public void ValidatePostLocation_NoBloggrHome_Post()
    {
      var bloggrNoFoldersStructure = new BloggrNoFoldersStructure();
      var db = CreateHomePost(null, "SomeTemplate");

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/01-Post");

        // Test Call
        bloggrNoFoldersStructure
          .ValidatePostLocation(post)
          .Should()
          .Be(false);
      }
    }

    [TestMethod]
    public void ValidatePostLocation_BloggrHome_Folder_Post()
    {
      var bloggrNoFoldersStructure = new BloggrNoFoldersStructure();
      var db = CreateHomeFolderPost();

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/Folder/01-Post");

        // Test Call
        bloggrNoFoldersStructure
          .ValidatePostLocation(post)
          .Should()
          .Be(false);
      }
    } 

    #endregion

    #region BloggrNoFoldersStructure.ValidatePostName(bloggrPost)

    [TestMethod]
    public void ValidatePostName_Valid()
    {
      var title = "01 Post About (123) ducks";
      var name = "01-Post-About-123-ducks";
      var expected = true;

      var bloggrNoFoldersStructure = new BloggrNoFoldersStructure();
      var db = new Db
      {
        new DbItem(name)
        {
          new DbField(FieldIDs.DisplayName)
          {
            Value = title
          }
        }
      };

      using (db)
      {
        var post = db.GetItem("/sitecore/content/" + name);

        // Main Test
        bloggrNoFoldersStructure
          .ValidatePostName(post)
          .Should()
          .Be(expected);
      }
    }

    [TestMethod]
    public void ValidatePostName_Invalid_Space()
    {
      var title = "01 Post AbouT (123) ducks";
      var name = "01-post about-123-ducks";
      var expected = false;

      var bloggrNoFoldersStructure = new BloggrNoFoldersStructure();
      var db = new Db
      {
        new DbItem(name)
        {
          new DbField(FieldIDs.DisplayName)
          {
            Value = title
          }
        }
      };

      using (db)
      {
        var post = db.GetItem("/sitecore/content/" + name);

        // Main Test
        bloggrNoFoldersStructure
          .ValidatePostName(post)
          .Should()
          .Be(expected);
      }
    }

    [TestMethod]
    public void ValidatePostName_Invalid_LastDash()
    {
      var title = "123-";
      var name = "123-";
      var expected = false;

      var bloggrNoFoldersStructure = new BloggrNoFoldersStructure();
      var db = new Db
      {
        new DbItem(name)
        {
          new DbField(FieldIDs.DisplayName)
          {
            Value = title
          }
        }
      };

      using (db)
      {
        var post = db.GetItem("/sitecore/content/" + name);

        // Main Test
        bloggrNoFoldersStructure
          .ValidatePostName(post)
          .Should()
          .Be(expected);
      }
    }

    #endregion

    #region Helpers

    /// <summary>
    /// /sitecore/content/Home[Bloggr Home]/01-Post
    /// </summary>
    [NotNull]
    private static Db CreateHomePost([CanBeNull] string name = null, [CanBeNull] string template = null)
    {
      var bloggrHome = new ID();

      return new Db
      {
        new DbTemplate(template ?? "Bloggr Home", bloggrHome),
        new DbItem("Home", new ID(), bloggrHome)
        {
            new DbItem(name ?? "01-Post")
        }
      };
    }

    /// <summary>
    /// /sitecore/content/Home[Bloggr Home]/Folder/01-Post
    /// </summary>
    [NotNull]
    private static Db CreateHomeFolderPost([CanBeNull] string name = null, [CanBeNull] DbField field = null)
    {
      var bloggrHome = new ID();
      var dbItem = new DbItem(name ?? "01-Post");
      if (field != null)
      {
        dbItem.Add(field);
      }

      return new Db
      {
        new DbTemplate("Bloggr Home", bloggrHome),
        new DbItem("Home", new ID(), bloggrHome)
        {
          new DbItem("Folder")
          {
            dbItem
          }
        }
      };
    }

    #endregion
  }
}
