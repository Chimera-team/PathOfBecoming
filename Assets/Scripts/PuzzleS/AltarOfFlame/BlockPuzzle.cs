using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockPuzzle : Puzzle
{
    [SerializeField] private int manaCount;
    [HideInInspector] public int CurrentMana;
    [SerializeField] int razlomStrength;
    [SerializeField] private Transform pointer;
    [SerializeField] Vector2[] positionsForPointer;
    [SerializeField] Animator[] animsForRazloms;

    private List<Block> blocksThatCanBeSucked = new List<Block>();

    //sound
    [SerializeField] public AudioClip damageBlack;
    [SerializeField] public AudioClip blackKill;
    [SerializeField] AudioClip solvePuzzle;
    [SerializeField] public AudioClip suckMana;
    [SerializeField] AudioClip pressedSound;
    [SerializeField] AudioClip stolbFilled;
    [SerializeField] AudioClip  pressedInFullStolb;
    [SerializeField] AudioClip razlomManaGive;
    [SerializeField] AudioClip bgMusic;

    [SerializeField] GameObject PuzzleTutor;
    private bool started = false;
    private bool allowedClicks = true;
    public int currentStolb;
    Stolb[] stolbs;
    public bool GameEnded = false;
    
    private void OnEnable()
    {
        PuzzleTutor.SetActive(true);
        SoundRecorder.Play_Music(bgMusic);
        CurrentMana = manaCount;
        stolbs = GetComponentsInChildren<Stolb>();
        BlackBlock[] blackBlocks = FindObjectsOfType<BlackBlock>();
        foreach(BlackBlock block in blackBlocks)
        {
            foreach (Block blockingBlock in block.cancellingBlocks)
                blocksThatCanBeSucked.Add(blockingBlock);
        }
        //отключение игрока
    }

    private void OnDisable()
    {
        SoundRecorder.Play_Music(Resources.Load<AudioClip>("Sounds/Music/Level2"));    
    }

    public void RemoveBlocksThatCanBeSucked(Block[] blocks)
    {
        foreach (Block block in blocks)
            blocksThatCanBeSucked.Remove(block);
    }

    public void Restart()
    {
        CurrentMana = manaCount;
        foreach (Stolb stolb in stolbs)
        {
            stolb.ResetBlocks();
        }
    }

    private void Update()
    {
        if(started)
        {
            if (Input.GetKeyDown(KeyCode.A))
                if (currentStolb > 0)
                    currentStolb--;
            if (Input.GetKeyDown(KeyCode.D))
                if (currentStolb < 3)
                    currentStolb++;
            if (!stolbs[currentStolb].fullStolb)
            {
                for (int i = 0; i < stolbs[currentStolb].blocks.Length; i++)
                {
                    if (!stolbs[currentStolb].blocks[i].isFilled)
                    {
                        if (stolbs[currentStolb].blocks[i].HaveRazlom)
                        {
                            int filling = stolbs[currentStolb].blocks[i].StolbToFill;
                            for (int j = 0; j < stolbs[filling].blocks.Length; j++)
                            {
                                if (!stolbs[filling].blocks[j].isFilled)
                                {
                                    if (stolbs[filling].blocks[j].isBlackBlock)
                                    {
                                        CheckWichAnimOn(filling, 0);
                                    }
                                    else if (!blocksThatCanBeSucked.Contains(stolbs[filling].blocks[j]))
                                    {
                                        CheckWichAnimOn(filling, 1);
                                    }
                                    else
                                    {
                                        CheckWichAnimOn(filling, 2);
                                    }
                                    break;
                                }

                            }
                        }
                        break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && allowedClicks)
            {
                if (!stolbs[currentStolb].fullStolb)
                {
                    for (int i = 0; i < stolbs[currentStolb].blocks.Length; i++)
                    {
                        if (!stolbs[currentStolb].blocks[i].isFilled)
                        {
                            //sound
                            SoundRecorder.Play_Effect(pressedSound);

                            CurrentMana = stolbs[currentStolb].blocks[i].FillBlock(CurrentMana);
                            break;
                        }
                    }
                    if (stolbs[currentStolb].blocks[stolbs[currentStolb].blocks.Length - 1].isFilled)
                    {
                        stolbs[currentStolb].fullStolb = true;
                        //sound
                        SoundRecorder.Play_Effect(stolbFilled);
                    }
                }
                else
                {
                    //sound
                    SoundRecorder.Play_Effect(pressedInFullStolb);
                }
            }
            pointer.position = new Vector2(transform.position.x + positionsForPointer[currentStolb].x,
                transform.position.y + positionsForPointer[currentStolb].y);
            GameEnded = CheckEnd();
            if (GameEnded)
            {
                //sound
                SoundRecorder.Play_Effect(solvePuzzle);
                Solve_Puzzle();
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.E))
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

    /// <summary>
    /// type 0 = заливка в черный блок
    /// type 1 = заливка в обычный блок
    /// type 2 = заливка в высасываемый блок
    /// </summary>
    /// <param name="stolbToFill"></param>
    /// <param name="type"></param>
    private void CheckWichAnimOn(int stolbToFill, int type)
    {
        int index=0;
        if (stolbToFill == 3)
            index = 0;
        else if (stolbToFill == 0)
            index = 1;
        else if (stolbToFill == 1)
            index = 2;
        if (type == 0)
            animsForRazloms[index].SetTrigger("Orange");
        else if (type == 1)
            animsForRazloms[index].SetTrigger("Yellow");
        else if (type == 2)
            animsForRazloms[index].SetTrigger("Black");

    }
    private bool CheckEnd()
    {
        bool ended = true;

        foreach (Stolb stolb in stolbs)
        {
            if (!stolb.fullStolb)
            {
                ended = false;
            }
        }
        return ended;
    }

    public void DoRazlom(int stolbNum)
    {
        SoundRecorder.Play_Effect(razlomManaGive);
        stolbs[stolbNum].AddRazlomPower(razlomStrength);
    }

    public void AllowOrNoClicks(bool what)
    {
        allowedClicks = what;
        foreach (Stolb stolb in stolbs)
        {
            stolb.clickable = what;
        }
        if(what)
        {
            CheckStolbs();
        }

    }
    private void CheckStolbs()
    {
        if(!stolbs[1].blocks[stolbs[1].blocks.Length - 1].GetComponent<Block>().isFilled)
        {
            stolbs[1].fullStolb = false;
        }
    }
}
