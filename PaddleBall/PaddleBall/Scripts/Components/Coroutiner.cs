using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall{
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
