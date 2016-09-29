using System;
using Xunit;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Airline
{
  public class CityTests : IDisposable
  {
    public CityTests()
    {
      string dataSource = "Data Source=(localdb)\\mssqllocaldb"; // Data Source identifies the server.
      string databaseName = "airline_test"; // Initial Catalog is the database name
      //Integrated Security sets the security of the database access to the Windows user that is currently logged in.
      DBConfiguration.ConnectionString = ""+dataSource+";Initial Catalog="+databaseName+";Integrated Security=SSPI;";
    }

    [Fact]
    public void T1_DoConstructorAndGettersWork()
    {
      //Arrange/Act
      City city = new City ("Seattle");
      //Assert
      Assert.Equal( "Seattle", city.GetName() );
      Assert.Equal( 0, city.GetId() );
    }

    [Fact]
    public void T2_Empty()
    {
      // Act
      int rows = City.GetAll().Count;

      // Assert
      Assert.Equal(0, rows);
    }

    [Fact]
    public void T3_OverrideEual()
    {
      //Arrange, Act
      City city1 = new City("Seattle");
      City city2 = new City("Seattle");

      //Assert
      Assert.Equal(city1, city2);
    }

    [Fact]
    public void T4_Save()
    {
      //Arrange
      City testCity = new City("Seattle");
      testCity.Save();

      //Act
      List<City> result = City.GetAll();
      List<City> testList = new List<City>{testCity};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void T5_SaveToId()
    {
      //Arrange
      City testCity = new City("Seattle");
      testCity.Save();

      //Act
      City savedCity = City.GetAll()[0];

      int result = savedCity.GetId();
      int testId = testCity.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void T6_Find()
    {
      //Arrange
      City testCity = new City("Seattle1");
      testCity.Save();

      //Act
      City result = City.Find(testCity.GetId());

      //Assert
      Assert.Equal(testCity, result);
    }

    [Fact]
    public void T7_AddFlight_AddFlightToCity()
    {
      //Arrange
      City testCity = new City("Seattle2");
      testCity.Save();

      Flight flightTest = new Flight("ontime","fl001");
      flightTest.Save();

      //Act
      testCity.AddFlight(flightTest);

      List<Flight> result = testCity.GetFlights();
      List<Flight> testList = new List<Flight> {flightTest};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void T8_GetFlights_ReturnAllCitiesFlights()
    {
      //Arrange
      City testCity = new City("Seattle3");
      testCity.Save();

      Flight flightTest = new Flight("ontime","fl003");
      flightTest.Save();

      Flight flightTest1 = new Flight("ontime","fl004");
      flightTest1.Save();

      //Act
      testCity.AddFlight(flightTest);
      List<Flight> result = testCity.GetFlights();
      List<Flight> testList = new List<Flight> {flightTest};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void T9_Delete_DeletesCitiesFromDB()
    {
      //Arrange
      Flight testFlight = new Flight("ontime","fl003");
      testFlight.Save();

      string testName = "Seattle1";
      City testCity = new City(testName);
      testCity.Save();

      //Act
      testCity.AddFlight(testFlight);
      testCity.Delete();

      List<City> resultFlightCities = testFlight.GetCities();
      List<City> testFlightCities = new List<City> {};

      //Assert
      Assert.Equal(testFlightCities, resultFlightCities);
    }

    public void Dispose()
    {
      City.DeleteAll();
      Flight.DeleteAll();

    }
  }
}
