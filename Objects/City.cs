using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Airline
{
  public class City
  {
    private int _id;
    private string _name;

    public City(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public static List<City> GetAll()
    {
      List<City> listCities = new List<City> {};

      SqlConnection conn = DB.Connection();
      conn.Open();

      string query = "SELECT * FROM cities;";
      SqlCommand cmd = new SqlCommand(query,conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      int id = 0;
      string name = null;
      while ( rdr.Read() )
      {
        id = rdr.GetInt32(0);
        name = rdr.GetString(1);
        City city = new City(name, id);
        listCities.Add(city);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return listCities;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string query = "INSERT INTO cities (name) OUTPUT INSERTED.id VALUES (@name);";
      SqlCommand cmd = new SqlCommand (query, conn);
      SqlParameter pam = new SqlParameter("@name", this.GetName());
      cmd.Parameters.Add(pam);

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
      string gotName = null;

      while( rdr.Read() )
      {
        gotId = rdr.GetInt32(0);
        gotName = rdr.GetString(1);
      }
      City city = new City (gotName, gotId);

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
    // public void Update(string name, int stylist)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   string query = "UPDATE cities SET name=@name OUTPUT INSERTED.description, INSERTED.stylist WHERE id = @id;";
    //   SqlCommand cmd = new SqlCommand(query,conn);
    //   SqlParameter
    //   cmd.Parameters.Add(
    //   {
    //     new SqlParameter( "@name", name ),
    //     new SqlParameter( "@stylist", stylist ),
    //     new SqlParameter( "@id", this.GetId() )
    //   });
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //   while ( rdr.Read() )
    //   {
    //     this._name = rdr.GetString(0);
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

    //AddFlight
    public void AddFlight(Flight newFlight)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities_flights (city_id, flight_id) VALUES (@CityId, @FlightId);", conn);

      SqlParameter cityIdParam = new SqlParameter("@CityId", this.GetId());
      cmd.Parameters.Add(cityIdParam);

      SqlParameter flightIdParam = new SqlParameter("@FlightId", newFlight.GetId());
      cmd.Parameters.Add(flightIdParam);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    //GetFlights
    public List<Flight> GetFlights()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT flights.* FROM cities JOIN cities_flights ON (cities.id = cities_flights.city_id) JOIN flights ON (cities_flights.flight_id = flights.id) WHERE cities.id = @CityId;", conn);

      SqlParameter cityIdParam = new SqlParameter("@CityId", this.GetId());
      cmd.Parameters.Add(cityIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Flight> flights = new List<Flight> {};

      while (rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        string flightStatus = rdr.GetString(1);
        string flightN = rdr.GetString(2);
        Flight newFlight = new Flight(flightStatus, flightN, flightId);
        flights.Add(newFlight);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return flights;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string query="DELETE FROM cities WHERE id = @CityId; DELETE FROM cities_flights WHERE city_id = @CityId;";
      SqlCommand cmd = new SqlCommand (query, conn);
      SqlParameter cityIdParameter = new SqlParameter("@CityId", this.GetId() );

      cmd.Parameters.Add(cityIdParameter);
      cmd.ExecuteNonQuery();

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
       bool nameEquality = ( newCity.GetName() == this.GetName() );

       Console.WriteLine(newCity.GetName() );
       Console.WriteLine(this.GetName() );
       bool idEquality = ( newCity.GetId() == this.GetId() );
       return ( idEquality && nameEquality);
     }
   }

   public override int GetHashCode()
   {
     return this.GetName().GetHashCode();
   }
  }
}
