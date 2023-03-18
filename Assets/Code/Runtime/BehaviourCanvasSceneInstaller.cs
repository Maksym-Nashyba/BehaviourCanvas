using Code.Runtime.StateMachineElements;
using Zenject;

namespace Code.Runtime
{
    public class BehaviourCanvasSceneInstaller : MonoInstaller<BehaviourCanvasSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BehaviourTreeBuilder>().FromNew().AsSingle();
        }
    }
}