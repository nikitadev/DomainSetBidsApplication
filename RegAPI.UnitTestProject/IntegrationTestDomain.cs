using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegAPI.Library.Infostructures.Implements;
using RegAPI.Library.Models.Domain;
using RegAPI.Library.Models.Implements;

namespace RegAPI.UnitTestProject
{
    [TestClass]
    public class IntegrationTestDomain
    {
        private string username = "test";
        private string password = "test";

        [TestMethod]
        public async Task Set_Rereg_Bids_Success()
        {
            var domainProvider = new DomainProvider(new RequestManager());

            var contacts = new Contacts
            {
                Description = "Vschizh site",
                Person = "Svyatoslav V Ryurik",
                PersonLocalName = "Рюрик Святослав Владимирович",
                PassportContent = "22 44 668800, выдан по месту жилья 01.09.1984",
                BirthDate = new DateTime(1984, 9, 1),
                PersonAddress = "12345, г. Вщиж, ул. Княжеска, д.1, Рюрику Святославу Владимировичу, князю Вщижскому",
                Phone = "+7 495 1234567",
                Email = "test@test.ru",
                Country = "RU"
            };

            var inputData = new SetReregBidsInputData
            {
                Contacts = contacts,
                Domains = new Domain[]
                {
                    new Domain { Name = "reg.ru", Price = 255 },
                    new Domain { Name = "reg.ru", Price = 400 }
                },
                NSServer = new NSServer()
            };

            var result = await domainProvider.SetReregBidsAsync(username, password, inputData);

            Assert.IsNotNull(result);
        }
    }
}
