using System;
using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _selectedColor;
    private Color _defaultColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Transform _content;
    private Vector2 _boardIndex;
    private bool _isSelected;
    private bool _isEmpty;
    public event Action<Tile> TileSelected;

    public bool IsEmpty => _isEmpty;
    public int X => (int)_boardIndex.x;
    public int Y => (int)_boardIndex.y;
    public Vector2 Size => transform.localScale;
    public Transform Content => _content;
    public Dot Dot
    {
        get
        {
            if (_content != null && _content.TryGetComponent<Dot>(out Dot dot))
            {
                return dot;
            }
            else
            {
                return null;
            }
        }
    }

    private void Awake()
    {
        _defaultColor = _renderer.color;
    }

    private void OnMouseDown()
    {
        if (_isSelected == true)
        {
            Deselect();
        }
        else if(!_content.TryGetComponent<Hole>(out Hole hole))
        {
            Select();
        }
    }

    public void Initialize(Vector2 boardIndex)
    {
        _boardIndex = boardIndex;
    }

    public bool Compare(Tile target)
    {
        return _content != null && target.Content != null && _content.tag == target.Content.tag;
    }

    public void SetContent<T>(T newItem) where T : MonoBehaviour
    {
        newItem.transform.SetParent(transform);
        
        if (_content == null)
            newItem.transform.localPosition = Vector2.zero;

        _content = newItem.transform;
        _isEmpty = false;
    }

    public IEnumerator Translate()
    {
        while (Mathf.Abs(_content.localPosition.x) > 0.1f)
        {
            _content.localPosition = Vector2.Lerp(_content.localPosition, Vector2.zero, 0.4f);
            yield return new WaitForFixedUpdate();
        }

        while (Mathf.Abs(_content.localPosition.y) > 0.1f)
        {
            _content.localPosition = Vector2.Lerp(_content.localPosition, Vector2.zero, 0.4f);
            yield return new WaitForFixedUpdate();
        }

        _content.localPosition = Vector2.zero;
    }

    public void Clear()
    {
        _content = null;
        _isEmpty = true;
    }

    public void Remove()
    {
        Dot.Remove();
        Clear();
    }

    public void Deselect()
    {
        _renderer.color = _defaultColor;
        _isSelected = false;
    }

    private void Select()
    {
        _renderer.color = _selectedColor;
        _isSelected = true;
        TileSelected?.Invoke(this);
    }
}