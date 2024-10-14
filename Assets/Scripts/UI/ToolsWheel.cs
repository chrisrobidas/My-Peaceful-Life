using StarterAssets;
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
        if (_selectedToolID == selectedToolID) return;

        _selectedToolID = selectedToolID;

        ThirdPersonController localPlayerThirdPersonController = GameManager.Instance.GetLocalPlayer().GetComponent<ThirdPersonController>();
        localPlayerThirdPersonController.SwapHeldTool(selectedToolID);
    }
}
