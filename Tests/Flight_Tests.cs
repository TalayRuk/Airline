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

    public void Dispose()
    {
      Flight.DeleteAll();
      City.DeleteAll();
    }
  }
}
