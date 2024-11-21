using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private List<Transform> _paths;
    [SerializeField] private List<Transform> _spawners;
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private float _time;
    [SerializeField] private float _spawnRate;
    [SerializeField] private GameObject _bonusPrefab;
    [SerializeField] private GameObject _enemyPrefab;

    private Transform _targetPosition;
    private bool _isMoving;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartSpawning().Forget();
    }

    private async UniTask StartSpawning()
    {
        var takenPaths = new HashSet<int>();

        while (true)
        {
            takenPaths.Clear();
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnRate));
            
            if (Random.value <= 1f)
            {
                Instantiate(_bonusPrefab);
                int randomPath;
                while (true)
                {
                    randomPath = Random.Range(0, _spawners.Count);
                    if (!takenPaths.Any(path => path == randomPath))
                    {
                        takenPaths.Add(randomPath);
                        break;
                    }
                }
                _bonusPrefab.transform.position = _spawners[randomPath].position;
            }
            
            var numberOfObjects = Random.Range(2, 5);

            for (int i = 0; i < numberOfObjects; i++)
            {
                Instantiate(_enemyPrefab);
                int randomPath;
                while (true)
                {
                    randomPath = Random.Range(0, _spawners.Count);
                    if (!takenPaths.Contains(randomPath))
                    {
                        takenPaths.Add(randomPath);
                        break;
                    }
                }
                _enemyPrefab.transform.position = _spawners[randomPath].position;
            }
        }
    }

    private void Update()
    {
        GetInput();
        UpdateTimer();
        MovePlayer();
    }
    
    private void UpdateTimer()
    {
        _timer.text = _time.ToString("F2");
        _time -= Time.deltaTime;
    }
    
    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MovePlayer(1); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MovePlayer(2); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MovePlayer(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(_paths.Count < 4) return;
            MovePlayer(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if(_paths.Count < 5) return;
            MovePlayer(5);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(_player.CurrentBonus != null)
            {
                _player.CurrentBonus.TryGetBonus(KeyCode.UpArrow);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(_player.CurrentBonus != null)
            {
                _player.CurrentBonus.TryGetBonus(KeyCode.DownArrow);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(_player.CurrentBonus != null)
            {
                _player.CurrentBonus.TryGetBonus(KeyCode.RightArrow);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(_player.CurrentBonus != null)
            {
                _player.CurrentBonus.TryGetBonus(KeyCode.LeftArrow);
            }
        }
    }
    
    private void MovePlayer(int position)
    {
        _targetPosition = _paths[position - 1];
        _isMoving = true;
    }

    private void MovePlayer()
    {
        if (_isMoving)
        {
            _player.transform.position = Vector2.MoveTowards(
                _player.transform.position,
                _targetPosition.position,
                _player.Speed * Time.deltaTime
            );
            
            if (Vector2.Distance(_player.transform.position, _targetPosition.position) < 0.01f)
            {
                _player.transform.position = _targetPosition.position;
                _isMoving = false;
            }
        }
    }

    public void AddTime(float time)
    {
        _time += time;
    }
    
    public void RemoveTime(float time)
    {
        _time -= time;
    }
}
