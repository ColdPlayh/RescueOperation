using System;
using System.Collections;
using UnityEngine;

namespace Building
{
    public class DestoryEffect : MonoBehaviour
    {
        public float destoryTime=2f;
        private void Start()
        {
            Destroy(gameObject,destoryTime);
        }
    }
}