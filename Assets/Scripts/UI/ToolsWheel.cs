using UnityEngine;

public class ToolsWheel : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private int _selectedToolID;

    public void OpenToolsWheel()
    {
        _animator.SetBool("ToolsWheelOpened", true);
    }

    public void CloseToolsWheel()
    {
        _animator.SetBool("ToolsWheelOpened", false);
    }

    public void UpdateSelectedToolID(int selectedToolID)
    {
        _selectedToolID = selectedToolID;

        // TODO: Play animations and sounds here...
        switch (_selectedToolID)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
        }
    }
}
