using Nancy;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nancy.ViewEngines.Razor;

namespace Airline
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/cities"] = _ => {
        List<City> AllCities = City.GetAll();
        return View["cities.cshtml", AllCities];
      };
      Get["/flights"] = _ => {
        List<Flight> AllFlights = Flight.GetAll();
        return View["flights.cshtml", AllFlights];
      };
      //Create a new City
      Get["/cities/new"] = _ => {
        return View["cities_form.cshtml"];
      };
      Post["/cities/new"] = _ => {
        City newCity = new City(Request.Form["city-name"]);
        newCity.Save();
        return View["success.cshtml"];
      };

      //create a new flight1
      Get["/flights/new"] = _ => {
        return View["flights_form.cshtml"];
      };

      Post["/flights/new"] =_=> {
        Flight newFlight = new Flight(Request.Form["flight-status"], Request.Form["flight-num"]);
        newFlight.Save();
        return View["success.cshtml"];
      };

      Get["cities/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        City SelectedCity = City.Find(parameters.id);
        List<Flight> CityFlights = SelectedCity.GetFlights();
        List<Flight> AllFlights = Flight.GetAll();
        model.Add("city", SelectedCity);
        model.Add("cityFlights", CityFlights);
        model.Add("allFlights", AllFlights);
        return View["city.cshtml", model];
      };

      Get["flights/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Flight SelectedFlight = Flight.Find(parameters.id);
        List<City> FlightCities = SelectedFlight.GetCities();
        List<City> AllCities = City.GetAll();
        model.Add("flight", SelectedFlight);
        model.Add("flightCities", FlightCities);
        model.Add("allCities", AllCities);
        return View["flight.cshtml", model];
      };

      Post["city/add_flight"] =_=> {
        Flight flight = Flight.Find(Request.Form["flight-id"]);
        City city = City.Find(Request.Form["city-id"]);
        city.AddFlight(flight);
        return View["success.cshtml"];
      };
      Post["flight/add_city"] = _ => {
      Flight flight = Flight.Find(Request.Form["flight-id"]);
      City city = City.Find(Request.Form["city-id"]);
      flight.AddCity(city);
      return View["success.cshtml"];
      };

    }
  }
}
