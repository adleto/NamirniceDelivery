using NamirniceDelivery.Data.Entities;
using Nexmo.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamirniceDelivery.Services.Additional
{
    public static class NexmoSend
    {
        private static readonly Client Client;
        static NexmoSend()
        {
            Client = new Client(creds: new Nexmo.Api.Request.Credentials
            {
                ApiKey = "93380ef5",
                ApiSecret = "rZPDmu1zQ0xsx9lJ"
            });
        }
        public static void SendSMS(string sms, string phoneNumber = "38762864343")
        {
            /*var results = */Client.SMS.Send(request: new SMS.SMSRequest
            {
                from = "NamirniceDelivery",
                to = phoneNumber,
                text = sms
            });
        }
        public static void ObavjestiRadnikaNovaNarudzba(AdministrativniRadnik radnik)
        {
            SendSMS("Imate novu narudžbu."/*, r.PhoneNumber*/);
        }
        public static void ObavjestiKupcaPrihvacenaNarudzba(Kupac k)
        {
            SendSMS("Vaša narudžba je procesuirana. Očekujte dostavu na vašu adresu."/*,k.PhoneNumber*/);
        }
        public static void PodsjetiKupce(List<Kupac> kupci)
        {
            foreach(var k in kupci)
            {
                SendSMS("Nove namirnice vas čekaju! Visit: https://p1873.app.fit.ba "/*,k.PhoneNumber*/);
            }
        }
    }
}
