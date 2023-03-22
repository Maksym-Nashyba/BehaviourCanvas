using BehaviourCanvas.Code.Runtime.StateMachineElements;
using Zenject;

namespace BehaviourCanvas.Code.Runtime
{
    public class BehaviourCanvasSceneInstaller : MonoInstaller<BehaviourCanvasSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BehaviourTreeBuilder>().FromNew().AsSingle();
        }
    }
}