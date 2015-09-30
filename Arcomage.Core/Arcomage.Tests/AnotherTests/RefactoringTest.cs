using System.Collections.Generic;
using System.Linq;
using Arcomage.Core.AlternativeServers;
using Arcomage.Core.ArcomageService;
using Arcomage.Entity.Cards;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Arcomage.Tests.AnotherTests
{
    //�������� ��������� ����� ������������
    [TestFixture]
    class RefactoringTest
    {

        [SetUp]
        public void Init()
        {

        }


        /// <summary>
        /// ����: ��������� ����������������� ��������� �����
        /// ���������: ������ ���� ��������� ����� ���� � ����
        /// </summary>
        [Test]
        public void ReInitCardTest()
        {
            IArcoServer host = new ArcoSQLLiteServer(@"arcomageDB.db");
            string cardFromServer = host.GetRandomCard();
            Card result = JsonConvert.DeserializeObject<List<Card>>(cardFromServer).FirstOrDefault();

            Assert.IsNotNull(result.cardAttributes, "�� ������ ���� ������ ��������");
            Assert.IsNotNull(result.price, "�� ������ ���� ������ ����");

        }

      
    }
}
