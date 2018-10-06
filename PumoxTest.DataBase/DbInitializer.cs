using Microsoft.EntityFrameworkCore;
using PumoxTest.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PumoxTest.DataBase
{
    public static class DbInitializer
    {
        public static void Initialize(PumoxTestContext context)
        {
            // Look for any students.
            if (context.Company.Any())
            {
                return;   // DB has been seeded
            }

            var companes = new Company[]
            {
            new Company{Name="Pumox",EstablishmentYear=1958,
                Employees =
                {
                    new Employe { FirstName="Karol", LastName="Korol", DateOfBirth= Convert.ToDateTime("19-03-1987"), JobTitle = JobTitle.Developer.ToString() }
                }
            },
            new Company{Name="Luxmed",EstablishmentYear=2018},
            };
            context.Company.AddRange(companes);
            context.SaveChanges();
        }
    }
}
