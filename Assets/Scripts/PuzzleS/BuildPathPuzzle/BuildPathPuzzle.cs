using System.Collections.Generic;
using UnityEngine;
using PlayerControls;

public class BuildPathPuzzle : Puzzle
{
    [SerializeField] int segmentsAmount;

    [SerializeField] Transform originalStart;
    [SerializeField] Transform playerPathStart;
    [SerializeField] Transform segmentPoolStart;

    [SerializeField] GameObject segmentPrefab;

    [SerializeField] List<Sprite> pathPool;

    [Header("Timer")]
    [SerializeField] Timer timer;
    [SerializeField] float wrongChoicePunishment;

    const float poolOffset = 0.2f;

    PathSegment[] original;
    List<PathSegment> playerPath;
    List<PathSegment> segmentsPool;

    int selectedIndex = 0;
    int pathIndex = 0;

    bool initial = true;

    void Start()
    {
        original = new PathSegment[segmentsAmount];
        playerPath = new List<PathSegment>();
        segmentsPool = new List<PathSegment>(new PathSegment[segmentsAmount]);

        Generete_Pools();
        timer.Start_Timer(() => Destroy(gameObject));
    }

    void Update()
    {
        Select_Segment();
        if (ControlButtonsHold.USESPELL)
            Choose_Segment();
    }

    void Select_Segment()
    {
        if (ControlButtonsHold.LEFT)
        {
            if (selectedIndex == 0)
                selectedIndex = Mathf.Max(segmentsPool.Count - 1, 0);
            else
                --selectedIndex;
        }
        else if (ControlButtonsHold.RIGHT)
        {
            if (selectedIndex == Mathf.Max(segmentsPool.Count - 1, 0))
                selectedIndex = 0;
            else
                ++selectedIndex;
        }
        else
            return;
        segmentsPool[selectedIndex].Select();
        if (initial)
            initial = false;
    }

    void Choose_Segment()
    {
        if (initial)
            return;
        if (segmentsPool[selectedIndex].index == original[pathIndex].index)
        {
            Vector3 movePosition = playerPathStart.position;
            movePosition.y = playerPathStart.position.y - pathIndex * segmentsPool[selectedIndex].rangeY;

            playerPath.Add(segmentsPool[selectedIndex]);
            segmentsPool.RemoveAt(selectedIndex);

            pathIndex = playerPath.Count;
            playerPath[pathIndex - 1].Choose(movePosition, Win_Condition);

            if (segmentsPool.Count > 0)
            {
                selectedIndex = 0;
                segmentsPool[selectedIndex].Select();
            }
        }
        else
            timer.Remove_Time(wrongChoicePunishment);
    }

    void Win_Condition()
    {
        if (pathIndex >= segmentsAmount)
            Solve_Puzzle();
    }

    void Generete_Pools()
    {
        int index;
        Vector3 origSegment = originalStart.position;

        int[] pullSequence = Generate_Sequence(segmentsAmount);
        Vector3 poolSegment = segmentPoolStart.position;

        for (int i = 0; i < segmentsAmount; i++)
        {
            index = Random.Range(0, pathPool.Count);
            Sprite curSegment = pathPool[index];
            pathPool.RemoveAt(index);

            original[i] = Instantiate(segmentPrefab, transform).GetComponent<PathSegment>();
            segmentsPool[pullSequence[i]] = (Instantiate(segmentPrefab, transform).GetComponent<PathSegment>());

            origSegment.y = originalStart.position.y - (i * original[i].rangeY);
            poolSegment.x = segmentPoolStart.position.x + (pullSequence[i] * (original[i].rangeX + poolOffset));


            original[i].Initialise(i, curSegment, true, origSegment);
            segmentsPool[pullSequence[i]].Initialise(i, curSegment, false, poolSegment);
        }
    }

    int[] Generate_Sequence(int count)
    {
        List<int> list = new List<int>();
        int[] arr = new int[count];
        for (int i = 0; i < count; i++)
            list.Add(i);
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, list.Count);
            arr[i] = list[index];
            list.RemoveAt(index);
        }
        return arr;
    }
}
