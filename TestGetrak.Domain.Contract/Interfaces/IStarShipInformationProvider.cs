using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestGetrak.Domain.Contract.Entities;

namespace TestGetrak.Domain.Contract.Interfaces
{
    public interface IStarShipInformationProvider
    {
        Task<List<StarShips>> GetStarShipsInformation(int distance = 0);


    }
}
