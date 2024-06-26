using System.Collections;
using Attributes;
using Events.GameEvents;
using Towers.Configuration;
using UnityEngine;
using Utils;

namespace LinearProjectiles
{
    [RequireComponent(typeof(TowerConfigurator))]
    public class SimpleProjectileLauncher : MonoBehaviour
    {
        [SerializeField] private GameEvent onWaveStart;
        [SerializeField] private GameEvent onEnterBuildMode;
        [SerializeField, Expandable] private SimpleProjectile projectilePrefab;
        
        private SimpleObjectPool<SimpleProjectile> _projectilePool;
        private TowerConfigurator _towerConfigurator;
        private Vector3 _shootingPosition;
        private Coroutine _shootingRoutine;
        
        private void Awake()
        {
            _towerConfigurator = GetComponent<TowerConfigurator>();
            _projectilePool = new SimpleObjectPool<SimpleProjectile>(projectilePrefab, 30, transform);
        }
        
        private void OnEnable()
        {
            onWaveStart.AddListener(StartShooting);
            onEnterBuildMode.AddListener(StopShooting);
        }
        
        private void OnDisable()
        {
            onWaveStart.RemoveListener(StartShooting);
            onEnterBuildMode.RemoveListener(StopShooting);
        }
        
        /// <summary>
        /// Sets the position where the projectile should go.
        /// </summary>
        /// <param name="position"> The position where the projectile should go. </param>
        public void SetShootingPosition(Vector3 position)
        {
            _shootingPosition = position;
        }
        
        private void StartShooting()
        {
            _shootingRoutine = StartCoroutine(ShootingRoutine());
        }
        
        private void StopShooting()
        {
            if (_shootingRoutine != null)
            {
                StopCoroutine(_shootingRoutine);
            }
        }
        
        private IEnumerator ShootingRoutine()
        {
            while (true)
            {
                var projectile = _projectilePool.GetPooledObject();
                var direction = (_shootingPosition - transform.position).normalized;

                // Places projectiles away from the tower to avoid collision with it.
                var startingPosition =
                    transform.position +
                    direction * projectilePrefab.transform.localScale.x +
                    direction * transform.localScale.x;
                
                projectile.transform.position = startingPosition;
                projectile.gameObject.SetActive(true);
                projectile.Shoot(_shootingPosition);
                
                yield return new WaitForSeconds(_towerConfigurator.TowerVariables.FireRate);
            }
        }
    }
}