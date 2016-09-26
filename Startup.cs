using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNet.Builder;
using Nancy;
using Nancy.Owin;
using Nancy.ViewEngines.Razor;

//Remember to:
// - Update database connection information.

namespace Airline
{
  public class Startup
  {
    public void Configure(IApplicationBuilder app)
    {
      app.UseOwin(x => x.UseNancy());
    }
  }
  public class CustomRootPathProvider : IRootPathProvider
  {
    public string GetRootPath()
    {
      return Directory.GetCurrentDirectory();
    }
  }
  public class RazorConfig : IRazorConfiguration
  {
    public IEnumerable<string> GetAssemblyNames()
    {
      return null;
    }

    public IEnumerable<string> GetDefaultNamespaces()
    {
      return null;
    }

    public bool AutoIncludeModelNamespace
    {
      get { return false; }
    }
  }
  public static class DBConfiguration
  {
    // Epicodus DataBase Information (Microsoft sequl server)
    // Server type: Database Engine
    // Server name: (localdb)\MSSQLLocalDB
    // Authentication: Windows Authentication

    public static string dataSource = "Data Source=(localdb)\\mssqllocaldb"; // Data Source identifies the server.
    public static string databaseName = "database"; // Initial Catalog is the database name
    //Integrated Security sets the security of the database access to the Windows user that is currently logged in.
    public static string ConnectionString = ""+dataSource+";Initial Catalog="+databaseName+";Integrated Security=SSPI;";
  }
}
