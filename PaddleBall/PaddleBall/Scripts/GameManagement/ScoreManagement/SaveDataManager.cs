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
            if (File.Exists(content.RootDirectory + "/savefile.dat")) {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fileStream = File.Open(content.RootDirectory + "/savefile.dat", FileMode.Open);
                currentDataSave = new DataSave((DataSave)bf.Deserialize(fileStream));
                fileStream.Close();
            }
            else {
                currentDataSave = new DataSave();
            }
        }

        public static void Save(DataSave newSave) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = File.Create(content.RootDirectory + "/savefile.dat");
            currentDataSave = newSave;
            bf.Serialize(fileStream, newSave);
            fileStream.Close();
        }

    }

    [Serializable]
    public class DataSave {
        List<Score> highScores = new List<Score>() {
            new Score(1, 1500, "CDE", false),
            new Score(2, 900, "KNZ", false),
            new Score(3, 800, "MNL", false),
            new Score(4, 700, "HDK", false),
            new Score(5, 600, "AKS", false),
            new Score(6, 500, "BRM", false),
            new Score(7, 400, "BOB", false),
            new Score(8, 300, "ASH", false),
            new Score(9, 200, "RYN", false),
            new Score(10, 100, "JSE", false)
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

