using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBaseAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _pressScale = 0.75f;

    [SerializeField] private Transform _buttonParent;

    private void Awake()
    {
        _buttonParent ??= transform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _buttonParent.DOScale(0.75f, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _buttonParent.DOScale(1f, 0.1f);
    }
}