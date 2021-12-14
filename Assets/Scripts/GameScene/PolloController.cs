using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolloController : MonoBehaviour
{
    public static PolloController Instance;

    [SerializeField] private Animator anim = null;
  
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAnimation(int correct, int wrong)
    {
        anim.SetInteger("correctNotes", correct);
        anim.SetInteger("wrongNotes", wrong);
    }

    public void SetActive(bool active)
    {
        anim.SetBool("gameActive", active);
    }

}
