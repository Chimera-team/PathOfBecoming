using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public static Puzzle current { get; private set; }
    bool solved = false;
    PuzzleController puzzleController;

    private void Awake()
    {
        puzzleController = transform.parent.GetComponent<PuzzleController>();
        current = this;
        Engine.current.playerController.Change_Controls<PuzzleHandler>();
    }

    public void Solve_Puzzle()
    {
        solved = true;
        Destroy(gameObject);
    }

    public void Fail_Puzzle()
    {
        solved = false;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        puzzleController.Handle_Puzzle_result(solved);
        Engine.current.playerController.Change_Controls<DefaultHandler>();
        current = null;
    }
}
