using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FindUniqueWords
{
	class Program
	{
		async static void Main(string[] args)
		{
			string filePath = args[0];
			if (!File.Exists(filePath))
			{
				Console.WriteLine("File not found!");
				return;
			}

			Dictionary<string, int> uniqueWords = new Dictionary<string, int>();
			using (StreamReader reader = new StreamReader(filePath, Encoding.Default))
			{
				string line = null;
				while ((line = await reader.ReadLineAsync()) != null)
				{
					if (string.IsNullOrEmpty(line))
						continue;

					Regex pattern = new Regex("[^а-яА-Яa-zA-Z’]");
					foreach (string word in pattern.Split(line.Trim().ToLower()))
					{
						if (string.IsNullOrEmpty(word))
							continue;

						if (!uniqueWords.ContainsKey(word))
							uniqueWords.Add(word, 1);
						else
							uniqueWords[word]++;
					}
				}

			}

			string saveFilePath = Environment.CurrentDirectory + "/result.txt";
			FileStream saveFileStream = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write);
			using (StreamWriter writer = new StreamWriter(saveFileStream, Encoding.Default))
				foreach (var item in uniqueWords.OrderByDescending(x => x.Value))
					await writer.WriteAsync($"{item.Key}\t{item.Value}\n");
			saveFileStream.Close();

		}
	}
}
