using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TestGetrak.Domain.Contract.Entities;
using TestGetrak.Domain.Contract.Interfaces;

namespace TestGetrak.Provider
{
    public class StarShipInformationProvider : IStarShipInformationProvider
    {
        private async Task<StarShips> GetStarshipsPerPage(int page)
        {
            var result = new StarShips();

            using (var client = new HttpClient())
            {
                var uri = "https://swapi.co/api/starships/";
                if (page != 1)
                {
                    uri = $"{uri}?page={page}";
                }

                var message = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(uri)
                };

                var response = await client.SendAsync(message);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<StarShips>(responseJson));
                }
                else
                {
                    throw new Exception("Coudln't get starships information");
                }
            }

            return result;

        }
        private async Task GetStops(int distance, StarShips starShips)
        {

            foreach (var item in starShips.results)
            {
                if (!item.MGLT.Contains("unknown") && !item.consumables.Contains("unknown"))
                {
                    decimal result = 0;
                    var aux = item.consumables.Split(" ");

                    if (item.consumables.ToLower().Contains("day"))
                    {
                        //Quantidade de horas em um dia
                        result = distance / ((24 * int.Parse(aux[0])) * int.Parse(item.MGLT));

                    }
                    else if (item.consumables.ToLower().Contains("week"))
                    {
                        //Quantidade de horas em uma semana
                        result = distance / ((168 * int.Parse(aux[0])) * int.Parse(item.MGLT));
                    }
                    else if (item.consumables.ToLower().Contains("month"))
                    {

                        //Quantidade de horas em um mes
                        result = distance / ((730 * int.Parse(aux[0])) * int.Parse(item.MGLT));
                    }
                    else if (item.consumables.ToLower().Contains("year"))
                    {
                        //Quantidade de horas em um ano
                        result = distance / ((8760 * int.Parse(aux[0])) * int.Parse(item.MGLT));

                    }
                    item.stops = result;
                }
                else item.stops = -1;
            }  
        }
        public async Task<List<StarShips>> GetStarShipsInformation(int distance = 0)
        {
            var result = new List<StarShips>();
            var starShips = new StarShips();
            try
            {
                var count = 1;
                do
                {
                    starShips = await GetStarshipsPerPage(count);
                    if (distance > 0)
                    {
                        await GetStops(distance, starShips);
                    }

                    if (starShips != null)
                    {
                        result.Add(starShips);
                    }
                    count++;
                } while (starShips.next != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }
    }
}
