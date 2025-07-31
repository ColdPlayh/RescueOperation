using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Manager
{
    public class EnemyManager : SingleTon<EnemyManager>
    {
        
        [Header("Way Setting")]
        //第一个为开始的位置
        public List<WayPoint> leftWayPointsList;
        public List<WayPoint> rightWayPointsList;
        
        private Queue<WayPoint> mLeftWayPoints=new Queue<WayPoint>();
        private Queue<WayPoint> mRightWayPoints=new Queue<WayPoint>();

        [Header("Wave Setting")] public Transform profucePoint;
        public WaveText waveText;

        public Text waveNumberText;
        
        public int waveCount=6;

        public float waveCd;

        public float enemySpookCd;

        public float enemySpiderCd;

        public float enemyBatCd;

        public float enemyShadeCd;

        public float enemyShadowCd;

        public Level2EnemyControl spookPrefab;
        public Level2EnemyControl spiderPrefab;
        public Level2EnemyControl batPrefab;
        public Level2EnemyControl shadePrefab;
        public Level2EnemyControl shadowPrefab;

        public int[] spooks;

        public int[] spiders;

        public int[] bats;

        public int[] shades;

        public int[] shadows;

        [HideInInspector]
        public List<Level2EnemyControl> buffer=new List<Level2EnemyControl>();

        private int currWave;

        private float mWaveCd;

        private WaveText currText;

        private bool isEndWave=false;
        private void Update()
        {
            if (currWave == waveCount - 1)
                return;
            if (buffer.Count <= 0)
            {
                //等待时间
                if (mWaveCd > 0)
                {
                    if(!currText)
                        currText = Instantiate(waveText,UIManager.Instance.otherCanvas.transform);
                    if(!currText.isInit)
                        currText.SetTime(mWaveCd);
                    mWaveCd -= Time.deltaTime;
                }
                else
                {
                    Produce(currWave);
                    mWaveCd = waveCd;
                }
                
            }

            if (checkWin())
            {
                Debug.Log("Win");
                UIManager.Instance.Victory();
            }
        }

        public bool checkWin()
        {
            if (currWave == waveCount-1 && buffer.Count<=0 && isEndWave)
            {
                return true;
            }

            return false;
        }
        public void Produce(int wave)
        {
         
            waveNumberText.text = (currWave+1).ToString();
            currWave += 1;
            StartCoroutine(ProduceEnemy(spooks[wave], spookPrefab,enemySpookCd));
            StartCoroutine(ProduceEnemy(spiders[wave],spiderPrefab,enemySpiderCd));
            StartCoroutine(ProduceEnemy(bats[wave],batPrefab,enemyBatCd));
            StartCoroutine(ProduceEnemy(shades[wave],shadePrefab,enemyShadeCd));
            StartCoroutine(ProduceEnemy(shadows[wave], shadowPrefab, enemyShadowCd));

        }

        IEnumerator ProduceEnemy(int count, Level2EnemyControl enemy,float cd)
        {

            if (count == 0)
            {
                yield break;
            }
            
            for (int i = 0; i < count; i++)
            {
                
                buffer.Add(Instantiate(enemy, profucePoint.position,profucePoint.rotation));
                GameModel.Instance.CurrEnemy = buffer.Count;
                yield return new WaitForSeconds(cd);
            }

            if (currWave == waveCount - 1)
            {
                isEndWave = true;
            }
        }
        
        /// <summary>
        /// 获取敌人路径的方法
        /// </summary>
        /// <returns></returns>
        public Queue<WayPoint> GetWayPoints()
        {
            Queue<WayPoint> result = new Queue<WayPoint>();
            float dice = Random.Range(0f, 1f);
            if ( dice>= 0.5f)
            {
                if (leftWayPointsList != null && leftWayPointsList.Count>0)
                {
                    foreach (var way in leftWayPointsList)
                    {
                        result.Enqueue(way);
                    }
                    return result;
                }
                else
                {
                    Debug.Log("Left路径不存在");
                    return null;
                }
            }
            else
            {
                if (rightWayPointsList != null && rightWayPointsList.Count>0)
                {
                    foreach (var way in rightWayPointsList)
                    {
                        result.Enqueue(way);
                    }
                    return result;
                }
                else
                {
                    Debug.Log("Right路径不存在");
                    return null;
                }
            }
            
        }
        
        
        
        
    }
}