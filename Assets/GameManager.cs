using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform[] plants;
    public float height;
    public Transform[] stuff;
    public GameObject[] Prefabs;
    public float time;

    private List<GameObject> objs = new List<GameObject>();
    private int Count;
    private bool start;
    private RaycastHit hitinfo;
    private RaycastHit hitinfo2;

    private bool IsFinish;
    public int maxHeight;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (IsFinish)
        {
            return;
        }

        int num = 0;
        /*for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i].GetComponent<Rigidbody>().)
            {

            }
        }*/

        if (Input.GetMouseButtonDown(0) && !start)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitinfo, 10000, 1 << LayerMask.NameToLayer("Water")))
            {
                MusicManager.Instance.Play(2);
                start = true;
            }
        }
        else if (Input.GetMouseButton(0) && start)
        {
            if (hitinfo.collider != null)
            {
                if (hitinfo.collider.TryGetComponent<BoxCollider>(out var collider))
                {
                    collider.enabled = false;
                }

                if (hitinfo.collider.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.freezeRotation = false;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitinfo2, 10000, 1 << LayerMask.NameToLayer("Plant")))
                {
                    float i = hitinfo.collider.GetComponent<Ctrlobj>().type == PrefabType.Ve ? 2 : 1;
                    hitinfo.transform.position = hitinfo2.transform.position +
                                                 Vector3.up * (checkHeight(hitinfo2.transform) + i);
                    hitinfo.transform.rotation = Quaternion.identity;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (start)
            {
                if (this.hitinfo2.collider != null)
                {
                    MusicManager.Instance.Play(3);
                    Count++;
                    hitinfo.transform.SetParent(hitinfo2.transform);
                    hitinfo.transform.rotation = Quaternion.identity;
                    hitinfo.transform.localPosition = Vector3.up * checkHeight(hitinfo2.transform);
                    CheckChild();
                    if (hitinfo.collider.TryGetComponent<BoxCollider>(out var collider))
                    {
                        collider.enabled = true;
                    }

                    if (hitinfo.collider.TryGetComponent<Rigidbody>(out var rb))
                    {
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }
                }
            }

            start = false;
        }


        int item = 0;
        for (int i = 0; i < plants.Length; i++)
        {
            if (plants[i].childCount > item)
            {
                item = plants[i].childCount;
            }
        }

        if (item > maxHeight)
        {
            maxHeight = item;
        }
        else if (maxHeight - item > 3)
        {
            GameOver();
            IsFinish = true;
        }
    }

    private void GameOver()
    {
        StopAllCoroutines();
        StartPanel.Instance.GameOver();
    }

    private void CheckChild()
    {
        for (int i = 0; i < plants.Length; i++)
        {
            if (plants[i].childCount == 0)
            {
                return;
            }
        }

        for (int i = plants.Length - 1; i >= 0; i--)
        {
            if (plants[i].childCount > 0)
            {
                Destroy(plants[i].GetChild(0).gameObject);
            }
        }

        for (int i = plants.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < plants[i].childCount; j++)
            {
                plants[i].GetChild(j).GetComponent<Ctrlobj>().Down();
            }
        }
    }

    private float checkHeight(Transform parent)
    {
        float height = 0;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).TryGetComponent<Ctrlobj>(out var asd))
            {
                switch (asd.type)
                {
                    case PrefabType.Cube:
                    case PrefabType.Hon:
                        height++;
                        break;
                    case PrefabType.Ve:
                        height += 2;
                        break;
                }
            }
        }

        return height;
    }

    public void RandomPrefab()
    {
        StartCoroutine(ReallyRandomPrefab());
    }

    IEnumerator ReallyRandomPrefab()
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            GameObject obj = GameObject.Instantiate(Prefabs[Random.Range(0, 3)]);
            obj.transform.position = stuff[Random.Range(0, stuff.Length)].position + Vector3.up * height;
            objs.Add(obj);
        }
    }
}