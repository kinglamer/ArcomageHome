using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Arcomage.Core.ArcomageService;
using Arcomage.Entity;
using Newtonsoft.Json;
using SQLite;
using Arcomage.Common;

namespace Arcomage.Core.AlternativeServers
{
    public class ArcoSQLLiteServer: IArcoServer
    {
        public ArcoSQLLiteServer(string connectionPath)
        {
            this.connectionPath = connectionPath;
        }

        public string connectionPath { get; private set; }

        public string GetRandomCard()
        {
            var returnVal = new List<Card>();
            using (var connect = new SQLiteConnection(connectionPath,  SQLiteOpenFlags.ReadWrite, false))
            {
            

              //  using (var fmd = connect.CreateCommand())
           //     {
                  //  fmd.CommandText = @"";
                 //   fmd.CommandType = CommandType.Text;
                    var r = connect.Query<Card>("select * from cards;");
                    foreach (var item in r)
                    {
                        var card = new Card();

                        card.id = item.id;
                        card.name = item.name;
                        card.description =item.description;
                 
                        returnVal.Add(card);
                    }
                

                    foreach (var item in returnVal)
                    {

                        var cardParam = new List<CardParams>();
                        var r2 = connect.Query<CardParams>(@"select * from cardParams where card_id = " + item.id + ";");
                        foreach (var item2 in r2)

                        {
                            cardParam.Add(new CardParams()
                            {
                                card = item,
                                id = item2.id,
                                value = item2.value,
                                key = item2.key
                            });
                        }

                        item.cardParams = cardParam;
                    }

              //  }
            }

            foreach (var item in returnVal)
            {
                item.Init();
            }

           returnVal.Randomize();







            return JsonConvert.SerializeObject(returnVal);

        }
    }
}
