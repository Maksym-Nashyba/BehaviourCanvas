using UnityEngine;
using Zenject;

namespace BehaviourCanvas.Code.Runtime
{
    public class GameObjectInstaller : MonoInstaller<GameObjectInstaller>
    {
        [SerializeField] private GameObjectContext _gameObjectContext;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_gameObjectContext).AsSingle();
        }
    }
}