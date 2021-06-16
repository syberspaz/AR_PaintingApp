using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarqueeTool : MonoBehaviour
{
    public RectTransform selectionBox;
    private Vector2 startPos;

    //Holds the information on the main image pannel the user interacts with
    [SerializeField]
    private GameObject mainImagePanel;

    private RectTransform mainPanelTransform;
    private RawImage mainPanelRawImage;

    private Texture2D modifiedTexture;

    //debug
    [SerializeField]
    private Text debugText;

    public void Start()
    {
        mainImagePanel.TryGetComponent<RectTransform>(out mainPanelTransform);
        mainImagePanel.TryGetComponent<RawImage>(out mainPanelRawImage);
    }


    public void Update()
    {
        Touch touch;
        touch = Input.GetTouch(0);


        //mouse/touch down
        if(touch.phase == TouchPhase.Began)//Input.GetMouseButtonDown(0))
        {
            startPos = touch.position;
        }
        //mouse/touch up
        if (touch.phase == TouchPhase.Ended)
        {
            ReleaseSelectionBox();
        }
        //mouse button held down
        if(touch.phase == TouchPhase.Moved)
        {
            UpdateSelectionBox(touch.position);
        }


    }

    private void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeSelf)
        {
            selectionBox.gameObject.SetActive(true);
        }

        float width = curMousePos.x - startPos.x;

        float height = curMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Vector2 newPos;
        newPos = startPos + new Vector2(width / 2, height / 2);

        selectionBox.anchoredPosition = newPos;
       

    }

    void ReleaseSelectionBox()
    {
        //when released visual dissapears
        selectionBox.gameObject.SetActive(false);

        Rect overlapArea;

        //Remove selected portion of image

        //Before we run a bunch of math, check to see if anything actually got selected
        if (Intersects(selectionBox.rect, mainPanelTransform.rect, out overlapArea))
        {
            debugText.text = overlapArea.ToString();
        }



            //need to find what pixels are overlapping from the selection box and the rawimage pannel
            /*
            //Selection Box
            Vector2 SelBoxBottomLeft;
            Vector2 SelBoxTopRight;

            //Image Box
            Vector2 ImgBoxBottomLeft;
            Vector2 ImgBoxTopRight;

            //Getting all corners by using size and position of the boxes
            SelBoxBottomLeft.x = (selectionBox.anchoredPosition.x - (selectionBox.sizeDelta.x / 2));
            SelBoxBottomLeft.y = (selectionBox.anchoredPosition.y - (selectionBox.sizeDelta.y / 2));

            SelBoxTopRight.x = (selectionBox.anchoredPosition.x + (selectionBox.sizeDelta.x / 2));
            SelBoxTopRight.y = (selectionBox.anchoredPosition.y + (selectionBox.sizeDelta.y / 2));

            ImgBoxBottomLeft.x = (mainPanelTransform.anchoredPosition.x - (mainPanelTransform.sizeDelta.x / 2));
            ImgBoxBottomLeft.y = (mainPanelTransform.anchoredPosition.y - (mainPanelTransform.sizeDelta.y / 2));

            ImgBoxTopRight.x = (mainPanelTransform.anchoredPosition.x + (mainPanelTransform.sizeDelta.x / 2));
            ImgBoxTopRight.y = (mainPanelTransform.anchoredPosition.y + (mainPanelTransform.sizeDelta.y / 2));
            */


        

     

    }
    public static bool Intersects(Rect r1, Rect r2, out Rect area)
    {
        area = new Rect();

        if (r2.Overlaps(r1))
        {
            float x1 = Mathf.Min(r1.xMax, r2.xMax);
            float x2 = Mathf.Max(r1.xMin, r2.xMin);
            float y1 = Mathf.Min(r1.yMax, r2.yMax);
            float y2 = Mathf.Max(r1.yMin, r2.yMin);
            area.x = Mathf.Min(x1, x2);
            area.y = Mathf.Min(y1, y2);
            area.width = Mathf.Max(0.0f, x1 - x2);
            area.height = Mathf.Max(0.0f, y1 - y2);

            return true;
        }

        return false;
    }

}
