using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIRotateRoom : MonoBehaviour {

    static public UIRotateRoom current;

    public GameObject[] buttons;

    private ComputerController currentComputer;

    void Awake()
    {
        current = this;
    }

    public void SetComputer( ComputerController cc )
    {

        currentComputer = cc;

        if ( currentComputer == null )
        {
            GetComponent<Animator>().SetBool("ShowScreen", false);
            return;
        }

        GetComponent<Animator>().SetBool("ShowScreen", true);

        for( int i = 0 ; i < buttons.Length ; i++ )
        {
            buttons[i].SetActive(i == cc.rotateTarget);
        }
    }

    public void RotatePiece()
    {
        currentComputer.GetComponent<RotateRoom>().Rotate();
    }

	
}
