using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DCEandDMUSample
{
    [Transaction(TransactionMode.ReadOnly)]
    public class CmdElevationWatcherUpdater : IExternalCommand
    {
        private static ElevationWatcherUpdater _updater = null;

        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Application app = uiapp.Application;

            // Register updater to react to view creation

            if (_updater == null)
            {
                _updater
                    = new ElevationWatcherUpdater(
                        app.ActiveAddInId);

                UpdaterRegistry.RegisterUpdater(_updater);

                ElementCategoryFilter f
                    = new ElementCategoryFilter(
                        BuiltInCategory.OST_Views);

                UpdaterRegistry.AddTrigger(
                    _updater.GetUpdaterId(), f,
                    Element.GetChangeTypeElementAddition());
            }
            else
            {
                UpdaterRegistry.UnregisterUpdater(_updater.GetUpdaterId());

                _updater = null;
            }
            return Result.Succeeded;
        }
    }
}