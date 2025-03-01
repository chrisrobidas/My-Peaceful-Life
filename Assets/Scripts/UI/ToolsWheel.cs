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

    private ThirdPersonController _localPlayerThirdPersonController;
    private StarterAssetsInputs _localPlayerStarterAssetsInputs;

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

        GetLocalPlayerThirdPersonController().SwapHeldTool(selectedToolID);
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

        float? angle = null;
        if (GetLocalPlayerStarterAssetsInputs().IsCurrentDeviceGamepad)
        {
            angle = GetToolSelectionAngleFromController();
        }
        else
        {
            angle = GetToolSelectionAngleFromMouse();
        }

        if (angle.HasValue)
        {
            float angleValue = angle.Value;
            SetButtonIndexFromAngle(angleValue);
            HighlightButton();
        }
    }

    private ThirdPersonController GetLocalPlayerThirdPersonController()
    {
        if (_localPlayerThirdPersonController == null)
        {
            _localPlayerThirdPersonController = GameManager.Instance.GetLocalPlayer().GetComponent<ThirdPersonController>();
        }

        return _localPlayerThirdPersonController;
    }

    private StarterAssetsInputs GetLocalPlayerStarterAssetsInputs()
    {
        if (_localPlayerStarterAssetsInputs == null)
        {
            _localPlayerStarterAssetsInputs = GetLocalPlayerThirdPersonController().GetComponent<StarterAssetsInputs>();
        }

        return _localPlayerStarterAssetsInputs;
    }

    private float? GetToolSelectionAngleFromMouse()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosition = GetLocalPlayerStarterAssetsInputs().CurrentToolSelectionInput.Selection;

        if (mousePosition == Vector2.zero) return null;  // To skip the invalid frame where mousePosition is (0,0,0) while the Cursor.lockState is changing to None

        Vector2 direction = mousePosition - screenCenter;

        // The player must have moved a bit the mouse away from the center of the screen to select a tool
        if (direction.magnitude < 40) return null;

        float angle = -(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        // Adjust the angle so the angle on left and right of the hand are 0 and 45
        float halfSegmentAngle = (360f / _toolsWheelButtons.Length) / 2;
        angle += 90f + halfSegmentAngle;

        // Adjust the angle to be between 0 and 360
        if (angle < 0) angle += 360f;

        return angle;
    }

    private float? GetToolSelectionAngleFromController()
    {
        Vector2 stickInput = GetLocalPlayerStarterAssetsInputs().CurrentToolSelectionInput.Selection;

        // Ignore small inputs (dead zone)
        if (stickInput.sqrMagnitude < 0.1f) return null;

        float angle = -(Mathf.Atan2(stickInput.y, stickInput.x) * Mathf.Rad2Deg);

        // Adjust the angle so the angle on left and right of the hand are 0 and 45
        float halfSegmentAngle = (360f / _toolsWheelButtons.Length) / 2;
        angle += 90f + halfSegmentAngle;

        // Adjust the angle to be between 0 and 360
        if (angle < 0) angle += 360;

        return angle;
    }

    private void SetButtonIndexFromAngle(float angle)
    {
        float segmentAngle = 360f / _toolsWheelButtons.Length;
        _hoveredToolID = Mathf.FloorToInt(angle / segmentAngle);
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
