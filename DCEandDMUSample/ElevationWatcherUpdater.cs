using System;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DCEandDMUSample
{
    /// <summary>
    /// Updater оповещающий пользователя при добавлении фасада.
    /// </summary>
    public class ElevationWatcherUpdater : IUpdater
    {
        static AddInId _appId;
        static UpdaterId _updaterId;

        public ElevationWatcherUpdater(AddInId id)
        {
            _appId = id;

            _updaterId = new UpdaterId(_appId, new Guid(
                                                   "fafbf6b2-4c06-42d4-97c1-d1b4eb593eff"));
        }

        public void Execute(UpdaterData data)
        {
            Document doc = data.GetDocument();
            Application app = doc.Application;
            foreach (ElementId id in
                data.GetAddedElementIds())
            {
                View view = doc.GetElement(id) as View;

                if (null != view
                    && ViewType.Elevation == view.ViewType)
                {
                    TaskDialog.Show("ElevationWatcher Updater",
                                    string.Format("Новый фасад'{0}'",
                                                  view.Name));
                }
            }
        }

        public string GetAdditionalInformation()
        {
            return "The Building Coder, "
                   + "http://thebuildingcoder.typepad.com";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.FloorsRoofsStructuralWalls;
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }

        public string GetUpdaterName()
        {
            return "ElevationWatcherUpdater";
        }
    }
}