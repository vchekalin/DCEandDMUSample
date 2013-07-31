#region Namespaces

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace DCEandDMUSample
{
    /// <summary>
    /// ������� �� ���������� ������ ���� �����
    /// </summary>
    [Transaction(TransactionMode.ReadOnly)]
    public class CmdElevationWatcher : IExternalCommand
    {
        static EventHandler<DocumentChangedEventArgs>
             _handler = null;

        /// <summary>
        /// ���������� ������ ��� ���� ����� � ���������� ��������� 
        /// Element Id ��� null ���� ��� �� ������
        /// </summary>
        static View FindElevationView(
          Document doc,
          ICollection<ElementId> ids)
        {
            View view = null;

            foreach (ElementId id in ids)
            {
                view = doc.GetElement(id) as View;

                if (null != view
                  && ViewType.Elevation == view.ViewType)
                {
                    break;
                }

                view = null;
            }
            return view;
        }

        
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Application app = uiapp.Application;

            // �������� �� ������� DocumentChanged
            if (_handler == null)
            {
                _handler =
                    OnDocumentChanged;

                app.DocumentChanged
                    += _handler;
            }
            else
            {
                //�������
                app.DocumentChanged -=
                    _handler;
                _handler = null;

            }
            return Result.Succeeded;
        }

        /// <summary>
        /// ��������� ������� DocumentChanged
        /// </summary>
        private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            Document doc = e.GetDocument();

            View view = FindElevationView(
              doc, e.GetAddedElementIds());

            if (null != view)
            {
                string msg = string.Format(
                  "�� ������ ��� ������� ����� ��� ����� "
                  + " '{0}'.",
                  view.Name);

                TaskDialog.Show("ElevationChecker", msg);
            }
        }
    }
}
