using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UI_Title : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _pressToStart;
    [SerializeField] Image _image;
    [SerializeField] float _shiftTime = 0.5f;

    private bool _isMoving = false;

    public delegate void Callback();

    // Start is called before the first frame update
    void Start()
    {
        _pressToStart.rectTransform.DOScale(1.1f, 0.75f).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
        _pressToStart.rectTransform.DORotate(-_pressToStart.rectTransform.rotation.eulerAngles, 2).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public bool GoUp()
    {
        if (_isMoving == false)
        {
            _isMoving = true;

            _image.transform.DOLocalMoveY(_image.rectTransform.rect.height * 2, _shiftTime).From(Vector3.zero);
            _image.transform.DOScale(2f, _shiftTime).OnComplete(() =>
            {
                _image.transform.localScale = new Vector3(1, 1, 1);
                gameObject.SetActive(false);

                _isMoving = false;
            });

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GoDown(Callback InFunc)
    {
        if (_isMoving == false)
        {
            gameObject.SetActive(true);

            _isMoving = true;

            _image.transform.localScale = new Vector3(1, 1, 1);
            _image.transform.DOLocalMove(Vector3.zero, _shiftTime).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                _isMoving = false;
                InFunc?.Invoke();
            });

            return true;
        }
        else
        {
            return false;
        }
    }
}
