using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SearchDuplicatesDocument
{
    public class LookingForDuplucates
    {
        private static List<string> documentsType = new List<string>() { "P", "ID", "DL" };
        private static List<string> countries = new List<string>() { "ESP", "FRA", "POR", "AND", "MOR" };

        private static List<string> personDuplicate = new List<string>();
        private static List<string> documentDuplicate = new List<string>();

        public static string CheckingDocuments(string[] documents)
        {
            string result = string.Empty;

            try
            {
                AddData(documents);

                foreach (string document in documents)
                {
                    string[] data = document.Split(',');

                    if (data.Length == 9)
                    {
                        bool incorrect = IncorrectData(data);

                        if (!incorrect)
                            result += data[0] + ", ";
                    }
                }

                return result.Substring(0, result.Length - 2);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void AddData(string[] data)
        {
            List<KeyValuePair<int, string>> personDocument = new List<KeyValuePair<int, string>>();
            List<KeyValuePair<int, string>> dataDocument = new List<KeyValuePair<int, string>>();

            foreach (string document in data)
            {
                string[] person = document.Split(',');
                if (person.Length == 9)
                {
                    personDocument.Add(new KeyValuePair<int, string>(Convert.ToInt32(person[0]), person[3] + person[4] + person[6] + person[7]));

                    dataDocument.Add(new KeyValuePair<int, string>(Convert.ToInt32(person[0]), person[5] + person[2] + person[1]));
                }
            }

            var duplicatesPersons = personDocument.GroupBy(x => x.Value).ToList();

            foreach (var person in duplicatesPersons)
            {
                bool first = true;
                foreach (var p in person)
                {
                    if (!first)
                        personDuplicate.Add(p.Key.ToString());
                    else
                        first = false;

                }
            }

            var duplicatesDocuments = personDocument.GroupBy(x => x.Value).ToList();

            foreach (var document in duplicatesDocuments)
            {
                bool first = true;

                foreach (var p in document)
                {
                    if (!first)
                        documentDuplicate.Add(p.Key.ToString());
                    else
                        first = false;
                }
            }
        }

        private static bool IncorrectData(string[] data)
        {
            bool result = true;

            if (personDuplicate.Contains(data[0]))
                return false;

            if (documentDuplicate.Contains(data[0]))
                return false;

            result = data[0].All(char.IsNumber);

            result = data[5].All(char.IsNumber);

            result = data[6].Length == 3;

            if (result)
                result = documentsType.Contains(data[1].ToUpper());

            if (result)
                result = countries.Contains(data[2].ToUpper());

            if (result)
            {
                try
                {
                    string date = string.Concat("20", data[7]);
                    DateTime dateResult;
                    result = DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateResult);
                }
                catch
                {
                    result = false;
                }
            }

            if (result)
            {
                try
                {
                    string date = string.Concat("20", data[8]);
                    int year = 2019;
                    DateTime expired = DateTime.ParseExact(date, "yyyymmdd", CultureInfo.CurrentCulture);

                    if(year > expired.Year)
                        result = false;
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
