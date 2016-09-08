using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PaddleBall
{

    public struct Range {
        public float min, max;
        public float diff { get { return Math.Abs(max - min); } }
        public Range(float min, float max) {
            this.min = min;
            this.max = max;
        }
    }
    public class Round {
        public int round;
        public int enemyCount;
        public float enemySpeed;
        public Range timeRangeBetweenSpawns;
        public Round(int round, int enemyCount, float speed, Range timeRangeBetweenSpawns)
        {
            this.round = round;
            this.enemyCount = enemyCount;
            this.enemySpeed = speed;
            this.timeRangeBetweenSpawns = timeRangeBetweenSpawns;
        }
    }

    public class EnemySpawner 
    {
        private static EnemySpawner instance;
        public static EnemySpawner Instance {
            get {
                if (instance == null) {
                    instance = new EnemySpawner();
                }
                return instance;
            }
        }
        Coroutiner myCoroutiner = new Coroutiner();
        ContentManager content;
        Random random = new Random();

        List<Round> rounds = new List<Round>() {
            //round, enCount, enSpeed, tbwtSpawn 
            new Round(1, 3, 1.8f,   new Range(0.8f,     1f)),
            new Round(2, 6, 2f,   new Range(1f,       1.2f)),
            new Round(3, 9, 2.2f,     new Range(.95f,     1.05f)),
            new Round(4, 12, 2.4f,   new Range(0.75f,    .95f)),
            new Round(5, 12, 2.4f,   new Range(0.8f,     1f)),
            new Round(6, 15, 2.6f,    new Range(0.6f,     .8f)),
            new Round(7, 17, 2.8f,   new Range(0.6f,     .8f)),
            new Round(8, 20, 3.0f,  new Range(0.6f,     .8f)),
            new Round(9, 25, 3.2f,    new Range(0.5f,     .7f)),
            new Round(10, 30, 3.4f,    new Range(0.45f,     .6f))
        };
        public int currentRound;
        int currentRoundIndex;
        int currentEnemyCount;

        public void LoadContent(ContentManager Content) {
            content = Content;
            myCoroutiner.StopAllCoroutines();
            currentRoundIndex = -1;
            currentRound = 0;
            currentEnemyCount = 0;
            myCoroutiner.StartCoroutine(StartNewRound());
        }

        public void DEBUG_ENTER_INTENSE_ROUND(int roundToEnter) {
            currentRound = currentRoundIndex = roundToEnter;
            

            for (int i = CircleCollider.allColliders[Layer.Enemy].Count - 1; i >= 0; i--) {
                CircleCollider.allColliders[Layer.Enemy][i].gameObject.Destroy();
            }

            //AudioManager.Instance.PlaySound(SoundFX.SERIOUSMODE);
            myCoroutiner.StopAllCoroutines();
            myCoroutiner.StartCoroutine(StartNewRound());
        }

        IEnumerator StartNewRound() {
            currentRoundIndex++;
            currentRound++;
            if (currentRoundIndex >= rounds.Count) {
                currentRoundIndex = rounds.Count-1;
            }
            int numEnemiesToSpawn = rounds[currentRoundIndex].enemyCount;
            List<Vector2> spawnSpots = GetProperlySpacedEnemySpawnAngles(numEnemiesToSpawn);

            for (int i = 0; i < numEnemiesToSpawn; i++) {
                SpawnEnemy(spawnSpots[i]);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                float timeToWait = random.Next(0, 100) * 0.01f * (rounds[currentRoundIndex].timeRangeBetweenSpawns.diff) + rounds[currentRoundIndex].timeRangeBetweenSpawns.min;
                while (stopwatch.ElapsedMilliseconds < timeToWait * 1000) {
                    yield return null;
                }
                stopwatch.Stop();

            }
        }

        Vector2 screenCenter = new Vector2(ScreenManager.Instance.Dimensions.X, ScreenManager.Instance.Dimensions.Y) / 2f;
        List<Vector2> GetProperlySpacedEnemySpawnAngles(int numEnemiesToSpawn) {
            List<int> spawnAngles = new List<int>();
            List<Vector2> spawnPoints = new List<Vector2>();
            int closestAngle = 30;
            for (int i = 0; i < numEnemiesToSpawn; i++) {
                if (i%6==0) {
                    spawnAngles = new List<int>();
                }
                int randomAngle = random.Next(0, 360);
                for (int j = 0; j < spawnAngles.Count; j++) {
                    int attemptNum = 0;
                    while ((Math.Abs(spawnAngles[j] - randomAngle) < closestAngle ||
                        Math.Abs(spawnAngles[j] - randomAngle) > 360 - closestAngle) && attemptNum<20) {
                        randomAngle = random.Next(0, 360);
                        attemptNum++;
                    }
                }
                spawnAngles.Add(randomAngle);
                float angleRadian = (float)randomAngle * ((float)Math.PI/180f);
                Vector2 spawnSpot = new Vector2((float)Math.Cos(angleRadian), (float)Math.Sin(angleRadian)) * 1200 + screenCenter;
                spawnPoints.Add(spawnSpot);
            }
            return spawnPoints;          
        }

        public void Update() {
            myCoroutiner.Update();
        }

        public void SpawnEnemy(Vector2 spawnPoint) {
            Enemy newEnemy = new Enemy();
            newEnemy.LoadContent(content);
            newEnemy.PostLoad();
            newEnemy.position = spawnPoint;
            newEnemy.SetVelocity(screenCenter - newEnemy.position, rounds[currentRoundIndex].enemySpeed);            
            currentEnemyCount++;
        }

        public void ReportEnemyDown() {
            currentEnemyCount--;
            if (currentEnemyCount == 0) {
                myCoroutiner.StartCoroutine(StartNewRound());
            }
        }
    }
}
