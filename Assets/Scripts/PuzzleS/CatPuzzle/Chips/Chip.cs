using System.Collections;
using UnityEngine;

public class Chip : MonoBehaviour
{
    public float timeToMove;
    protected static bool chipMoving = false;
    protected int[] index;

    protected PlayField field;

    const float EPS = 0.001f;

    private void Awake()
    {
        chipMoving = false;
    }

    public void Initialise(PlayField field)
    {
        this.field = field;
    }

    public virtual void Move(Vector3 target, int[] index)
    {
        StartCoroutine(MoveTo(target, timeToMove));
        Set_Index(index);
    }

    public void Set_Index(int[] index)
    {
        this.index = index;
    }

    protected IEnumerator MoveTo(Vector3 target, float time)
    {
        chipMoving = true;
        float distance = Vector3.Distance(target, gameObject.transform.localPosition);
        float speed = distance / time;
        while ((target - gameObject.transform.localPosition).sqrMagnitude > EPS)
        {
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, target, speed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        chipMoving = false;
    }

    private void OnMouseDown()
    {
        if (!chipMoving && Input.GetMouseButtonDown(0))
            field.queue.Enqueue(new QueueItem(index, null));
    }
}
