using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attached to second char, the healer controlling how player input the character when active
public class HealerController : MonoBehaviour
{


    public Rigidbody2D rb; // physical entity


    public Vector2 movementDirection; // from input x,y axis based determination of direction
    public float msi; //from input ; strenght of input between zero and one


    // keyups on keys specified in the editor cause these to be true and trigger an action
    private bool _a;
    private bool _q;
    private bool _w;
    private bool _e;
    private bool _mouse0;
    private bool _mouse1;

    public GameObject attackingDirection; // object required to get a direction of attack vector, assigned in editor, child of character


    private void Start()
    {
        attackingDirection.transform.localPosition = new Vector2(0, -0.5f); // initial position of the object to make sure the character may attack before moving for the first time
    }

    // every frame
    void Update()
    {
        if (transform == Camera.main.GetComponent<camerafollow>().target)
        {
            ProcessInputs();
            Aim();
            Attack();
        }
    }


    void ProcessInputs()
    {
        //movement based on input strength and directional buttons
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementDirection.Normalize();
        msi = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f); // strenght has to be between zero and one, speed based on input strenght times speed of the status controller attached to character
        GetComponent<MovementController>().Move(movementDirection, msi);

        //pressing and releasing one of the keys results in the corresponding bool being true for the frame, triggering the related actions in the Attack method
        _a = Input.GetButtonUp("a");
        _q = Input.GetButtonUp("q");
        _w = Input.GetButtonUp("w");
        _e = Input.GetButtonUp("e");
        _mouse0 = Input.GetButtonUp("mouse0");
        _mouse1 = Input.GetButtonUp("mouse1");

    }

    //move player based on input and play movement animation


    // set attacking direction object's position in 0.5 distance, direction of movement to generate a direction vector
    void Aim()
    {
        if (movementDirection != Vector2.zero)
        {
            attackingDirection.transform.localPosition = movementDirection * 0.5f;
        }
    }




    void Attack()
    {
        // may be interchanged with other ability scripts that are attached to the character object
        if (_q)
        {
            this.GetComponent<HealWave>().Blast();
        }

        if (_e)
        {
            this.GetComponent<HealBolt>().Blast();
        }

        if (_a)
        {
            this.GetComponent<SelfHeal>().Heal();
        }
        if (_w)
        {
            this.GetComponent<Teleport>().Backjump();
        }

        if (_mouse0)
        {
            GetComponent<HealWave>().BlastMouse();
        }

        if (_mouse1)
        {
            GetComponent<HealBolt>().BlastMouse();
        }
    }

}
