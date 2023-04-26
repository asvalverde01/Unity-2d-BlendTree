using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionalAnimationStateController : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;

    public float acceleration = 2.0f;
    public float decelaration = 2.0f;
    public float maximunWalkVelocity = 0.5f;
    public float maximunRunVelocity = 2.0f;

    int VelocityZHash;
    int VelocityXHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        VelocityZHash = Animator.StringToHash("Velocity Z");
        VelocityXHash = Animator.StringToHash("Velocity X");

    }

    void changeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxtVelocity)
    {
        //
    }


    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        float currentMaxtVelocity = runPressed ? maximunRunVelocity : maximunWalkVelocity;


        if (forwardPressed && velocityZ < currentMaxtVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (leftPressed && velocityX > -currentMaxtVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        if (rightPressed && velocityX < currentMaxtVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        if(!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * decelaration;

        }

        if (!forwardPressed && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }

        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * decelaration;
        }

        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * decelaration;
        }

        //Reset Velocity
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        //lock forward
        if (forwardPressed && runPressed && velocityZ > currentMaxtVelocity)
        {
            velocityZ = currentMaxtVelocity;
        }
        else if (forwardPressed && velocityZ > currentMaxtVelocity)
        {
            velocityZ -= Time.deltaTime * decelaration;
            if (velocityZ > currentMaxtVelocity && velocityZ < (currentMaxtVelocity + 0.05))
            {
                velocityZ = currentMaxtVelocity;
            }
        }
        else if (forwardPressed && velocityZ < currentMaxtVelocity && velocityZ > (currentMaxtVelocity - 0.05f))
        {
            velocityZ = currentMaxtVelocity;
        }

        //lock left
        if (leftPressed && runPressed && velocityX < -currentMaxtVelocity)
        {
            velocityX = -currentMaxtVelocity;
        }
        else if (leftPressed && velocityX < -currentMaxtVelocity)
        {
            velocityX += Time.deltaTime * decelaration;
            if (velocityX < -currentMaxtVelocity && velocityX > (-currentMaxtVelocity - 0.05))
            {
                velocityX = -currentMaxtVelocity;
            }
        }
        else if (leftPressed && velocityX > currentMaxtVelocity && velocityX < (-currentMaxtVelocity + 0.05f))
        {
            velocityX = -currentMaxtVelocity;
        }

        //lock right
        if (rightPressed && runPressed && velocityX > currentMaxtVelocity)
        {
            velocityX = currentMaxtVelocity;
        }
        else if (rightPressed && velocityX < currentMaxtVelocity)
        {
            velocityX += Time.deltaTime * decelaration;
            if (velocityX < currentMaxtVelocity && velocityX > (currentMaxtVelocity - 0.05))
            {
                velocityX = currentMaxtVelocity;
            }
        }
        else if (rightPressed && velocityX > -currentMaxtVelocity && velocityX < (currentMaxtVelocity + 0.05f))
        {
            velocityX = currentMaxtVelocity;
        }



        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);
    }
}
