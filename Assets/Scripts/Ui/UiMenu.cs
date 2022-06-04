using System;
using System.Collections;
using UnityEngine;

namespace CockroachHunter.Ui
{
    public class UiMenu : MonoBehaviour
    {
        public virtual IEnumerator Open()
        {
            gameObject.SetActive(true);
            yield break;
        }
        
        public virtual IEnumerator Close()
        {
            gameObject.SetActive(false);
            yield break;
        }
    }
}