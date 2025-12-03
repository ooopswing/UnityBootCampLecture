using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

       int maxHp = 100;
       int monsterAttack = 50;
       
       int hp = maxHp;
        hp -= monsterAttack;
        //   hp += monsterAttack;
        hp = maxHp;

        //    Debug.Log("hp" + " : " + 100);
        Debug.Log($"hp : {hp}");

     

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
