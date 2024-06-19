using System.Collections;
using System.Collections.Generic;
using Events.GameEvents;
using Health;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using WorldGrid;

namespace WaveSystem
{
    public class BloomWaveManager : MonoBehaviour
    {
        private const int WaveMaxIndex = 3;
        
        [Header("Waves")] 
        [SerializeField] private List<NavMeshAgent> waveOne;
        [SerializeField] private List<NavMeshAgent> waveTwo;
        [SerializeField] private List<NavMeshAgent> waveThree;

        [Header("Spawn References")] 
        [SerializeField] private float spawnInterval;
        [SerializeField] private GridGenerator gridGenerator;
        
        [Header("Events")]
        [SerializeField] private GameEvent onWavesCompleted;
        [SerializeField] private UnityEvent onWaveCompleted = new();
        
        private readonly List<NavMeshAgent> _activeEnemies = new();

        private List<NavMeshAgent>[] _waves;
        private int _waveIndex;
        private int _enemyCount;

        private void Awake()
        {
            _waves = new[]
            {
                waveOne,
                waveTwo,
                waveThree
            };
        }

        public void SpawnWave()
        {
            _activeEnemies.Clear();

            var wave = _waves[_waveIndex];

            foreach (var enemy in wave)
            {
                var instantiatedEnemy = Instantiate(
                    enemy,
                    gridGenerator.PathStartPosition,
                    Quaternion.identity,
                    transform
                );
                
                _enemyCount++;
                
                instantiatedEnemy.gameObject.SetActive(false);
                instantiatedEnemy.GetComponent<Damageable>().SubscribeToDeathEvent(DecrementEnemyCount);

                _activeEnemies.Add(instantiatedEnemy);
            }

            _waveIndex++;

            StartCoroutine(StartWave());
        }
        
        private void DecrementEnemyCount()
        {
            _enemyCount--;

            if (_enemyCount != 0) 
                return;
            
            onWaveCompleted.Invoke();
                
            if (_waveIndex == WaveMaxIndex)
                onWavesCompleted?.Invoke();
        }

        private IEnumerator StartWave()
        {
            foreach (var enemy in _activeEnemies)
            {
                enemy.gameObject.SetActive(true);
                enemy.enabled = true;
                enemy.SetDestination(gridGenerator.PathEndPosition);
                
                yield return new WaitForSeconds(spawnInterval);
            }
            
            yield return null;
        }
    }
}