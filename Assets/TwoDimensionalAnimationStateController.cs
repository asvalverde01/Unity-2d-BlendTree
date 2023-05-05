using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionalAnimationStateController : MonoBehaviour
{
    Animator animator;
    CharacterController characterController;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;

    public float acceleration = 4.0f; // Aceleración del personaje
    public float deceleration = 4.0f; // Deceleración del personaje
    public float maximumWalkVelocity = 2f; // Velocidad máxima de caminata
    public float maximumRunVelocity = 8.0f; // Velocidad máxima de carrera
    public float rotationSpeed = 360.0f; // Velocidad de rotación del personaje
    public float jumpForce = 8.0f; // Fuerza del salto
    public float gravity = 30.0f; // Gravedad

    int VelocityZHash;
    int VelocityXHash;
    int IsFallingHash;

    Vector3 moveDirection = Vector3.zero;

    // Start se llama antes del primer frame
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        VelocityZHash = Animator.StringToHash("Velocity Z");
        VelocityXHash = Animator.StringToHash("Velocity X");
        IsFallingHash = Animator.StringToHash("isFalling");
    }

    // Update se llama una vez por frame
    void Update()
    {
        HandleMovement(); // Maneja el movimiento del personaje
        HandleJump(); // Maneja el salto del personaje
        HandleGravity(); // Maneja la gravedad del personaje
        HandleRotation(); // Maneja la rotación del personaje
        CheckGrounded(); // Verifica si el personaje está tocando el suelo
    }

    void HandleMovement()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W); // Verifica si se está presionando la tecla W
        bool leftPressed = Input.GetKey(KeyCode.A); // Verifica si se está presionando la tecla A
        bool rightPressed = Input.GetKey(KeyCode.D); // Verifica si se está presionando la tecla D
        bool runPressed = Input.GetKey(KeyCode.LeftShift); // Verifica si se está presionando la tecla Shift izquierdo

        float currentMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity; // Establece la velocidad máxima actual del personaje

        if (characterController.isGrounded) // Verifica si el personaje está tocando el suelo
        {
            moveDirection = new Vector3(velocityX, 0, velocityZ); // Establece la dirección del movimiento
            moveDirection = transform.TransformDirection(moveDirection); // Transforma la dirección del movimiento en relación a la rotación del personaje
        }

        if (forwardPressed && velocityZ < currentMaxVelocity) // Verifica si se está presionando la tecla W y si la velocidad actual en Z es menor que la velocidad máxima actual
        {
            velocityZ += Time.deltaTime * acceleration; // Aumenta la velocidad en Z
        }

        if (leftPressed && velocityX > -currentMaxVelocity) // Verifica si se está presionando la tecla A y si la velocidad actual en X es mayor que la velocidad máxima actual negativa
        {
            velocityX -= Time.deltaTime * acceleration; // Disminuye la velocidad en X
        }

        if (rightPressed && velocityX < currentMaxVelocity) // Verifica si se está presionando la tecla D y si la velocidad actual en X es menor que la velocidad máxima actual
        {
            velocityX += Time.deltaTime * acceleration; // Aumenta la velocidad en X
        }

        if (!forwardPressed && velocityZ > 0.0f) // Verifica si no se está presionando la tecla W y si la velocidad actual en Z es mayor que cero
        {
            velocityZ -= Time.deltaTime * deceleration; // Disminuye la velocidad en Z
        }

        if (!forwardPressed && velocityZ < 0.0f) // Verifica si no se está presionando la tecla W y si la velocidad actual en Z es menor que cero
        {
            velocityZ = 0.0f; // Establece la velocidad en Z en cero
        }

        if (!leftPressed && velocityX < 0.0f) // Verifica si no se está presionando la tecla A y si la velocidad actual en X es menor que cero
        {
            velocityX += Time.deltaTime * deceleration; // Aumenta la velocidad en X
        }

        if (!rightPressed && velocityX > 0.0f) // Verifica si no se está presionando la tecla D y si la velocidad actual en X es mayor que cero
        {
            velocityX -= Time.deltaTime * deceleration; // Disminuye la velocidad en X
        }

        //Reset Velocity
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f)) // Verifica si no se está presionando la tecla A ni la tecla D, si la velocidad actual en X no es cero y si la velocidad actual en X está entre -0.05 y 0.05
        {
            velocityX = 0.0f; // Establece la velocidad en X en cero
        }

        //lock forward
        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity) // Verifica si se está presionando la tecla W, la tecla Shift izquierdo y si la velocidad actual en Z es mayor que la velocidad máxima actual
        {
            velocityZ = currentMaxVelocity; // Establece la velocidad en Z en la velocidad máxima actual
        }
        else if (forwardPressed && velocityZ > currentMaxVelocity) // Verifica si se está presionando la tecla W y si la velocidad actual en Z es mayor que la velocidad máxima actual
        {
            velocityZ -= Time.deltaTime * deceleration; // Disminuye la velocidad en Z
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05)) // Verifica si la velocidad en Z es mayor que la velocidad máxima actual y si la velocidad en Z está entre la velocidad máxima actual y la velocidad máxima actual más 0.05
            {
                velocityZ = currentMaxVelocity; // Establece la velocidad en Z en la velocidad máxima actual
            }
        }
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f)) // Verifica si se está presionando la tecla W, si la velocidad actual en Z es menor que la velocidad máxima actual y si la velocidad actual en Z está entre la velocidad máxima actual menos 0.05 y la velocidad máxima actual
        {
            velocityZ = currentMaxVelocity; // Establece la velocidad en Z en la velocidad máxima actual
        }

        //lock left
        if (leftPressed && runPressed && velocityX < -currentMaxVelocity) // Verifica si se está presionando la tecla A, la tecla Shift izquierdo y si la velocidad actual en X es menor que la velocidad máxima actual negativa
        {
            velocityX = -currentMaxVelocity; // Establece la velocidad en X en la velocidad máxima actual negativa
        }
        else if (leftPressed && velocityX < -currentMaxVelocity) // Verifica si se está presionando la tecla A y si la velocidad actual en X es menor que la velocidad máxima actual negativa
        {
            velocityX += Time.deltaTime * deceleration; // Aumenta la velocidad en X
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05)) // Verifica si la velocidad en X es menor que la velocidad máxima actual negativa y si la velocidad en X está entre la velocidad máxima actual negativa y la velocidad máxima actual negativa menos 0.05
            {
                velocityX = -currentMaxVelocity; // Establece la velocidad en X en la velocidad máxima actual negativa
            }
        }
        else if (leftPressed && velocityX > currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f)) // Verifica si se está presionando la tecla A, si la velocidad actual en X es mayor que la velocidad máxima actual y si la velocidad actual en X está entre la velocidad máxima actual negativa más 0.05 y la velocidad máxima actual
        {
            velocityX = -currentMaxVelocity; // Establece la velocidad en X en la velocidad máxima actual negativa
        }

        //lock right
        if (rightPressed && runPressed && velocityX > currentMaxVelocity) // Verifica si se está presionando la tecla D, la tecla Shift izquierdo y si la velocidad actual en X es mayor que la velocidad máxima actual
        {
            velocityX = currentMaxVelocity; // Establece la velocidad en X en la velocidad máxima actual
        }
        else if (rightPressed && velocityX < currentMaxVelocity) // Verifica si se está presionando la tecla D y si la velocidad actual en X es menor que la velocidad máxima actual
        {
            velocityX += Time.deltaTime * deceleration; // Aumenta la velocidad en X
            if (velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05)) // Verifica si la velocidad en X es menor que la velocidad máxima actual y si la velocidad en X está entre la velocidad máxima actual menos 0.05 y la velocidad máxima actual
            {
                velocityX = currentMaxVelocity; // Establece la velocidad en X en la velocidad máxima actual
            }
        }
        else if (rightPressed && velocityX > -currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05f)) // Verifica si se está presionando la tecla D, si la velocidad actual en X es mayor que la velocidad máxima actual negativa y si la velocidad actual en X está entre la velocidad máxima actual y la velocidad máxima actual más 0.05
        {
            velocityX = currentMaxVelocity; // Establece la velocidad en X en la velocidad máxima actual
        }

        animator.SetFloat(VelocityZHash, velocityZ); // Establece el valor de la velocidad en Z en el animator
        animator.SetFloat(VelocityXHash, velocityX); // Establece el valor de la velocidad en X en el animator
    }

    void HandleJump()
    {
        bool jumpPressed = Input.GetButtonDown("Jump"); // Verifica si se presionó el botón de salto

        if (characterController.isGrounded) // Verifica si el personaje está tocando el suelo
        {
            if (jumpPressed) // Verifica si se presionó el botón de salto
            {
                moveDirection.y = jumpForce; // Establece la fuerza del salto
                animator.SetTrigger("Jump"); // Activa la animación de salto
            }
        }
        else
        {
            animator.SetBool(IsFallingHash, true); // Establece la variable isFalling en true en el animator
        }
    }

    void HandleGravity()
    {
        if (characterController.isGrounded) // Verifica si el personaje está tocando el suelo
        {
            moveDirection.y = 0; // Establece la velocidad en Y en cero
            animator.SetBool(IsFallingHash, false); // Establece la variable isFalling en false en el animator
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime; // Aplica la gravedad al movimiento en Y
        }

        characterController.Move(moveDirection * Time.deltaTime); // Mueve el personaje
    }

    void HandleRotation()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Obtiene el valor del eje horizontal
        transform.Rotate(0, horizontalInput * rotationSpeed * Time.deltaTime, 0); // Rota el personaje en relación al eje horizontal
    }

    void CheckGrounded()
    {
        Debug.Log("Player is falling. " + animator.GetBool("isFalling")); // Imprime en la consola si el personaje está cayendo o no
        if (characterController.isGrounded) // Verifica si el personaje está tocando el suelo
        {
            Debug.Log("Player is touching the ground."); // Imprime en la consola que el personaje está tocando el suelo
        }
        else
        {
            Debug.Log("Player is in the air."); // Imprime en la consola que el personaje está en el aire
        }
    }
}
