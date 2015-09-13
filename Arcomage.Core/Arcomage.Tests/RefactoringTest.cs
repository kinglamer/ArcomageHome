using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core;
using Arcomage.Core.ArcomageService;
using Arcomage.Core.AlternativeServers;
using Arcomage.Entity;
using Arcomage.Tests.Moq;
using Arcomage.Tests.MoqStartParams;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Arcomage.Tests
{
    //Тестирую некоторые этапы рефакторинга
    [TestFixture]
    class RefactoringTest
    {

        [SetUp]
        public void Init()
        {

        }


        /// <summary>
        /// Цель: проверить переинициализацию некоторых полей
        /// Результат: должны быть заполнены новые поля у карт
        /// </summary>
        [Test]
        public void ReInitCardTest()
        {
            IArcoServer host = new ArcoSQLLiteServer(@"arcomageDB.db");
            string cardFromServer = host.GetRandomCard();
            Card result = JsonConvert.DeserializeObject<List<Card>>(cardFromServer).FirstOrDefault();

            Assert.IsNotNull(result.cardAttributes, "Не должно быть пустым атрибуты");
            Assert.IsNotNull(result.price, "Не должно быть пустым цена");

        }

      
    }
}
