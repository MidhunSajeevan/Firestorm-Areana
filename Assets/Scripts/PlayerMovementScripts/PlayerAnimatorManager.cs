
using UnityEngine;

	public class PlayerAnimatorManager : MonoBehaviour
	{
        public Animator animator;
        int horizontal;
        int vertical;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            horizontal = Animator.StringToHash("horizontal");
            vertical = Animator.StringToHash("vertical");
        }

        public void UpdateAnimatorValues(float horizontalInput, float verticalInput)
        {
            animator.SetFloat(horizontal,horizontalInput,0.1f,Time.deltaTime);
            animator.SetFloat(vertical,verticalInput,0.1f, Time.deltaTime); 
        }
    }
