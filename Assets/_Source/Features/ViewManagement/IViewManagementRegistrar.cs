namespace _Source.Features.ViewManagement
{
    public interface IViewManagementRegistrar
    {
        void RegisterView(ViewType viewType, IClosableView view);
    }
}
