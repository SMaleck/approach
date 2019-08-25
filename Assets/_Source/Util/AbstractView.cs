using _Source.Features.ViewManagement;

namespace _Source.Util
{
    public class AbstractView : AbstractDisposableMonoBehaviour, IClosableView
    {        
        public void Open()
        {
            SetActive(true);
        }

        public void Close()
        {
            SetActive(false);
        }

        protected void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
