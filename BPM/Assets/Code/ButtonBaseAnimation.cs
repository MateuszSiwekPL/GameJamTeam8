using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBaseAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _pressScale = 0.75f;

    [SerializeField] private Transform _buttonParent;
    [SerializeField] private AudioSource _audioSource;
    
    private Vector3 _initialScale;

    private void Awake()
    {
        _buttonParent ??= transform;
        _initialScale = _buttonParent.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _buttonParent.DOScale(_pressScale, 0.1f);
        
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _buttonParent.DOScale(_buttonParent.localScale, 0.1f);
    }
}