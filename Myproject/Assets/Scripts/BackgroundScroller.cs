using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BackgroundScroller : MonoBehaviour
{
    public RectTransform panel;  
    public GameObject iconPrefab;
    public float spacing = 200f; 
    public float speed = 50f;    
    public float diagonalOffset = 50f; 

    private List<RectTransform> icons = new List<RectTransform>();
    private float panelWidth;
    private float panelHeight;
    private int rows, cols;

    void Start()
    {
        panelWidth = panel.rect.width;
        panelHeight = panel.rect.height;

        cols = Mathf.CeilToInt(panelWidth / spacing) + 2; 
        rows = Mathf.CeilToInt(panelHeight / spacing) + 2;

        float startX = -cols / 2f * spacing; 
        float startY = -rows / 2f * spacing;  

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                RectTransform rt = Instantiate(iconPrefab, panel).GetComponent<RectTransform>();
                float posX = startX + x * spacing;
                float posY = startY + y * spacing + (x % 2) * diagonalOffset; 
                rt.anchoredPosition = new Vector2(posX, posY);
                icons.Add(rt);
            }
        }

    }

    void Update()
    {
        foreach (var icon in icons)
        {
            icon.anchoredPosition += Vector2.up * speed * Time.deltaTime;

            if (icon.anchoredPosition.y > panelHeight + spacing)
            {
                icon.anchoredPosition -= new Vector2(0, rows * spacing);
            }
        }
    }
}
