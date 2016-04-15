using System.Collections.Generic;

namespace AccountsVork.Infrastructure
{
    public static class ServiceCompanies
    { 
        public static List<string> GetServiceCompaniesList()
        {
            return new List<string>()
            {
                "ККС", "АйСиЭл"
            };
        }
    }
}
