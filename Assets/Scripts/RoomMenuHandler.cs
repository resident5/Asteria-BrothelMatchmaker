using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMenuHandler : MonoBehaviour
{
    private Animator anim;
    private bool isMenuOpen = false;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenCloseMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("isOpen", isMenuOpen);
    }
}
