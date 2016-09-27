using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
// Remember to:
// - Update ProjectCore namespace

namespace Airline
{
  public class Flight
  {
    private int _id;
    private string _status;
    private string _flightN;

    pubic Flight(string status, string flightN, int id = 0)
    {
      _status = status;
      _flightN = flightN;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetStatus()
    {
      return _status;
    }
    public string GetFlightN()
    {
      return _flightN;
    }
    public void SetStatus(string newStatus)
    {
      _status = newStatus;
    }
    public void SetFlightN(string newFlightN)
    {
      _flightN = newFlightN;
    }
    public static List<Flight> GetAll()
    {
      List<Flight> listFlights = new List<Flight> {};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      int id = 0;
      string name = null;
      while ( rdr.Read() )
      {
        id = rdr.GetInt32(0);
        status = rdr.GetString(1);
        flightN = rdr.GetString(2);
        Flight flight = new Flight(status, flightN, id);
        listFlights.Add(flight);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return listFlights;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string query = "INSERT INTO flights (status, flight_number) OUTPUT INSERTED.id VALUES (@status, @flightN);";
      SqlCommand cmd = new SqlCommand (query, conn);
      SqlParameter statusParameter = new SqlParameter("@status", this.GetStatus());
      SqlParameter flightNParameter = new SqlParameter("@flightN", this.GetFlightN());
      cmd.Parameters.Add(statusParameter);


      SqlDataReader rdr = cmd.ExecuteReader();

      while ( rdr.Read() )
      {
        _id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static City Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string query="SELECT * FROM cities WHERE id = @id;";
      SqlCommand cmd = new SqlCommand (query, conn);
      SqlParameter idParameter = new SqlParameter("@id", id );
      cmd.Parameters.Add(idParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int gotId = 0;
      string gotStatus = null;

      while( rdr.Read() )
      {
        gotId = rdr.GetInt32(0);
        gotStatus = rdr.GetString(1);
      }
      City city = new City (gotStatus, gotId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return city;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string nonQuery = "DELETE FROM cities;";
      SqlCommand cmd = new SqlCommand(nonQuery,conn);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    // public void Update(string status, int stylist)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   string query = "UPDATE cities SET status=@status OUTPUT INSERTED.description, INSERTED.stylist WHERE id = @id;";
    //   SqlCommand cmd = new SqlCommand(query,conn);
    //   SqlParameter
    //   cmd.Parameters.Add(
    //   {
    //     new SqlParameter( "@status", status ),
    //     new SqlParameter( "@stylist", stylist ),
    //     new SqlParameter( "@id", this.GetId() )
    //   });
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //   while ( rdr.Read() )
    //   {
    //     this._status = rdr.GetString(0);
    //     this._stylist = rdr.GetInt32(1);
    //   }
    //
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string query="DELETE FROM cities WHERE id = @id;";
      SqlCommand cmd = new SqlCommand (query, conn);
      SqlParameter idParameter = new SqlParameter("id", this._id );
      cmd.Parameters.Add(idParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      if (conn != null)
      {
        conn.Close();
      }
    }
    //Overrides
   public override bool Equals(System.Object otherCity)
   {
     if (!(otherCity is City))
     {
       return false;
     }
     else
     {
       City newCity = (City) otherCity;
       bool statusEquality = ( newCity.GetStatus() == this.GetStatus() );

       Console.WriteLine(newCity.GetStatus() );
       Console.WriteLine(this.GetStatus() );
       bool idEquality = ( newCity.GetId() == this.GetId() );
       return ( idEquality && statusEquality);
     }
   }
   public override int GetHashCode()
   {
     return this.GetStatus().GetHashCode();
   }
  }
}

    }

  }

}
