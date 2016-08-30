using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaddleBall
{
    public class Round {
        public int round;
        public int enemyCount;
        public int speed;
        public int timeBetweenSpawn;
        public Round(int round, int enemyCount, int speed, int timeBetweenSpawn)
        {
            this.round = round;
            this.enemyCount = enemyCount;
            this.speed = speed;
            this.timeBetweenSpawn = timeBetweenSpawn;
        }
    }

    public class EnemySpawner 
    {
        


        public List<Round> rounds = new List<Round>();

        void AddtoList(Round newRound)
        {
            rounds.Add(newRound);
        }
      /*  
       public int GetEnemyCount(int round)
        {
            foreach(EnemySpawner temp in rounds)
            {
                if (temp.round == round)
                    return temp.enemyCount;
            }
            return 0;
        }

        int GetSpeed(int round)
        {
            foreach (EnemySpawner temp in rounds)
            {
                if (temp.round == round)
                    return temp.speed;
            }
            return 0;
        }
        int GetTimeBetweenSpawn(int round)
        {
            foreach (EnemySpawner temp in rounds)
            {
                if (temp.round == round)
                    return temp.timeBetweenSpawn;
            }
            return 0;
        }
        */
        public void FillDataInEnemySpawner()
        {
            AddtoList(new Round(1,1,1,1));
            AddtoList(new Round(2,2,1,1));
            AddtoList(new Round(3,2,1,1));
            AddtoList(new Round(4,3,1,1));
            AddtoList(new Round(5,3,1,1));
            AddtoList(new Round(6,3,1,1));
            AddtoList(new Round(7,4,1,1));
            AddtoList(new Round(8,4,1,1));

        }



    }
}
