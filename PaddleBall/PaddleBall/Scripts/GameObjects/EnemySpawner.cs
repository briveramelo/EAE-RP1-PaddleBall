using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PaddleBall
{
    public class Round {
        public int round;
        public int enemyCount;
        public float enemySpeed;
        public float timeBetweenSpawns;
        public Round(int round, int enemyCount, float speed, float timeBetweenSpawn)
        {
            this.round = round;
            this.enemyCount = enemyCount;
            this.enemySpeed = speed;
            this.timeBetweenSpawns = timeBetweenSpawn;
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
        List<Round> rounds = new List<Round>() {
            //round, enCount, enSpeed, tbwtSpawn 
            new Round(1, 2, 1f, 1),
            new Round(2, 3, 1.2f, 1f),
            new Round(3, 4, 1.4f, 1f),
            new Round(4, 4, 1.6f, .9f),
            new Round(5, 5, 1.8f, .9f),
            new Round(6, 5, 2f, .9f),
            new Round(7, 7, 2.3f, .85f),
            new Round(8, 10, 2.6f, .8f),
            new Round(9, 10, 3f, .7f)
        };
        int currentRound;
        int currentEnemyCount;

        public void LoadContent(ContentManager Content) {
            content = Content;
            myCoroutiner.StopAllCoroutines();
            currentRound = -1;
            currentEnemyCount = 0;
            myCoroutiner.StartCoroutine(StartNewRound());
        }

        IEnumerator StartNewRound() {
            currentRound++;
            if (currentRound >= rounds.Count) {
                currentRound = rounds.Count-1;
            }
            if (currentRound < rounds.Count) {
                int numEnemiesToSpawn = rounds[currentRound].enemyCount;
                List<Vector2> spawnSpots = GetProperlySpacedEnemySpawnAngles(numEnemiesToSpawn);

                for (int i = 0; i < numEnemiesToSpawn; i++) {
                    SpawnEnemy(spawnSpots[i]);

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    while (stopwatch.ElapsedMilliseconds < rounds[currentRound].timeBetweenSpawns * 1000) {
                        yield return null;
                    }
                    stopwatch.Stop();

                }
            }
        }

        Vector2 screenCenter = new Vector2(ScreenManager.Instance.Dimensions.X, ScreenManager.Instance.Dimensions.Y) / 2f;
        List<Vector2> GetProperlySpacedEnemySpawnAngles(int enemyCount) {
            List<int> spawnAngles = new List<int>();
            List<Vector2> spawnPoints = new List<Vector2>();
            List<Vector2> spawnPointsZero = new List<Vector2>();
            int closestAngle = 30;
            for (int i = 0; i < enemyCount; i++) {
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
            newEnemy.SetVelocity(screenCenter - newEnemy.position, rounds[currentRound].enemySpeed);
            GameObject.allGameObjects.Add(newEnemy);
            currentEnemyCount++;
        }

        public void ReportEnemyDown() {
            currentEnemyCount--;
            if (currentEnemyCount == 0) {
                myCoroutiner.StartCoroutine(StartNewRound());
            }
        }
        
        Random random = new Random();
    }
}
