using System.Data;
using System.Data.SqlClient;
// Remember to:
// - Update ProjectCore namespace

namespace Airline
{
  public class DB
  {
    public static SqlConnection Connection()
    {
      SqlConnection conn = new SqlConnection(DBConfiguration.ConnectionString);
      return conn;
    }
  }
}
