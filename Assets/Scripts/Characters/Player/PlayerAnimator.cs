using UnityEngine;
using PlayerControls;

public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private float walk;
    private PlayerController PC;
    bool faceRight = true;

    const float _Eps = 0.001f; 

    AudioClip[] steps;
    AudioClip jump;

    void Start()
    {
        PC = transform.parent.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        steps = Resources.LoadAll<AudioClip>("Sounds/Effects/Footsteps/Forest");
        jump = Resources.Load<AudioClip>("Sounds/Effects/Footsteps/jump");
    }

    public void Step()
    {
        SoundRecorder.Play_Effect(steps[Random.Range(0, steps.Length)]);
    }

    public void Jump()
    {
        SoundRecorder.Play_Effect(jump);
    }

    void Update()
    {
        walk = ControlButtonsAxis.xAxisRaw;
        if(walk != 0 && PC.rb.velocity.sqrMagnitude > _Eps)
        {
            anim.SetBool("walk", true);
            if (faceRight == false && walk > 0)
                Flip();
            else if (faceRight == true && walk < 0)
                Flip();
        }
        else
            anim.SetBool("walk", false);

        if (!PC.isGround)
            anim.SetBool("jump", true);
        else
            anim.SetBool("jump", false);
    }

    public void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(Vector3.up * 180);
    }
}
