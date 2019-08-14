namespace _Source.Features.ViewManagement
{
    public interface IViewManagementController
    {
        void OpenView(ViewType viewType);
        void CloseView(ViewType viewType);
    }
}
