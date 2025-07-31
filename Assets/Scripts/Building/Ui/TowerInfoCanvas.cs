using System;
using UnityEngine;
using UnityEngine.UI;


public class TowerInfoCanvas : MonoBehaviour
{
     private Text currCoinText;
     private Text currScoreText;
     private Text currFallText;
     private Text maxFallText;
     private Text currTimeText;
     private Text currEnemyText;
     private Text currTowerText;
     private float timeSpend;
     private void Awake()
     {
          currCoinText = transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>();
          currScoreText = transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Text>();
          currFallText = transform.GetChild(1).GetChild(1).GetChild(2).GetComponent<Text>();
          maxFallText = transform.GetChild(1).GetChild(1).GetChild(3).GetComponent<Text>();
          currTimeText = transform.GetChild(1).GetChild(1).GetChild(4).GetComponent<Text>();
          currEnemyText = transform.GetChild(1).GetChild(1).GetChild(5).GetComponent<Text>();
          currTowerText = transform.GetChild(1).GetChild(1).GetChild(6).GetComponent<Text>();
          
         

     }

     private void Start()
     {
          GameModel.Instance.OnCurrCoinChanged += OnCurrCoinChanged;
          GameModel.Instance.OnCurrScoreChanged += OnCurrScoreChanged;
          GameModel.Instance.OnCurrFallChanged += OnCurrFallChanged;
          GameModel.Instance.OnMaxFallChanged += OnMaxFallChanged;
          GameModel.Instance.OnCurrTimeChanged += OnCurrTimeChanged;
          GameModel.Instance.OnCurrEnemyChanged += OnCurrEnemyChanged;
          GameModel.Instance.OnCurrTowerChanged += OnCurrTowerChanged;
          
          currCoinText.text = GameModel.Instance.CurrCoin.ToString();
          currScoreText.text = GameModel.Instance.CurrScore.ToString();
          currFallText.text = GameModel.Instance.CurrFall.ToString();
          maxFallText.text = "/"+GameModel.Instance.MAXFall;
          currTimeText.text = GameModel.Instance.CurrTime;
          currEnemyText.text = GameModel.Instance.CurrEnemy.ToString();
          currTowerText.text = GameModel.Instance.CurrTower.ToString();
     }

     private void Update()
     {
          timeSpend += Time.deltaTime;

          int hour = (int)timeSpend / 3600;
          int minute = ((int)timeSpend - hour * 3600) / 60;
          int second = (int)timeSpend - hour * 3600 - minute * 60;

          GameModel.Instance.CurrTime = string.Format("{0:D2}:{1:D2}", minute, second);
     }

     public void OnCurrCoinChanged(int currCoin)
     {
          currCoinText.text = currCoin.ToString();
     }
     public void OnCurrScoreChanged(int currScore)
     {
          currScoreText.text = currScore.ToString();
     }
     public void OnCurrFallChanged(int currFall)
     {
          currFallText.text = currFall.ToString();
          if (currFall == 0)
          {
               GameManager.Instance.GameOver();
          }
     }
     public void OnMaxFallChanged(int maxFall)
     {
          maxFallText.text ="/"+ maxFall;
     }
     public void OnCurrTimeChanged(string currTime)
     {
          currTimeText.text = currTime;
     }
     public void OnCurrEnemyChanged(int currEnemy)
     {
          currEnemyText.text = currEnemy.ToString();
     }
     public void OnCurrTowerChanged(int currTower)
     {
          currTowerText.text = currTower.ToString();
     }

     private void OnDisable()
     {
          if (GameModel.isInitialized())
          {
               GameModel.Instance.OnCurrCoinChanged -= OnCurrCoinChanged;
               GameModel.Instance.OnCurrScoreChanged -= OnCurrScoreChanged;
               GameModel.Instance.OnCurrFallChanged -= OnCurrFallChanged;
               GameModel.Instance.OnMaxFallChanged -= OnMaxFallChanged;
               GameModel.Instance.OnCurrTimeChanged -= OnCurrTimeChanged;
               GameModel.Instance.OnCurrEnemyChanged -= OnCurrEnemyChanged;
               GameModel.Instance.OnCurrTowerChanged -= OnCurrTowerChanged;
          }
              
     }
}
