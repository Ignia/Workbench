using System.Web;
using System.Web.Mvc;

namespace Ignia.Workbench.DeconstructedWebApi {

  /// <summary>
  ///   Provides a means to add filters.
  ///   ??? What are filters ???
  /// </summary>
  public class FilterConfig {

    /// <summary>
    ///   Registers the global filters.
    /// </summary>
    /// <remarks>
    ///   The RegisterGlobalFilters() method is called by the Global.asax with the GlobalFilters.Filters collection as a parameter. 
    ///   It then uses this collection to add any filters that are intended to be used globally. 
    /// </remarks>
    /// <param name="filters">The filters.</param>
    public static void RegisterGlobalFilters(GlobalFilterCollection filters) {

      //Adds support for the [Error] attribute to configure error behavior within WebAPI controllers. ???
      filters.Add(new HandleErrorAttribute());

    }

  } //Class
} //Namespace
