using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NutriFitWeb.Services
***REMOVED***
    public static class ViewRenderService
    ***REMOVED***

        public static async Task<string> RenderViewToStringAsync(this Controller controller, string viewNamePath, object model = null)
        ***REMOVED***
            if (string.IsNullOrEmpty(viewNamePath))
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

            controller.ViewData.Model = model;

            if (model == null)
            ***REMOVED***
                controller.ViewData.ModelState.Clear();
        ***REMOVED***

            using StringWriter writer = new();
            try
            ***REMOVED***
                var view = FindView(controller, viewNamePath);
                ViewContext viewContext = new(
                    controller.ControllerContext,
                    view,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await view.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
        ***REMOVED***
            catch (Exception exc)
            ***REMOVED***
                return $"Failed - ***REMOVED***exc.Message***REMOVED***";
        ***REMOVED***
    ***REMOVED***

        private static IView FindView(Controller controller, string viewNamePath)
        ***REMOVED***
            IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

            ViewEngineResult viewResult;
            if (viewNamePath.EndsWith(".cshtml"))
                viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
            else
                viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

            if (!viewResult.Success)
            ***REMOVED***
                var endPointDisplay = controller.HttpContext.GetEndpoint().DisplayName;

                if (endPointDisplay.Contains(".Areas."))
                ***REMOVED***
                    //search in Areas
                    var areaName = endPointDisplay.Substring(endPointDisplay.IndexOf(".Areas.") + ".Areas.".Length);
                    areaName = areaName.Substring(0, areaName.IndexOf(".Controllers."));

                    viewNamePath = $"~/Areas/***REMOVED***areaName***REMOVED***/views/***REMOVED***controller.HttpContext.Request.RouteValues["controller"]***REMOVED***/***REMOVED***controller.HttpContext.Request.RouteValues["action"]***REMOVED***.cshtml";

                    viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
            ***REMOVED***

                if (!viewResult.Success)
                    throw new Exception($"A view with the name '***REMOVED***viewNamePath***REMOVED***' could not be found");

        ***REMOVED***

            return viewResult.View;
    ***REMOVED***

***REMOVED***

***REMOVED***
