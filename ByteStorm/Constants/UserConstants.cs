using ByteStorm.Models;
using System.Net.PeerToPeer.Collaboration;

namespace ByteStorm.Constants
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { Username= "pepe", Password = "pepe123", Rol = "Comandante", Fullname = "Pepe Hurtado García", Correo= "pepehg@bytestorm.es"},
            new UserModel() { Username= "julio", Password = "julio123", Rol = "Soldado", Fullname = "Julio Fernandez Cánovas", Correo= "juliofc@bytestorm.es"},
        };

    }
}
