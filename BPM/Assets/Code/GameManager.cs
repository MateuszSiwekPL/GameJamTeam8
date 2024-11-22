using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private Canvas _canvas;
    [SerializeField] private EndScreen _endScreen;
    [SerializeField] private TextMeshProUGUI _bonusText;
    [SerializeField] private CanvasGroup _bonusTextCanvas;
    [SerializeField] private Image _progressImage;
    [SerializeField] private float timeToWait;
    
    
    private bool _shouldSpawn = true;
    private int _currentPath = 0;

    private Transform _targetPosition;
    private bool _isMoving;
    private CancellationTokenSource _ct;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        await UniTask.Delay(TimeSpan.FromMilliseconds(timeToWait));
        _progressImage.DOFillAmount(0, _hiddenTime).SetEase(Ease.Linear);
        StartSpawning().Forget();
        StartBit().Forget();
    }
    
    private async UniTask StartBit()
    {
        while (_shouldSpawn)
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(_bitRate));
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
            await UniTask.Delay(TimeSpan.FromMilliseconds(_spawnRate));

            for (int i = 0; i < _spawners.Count/2; i++)
            {
                var enemy = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)]);
                int randomPath;
                while (true)
                {
                    randomPath = Random.Range(0, _spawners.Count);
                    if (!takenPaths.Contains(randomPath) && 
                        !takenPaths.Contains(randomPath + 1) && 
                        !takenPaths.Contains(randomPath - 1))
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

    private async UniTask ShowEndScreen(bool win, bool imidiate = false)
    {
        if (!imidiate)
        {
            await UniTask.WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
        }

        var endScreen = Instantiate(_endScreen, _canvas.transform);
        endScreen.Setup(win, Mathf.Max(_time, 0f));
    }
    
    private void UpdateTimer()
    {
        _time -= Time.deltaTime;
        _timer.text = _time.ToString("F2") + "<size=40>s";
        
        if(_time <= 0)
        {
            _timer.text = "Party Over";
            _shouldSpawn = false;
            Debug.Log("LOSE");
            ShowEndScreen(false, true).Forget();
        }
    }
    
    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckBit())
            {
                AddTime(1f);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CheckBit();
            MovePlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CheckBit();
            MovePlayer(-1);
        }
    }
    
    private bool CheckBit()
    {
        var hit = Physics2D.Raycast(_raycastOrigin.position, Vector2.down, 1f);
        if (hit.collider != null && hit.collider.TryGetComponent<BitObjectBehaviour>(out var bit))
        {
            Destroy(bit.gameObject);
            return true;
        }
        else
        {
            RemoveTime(1f);
            return false;
        }
    }
    
    private void MovePlayer(int move)
    {
        var index = _currentPath + move;
        
        if(index < 0 || index >= _paths.Count)
        {
            return;
        }

        if (move > 0)
        {
            _player.transform.rotation = Quaternion.Euler(0, 0, -30);
        }
        else
        {
            _player.transform.rotation = Quaternion.Euler(0, 0, 30);
        }
        
        _currentPath = index;
        
        _targetPosition = _paths[index];
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
                _player.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    
    public void RemoveBits()
    {
        foreach (var o in GameObject.FindGameObjectsWithTag("Bit"))
        {
            var bit = o.GetComponent<BitObjectBehaviour>();
            bit.RemoveBit();
        }
    }

    public void AddTime(float time)
    {
        _time += time;
        _timer.text = _time.ToString("F2") + "<size=40>s";
        AnimateBonus(true, time).Forget();
    }
    
    public void RemoveTime(float time)
    {
        _time -= time;

        AnimateBonus(false, time).Forget();
    }

    private async UniTask AnimateBonus(bool isPlus, float value)
    {
        _ct?.Cancel();
        _ct = new CancellationTokenSource();
        
        if (isPlus)
        {
            _bonusText.text = "+" + value.ToString("F0") + "<size=20>s";
            _bonusText.color = Color.green;
        }
        else
        {
            _bonusText.text = "-" + value.ToString("F0") + "<size=20>s";
            _bonusText.color = Color.red;
        }

        var scale = _player.transform.localScale;

        try
        {
            if (isPlus)
            {
                await UniTask.WhenAll(
                    _bonusTextCanvas.DOFade(1, _bitRate / 2000).ToUniTask(cancellationToken: _ct.Token),
                    _player.transform.DOScale(scale * 1.2f, _bitRate / 2000).ToUniTask(cancellationToken: _ct.Token)
                );

                await UniTask.WhenAll(
                    _bonusTextCanvas.DOFade(0, _bitRate / 2000).ToUniTask(cancellationToken: _ct.Token),
                    _player.transform.DOScale(scale, _bitRate / 2000).ToUniTask(cancellationToken: _ct.Token)
                );
            }
            else
            {
                await _bonusTextCanvas.DOFade(1, _bitRate / 2000).ToUniTask(cancellationToken: _ct.Token);
                await _bonusTextCanvas.DOFade(0, _bitRate / 2000).ToUniTask(cancellationToken: _ct.Token);
            }
        }
        catch (OperationCanceledException)
        {
            _bonusTextCanvas.alpha = 0;
            _player.transform.localScale = scale;
        }
        
        _player.transform.localScale = scale;
    }
}
