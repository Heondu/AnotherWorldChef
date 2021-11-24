using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int HP { get; set; }
    public int MaxHP = 100;
    public int Damage = 10;
    public List<Skill> skills = new List<Skill>();

    private void Awake()
    {
        HP = MaxHP;
    }
}
