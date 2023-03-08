using Code.Runtime.StateMachineElements;
using Zenject;

namespace Code.Runtime.Initialization
{
    internal class BehaviourCanvasSceneInstaller : MonoInstaller<BehaviourCanvasSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BehaviourTreeBuilder>().FromNew().AsSingle();
        }
    }
}