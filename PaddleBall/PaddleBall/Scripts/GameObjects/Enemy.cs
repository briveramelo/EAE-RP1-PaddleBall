using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;


namespace PaddleBall
{
    class Enemy : GameObject
    {


        float radOfSheild = 100f;
        Vector2 velocity = Vector2.Zero;
        public int round_count= 1;
        EnemySpawner enemySpawner = new EnemySpawner();

        public Enemy() : base()
        { }

        private static Enemy instance;
        public static Enemy Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Enemy();
                }
                return instance;
            }
        }
        
        public override void LoadContent(ContentManager Content)
        {
            texturePath = "Images/Enemy_one";
            SetLayerDepth(1.00f);
            position = RandomNumberGenerator(1920, 1080);
            Vector2 pos = (screenCenter - position);
            pos.Normalize();
            SetVelocity(pos);
            enemySpawner.FillDataInEnemySpawner();

            base.LoadContent(Content);
        }


        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }
        
        public override void Update(GameTime gameTime)
        {
            
            Vector2 pos = (screenCenter - position);
            pos.Normalize();
            SetVelocity(pos);
            position += velocity;
            if (Vector2.Distance(position, screenCenter) < radOfSheild)
            {
                Destroy();
            }
            decideRound();
            // collision will destroy enemy
        }

        void decideRound()
        {

            int numEnemiesOnScreen = 0;
            for (int i = 0; i < GameObject.allGameObjects.Count; i++)
            {
                if (GameObject.allGameObjects[i].layerDepth == 1.00f)           // adding identifier to reco enemy
                    numEnemiesOnScreen++;
            }

            if (numEnemiesOnScreen == 0)
            {
                round_count = round_count + 1;
                int enemiestobespawned = enemySpawner.rounds[round_count].enemyCount;
                Debug.WriteLine(round_count);
                Debug.WriteLine(enemiestobespawned);
                for (int i = 0; i < enemiestobespawned; i++)
                {
                    EnemyCreate();
                }
            }
        }

        public void EnemyCreate()
        {
            Enemy newEnemy = new Enemy();
            newEnemy.LoadContent(content);
            newEnemy.PostLoad();
            GameObject.allGameObjects.Add(newEnemy);
        }


        Vector2 RandomNumberGenerator(int maxX, int maxY)
        {
            int x = 0, y = 0;
            int temp = GetRandomNumber(0, 4) % 4;
            switch (temp)
            {
                case 0:
                    x = GetRandomNumber(0, maxX);
                    y = -1;
                    break;

                case 1:
                    y = GetRandomNumber(0, maxY);
                    x = -1;
                    break;

                case 2:
                    x = GetRandomNumber(0, maxX);
                    y = maxY + 1;
                    break;
                case 3:
                    y = GetRandomNumber(0, maxY);
                    x = -1;
                    break;
            }
            return new Vector2(x, y);
        }
        private static readonly Random getrandom = new Random();
        public static int GetRandomNumber(int min, int max)
        {

            { // synchronize
                return getrandom.Next(min, max);
            }
        }


    }
}