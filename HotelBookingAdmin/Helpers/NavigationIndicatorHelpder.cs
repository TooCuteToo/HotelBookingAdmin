using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelBookingAdmin.Helpers
{
  public static class NavigationIndicatorHelper
  {
    public static string IsSelected(this HtmlHelper htmlHelper, string controllers, string actions, string cssClass = "selected")
    {
      string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
      string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

      IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
      IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');

      return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
          cssClass : String.Empty;
    }
  }
}