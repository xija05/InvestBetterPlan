using InvestBetterPlan_RestAPI.Models.Dto;

namespace InvestBetterPlan_RestAPI.Data
{
    public static class UserStore
    {
        public static List<UserDTO> usersList = new List<UserDTO>
            {
                new UserDTO{ Id= 1, NombreCompleto= "Ximena Jauregui", NombreCompletoAdvisor = "APF", FechaCreacion = new DateTime(2019,3,15)},
                new UserDTO{ Id= 2, NombreCompleto= "Darren Chris", NombreCompletoAdvisor = "APF", FechaCreacion = new DateTime(2015,4,23) },
                new UserDTO{ Id= 3, NombreCompleto= "Maria Chuquimia", NombreCompletoAdvisor = "APF", FechaCreacion = new DateTime(2009,6,8)},
                new UserDTO{ Id= 4, NombreCompleto= "Felipe Cabreales", NombreCompletoAdvisor = "APF", FechaCreacion = new DateTime(2007,11,30) }
            };
    }

}
