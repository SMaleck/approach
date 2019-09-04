using System.Collections.Generic;
using System.Linq;

namespace _Source.Features.ViewManagement
{
    public class ViewManagementController : IViewManagementController, IViewManagementRegistrar
    {
        private readonly Dictionary<ViewType, IClosableView> _views;
        private readonly List<IClosableView> _openViews;

        public ViewManagementController()
        {
            _views = new Dictionary<ViewType, IClosableView>();
            _openViews = new List<IClosableView>();
        }

        public void RegisterView(ViewType viewType, IClosableView view, bool startClosed = true)
        {
            _views.Add(viewType, view);

            if (startClosed)
            {
                view.Close();
            }
        }

        public void OpenView(ViewType viewType)
        {
            if (_views.TryGetValue(viewType, out var view))
            {
                view.Open();
                _openViews.Add(view);
            }
        }

        public void CloseView(ViewType viewType)
        {
            if (_views.TryGetValue(viewType, out var view))
            {
                view.Close();
                _openViews.Remove(view);
            }
        }

        public void CloseLastOpenView()
        {            
            var view = _openViews.LastOrDefault();
            if (view != null)
            {
                view.Close();
                _openViews.Remove(view);
            }
        }
    }
}
