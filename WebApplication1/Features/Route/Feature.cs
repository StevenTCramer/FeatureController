namespace WebApplication1.Features.Route
{
  using System.Web.Mvc;

  public class Feature
  {
    public class FeatureController : Controller
    {
      // URL = http://localhost:23187/Route/Feature
      public ActionResult Get()
      {
        return Content("Get Action");
      }

      public ActionResult Post()
      {
        return Content("Post Action");
      }
    }
  }
}
