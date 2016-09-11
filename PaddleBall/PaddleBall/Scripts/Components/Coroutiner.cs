using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall{

    /// <summary>
    /// Provides the scheduled timing of events like the IEnumerators and Coroutines in Unity.
    /// The one difference you must remember is to call the Update function of this coroutiner 
    /// for every frame in your gameobject or other class's update function as well
    /// </summary>
    public class Coroutiner {
        public List<IEnumerator> coroutines = new List<IEnumerator>();
        public void StartCoroutine(IEnumerator coroutine) {
            coroutines.Add(coroutine);
        }
        public void StopAllCoroutines() {
            coroutines = new List<IEnumerator>();
        }

        public void Update() {
            for (int i = coroutines.Count - 1; i >= 0; i--) {
                if (!coroutines[i].MoveNext()) {
                    coroutines.RemoveAt(i);
                }
            }
        }
    }
}
