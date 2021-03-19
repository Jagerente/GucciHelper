//using System;
//using System.Collections.Generic;
//using DiscordBot.Configuration;
//using DiscordBot.Extensions;

//namespace DiscordBot.Modules.Quotes
//{
//    class QuotesModule : QuotesConfiguration
//    {
//        private static readonly Queue<QuotesModule> _allQuotes = new Queue<QuotesModule>();

//        public string Quote
//        {
//            get
//            {
//                return Quotes[new Random().Next(0, Quotes.Length - 1)];
//            }
//        }

//        public QuotesModule(string path)//Restore quotes information
//        {
//            var quotes = JsonStorage.RestoreObject<QuotesConfiguration>(path);

//            Author = quotes.Author;

//            Image = quotes.Image;

//            for (int i = 0; i < quotes.Keys.Length; i++)//Get all of the keywords in array
//            {
//                Keys[i] = quotes.Keys[i];
//            }

//            for (int i = 0; i < quotes.Quotes.Length; i++)//Get all of the quotes in array
//            {
//                Quotes[i] = quotes.Quotes[i];
//            }

//            _allQuotes.Enqueue(this);
//        }

//        public static QuotesModule Find(string author)
//        {
//            foreach (QuotesModule quote in _allQuotes)
//            {
//                foreach (string key in quote.Keys)//Searching for the keyword
//                {
//                    if (key.Equals(author))
//                    {
//                        return quote;
//                    }
//                }
//            }
//            return null;
//        }

//        public static Queue<QuotesModule> GetAll()
//        {
//            return _allQuotes;
//        }

//        public static QuotesModule GetRandom()
//        {
//            int random = new Random().Next(0, _allQuotes.Count - 1);
//            var quotes = _allQuotes.GetEnumerator();
//            int i = 0;
//            while (quotes.MoveNext())
//            {
//                if (i++ == random)
//                {
//                    return quotes.Current;
//                }
//            }
//            return _allQuotes.Peek();
//        }

//        //Files initialization for keeping them in memory
//        //public static void Init(string dir)
//        //{
//        //    _allQuotes.Clear();
//        //    string[] files = System.IO.Directory.GetFiles(dir);
//        //    foreach (string path in files)
//        //    {
//        //        new QuotesModule(path);
//        //    }
//        //}

//        //public string this[int id]
//        //{
//        //    get
//        //    {
//        //        return Quotes[id];
//        //    }
//        //}
//    }
//}
