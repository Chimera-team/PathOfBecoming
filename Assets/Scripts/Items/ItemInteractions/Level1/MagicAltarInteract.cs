using AnimationUtils.RenderUtils;
using UnityEngine;
using Cinemachine;

public class MagicAltarInteract : PlaceForItem
{
    [SerializeField] CinemachineVirtualCamera rockCamera;
    [SerializeField] SpriteRenderer rock;

    void Show_Rock()
    {
        Engine.current.dialogueSystem.SetUI(false);
        rockCamera.Priority = 11;
        rock.gameObject.SetActive(true);
        rock.Unfade(2f, () =>
        {
            rockCamera.Priority = 1;
            Engine.current.dialogueSystem.SetUI(true);
        });
    }

    public void Hide_Rock()
    {
        Engine.current.dialogueSystem.SetUI(false);
        rockCamera.Priority = 11;
        rock.Fade(2f, () =>
        {
            rockCamera.Priority = 1;
            Engine.current.dialogueSystem.SetUI(true);
            rock.gameObject.SetActive(false);
        });
    }

    public override void On_Detach()
    {
        base.On_Detach();
        Show_Rock();
    }
}
