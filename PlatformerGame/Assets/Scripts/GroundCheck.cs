using UnityEngine;

namespace Player
{
    public class GroundCheck : MonoBehaviour
    {
        private int groundContactCount = 0;
        public bool IsGrounded => groundContactCount > 0;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.CompareTag("Ground"))
            {
                groundContactCount++;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.CompareTag("Ground"))
            {
                groundContactCount--;
                if (groundContactCount < 0) groundContactCount = 0;
            }
        }
    }
}