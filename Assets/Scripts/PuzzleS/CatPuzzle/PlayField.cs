using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    [SerializeField] Puzzle controller;
    [SerializeField] int height;
    [SerializeField] int width;

    [SerializeField] GameObject emptyChip;
    [SerializeField] GameObject mouseChip;
    [SerializeField] GameObject catChip;
    [SerializeField] GameObject holeChip;

    [SerializeField] List<Sprite> mouseSprites;

    public Queue<QueueItem> queue = new Queue<QueueItem>();

    Chip[,] chips;
    Vector3[,] chipPositions;

    int winCondition;
    int miceInHole = 0;

    BoxCollider2D fieldFrame;

    void Start()
    {
        fieldFrame = GetComponent<BoxCollider2D>();
        fieldFrame.size = new Vector2(width, height);
        gameObject.transform.localScale = new Vector3(height, width, 1);

        Initialise_Field();
    }

    void FixedUpdate()
    {
        Check_Queue(queue);
    }

    void Check_Queue(Queue<QueueItem> queue)
    {
        while(queue.Count > 0)
            Field_Event(queue.Dequeue());
    }

    void Check_Win_Condition()
    {
        if (miceInHole == winCondition)
            controller.Solve_Puzzle();
    }

    void Move_Chip(QueueItem item, int[] targetIndex)
    {
        chips[item.index[0], item.index[1]].Move(chipPositions[targetIndex[0], targetIndex[1]], targetIndex);

        chips[targetIndex[0], targetIndex[1]] = chips[item.index[0], item.index[1]]; //swap
        chips[item.index[0], item.index[1]] = null;
    }

    void Field_Event(QueueItem item)
    {
        int[] targetIndex = Look_Adjacent_Places(item);
        if (targetIndex == null)
            return;

        if (item.searchObject == null)
            Move_Chip(item, targetIndex);
        else if (chips[item.index[0], item.index[1]].GetType() != typeof(HoleChip)) //mouse is caught
            Destroy(controller.gameObject);
        else //mouse adjacent to hole
        {
            miceInHole += 1;
            Transform_Chip<Chip>(targetIndex, emptyChip);
            Check_Win_Condition();
        }
    }
    int[] Look_Adjacent_Places(QueueItem item)
    {
        int[][] adjacent_places = new int[][]
        {
            new int[] { item.index[0], item.index[1] + 1},
            new int[] { item.index[0], item.index[1] - 1},
            new int[] { item.index[0] + 1, item.index[1]},
            new int[] { item.index[0] - 1, item.index[1]},
        };
        foreach(int[] place in adjacent_places)
        {
            try
            {
                if (chips[place[0], place[1]].GetType() == item.searchObject)
                    return place;
            }
            catch (NullReferenceException)
            {
                if (item.searchObject == null)
                    return place;
            }
            catch (IndexOutOfRangeException)
            {
                continue;
            }
        }
        return null;
    }

    void Transform_Chip<T>(int[] index, GameObject prefab) where T : Chip
    {
        GameObject clone;
        Destroy(chips[index[0], index[1]].gameObject);
        clone = Instantiate(prefab, chipPositions[index[0], index[1]], Quaternion.identity, transform.parent);
        clone.transform.localPosition = chipPositions[index[0], index[1]];
        T chip = clone.GetComponent<T>();
        chip.Initialise(this);
        chips[index[0], index[1]] = chip;
        chips[index[0], index[1]].Set_Index(index);
    }

    void Initialise_Field()
    {

        int[] center = new int[] { Mathf.CeilToInt(height / 2), Mathf.CeilToInt(width / 2) };
        int[] empty = new int[] { height - 1, width - 2 };
        List<int[]> mouseSpawn = new List<int[]>
        {
            new int[]{0, 0},
            new int[]{0, width-1},
            new int[]{height-1, 0},
            new int[]{height-1, width-1}
        };
        List<int[]> catSpawn = new List<int[]>
        {
            new int[]{center[0]-1, center[1]-1},
            new int[]{center[0]-1, center[1]+1},
            new int[]{center[0]+1, center[1]-1},
            new int[]{center[0]+1, center[1]+1},
        };

        winCondition = mouseSpawn.Count;

        chips = new Chip[height, width];
        chipPositions = new Vector3[height, width];

        Collider2D coll;
        GameObject clone;

        Chip chipScript;

        for (int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {

                //fill chips positions
                if (i == 0 && j == 0)
                    chipPositions[i, j] = new Vector3(-fieldFrame.bounds.extents.x, fieldFrame.bounds.extents.y, fieldFrame.bounds.extents.z);
                else if (j == 0 || (i == height - 1 && j == width - 1))
                {
                    coll = chips[i - 1, j].GetComponent<Collider2D>();
                    chipPositions[i, j] = coll.bounds.min;
                }
                else
                {
                    coll = chips[i, j - 1].GetComponent<Collider2D>();
                    chipPositions[i, j] = coll.bounds.max;
                    if (i == empty[0] && j == empty[1]) //no chip here
                    {
                        chipPositions[i, j].z = 0;
                        continue;
                    }
                }
                chipPositions[i, j].z = 0;

                //spawn chips
                if (mouseSpawn.Count > 0 && i == mouseSpawn[0][0] && j == mouseSpawn[0][1]) //MouseChip
                {
                    clone = Instantiate(mouseChip, chipPositions[i, j], Quaternion.identity, transform.parent);
                    clone.transform.localPosition = chipPositions[i, j];
                    MouseChip mouseChipScript = clone.GetComponent<MouseChip>();
                    mouseChipScript.Initialise(mouseSprites[0], this);
                    mouseSprites.RemoveAt(0);
                    chips[i, j] = mouseChipScript;
                    mouseSpawn.RemoveAt(0);
                }
                else if (catSpawn.Count > 0 && i == catSpawn[0][0] && j == catSpawn[0][1]) //CatChip
                {
                    clone = Instantiate(catChip, chipPositions[i, j], Quaternion.identity, transform.parent);
                    clone.transform.localPosition = chipPositions[i, j];
                    chipScript = clone.GetComponent<Chip>();
                    chipScript.Initialise(this);
                    chips[i, j] = chipScript;
                    catSpawn.RemoveAt(0);
                }
                else if (i == center[0] && j == center[1]) //HoleChip
                {
                    clone = Instantiate(holeChip, chipPositions[i, j], Quaternion.identity, transform.parent);
                    clone.transform.localPosition = chipPositions[i, j];
                    chipScript = clone.GetComponent<Chip>();
                    chipScript.Initialise(this);
                    chips[i, j] = chipScript;
                }
                else //Chip
                {
                    clone = Instantiate(emptyChip, chipPositions[i, j], Quaternion.identity, transform.parent);
                    clone.transform.localPosition = chipPositions[i, j];
                    chipScript = clone.GetComponent<Chip>();
                    chipScript.Initialise(this);
                    chips[i, j] = chipScript;
                }
                chips[i, j].Set_Index(new int[] { i, j });
            }
        }

        fieldFrame.enabled = false;
    }
}
