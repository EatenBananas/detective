using GameManagers;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Raycaster>().AsSingle().NonLazy();
        }
    }
}
