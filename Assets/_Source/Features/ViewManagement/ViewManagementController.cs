using System.Collections.Generic;

namespace _Source.Features.ViewManagement
{
    public class ViewManagementController : IViewManagementController, IViewManagementRegistrar
    {
        private readonly Dictionary<ViewType, IClosableView> _views;

        public ViewManagementController()
        {
            _views = new Dictionary<ViewType, IClosableView>();
        }

        public void RegisterView(ViewType viewType, IClosableView view)
        {
            _views.Add(viewType, view);
        }

        public void OpenView(ViewType viewType)
        {
            _views.TryGetValue(viewType, out var view);
            view?.Open();
        }

        public void CloseView(ViewType viewType)
        {
            _views.TryGetValue(viewType, out var view);
            view?.Close();
        }
    }
}
