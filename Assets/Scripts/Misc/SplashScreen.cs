using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WordHoarder.Managers.Static.Generic;

namespace WordHoarder.Misc
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField]
        public float displayForSeconds = 3f;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(DisplaySplashScreen());
        }

        IEnumerator DisplaySplashScreen()
        {
            yield return new WaitForSeconds(displayForSeconds);
            LevelManager.LoadMainMenu();
        }
    }
}