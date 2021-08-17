using UnityEngine;

public class ExitPuzzle : MonoBehaviour
{
    [SerializeField] GameObject puzzle;

    private void Exit()
    {
        Destroy(puzzle);
    }

    private void OnMouseDown()
    {
        Exit();
    }
}
