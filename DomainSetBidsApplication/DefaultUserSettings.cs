using System;
using RegAPI.Library.Models.Domain;

namespace DomainSetBidsApplication
{
    public sealed class DefaultUserSettings
    {
        public const string UserName = "test";
        public const string Password = "test";

        public static Contacts GetContacts()
        {
            return new Contacts
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
        }
    }
}
