using System.Web;
using System.Web.Mvc;

namespace Ignia.Workbench.DeconstructedWebApi {

  /*============================================================================================================================
  | CLASS: FILTER CONFIGURATION
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   Provides a centralized registration of filters to be used across the project. 
  ///  </summary>
  ///  <remarks>
  ///    Filters allow for code to be bound to multiple controllers and actions based on particular events, such as 
  ///    authorization, errors, executing an action, or processing a result. Filters can be used to require authentication or 
  ///    HTTPS, add output caching, centralize global error handling, etc.
  /// </remarks>
  public class FilterConfig {

    /*==========================================================================================================================
    | METHOD: REGISTER GLOBAL FILTERS
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Registers filters that will be used for all incoming Web API requests.
    /// </summary>
    /// <remarks>
    ///   The RegisterGlobalFilters() method is called by the Global.asax with the GlobalFilters.Filters collection as a 
    ///   parameter. It then uses this collection to add any filters that are intended to be used globally. 
    /// </remarks>
    /// <param name="filters">Reference to the global collection of filters.</param>
    public static void RegisterGlobalFilters(GlobalFilterCollection filters) {

      //Provides global support for error handling across all WebAPI controllers. The behavior can be customized centrally via 
      //the filter, or it can be overwritten on a per-action basis using the [HandleError] attribute.  
      filters.Add(new HandleErrorAttribute());

    }

  } //Class
} //Namespace
