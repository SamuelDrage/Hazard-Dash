using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelPanel : MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler, IScrollHandler
{
    public ScrollRect MainScroll;
    public GameObject defaultRating;
    public GameObject oneStarRating;
    public GameObject twoStarRating;
    public GameObject threeStarRating;
    
    public void SetupLevelPanel(float score, int rating){
        defaultRating.SetActive(false);
        oneStarRating.SetActive(false);
        twoStarRating.SetActive(false);
        threeStarRating.SetActive(false);

        if(rating == 0){
            defaultRating.SetActive(true);
        } else if(rating == 1){
            oneStarRating.SetActive(true);
        } else if(rating == 2){
            twoStarRating.SetActive(true);
        } else {
            threeStarRating.SetActive(true);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MainScroll.OnBeginDrag(eventData);
    }
 
 
    public void OnDrag(PointerEventData eventData)
    {
        MainScroll.OnDrag(eventData);
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        MainScroll.OnEndDrag(eventData);
    }
 
    public void OnScroll(PointerEventData data)
    {
        MainScroll.OnScroll(data);
    }
}
