using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawnChild : MonoBehaviour
{
    
    [SerializeField] RectTransform grid;
    [SerializeField] RectTransform child;
    [SerializeField] Color primaryColor;
    [SerializeField] Color secondaryColor;
    bool[,] occupied = new bool[3,3];
    List<GameObject> children = new List<GameObject>();

    void Start()
    {
        for(int i=0; i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                occupied[i, j] = false;
                GetCell(i, j).GetComponent<Image>().color = primaryColor;
            }
        }
    }

    public void PopulateGrid()
    {
        if(children.Count != 0)
        {
            foreach(GameObject c in children)
            {
                DestroyImmediate(c);
                Debug.Log(children.Count);
            }
            children.Clear();
            ResetGrid();
        }
        while(!GridFilled())
        {
            RectTransform newChild = Instantiate(child,grid);
            newChild.gameObject.SetActive(true);
            PlaceChild(newChild);
            children.Add(newChild.gameObject);
        }
    }

    void ResetGrid()
    {
        for(int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                occupied[i, j] = false;
                GetCell(i, j).GetComponent<Image>().color = primaryColor;
            }
        }
    }

    bool GridFilled()
    {
        for(int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                if (!occupied[i, j]) return false;
            }
        }
        return true;
    }
    Vector2 GetPosition()
    {
        for(int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                if (!occupied[i, j])
                {
                    return new Vector2(i, j);
                }
            }
        }
        return new Vector2();
    }
    Vector2 Enlarge(Vector2 pos)
    {
        bool cey = pos.x + 1 < 3 && !occupied[(int)(pos.x + 1), (int)pos.y];
        bool cex = pos.y + 1 < 3 && !occupied[(int)(pos.x), (int)(pos.y+1)];
        if (cey && Random.Range(0f,1f) >= 0.5)
        {
            return new Vector2(0, 1);
        }
        if(cex && Random.Range(0f,1f) >= 0.5)
        {
            return new Vector2(1, 1);
        }
        return new Vector2(0, 0);
    }
    void PlaceChild(RectTransform child)
    {
        Vector2 pos = GetPosition();
        Vector2 enl = Enlarge(pos);
        child.sizeDelta = new Vector2(70, 70 + 70 * enl.y);
        child.Rotate(Vector3.forward, 90 * enl.x);
        int delx = 35 + (int)enl.y * (int)enl.x * 35;
        int dely = 35 + (int)enl.y * (int)(1 - enl.x) * 35;
        child.transform.name = "(" + ((int)pos.x).ToString() + "," + ((int)pos.y).ToString() + ")";
        child.anchoredPosition = new Vector2(delx + 70 * (int)pos.y, -dely - 70 * (int)pos.x);
        UpdateGrid(pos, enl);
    }

    void UpdateGrid(Vector2 pos,Vector2 enl)
    {
        occupied[(int)pos.x, (int)pos.y] = true;
        if((int)enl.y == 1)
        {
            GetCell((int)pos.x, (int)pos.y).GetComponent<Image>().color = secondaryColor;
            if((int)enl.x == 1)
            {
                occupied[(int)(pos.x), (int)(pos.y + 1)] = true;
                GetCell((int)pos.x, (int)pos.y+1).GetComponent<Image>().color = secondaryColor;
            }
            else
            {
                occupied[(int)(pos.x + 1), (int)(pos.y)] = true;
                GetCell((int)pos.x + 1, (int)pos.y).GetComponent<Image>().color = secondaryColor;
            }
        }
    }

    GameObject GetCell(int x,int y)
    {
        return grid.GetChild(x).GetChild(y).gameObject;
    }
}
