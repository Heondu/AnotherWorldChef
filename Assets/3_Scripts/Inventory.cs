using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }
    private static Inventory instance;
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChanged;

    public List<Skill> skills = new List<Skill>();
    public int space = 16;
    public Skill[] shortcuts = new Skill[4];


    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < space; i++)
        {
            skills.Add(null);
        }
    }

    public bool Add(Skill skill)
    {
        for (int i = 0; i < space; i++)
        {
            if (skills[i] == null)
            {
                skills[i] = skill;
                if (onItemChanged != null)
                    onItemChanged.Invoke();
                return true;
            }
        }

        return false;
    }

    public void Remove(Skill skill)
    {
        for (int i = 0; i < space; i++)
        {
            if (skills[i] == skill)
            {
                skills[i] = null;
            }
        }

        if (onItemChanged != null)
            onItemChanged.Invoke();
    }

    public void ChangeSlot(InventorySlot from, InventorySlot to)
    {
        if (from.isShortcut)
        {
            if (to.isShortcut)
            {
                Skill temp = shortcuts[from.index];
                shortcuts[from.index] = to.skill;
                shortcuts[to.index] = temp;
            }
            else
            {
                Skill temp = shortcuts[from.index];
                shortcuts[from.index] = to.skill;
                skills[to.index] = temp;
            }
        }
        else if (to.isShortcut)
        {
            Skill temp = skills[from.index];
            skills[from.index] = to.skill;
            shortcuts[to.index] = temp;
        }
        else
        {
            Skill temp = skills[from.index];
            skills[from.index] = to.skill;
            skills[to.index] = temp;
        }

        if (onItemChanged != null)
            onItemChanged.Invoke();
    }
}
