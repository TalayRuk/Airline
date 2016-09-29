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

    public Flight(string status, string flightN, int id = 0)
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
    //Overrides
    public override bool Equals(System.Object otherFlight)
    {
      if (!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
        Flight newFlight = (Flight) otherFlight;
        bool statusEquality = ( newFlight.GetStatus() == this.GetStatus() );
        bool flightNEquality = (newFlight.GetFlightN() == this.GetFlightN());
        bool idEquality = ( newFlight.GetId() == this.GetId() );
        return (idEquality && statusEquality && flightNEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetStatus().GetHashCode();
    }
    public static List<Flight> GetAll()
    {
      List<Flight> listFlights = new List<Flight> {};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      int id = 0;
      string status = null;
      string flightN = null;

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
      // SqlParameter statusParameter = new SqlParameter("@status", this.GetStatus());
      // SqlParameter flightNParameter = new SqlParameter("@flightN", this.GetFlightN());
      // cmd.Parameters.Add(statusParameter);
      // cmd.Parameters.Add(flightNParameter);
      cmd.Parameters.Add(new SqlParameter("@status", this.GetStatus()));
      cmd.Parameters.Add(new SqlParameter("@flightN", this.GetFlightN()));



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
    public static Flight Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string query="SELECT * FROM flights WHERE id = @FlightId;";
      SqlCommand cmd = new SqlCommand (query, conn);
      SqlParameter idParam = new SqlParameter("@FlightId", id.ToString());
      cmd.Parameters.Add(idParam);
      SqlDataReader rdr = cmd.ExecuteReader();

      int gotId = 0;
      string gotStatus = null;
      string gotFlightN = null;

      while( rdr.Read() )
      {
        gotId = rdr.GetInt32(0);
        gotStatus = rdr.GetString(1);
        gotFlightN = rdr.GetString(2);
      }
      Flight flight = new Flight (gotStatus, gotFlightN, gotId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return flight;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string nonQuery = "DELETE FROM flights;";
      SqlCommand cmd = new SqlCommand(nonQuery,conn);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public void Update(string status, string flightN, int id=0)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string query = "UPDATE flights SET status = @status OUTPUT INSERTED.status WHERE id = @flightId;";
      SqlCommand cmd = new SqlCommand(query, conn);
      cmd.Parameters.Add(new SqlParameter( "@status", status ));
      cmd.Parameters.Add(new SqlParameter( "@flightId", id.ToString() ));


      SqlDataReader rdr = cmd.ExecuteReader();


      while ( rdr.Read() )
      {
        _status = rdr.GetString(0);
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
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      string query="DELETE FROM flights WHERE id = @flightId; DELETE FROM cities_flights WHERE flight_id = @flightId";
      SqlCommand cmd = new SqlCommand (query, conn);
      SqlParameter flightIdParam = new SqlParameter("@flightId", this.GetId());

      cmd.Parameters.Add(flightIdParam);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    //AddCity
    public void AddCity(City newCity)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities_flights (city_id,flight_id) VALUES (@CityId, @FlightId);", conn);
      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = newCity.GetId();
      cmd.Parameters.Add(cityIdParameter);

      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = this.GetId();
      cmd.Parameters.Add(flightIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    //GetTasks
    public  List<City> GetCities()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT cities.* FROM flights JOIN cities_flights ON (flights.id = cities_flights.flight_id) JOIN cities ON (cities_flights.city_id = cities.id) WHERE flights.id = @FlightId;", conn);

      SqlParameter flightIdParam = new SqlParameter("@FlightId", this.GetId());
      cmd.Parameters.Add(flightIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<City> cities = new List<City> {};
      while (rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        cities.Add(newCity);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return cities;
    }
  }
}
