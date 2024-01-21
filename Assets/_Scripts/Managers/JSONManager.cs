using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets.Resources.Entities;

namespace Assets._Scripts.Managers
{
    /// <summary>
    /// Manages I/O of scores using JSON serialization
    /// </summary>
    public class ScoreManager : StaticInstance<ScoreManager>
    {
        private string _filePath;

        void Awake()
        {
            _filePath = Application.persistentDataPath + "/HighScores/highscores.json";

            if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            }
        }

        /// <summary>
        /// Serializes given list of scores and saves them to the file
        /// </summary>
        /// <param name="scoresToSave">list of scores that you want to save</param>
        public void SaveScores(List<HighScore> scoresToSave)
        {
            string json = JsonUtility.ToJson(new SerializableList<HighScore>(scoresToSave), true);
            File.WriteAllText(_filePath, json);
        }

        /// <summary>
        /// Loading list of high scores
        /// </summary>
        /// <returns>list of high scores</returns>
        public List<HighScore> LoadScores()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                return JsonUtility.FromJson<SerializableList<HighScore>>(json).ToList();
            }
            else
                return new List<HighScore>();
        }

        protected override void OnApplicationQuit()
        {
            var scoresToSave = GameManager.Instance.GetHightScores();
            SaveScores(scoresToSave);
            base.OnApplicationQuit();
        }
    }

    [System.Serializable]
    public class SerializableList<T>
    {
        [SerializeField]
        private List<T> _items;

        public SerializableList(List<T> items)
        {
            _items = items;
        }

        public List<T> ToList()
        {
            return _items;
        }
    }
}
