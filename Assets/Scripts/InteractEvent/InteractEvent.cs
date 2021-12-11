using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public abstract class InteractEvent : MonoBehaviour
{
    public UnityEvent onSuccess;
    public UnityEvent onFail;

    public static InteractEvent current { get; private set; }

    public virtual void Start_Event()
    {
        if (TryGetComponent(out Collider2D coll))
        {
            coll.enabled = false;
            onFail.AddListener(() => coll.enabled = true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        current = this;
        Engine.current.playerController.interactIndicator.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        current = null;
        Engine.current.playerController.interactIndicator.gameObject.SetActive(false);
    }
}
