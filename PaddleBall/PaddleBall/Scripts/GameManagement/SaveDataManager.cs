using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework.Content;

namespace PaddleBall{
    public static class SaveDataManager {

        public static DataSave CopyCurrentDataSave() {
            return new DataSave(currentDataSave);
        }
        static DataSave currentDataSave;
        static ContentManager content;

        public static void LoadContent(ContentManager Content) {
            content = Content;
            if (File.Exists(content.RootDirectory + "/SaveData/savefile.dat")) {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fileStream = File.Open(content.RootDirectory + "/SaveData/savefile.dat", FileMode.Open);
                currentDataSave = new DataSave((DataSave)bf.Deserialize(fileStream));
                fileStream.Close();
            }
            else {
                currentDataSave = new DataSave();
            }
        }

        public static void Save(DataSave newSave) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = File.Create(content.RootDirectory + "/SaveData/savefile.dat");
            currentDataSave = newSave;
            bf.Serialize(fileStream, newSave);
            fileStream.Close();
        }

    }

    [Serializable]
    public class DataSave {
        List<Score> highScores = new List<Score>() {
            new Score(1, 1500, "CDE"),
            new Score(2, 900, "KNZ"),
            new Score(3, 800, "MNL"),
            new Score(4, 700, "HDK"),
            new Score(5, 600, "AKS"),
            new Score(6, 500, "BRM"),
            new Score(7, 400, "BOB"),
            new Score(8, 300, "ASH"),
            new Score(9, 200, "RYN"),
            new Score(10, 100, "JSE")
        };

        public List<Score> GetHighScores() {
            return highScores;
        }

        public DataSave(List<Score> highScores) {
            this.highScores = highScores;
        }
        public DataSave(DataSave dataSave) {
            this.highScores = dataSave.highScores;
        }
        public DataSave() { }
    }
}

