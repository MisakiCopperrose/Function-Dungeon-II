using Towers.Configuration;
using Towers.Configuration.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Towers.Placement
{
    public class TowerFinder : MonoBehaviour
    {
        [SerializeField] private TowerConfigurationEvent onTowerFound = new();
        
        private RaycastHit _onClickHit;

        /// <summary>
        /// Subscribes to the OnClick event.
        /// </summary>
        /// <param name="context"></param>
        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Started)
                return;
            
            if (UnityEngine.Camera.main == null) 
                return;
            
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (!Physics.Raycast(ray, out _onClickHit))
                return;
            
            if (!_onClickHit.collider.TryGetComponent<TowerConfigurator>(out var tower))
                return;
            
            onTowerFound.Invoke(tower);
        }
        
        /// <summary>
        /// Subscribes to the OnSuitablePlacement event.
        /// </summary>
        /// <param name="action"> The action to subscribe to. </param>
        public void SubscribeToOnSuitablePlacement(UnityAction<TowerConfigurator> action)
        {
            onTowerFound.AddListener(action);
        }
        
        /// <summary>
        /// Unsubscribes from the OnSuitablePlacement event.
        /// </summary>
        /// <param name="action"> The action to unsubscribe from. </param>
        public void UnsubscribeFromOnSuitablePlacement(UnityAction<TowerConfigurator> action)
        {
            onTowerFound.RemoveListener(action);
        }
    }
}