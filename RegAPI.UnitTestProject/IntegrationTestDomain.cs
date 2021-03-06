﻿using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegAPI.Library;
using RegAPI.Library.Models.Domain;

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
            var apiFactory = new ApiFactory();

            var contacts = new Contacts
            {
                Description = "Vschizh site",
                Person = "Svyatoslav V Ryurik",
                PersonLocalName = "Рюрик Святослав Владимирович",
                Passport = "22 44 668800, выдан по месту жилья 01.09.2004",
                BirthDate = new DateTime(1984, 9, 1),
                PersonAddress = "12345, г. Вщиж, ул. Княжеска, д.1, Рюрику Святославу Владимировичу, князю Вщижскому",
                Phone = "+7 495 1234567",
                Email = "1@mail.ru",
                Country = "RU"
            };

            var inputData = new SetReregBidsInputData
            {
                Contacts = contacts,
                Domains = new Domain[]
                {
                    new Domain { Name = "0147.ru.ru", Price = 999 },
                    new Domain { Name = "billy.su", Price = 499 }
                },
                NSServer = new NSServer()
            };

            var result = await apiFactory.Domain.SetReregBidsAsync(username, password, inputData);

            Assert.IsNotNull(result);
        }
    }
}
