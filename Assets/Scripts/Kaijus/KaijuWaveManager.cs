using Events.GameEvents;
using UnityEngine;

namespace Kaijus
{
    public class KaijuWaveManager : MonoBehaviour
    {
        [Header("Variables for pre-defined level")]
        [Tooltip("Insert a pre-defined Kaijulevel scriptable object in here")]
        [SerializeField] private KaijuLevel levelToPlay;

        /// <summary>
        /// Random level is conditinial bool for this block of variables
        /// </summary>
        [Header("Variables for random level")]
        [SerializeField] private bool randomLevel;
        [Tooltip("Prefabs for kaijus which can be included in the random level")]
        [SerializeField] private GameObject[] kaijuPrefabs;
        [Tooltip("Amount of kaijus in the random level")]
        [SerializeField] private int kaijusInLevel = 3;

        [Header("Events")]
        [SerializeField] private GameEvent onKaijuDie;

        private GameObject[] _kaijuLevel;
        private int _currentKaijuInLevel;

        private void Awake()
        {
            onKaijuDie.AddListener(NextKaijuInLevel);
        }

        private void Start()
        {
            if (randomLevel)
            {
                GenerateRandomLevel();
            }
            else
            {
                GeneratePredefinedLevel();
            }
        }

        /// <summary>
        /// Generates a random kaijulevel and saves it in an array. Spawns the first kaiju
        /// </summary>
        private void GenerateRandomLevel()
        {
            _kaijuLevel = new GameObject[kaijusInLevel];
            for (var i = 0; i < kaijusInLevel; i++)
            {
                _kaijuLevel[i] = kaijuPrefabs[Random.Range(0, kaijuPrefabs.Length)];
            }
            SpawnKaiju();
        }

        /// <summary>
        /// Generates a pre-defined kaijulevel and saves it in an array. Spawns the first kaiju
        /// </summary>
        private void GeneratePredefinedLevel()
        {
            _kaijuLevel = new GameObject[levelToPlay.KaijuCount];
            for (var i = 0; i < levelToPlay.KaijuCount; i++)
            {
                _kaijuLevel[i] = levelToPlay.KaijuPrefabs[i];
            }
            kaijusInLevel = levelToPlay.KaijuCount;
            SpawnKaiju();
        }

        /// <summary>
        /// Checks if there are any kaijus left in the level. If yes spawn the next kaiju
        /// </summary>
        private void NextKaijuInLevel()
        {
            if (_currentKaijuInLevel == kaijusInLevel)
            {
                //TODO player has killed all kaijus in a level, Should this be here?
            }
            _currentKaijuInLevel++;
            SpawnKaiju();
        }

        /// <summary>
        /// Spawns the kaiju with index _currentKaijuInLevel.
        /// </summary>
        private void SpawnKaiju()
        {
            Instantiate(_kaijuLevel[_currentKaijuInLevel]);
        }
    }
}
