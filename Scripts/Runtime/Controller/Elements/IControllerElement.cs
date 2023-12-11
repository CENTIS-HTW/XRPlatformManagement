using CENTIS.XRPlatformManagement.Controller.Manager;

namespace CENTIS.XRPlatformManagement.Controller.Elements
{
    public interface IControllerElement
    {
        void InitializeControllerElement(ControllerModelSpawner controllerModelSpawner, ControllerElementServiceLocator controllerElementServiceLocator);
    }
}
