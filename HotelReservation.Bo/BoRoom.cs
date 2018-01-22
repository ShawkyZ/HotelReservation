using System.Collections.Generic;
using System.Linq;
using HotelReservation.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Bo
{
    public class BoRoom : BoBase<DtoRoom>
    {
        public DtoRoom Get(string number){
            return Repository.Get(x => x.Number == number).Include(x => x.RoomType).FirstOrDefault();
        }
    }
}