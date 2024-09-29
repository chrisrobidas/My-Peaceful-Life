using UnityEngine;

public class ToolsWheelButton : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private Sprite _icon;
    

    private Animator _buttonAnimator;
    
    public void OnSelected()
    {
        UIManager.Instance.UpdateSelectedTool(_id, _icon);
        UIManager.Instance.CallCloseToolsWheel();
    }

    public void OnHoverEnter()
    {
        _buttonAnimator.SetBool("Hovered", true);
    }

    public void OnHoverExit()
    {
        _buttonAnimator.SetBool("Hovered", false);
    }

    private void Start()
    {
        _buttonAnimator = GetComponent<Animator>();
    }
}
