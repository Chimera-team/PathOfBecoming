using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoorPuzzleLine
{
    
    public SpriteRenderer leftSr;
    public SpriteRenderer rightSr;
    public bool IsActive = true;
    
    public void DisableLine()
    {
        leftSr.enabled = false;
        rightSr.enabled = false;
        IsActive = false;
    }
    public void EnableLine()
    {
        leftSr.enabled = true;
        rightSr.enabled = true;
        IsActive = true;
    }

}


public class DoorPuzzle : Puzzle
{
    //[SerializeField] AudioSource source;
    //[SerializeField] AudioClip sliderSound;
    [SerializeField] AudioClip loseSound;
    [SerializeField] AudioClip emptyClick;
    [SerializeField] AudioClip correctSound;
    [SerializeField] AudioClip bgMusic;
    [SerializeField] Begunok begunok;
    float topPoint;

    [SerializeField] private DoorPuzzleLine[] lines;
    [SerializeField] List<Sprite> sprites;

    [SerializeField] GameObject PuzzleTutor;
    private bool started = false;

    int correctAnswer;
    bool canPress = true;
    public bool FINISHED = false;
    private bool lastMove = false;
    private void OnEnable()
    {
        PuzzleTutor.SetActive(true);
        SoundRecorder.Play_Music(bgMusic);
        topPoint = begunok.GetMaxHeight();
        foreach (DoorPuzzleLine dpl in lines)
            dpl.IsActive = true;
        RandomizeBegunokImage();

    }
    private void OnDisable()
    {
        SoundRecorder.Play_Effect(loseSound);
        SoundRecorder.Play_Music(Resources.Load<AudioClip>("Sounds/Music/Level2"));
    }
    private void Update()
    {
        if(started)
        {
            int answered = 0;
            if (Input.GetKeyDown(KeyCode.Space) && canPress && !FINISHED)
            {
                Clicked();
                canPress = false;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !canPress)
            {
                SoundRecorder.Play_Effect(emptyClick);
            }
            foreach (DoorPuzzleLine dpl in lines)
            {
                if (dpl.IsActive)
                    answered++;
            }
            if (answered == 0)
                Solve_Puzzle();

            if (begunok.GetCurrentHeight() == topPoint)
            {
                Debug.Log(begunok.NumberTimesWent);
                //if(begunok.NumberTimesWent != 5-answered && !lastMove && begunok.AtTopPoint)
                //{
                //    OnLose();
                //}
                if (!lines[correctAnswer].IsActive)
                {
                    RandomizeBegunokImage();
                    canPress = true;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DisableTutor();
            }
        }
    }

    public void DisableTutor()
    {
        PuzzleTutor.SetActive(false);
        started = true;
    }
    private void OnLose()
    {
        Debug.Log("lose");
        Fail_Puzzle();
    }

    private void RandomizeBegunokImage()
    {
        while (true)
        {
            correctAnswer = Random.Range(0, 5);
            if (lines[correctAnswer].IsActive)
            {
                begunok.spriteRenderer.sprite = sprites[correctAnswer];
                break;
            }

        }
    }

    private void Clicked()
    {
        int whereClicked;
        float begunokPos = begunok.GetCurrentHeight();

        if (begunokPos < 3.5f && begunokPos > 2.5f)
        {
            whereClicked = 0;

        }
        else if (begunokPos < 2f && begunokPos > 1f)
        {
            whereClicked = 1;

        }
        else if (begunokPos < 0.5f && begunokPos > -0.5f)
        {
            whereClicked = 2;

        }
        else if (begunokPos < -1f && begunokPos > -2f)
        {
            whereClicked = 3;

        }
        else if (begunokPos < -2.5f && begunokPos > -3.5f)
        {
            whereClicked = 4;

        }
        else
        {
            whereClicked = -1;

        }
        if (whereClicked != -1)
        {
            if (!lastMove)
            {
                if (lines[whereClicked].IsActive && whereClicked == correctAnswer)
                {
                    //ColorCorrectBlocks();
                    lines[whereClicked].DisableLine();
                    //sound 
                    SoundRecorder.Play_Effect(correctSound);
                }
                else
                {
                    OnLose();

                }
            }
            else if (lines[whereClicked].IsActive && whereClicked == correctAnswer)
            {
                lines[whereClicked].DisableLine();
            }
            else
            {
                OnLose();

            }
        }
        else
        {
            OnLose();
        }
    }
}
