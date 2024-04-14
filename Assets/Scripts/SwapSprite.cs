using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwapSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject HoverPanel;
    /*
    // Start is called before the first frame update
    void Start()
    {
        // buttonImage = GetComponent<Image>();
    }
    */

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverPanel.SetActive(false);
    }
    
    /*
    public void ChangeSprite()
    {
        buttonImage.sprite = swappedSprite;
    }
    */
}
