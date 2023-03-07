using Zenject;

namespace Code.Runtime
{
    internal class BehaviourCanvasSceneInstaller : MonoInstaller<BehaviourCanvasSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BehaviourTreeBuilder>().FromNew().AsSingle();
        }
    }
}