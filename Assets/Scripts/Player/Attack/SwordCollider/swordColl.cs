using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerAtt
{
    public class swordColl : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.GetComponent<Enemy>().DestroyEnemy();
        }
    }
}
