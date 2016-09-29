using System;
using Xunit;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Airline
{
  public class FlightTests : IDisposable
  {
    public FlightTests()
    {
      string dataSource = "Data Source=(localdb)\\mssqllocaldb"; // Data Source identifies the server.
      string databaseName = "airline_test"; // Initial Catalog is the database name
      //Integrated Security sets the security of the database access to the Windows user that is currently logged in.
      DBConfiguration.ConnectionString = ""+dataSource+";Initial Catalog="+databaseName+";Integrated Security=SSPI;";
    }

    [Fact]
    public void T1_Empty()
    {
      //Arrange, Act
      int rows = Flight.GetAll().Count;

      //Assert
      Assert.Equal(0, rows);
    }

    [Fact]
    public void T2_OverrideEqual()
    {
      //Arrange, Act
      Flight flight1 = new Flight("ontime","fl001");
      Flight flight2 = new Flight("ontime","fl001");

      //Assert
      Assert.Equal(flight1, flight2);
    }

    [Fact]
    public void T3_Save()
    {
      //Arrange
      Flight flightTest = new Flight("ontime","fl001");
      flightTest.Save();

      // Act
      List<Flight> result = Flight.GetAll();
      List<Flight> testList = new List<Flight>{flightTest};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void T4_SaveToId()
    {
      //Arrange
      Flight flightTest = new Flight("ontime","fl001");
      flightTest.Save();

      //Act
      Flight savedFlight = Flight.GetAll()[0];

      int result = savedFlight.GetId();
      int testId = flightTest.GetId();

      // Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void T5_Find()
    {
      //Arrange
      Flight testFlight = new Flight("ontime","fl001");
      testFlight.Save();

      // Act
      Flight result = Flight.Find(testFlight.GetId());

      //Assert
      Assert.Equal(testFlight, result);
    }

    [Fact]
    public void T6_Update()
    {
      // Arrange
      string status = "ontime";
      string flightN = "fl001";
      Flight testFlight = new Flight(status, flightN);
      testFlight.Save();


      //Act
      Console.WriteLine(testFlight.GetStatus());
      testFlight.Update("delay", "fl001", testFlight.GetId());
      testFlight.Save();
      Console.WriteLine(testFlight.GetStatus());
      Flight result = Flight.GetAll()[0];
      Console.WriteLine(result);

      //Assert
      Assert.Equal(testFlight.GetStatus(), "delay");
    }

    [Fact]
    public void Test7_DeletesFlightFromDB()
    {
      //Arrange
      string name = "BKK";
      City testCity1 = new City(name);
      testCity1.Save();

      string name2 = "Delayed";
      string flight2 = "fl001";
      Flight testFlight2 = new Flight(name2, flight2);
      testFlight2.Save();

      //Act
      testFlight2.AddCity(testCity1);
      testFlight2.Delete();

      List<Flight> result = testCity1.GetFlights();
      List<Flight> testList = new List<Flight> {};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test8_AddCities()
    {
      Flight testFlight = new Flight("ontime", "f1001");
      testFlight.Save();

      City testCity = new City("Seattle");
      testCity.Save();

      City testCity2 = new City("Toronto");
      testCity2.Save();

      testFlight.AddCity(testCity);
      testFlight.AddCity(testCity2);

      List<City> result = testFlight.GetCities();
      List<City> testList = new List<City>{testCity, testCity2};

      Assert.Equal(testList, result);

    }

    [Fact]
    public void Test9_GetCities_ReturnsAllFlightCities()
    {
      //Arrange
      Flight testFlight = new Flight("ontime", "f100");
      testFlight.Save();

      City testCity1 = new City("Seattle1");
      testCity1.Save();

      City testCity2 = new City("Toronto1");
      testCity2.Save();

      //Act
      testFlight.AddCity(testCity1);
      List<City> savedCities = testFlight.GetCities();
      List<City> testList = new List<City> {testCity1};

      //Assert
      Assert.Equal(testList, savedCities);
    }



    public void Dispose()
    {
      Flight.DeleteAll();
      City.DeleteAll();
    }
  }
}
