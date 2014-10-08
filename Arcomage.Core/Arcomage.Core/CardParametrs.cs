using Arcomage.Core.Parametrs;
using Newtonsoft.Json;

namespace Arcomage.Core
{
    /// <summary>
    /// Основные параметры карты, что она может добавлять или убавлять себе или противнику
    /// </summary>
    public class CardParametrs 
    {
        public string name { get; set; }
        public int id { get; set; }

        //Атрибуты применяемые к характеристикам игрока
        public Buildings playerBuildings { get; set; }
        public SourcesOfResources playerSources { get; set; }
        public Resources playerResources { get; set; }

        //Атрибуты применяемые к характеристикам противника
        public Buildings enemyBuildings { get; set; }
        public SourcesOfResources enemySources { get; set; }
        public Resources enemyResources { get; set; }

        //Стоимость использования карты
        public Resources cardCost { get; set; }

        public CardParametrs()
        {
            playerBuildings = new Buildings();
            playerSources = new SourcesOfResources();
            playerResources = new Resources();

            enemyBuildings = new Buildings();
            enemySources = new SourcesOfResources();
            enemyResources = new Resources();

            cardCost = new Resources();

            //ArcomageService.ArcoServerClient host = new ArcomageService.ArcoServerClient();
            //var cardFromServer = host.GetRandomCard();

            //CardParametrs newParametrs = JsonConvert.DeserializeObject<CardParametrs>(cardFromServer);
            //name = newParametrs.name;
            //cardCost.Animals = newParametrs.cardCost.Animals;

            //todo: описать получение информации о картах с сервера
            /*Идея хранения следующая: есть БД данных с таблицей, в ней перечислены значения полей. 
             Делается запрос к БД и получается рандомный номер записи. Данное решение на время версии 0.1.0 */
        }
    }
}