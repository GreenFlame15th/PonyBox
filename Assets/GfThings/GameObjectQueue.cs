using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectQueue
{
    private Queue<Queueable> gameObjects = new Queue<Queueable>();
    [SerializeField] private GameObject preFab;

    public GameObjectQueue(GameObject preFab)
    {
        this.preFab = preFab;
    }

    public Queueable GetGameObject()
    {
        if (gameObjects.TryDequeue(out Queueable toReturn))
        {
            toReturn.gameObject.SetActive(true);
            toReturn.GetComponent<Queueable>().DeQueue();
            return toReturn;
        }
        
        Queueable queueable = GameObject.Instantiate(preFab).GetComponent<Queueable>();
        queueable.SetQueue(this);
        queueable.GetComponent<Queueable>().DeQueue();
        return queueable;
    }

    public Queueable GetGameObject(Vector3 position)
    {
        Queueable queueable = GetGameObject();
        queueable.gameObject.transform.position = position;
        return queueable;
    }

    public Queueable GetGameObject(Vector3 position, Quaternion rotation, Transform parent)
    {
        Queueable queueable = GetGameObject();
        queueable.gameObject.transform.position = position;
        queueable.gameObject.transform.rotation = rotation;
        queueable.gameObject.transform.parent = parent;
        return queueable;
    }

    public void Enqueue(Queueable queueable) { gameObjects.Enqueue(queueable); }
}
