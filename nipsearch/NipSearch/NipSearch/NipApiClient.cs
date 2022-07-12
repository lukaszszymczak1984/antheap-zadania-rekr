using NipSearch.Entities;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NipSearch
{
    public class NipApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        // rezultat testowy
        private string _responseTest = @"{
  ""result"" : {
    ""subject"" : {
      ""authorizedClerks"" : [ ],
      ""regon"" : ""regon"",
      ""restorationDate"" : ""2019-02-21"",
      ""workingAddress"" : ""ul/ Prosta 49 00-838 Warszawa"",
      ""hasVirtualAccounts"" : true,
      ""statusVat"" : ""Zwolniony"",
      ""krs"" : ""0000636771"",
      ""restorationBasis"" : ""Ustawa o podatku od towarów i usług art. 96"",
      ""accountNumbers"" : [ ""90249000050247256316596736"", ""90249000050247256316596736"" ],
      ""registrationDenialBasis"" : ""Ustawa o podatku od towarów i usług art. 96"",
      ""nip"" : ""1111111111"",
      ""removalDate"" : ""2019-02-21"",
      ""partners"" : [ ],
      ""name"" : ""ABC Jan Nowak"",
      ""registrationLegalDate"" : ""2018-02-21"",
      ""removalBasis"" : ""Ustawa o podatku od towarów i usług Art. 97"",
      ""pesel"" : ""22222222222"",
      ""representatives"" : [ {
        ""firstName"" : ""Jan"",
        ""lastName"" : ""Nowak"",
        ""nip"" : ""1111111111"",
        ""companyName"" : ""Nazwa firmy""
      }, {
        ""firstName"" : ""Jan"",
        ""lastName"" : ""Nowak"",
        ""nip"" : ""1111111111"",
        ""companyName"" : ""Nazwa firmy""
      } ],
      ""residenceAddress"" : ""ul/ Chmielna 85/87 00-805 Warszawa"",
      ""registrationDenialDate"" : ""2019-02-21""
    },
    ""requestDateTime"": ""19-11-2019 14:58:49"",
    ""requestId"" : ""d2n10-84df1a1""
  }
}";

        public NipApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Pobieranie danych z API
        /// </summary>
        /// <param name="nip"></param>
        /// <returns></returns>
        public async Task<ResponseEntity> Get(string nip)
        {
            // utworzenie klienta http
            var httpClient = _httpClientFactory.CreateClient("nipClient");

            // pobieranie danych
            var response = await httpClient.GetAsync(nip + "?date=" + DateTime.Now.ToString("yyyy-MM-dd"));

            // odczyt odpowiedzi
            var content = await response.Content.ReadAsStringAsync();

            // parsowanie odpowiedzi do Json
            JsonNode responseNode = JsonNode.Parse(content)!;

            // ignorowanie wielkości liter podczas deserializacji
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // sprawdzenie, czy przedsiębiorca został znaleziony
            if (response.IsSuccessStatusCode)
            {
                // znalezienie obiektu przedsiębiorcy
                JsonNode resultNode = responseNode?["result"]!;
                JsonNode subjectNode = resultNode?["subject"]!;

                // deserializacja do obiektu
                var subject = subjectNode?.Deserialize<Subject>(options);
                var result = new ResponseEntity() { Subject = subject };

                // utworzenie obiektów numerów kont z tablicy stringów
                result.Subject?.CreateAccounts();

                // zwrócenie danych
                return result;
            }
            else
            {
                // zwrócenie komunikatu błędu
                JsonNode messageNode = responseNode?["message"]!;
                var message = messageNode?.Deserialize<string>(options);
                return new() { ErrorMessage = message };
            }
        }

        /// <summary>
        /// Ze względu na limit żądań możliwość pobrania danych testowych
        /// </summary>
        /// <returns></returns>
        public ResponseEntity GetTestDatas()
        {
            // ignorowanie wielkości liter podczas deserializacji
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // utworzenie testowej odpowiedzi
            JsonNode responseNode = JsonNode.Parse(_responseTest)!;
            JsonNode resultNode = responseNode?["result"]!;
            JsonNode subjectNode = resultNode?["subject"]!;

            // deserializacja do obiektu
            var subject = subjectNode?.Deserialize<Subject>(options);
            var result = new ResponseEntity() { Subject = subject };

            // utworzenie obiektów numerów kont z tablicy stringów
            result.Subject?.CreateAccounts();

            // zwrócenie danych
            return result;
        }
    }
}
