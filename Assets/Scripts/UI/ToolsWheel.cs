using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class ToolsWheel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Button[] _toolsWheelButtons;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _hoverColor;

    private int _selectedToolID;
    private int _hoveredToolID;

    public void OpenToolsWheel()
    {
        _animator.SetBool("ToolsWheelOpened", true);
    }

    public void CloseToolsWheel()
    {
        _animator.SetBool("ToolsWheelOpened", false);
        _toolsWheelButtons[_hoveredToolID].Select();
    }

    public void UpdateSelectedToolID(int selectedToolID)
    {
        if (_selectedToolID == selectedToolID) return;

        _selectedToolID = selectedToolID;

        ThirdPersonController localPlayerThirdPersonController = GameManager.Instance.GetLocalPlayer().GetComponent<ThirdPersonController>();
        localPlayerThirdPersonController.SwapHeldTool(selectedToolID);
    }

    private void Start()
    {
        for (int i = 0; i < _toolsWheelButtons.Length; i++)
        {
            _toolsWheelButtons[i].image.color = _normalColor;
        }

        _toolsWheelButtons[0].image.color = _hoverColor;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsSelectingTool) return;

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosition = Input.mousePosition;
        Vector2 direction = mousePosition - screenCenter;

        // The player must have moved a bit the mouse away from the center of the screen to select a tool
        if (direction.magnitude < 40) return;

        float angle = -(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        // Adjust the angle so the angle on left and right of the hand are 0 and 45
        float halfSegmentAngle = (360f / _toolsWheelButtons.Length) / 2;
        angle += 90f + halfSegmentAngle;

        // Adjust the angle to be between 0 and 360
        if (angle < 0) angle += 360f;

        _hoveredToolID = GetButtonIndexFromAngle(angle);
        HighlightButton();
    }

    private int GetButtonIndexFromAngle(float angle)
    {
        float segmentAngle = 360f / _toolsWheelButtons.Length;
        int buttonIndex = Mathf.FloorToInt(angle / segmentAngle);
        return buttonIndex;
    }

    private void HighlightButton()
    {
        for (int i = 0; i < _toolsWheelButtons.Length; i++)
        {
            _toolsWheelButtons[i].GetComponent<ToolsWheelButton>().HoverExit();
            _toolsWheelButtons[i].image.color = _normalColor;
        }

        _toolsWheelButtons[_hoveredToolID].GetComponent<ToolsWheelButton>().HoverEnter();
        _toolsWheelButtons[_hoveredToolID].image.color = _hoverColor;
    }
}
