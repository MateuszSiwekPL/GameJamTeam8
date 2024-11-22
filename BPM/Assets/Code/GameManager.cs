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
    [SerializeField] private float _bitRate;
    [SerializeField] private GameObject _bonusPrefab;
    [SerializeField] private List<GameObject>_enemyPrefabs;
    [SerializeField] private GameObject _bitPrefab;
    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private Transform _bitSpawnPoint;
    [SerializeField] private float _hiddenTime;
    
    
    private bool _shouldSpawn = true;

    private Transform _targetPosition;
    private bool _isMoving;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartSpawning().Forget();
        StartBit().Forget();
    }
    
    private async UniTask StartBit()
    {
        while (_shouldSpawn)
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(_bitRate * 1000));
            var bit = Instantiate(_bitPrefab);
            bit.transform.position = _bitSpawnPoint.position;
        }
    }

    private async UniTask StartSpawning()
    {
        var takenPaths = new HashSet<int>();

        while (_shouldSpawn)
        {
            takenPaths.Clear();
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnRate));
            
            if (Random.value <= 0.2f)
            {
                var bonus = Instantiate(_bonusPrefab);
                int randomPathBonus;
                while (true)
                {
                    randomPathBonus = Random.Range(0, _spawners.Count);
                    if (!takenPaths.Contains(randomPathBonus))
                    {
                        takenPaths.Add(randomPathBonus);
                        break;
                    }
                }
                bonus.transform.position = _spawners[randomPathBonus].position;
            }
            
            var numberOfObjects = Random.Range(2, 5);

            for (int i = 0; i < numberOfObjects; i++)
            {
                var enemy = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)]);
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
                enemy.transform.position = _spawners[randomPath].position;
            }
        }
    }

    private void Update()
    {
        GetInput();
        MovePlayer();
        if (_shouldSpawn)
        {
            UpdateTimer();
            UpdateHiddenTimer();
        }
    }
    
    private void UpdateHiddenTimer()
    {
        _hiddenTime -= Time.deltaTime;
        
        if(_hiddenTime <= 0)
        {
            _shouldSpawn = false;
            Debug.Log("WIN");
            ShowEndScreen(true).Forget();
        }
    }

    private async UniTask ShowEndScreen(bool win)
    {
        await UniTask.WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    
    private void UpdateTimer()
    {
        _timer.text = _time.ToString("F2");
        _time -= Time.deltaTime;
        
        if(_time <= 0)
        {
            _shouldSpawn = false;
            Debug.Log("LOSE");
            ShowEndScreen(false).Forget();
        }
    }
    
    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CheckBit();
            MovePlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CheckBit();
            MovePlayer(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CheckBit();
            MovePlayer(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(_paths.Count < 4) return;
            CheckBit();
            MovePlayer(4);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if(_paths.Count < 5) return;
            CheckBit();
            MovePlayer(5);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CheckBit();
            if (_player.CurrentBonus != null)
            {
                _player.CurrentBonus.TryGetBonus(KeyCode.UpArrow);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CheckBit();
            if (_player.CurrentBonus != null)
            {
                _player.CurrentBonus.TryGetBonus(KeyCode.DownArrow);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CheckBit();
            if (_player.CurrentBonus != null)
            {
                _player.CurrentBonus.TryGetBonus(KeyCode.RightArrow);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CheckBit();
            if (_player.CurrentBonus != null)
            {
                _player.CurrentBonus.TryGetBonus(KeyCode.LeftArrow);
            }
        }
    }
    
    private void CheckBit()
    {
        var hit = Physics2D.Raycast(_raycastOrigin.position, Vector2.down, 1f);
        if (hit.collider != null && hit.collider.TryGetComponent<BitObjectBehaviour>(out var bit))
        {
            Destroy(bit.gameObject);
        }
        else
        {
            RemoveTime(1f);
        }
    }
    
    private void MovePlayer(int position)
    {
        _targetPosition = _paths[position - 1];
        _isMoving = true;
        _player.BoxCollider2D.enabled = false;
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
                _player.BoxCollider2D.enabled = true;
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
