using UnityEngine;
using System.Collections;

public class socialButtons : MonoBehaviour
{
    public enum buttonType
    {
        facebook = 0,
        twitter = 1,
        website = 2
    }

    public buttonType type;

    private bool onMouseOver = false;

    private void Update()
    {
        if (onMouseOver && Input.GetKeyUp(KeyCode.Mouse0))
        {
#if UNITY_WEBPLAYER
        if (type == buttonType.facebook)
            Application.ExternalEval("window.open('https://www.facebook.com/pages/Impossible-6/689811171069268?ref=hl','_blank')");
        else if (type == buttonType.twitter)
            Application.ExternalEval("window.open('https://twitter.com/impossible_6','_blank')");
        else if (type == buttonType.website)
            Application.ExternalEval("window.open('http://impossiblesix.net','_blank')");


#else
            if (type == buttonType.facebook)
                System.Diagnostics.Process.Start("https://www.facebook.com/pages/Impossible-6/689811171069268?ref=hl");
            else if (type == buttonType.twitter)
                System.Diagnostics.Process.Start("https://twitter.com/impossible_6");
            else if (type == buttonType.website)
                System.Diagnostics.Process.Start("http://impossiblesix.net");
#endif
        }
    }

    private void OnMouseEnter()
    {
        onMouseOver = true;
        renderer.material.color = Color.grey;
    }

    private void OnMouseExit()
    {
        onMouseOver = false;
        renderer.material.color = Color.white;
    }
}