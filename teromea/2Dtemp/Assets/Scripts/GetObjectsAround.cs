using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObjectsAround : MonoBehaviour
{
    public List<GameObject> objectsAround;
    public float range = 1.0f;

    public LayerMask layerMask;
    private Rigidbody2D rig;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        StartCoroutine(GetObjAround(layerMask));
    }

    private IEnumerator GetObjAround(LayerMask layerMask)
    {
        while(true)
        {
            List<Collider2D> cols = new List<Collider2D>(Physics2D.OverlapCircleAll(rig.position, range, layerMask: layerMask));
            objectsAround.Clear();
            foreach(Collider2D col in cols)
            {
                objectsAround.Add(col.gameObject);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
