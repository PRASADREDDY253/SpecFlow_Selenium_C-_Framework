
using Newtonsoft.Json.Linq;
using SampleProject.SupportFunctions;
using System;
using System.Collections.Generic;
using System.IO;

namespace BDD_Core.Utils
{
    public class JSONHelper
    {
        private Dictionary<string, JToken> fields;

        /// <summary>
        /// This will read data from Json file and return data based on the key provided
        /// </summary>
        /// <param name="key"></param>
        /// <param name="jsonFileName"></param>
        public dynamic ReadInputData(string key, string fileName)
        {
            dynamic data = null;
            try
            {
                var path = Path.Combine(@"Data\JsonFiles", @fileName);
                string jsonData = GetDataFromFile(path);
                data = JToken.Parse(jsonData);
                CheckKeyPresentInTestData(data, key, fileName);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            return fields[key];

        }


        /// <summary>
        /// Verify the key and value is present in json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="key"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public void CheckKeyPresentInTestData(dynamic data, string key, string fileName)
        {
            bool flag = false;
            Dictionary<string, JToken> jsonDictionary = JsonFieldsCollector(data);
            foreach (KeyValuePair<string, JToken> keyValue in jsonDictionary)
                foreach (string item in jsonDictionary.Keys)
                {
                    if (item.Equals(key))
                    {
                        flag = true;
                        CheckValueEmptyOrNull(jsonDictionary[key], key);
                        break;

                    }
                }
            if (!flag)
            {
                throw new Exception("The specified key " + key + " not present in " + fileName);
            }
        }

        public void CheckValueEmptyOrNull(JToken token, string key)
        {
            if ((token == null) ||
             (token.Type == JTokenType.Array && !token.HasValues) ||
             (token.Type == JTokenType.Object && !token.HasValues) ||
             (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
             (token.Type == JTokenType.Null))
            {
                throw new Exception("The value corresponding to the key '" + key + "' is Null or Empty");
            }
        }


        public Dictionary<string, JToken> JsonFieldsCollector(JToken token)
        {
            fields = new Dictionary<string, JToken>();
            CollectFields(token);
            return fields;
        }


        /// <summary>
        /// Generic Recurrsive Method to parse through token types Object,Array 
        /// </summary>
        /// <param name="jToken"></param>
        private void CollectFields(JToken jToken)
        {

            switch (jToken.Type)
            {
                case JTokenType.Object:
                    var d = jToken.Children<JProperty>();
                    foreach (JProperty p in d)
                    {
                        fields.Add(p.Path, p.Value);
                    }

                    //foreach (var child in jToken.Children<JProperty>())
                    //    fields.Add(child.ToString(), "");
                    break;
                case JTokenType.Array:
                    foreach (var child in jToken.Children())
                        CollectFields(child);
                    break;
                case JTokenType.Property:
                    CollectFields(((JProperty)jToken).Value);
                    break;
                default:
                    fields.Add(jToken.Path, (JValue)jToken);
                    break;
            }
        }

        /// <summary>
        /// This will read data from Json file and return data in the form of string
        /// </summary>
        /// <param name="pathOfJsonFile"></param>
        private string GetDataFromFile(string path)
        {

            string data = "";

            try
            {
                string filePath = Path.Combine(FileHandler.GetProjectDirectory(), path);
                using (Stream s = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    StreamReader sr = new StreamReader(s);
                    data = sr.ReadToEnd();
                }
            }
            catch (FileLoadException ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }


            return data;
        }

    }
}
