using UnityEngine;
using UnityEngine.Events;

public class GuiDie : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public Sprite[] DiceImages;

    private const string Appear = "Appear";
    private const string Roll = "Roll";
    private Animator _animator;

    public GuiDie Child;
    public readonly UnityEvent OnClick = new UnityEvent();

    private uint _value;

    public uint Number
    {
        set
        {
            var isNewValue = (value != _value);
            if (!isNewValue && _spriteRenderer.enabled) return;

            _value = value;

            var animationToRun = (_spriteRenderer.enabled == false) ? Appear : Roll;

            _animator.SetTrigger(animationToRun);
            _spriteRenderer.enabled = true;
        }
    }

    private void OnEnable()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _animator = GetComponent<Animator>();
    }

    private void OnMouseUpAsButton()
    {
        OnClick.Invoke();
    }

    public void SwitchValue()
    {
        if (_value == 0) return;
        _spriteRenderer.sprite = DiceImages[_value - 1];
    }

    public void Hide()
    {
        _spriteRenderer.enabled = false;
    }
}