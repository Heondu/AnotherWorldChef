using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public GameObject SkillPrefab;
    public float rotateSpeed = 10;
    public float interval = 0.1f;
    private float count, countAngle;

    private void Awake()
    {
        countAngle = Random.Range(0, 360);
    }
    private void Update()
    {
        countAngle += rotateSpeed * Time.deltaTime;
        count -= Time.deltaTime;
        if(count <= 0)
        {
            count = interval;
            GameObject go = Instantiate(SkillPrefab, transform.position, Quaternion.Euler(0, countAngle % 360, 0));
            Debug.Log(go.transform.eulerAngles);
            go.GetComponent<Skill>().Init(GetComponent<Skill>().damage, GetComponent<Skill>().eventInstigator);
        }
    }


}
