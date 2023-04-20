using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queueable : MonoBehaviour
{
    private GameObjectQueue queue;
    public void SetQueue(GameObjectQueue queue) { this.queue = queue; }

    public void Fold()
    {
        queue.Enqueue(this);
        gameObject.SetActive(false);
    }

    public virtual void DeQueue() {; }
}
