using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Arcomage.Core.ArcomageService;
using Arcomage.Entity;
using Newtonsoft.Json;

namespace Arcomage.Core.Foo
{
    public class ArcoLocalServer: IArcoServer
    {
        public ArcoLocalServer(string connectionPath)
        {
            this.connectionPath = connectionPath;
        }

        public string connectionPath { get; private set; }

        public string GetRandomCard()
        {

            List<Card> returnVal = new List<Card>();
            using (SQLiteConnection connect = new SQLiteConnection(@"Data Source=" + connectionPath))
            {
              
                connect.Open();


                using (SQLiteCommand fmd = connect.CreateCommand())
                {
                    fmd.CommandText = @"select * from cards";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        var card = new Card();

                        card.id = Convert.ToInt32(r["id"]);
                        card.name = Convert.ToString(r["name"]);
                        card.description = Convert.ToString(r["description"]);

                        returnVal.Add(card);

                    }
                    r.Close();

                    foreach (var item in returnVal)
                    {
                        var cardParam = new List<CardParams>();

                        fmd.CommandText = @"select * from cardParams where card_id = " + item.id;
                        fmd.CommandType = CommandType.Text;
                        r = fmd.ExecuteReader();
                        while (r.Read())
                        {
                            cardParam.Add(new CardParams()
                            {
                                card = item,
                                id = Convert.ToInt32(r["id"]),
                                value = Convert.ToInt32(r["value"]),
                                key = GameControllerHelper.ConvertObjToEnum<Specifications>(r["key"])
                            });
                        }
                        r.Close();

                        item.cardParams = cardParam;
                    }

                }
            }










            return JsonConvert.SerializeObject(returnVal);

        }
    }
}
