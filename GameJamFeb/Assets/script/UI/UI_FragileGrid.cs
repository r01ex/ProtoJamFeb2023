using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FragileGrid : MonoBehaviour
{
    [SerializeField] GridLayoutGroup _gridLayoutGroup;
    [SerializeField] Image[] _images;
    [SerializeField] int _maxCol;
    [SerializeField] int _minCol;
    RectTransform _transform;

    public Sprite FragileSprite;
    public Sprite BrokenSprite;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    public void SetCount(int Total, int Safe, int Broken)
    {
        Debug.Assert(Total >= Safe + Broken);

        if (Total > _images.Length)
        {
            Broken -= Total - _images.Length;

            Total = _images.Length;

            if (Safe > _images.Length)
            {
                Safe = _images.Length;
            }
        }

        int i = 0;
        for(; i < Safe; ++i)
        {
            _images[i].sprite = FragileSprite;
            _images[i].color = Color.white;
            _images[i].gameObject.SetActive(true);
        }
        for(; i < Total - Broken; ++i)
        {
            _images[i].sprite = FragileSprite;
            _images[i].color = Color.gray;
            _images[i].gameObject.SetActive(true);
        }
        for (; i < Total; ++i )
        {
            _images[i].sprite = BrokenSprite;
            _images[i].color = Color.white;
            _images[i].gameObject.SetActive(true);
        }
        for (; i < _images.Length; ++i)
        {
            _images[i].gameObject.SetActive(false);
        }

        int col = Mathf.Clamp(Total, _minCol, _maxCol);
        int size = Mathf.FloorToInt(_transform.rect.width / col);
        _gridLayoutGroup.cellSize = new Vector2(size, size);
    }
}
