namespace WebApplication1.Infrastructure
{
  using System;
  using System.Web.Mvc;
  using System.Web.Mvc.Async;
  using System.Web.Routing;

  public class ControllerFactory : DefaultControllerFactory
  {
    const string BaseFeatureNamespace = "WebApplication1.Features";
    const string FeatureControllerName = "FeatureController";
    protected override Type GetControllerType(RequestContext aRequestContext, string aRoute)
    {
      Type type = base.GetControllerType(aRequestContext, aRoute);
      if (type == null)      {
        string Feature = aRequestContext.RouteData.GetRequiredString(valueName: "action");
        string typeName = $"{BaseFeatureNamespace}.{aRoute}.{Feature}+{FeatureControllerName}";

        System.Reflection.Assembly assembly = typeof(ControllerFactory).Assembly;
        type = assembly.GetType(typeName);
      }
      return type;
    }
    public override IController CreateController(RequestContext aRequestContext, string aControllerName)
    {
      IController controller = base.CreateController(aRequestContext, aControllerName);
      return ReplaceActionInvoker(controller);
    }

    private IController ReplaceActionInvoker(IController aController)
    {
      var mvcController = aController as Controller;
      if (mvcController != null)
      {
        mvcController.ActionInvoker = new FeatureActionInvoker();
      }

      return aController;
    }


    public class FeatureActionInvoker : AsyncControllerActionInvoker
    {
      protected override ActionDescriptor FindAction(ControllerContext aControllerContext, ControllerDescriptor aControllerDescriptor, string aActionName)
      {
        ActionDescriptor actionDescriptor = base.FindAction(aControllerContext, aControllerDescriptor, actionName: aActionName);
        if (actionDescriptor == null)
        {
          actionDescriptor = base.FindAction(aControllerContext, aControllerDescriptor, actionName: aControllerContext.HttpContext.Request.HttpMethod);
        }
        return actionDescriptor;
      }
    }
  }
}