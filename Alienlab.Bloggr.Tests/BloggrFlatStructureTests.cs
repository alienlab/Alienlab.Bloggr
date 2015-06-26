namespace Alienlab.Bloggr.Tests
{
  using System;
  using FluentAssertions;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using NSubstitute;
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb;

  [TestClass]
  public class BloggrFlatStructureTests
  {
    #region BloggrFlatStructure.ValidatePostLocation(bloggrPost)

    [TestMethod]
    public void ValidatePostLocation_Home_Post()
    {
      var bloggrFlatStructure = new BloggrFlatStructure();
      var db = CreateHomePost();

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/01-Post");

        // Test Call
        bloggrFlatStructure
          .ValidatePostLocation(post)
          .Should()
          .Be(true);
      }
    }

    [TestMethod]
    public void ValidatePostLocation_NoBloggrHome_Post()
    {
      var bloggrFlatStructure = new BloggrFlatStructure();
      var db = CreateHomePost(null, "SomeTemplate");

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/01-Post");

        // Test Call
        bloggrFlatStructure
          .ValidatePostLocation(post)
          .Should()
          .Be(false);
      }
    }

    [TestMethod]
    public void ValidatePostLocation_BloggrHome_Folder_Post()
    {
      var bloggrFlatStructure = new BloggrFlatStructure();
      var db = CreateHomeFolderPost();

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/Folder/01-Post");

        // Test Call
        bloggrFlatStructure
          .ValidatePostLocation(post)
          .Should()
          .Be(false);
      }
    } 

    #endregion

    #region BloggrFlatStructure.ValidatePostName(bloggrPost)

    [TestMethod]
    public void ValidatePostName_Valid()
    {
      var name = "01-Post-about-123-ducks";
      var expected = true;

      var bloggrFlatStructure = new BloggrFlatStructure();
      var db = new Db
      {
        new DbItem(name)
      };

      using (db)
      {
        var post = db.GetItem("/sitecore/content/" + name);

        // Main Test
        bloggrFlatStructure
          .ValidatePostName(post)
          .Should()
          .Be(expected);
      }
    }

    [TestMethod]
    public void ValidatePostName_Invalid_Space()
    {
      var name = "01-Post about-123-ducks";
      var expected = false;

      var bloggrFlatStructure = new BloggrFlatStructure();
      var db = new Db
      {
        new DbItem(name)
      };

      using (db)
      {
        var post = db.GetItem("/sitecore/content/" + name);

        // Main Test
        bloggrFlatStructure
          .ValidatePostName(post)
          .Should()
          .Be(expected);
      }
    }

    [TestMethod]
    public void ValidatePostName_Invalid_NoPrefix()
    {
      var name = "Post-about-123-ducks";
      var expected = false;

      var bloggrFlatStructure = new BloggrFlatStructure();
      var db = new Db
      {
        new DbItem(name)
      };

      using (db)
      {
        var post = db.GetItem("/sitecore/content/" + name);

        // Main Test
        bloggrFlatStructure
          .ValidatePostName(post)
          .Should()
          .Be(expected);
      }
    }

    [TestMethod]
    public void ValidatePostName_Invalid_NoDash()
    {
      var name = "123Post-about-123-ducks";
      var expected = false;

      var bloggrFlatStructure = new BloggrFlatStructure();
      var db = new Db
      {
        new DbItem(name)
      };

      using (db)
      {
        var post = db.GetItem("/sitecore/content/" + name);

        // Main Test
        bloggrFlatStructure
          .ValidatePostName(post)
          .Should()
          .Be(expected);
      }
    }

    [TestMethod]
    public void ValidatePostName_Invalid_NoDash_NoText()
    {
      var name = "123";
      var expected = false;

      var bloggrFlatStructure = new BloggrFlatStructure();
      var db = new Db
      {
        new DbItem(name)
      };

      using (db)
      {
        var post = db.GetItem("/sitecore/content/" + name);

        // Main Test
        bloggrFlatStructure
          .ValidatePostName(post)
          .Should()
          .Be(expected);
      }
    }

    [TestMethod]
    public void ValidatePostName_Invalid_NoText()
    {
      var name = "123-";
      var expected = false;

      var bloggrFlatStructure = new BloggrFlatStructure();
      var db = new Db
      {
        new DbItem(name)
      };

      using (db)
      {
        var post = db.GetItem("/sitecore/content/" + name);

        // Main Test
        bloggrFlatStructure
          .ValidatePostName(post)
          .Should()
          .Be(expected);
      }
    }

    #endregion

    #region BloggrFlatStructure.TryToFix(bloggrPost)

    [TestMethod]
    public void TryToFix_MockValidLocation_MockValidName()
    {
      var bloggrFlatStructure = Substitute.ForPartsOf<BloggrFlatStructure>();
      var db = CreateHomePost();

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/01-Post");

        // Mock
        bloggrFlatStructure
          .ValidatePostLocation(post)
          .Returns(true);

        bloggrFlatStructure
          .ValidatePostName(post)
          .Returns(true);

        // Test Call
        bloggrFlatStructure
          .TryToFix(post)
          .Should()
          .Be(true);

        // Additional Tests
        bloggrFlatStructure
          .Received(1)
          .ValidatePostLocation(post);

        bloggrFlatStructure
          .Received(1)
          .ValidatePostName(post);
      }
    }

    [TestMethod]
    public void TryToFix_NoBloggrHome_MockInvalidLocation_MockValidName()
    {
      var bloggrFlatStructure = Substitute.ForPartsOf<BloggrFlatStructure>();
      var db = new Db
      {
        new DbItem("Home")
        {
          new DbItem("01-Post")
        }
      };

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/01-Post");

        // Mock
        bloggrFlatStructure
          .ValidatePostLocation(post)
          .Returns(false);

        bloggrFlatStructure
          .ValidatePostName(post)
          .Returns(true);

        // Test Call
        bloggrFlatStructure
          .TryToFix(post)
          .Should()
          .Be(false);

        // Additional Tests
        bloggrFlatStructure
          .Received(1)
          .ValidatePostLocation(post);

        bloggrFlatStructure
          .Received(1)
          .ValidatePostName(post);
      }
    }

    [TestMethod]
    public void TryToFix_NoBloggrHome_MockInvalidLocation_MockInvalidName()
    {
      var bloggrFlatStructure = Substitute.ForPartsOf<BloggrFlatStructure>();
      var db = new Db
      {
        new DbItem("Home")
        {
          new DbItem("Post")
        }
      };

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/Post");

        // Mock
        bloggrFlatStructure
          .ValidatePostLocation(post)
          .Returns(false);

        bloggrFlatStructure
          .ValidatePostName(post)
          .Returns(false);

        // Test Call
        bloggrFlatStructure
          .TryToFix(post)
          .Should()
          .Be(false);

        // Additional Tests
        bloggrFlatStructure
          .Received(1)
          .ValidatePostLocation(post);

        bloggrFlatStructure
          .Received(1)
          .ValidatePostName(post);
      }
    }

    [TestMethod]
    public void TryToFix_InvalidLocation_ValidName()
    {
      var bloggrFlatStructure = Substitute.ForPartsOf<BloggrFlatStructure>();
      var db = CreateHomeFolderPost();

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/Folder/01-Post");
        
        // Test Call
        bloggrFlatStructure
          .TryToFix(post)
          .Should()
          .Be(true);

        // Additional Tests
        post.Paths.FullPath
          .Should()
          .BeEquivalentTo("/SitecoRE/content/Home/01-Post");

        bloggrFlatStructure
          .Received(2)
          .ValidatePostLocation(post);

        bloggrFlatStructure
          .Received(1)
          .ValidatePostName(post);
      }
    }

    [TestMethod]
    public void TryToFix_InvalidLocation_InvalidName()
    {
      var bloggrFlatStructure = Substitute.ForPartsOf<BloggrFlatStructure>();
      var db = CreateHomeFolderPost("Post About 13-year old man named Alan");

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/Folder/Post About 13-year old man named Alan");

        // Test Call
        bloggrFlatStructure
          .TryToFix(post)
          .Should()
          .Be(true);

        // Additional Tests
        post.Paths.FullPath
          .Should()
          .BeEquivalentTo("/sitecore/content/Home/0-Post-about-13-year-old-man-named-Alan");

        bloggrFlatStructure
          .Received(2)
          .ValidatePostLocation(post);

        bloggrFlatStructure
          .Received(2)
          .ValidatePostName(post);
      }
    }

    [TestMethod]
    public void TryToFix_InvalidLocation()
    {
      var bloggrFlatStructure = Substitute.ForPartsOf<BloggrFlatStructure>();
      var db = CreateHomeFolderPost();

      using (db)
      {
        var post = db.Get("/sitecore/content/Home/Folder/01-Post");

        // Mock
        bloggrFlatStructure
          .ValidatePostName(post)
          .Returns(true);

        // Main Test
        bloggrFlatStructure
          .TryToFix(post)
          .Should()
          .Be(true);

        // Additional Tests
        bloggrFlatStructure
          .Received(2)
          .ValidatePostLocation(post);

        bloggrFlatStructure
          .Received(1)
          .ValidatePostName(post);
      }
    } 

    #endregion

    #region BloggrFlatStructure.FixName(bloggrPost)

    [TestMethod]
    public void FixName_OnlyName()
    {
      var bloggrFlatStructure = new BloggrFlatStructure();
      var db = new Db
      {
        new DbItem("hello My dear 123")
      };

      using (db)
      {
        var helloItem = db.Get("/sitecore/content/Hello my dear 123");

        new Action(() => bloggrFlatStructure.FixName(helloItem))
          .ShouldThrow<InvalidOperationException>();

      }
    } 

    [TestMethod]
    public void FixName_DisplayName()
    {
      var bloggrFlatStructure = new BloggrFlatStructure();
      var db = new Db
      {
        new DbItem("hello")
        {
          new DbField("__Display name")
          {
            Value = @"- hello, Приветствую, вітаю... Something like this; (incredible!)-111- //#|@@!@#%^&*"
          }
        }
      };

      using (db)
      {
        var helloItem = db.Get("/sitecore/content/hello");

        bloggrFlatStructure
          .FixName(helloItem)
          .Should()
          .Be(@"Hello-privetstvuyu-vitayu-something-like-this-(incredible)-111");
      }
    } 

    #endregion

    #region Helpers

    /// <summary>
    /// /sitecore/content/Home[BloggrHome]/01-Post
    /// </summary>
    [NotNull]
    private static Db CreateHomePost([CanBeNull] string name = null, [CanBeNull] string template = null)
    {
      var bloggrHome = new ID();

      return new Db
      {
        new DbTemplate(template ?? "BloggrHome", bloggrHome),
        new DbItem("Home", new ID(), bloggrHome)
        {
            new DbItem(name ?? "01-Post")
        }
      };
    }

    /// <summary>
    /// /sitecore/content/Home[BloggrHome]/Folder/01-Post
    /// </summary>
    [NotNull]
    private static Db CreateHomeFolderPost([CanBeNull] string name = null)
    {
      var bloggrHome = new ID();
      return new Db
      {
        new DbTemplate("BloggrHome", bloggrHome),
        new DbItem("Home", new ID(), bloggrHome)
        {
          new DbItem("Folder")
          {
            new DbItem(name ?? "01-Post")
          }
        }
      };
    }

    #endregion
  }
}
