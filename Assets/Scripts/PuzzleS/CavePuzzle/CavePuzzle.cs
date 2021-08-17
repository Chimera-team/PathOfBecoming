using UnityEngine;
using AnimationUtils.RenderUtils;

public class CavePuzzle : MonoBehaviour
{
    bool[] isPressed = new bool[5] { false, false, false, false, false };
    int[] whatShouldBePressed = new int[5] { 8, 0, 11, 6, 12 };
    int currnum = 0;
    public bool GameWin;
    bool canUWin = true;
    [SerializeField] PuzzleButtons[] puzzleButtons;
    private Color colorIfUnpressed = new Color(255f, 155f, 0f, 0f);
    [SerializeField] PuzzleController interactController;

    [SerializeField] SpriteRenderer solveHologram;
    const float timeToFade = 1f;
    private void Update()
    {
        if (isPressed[0] && isPressed[1] && isPressed[2] && isPressed[3] && isPressed[4]&& canUWin)
        {
            GameWin = true;
            Debug.Log("игра выиграна");
            Interface.current.inventory.Remove_From_Inventory(typeof(Notes), true);
            Solve_Animation();
        }   
    }
    public void OnPuzzleButtonClick(int num)
    {
        if (whatShouldBePressed[currnum] == num)
        {
            isPressed[currnum] = true;
            if(currnum<4)
            {
                currnum++;
            } 
        }
        else
        {
            canUWin = false;
        }
    }
    public void ClosePuzzle()
    {
        Engine.current.dialogueSystem.SetUI(true);
        for(int i = 0;i<5;i++)
        {
            isPressed[i] = false;
        }
        canUWin = true;
        currnum = 0;
        for(int i = 0;i<16;i++)
        {
            puzzleButtons[i].Sprite.color = colorIfUnpressed;
        }
        interactController.Handle_Puzzle_result(false);
        gameObject.SetActive(false);
    }

    public void Solve_Animation()
    {
        gameObject.SetActive(false);
        solveHologram.gameObject.SetActive(true);
        solveHologram.Unfade(timeToFade, () =>
        {
            solveHologram.Fade(timeToFade, () =>
            {
                solveHologram.gameObject.SetActive(false);
                interactController.Handle_Puzzle_result(true);
                Destroy(gameObject);
            });
        });
    }
}
