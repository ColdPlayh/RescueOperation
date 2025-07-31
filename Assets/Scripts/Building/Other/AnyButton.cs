using System;
using UnityEngine;
using UnityEngine.UI;


public class AnyButton : MonoBehaviour
{
    private Button button;
    private Text buttonText;
    
    private Transform buildTrans;
    private BuildingCanvas buildCanvas;
    
    private LevelUpCanvas levelUpCanvas;
    
    
    
    [HideInInspector] public GameObject CanvasPrefab;
    

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText=transform.GetChild(0).GetComponent<Text>();
        
        button.onClick.AddListener(show);
    }

    public void setTextAndPrefab(string input,GameObject prefab,Transform trans)
    {
        buttonText.text = input;
        CanvasPrefab = prefab;
        buildTrans = trans;
    }

    private void show()
    {
        if (CanvasPrefab.TryGetComponent(out buildCanvas))
        {
            buildCanvas=Instantiate(CanvasPrefab).GetComponent<BuildingCanvas>();
            buildCanvas.setTrans(buildTrans);
        }
        else if(CanvasPrefab.TryGetComponent(out levelUpCanvas))
        {
            levelUpCanvas = Instantiate(CanvasPrefab).GetComponent<LevelUpCanvas>();
            levelUpCanvas.Init(buildTrans.GetComponent<DefenceBuildingState>());
        }
        Destroy(gameObject);
    }
    
}
