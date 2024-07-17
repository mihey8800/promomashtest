namespace Promomash.EntityFramework.Initialization.Seed;

/// <summary>
/// SeedData
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Countries
    /// </summary>
    public static List<Country> Countries =>
    [
        new Country
        {
            Id = Guid.Parse("88941081-2cab-4a32-bd33-e84dc12c6bd9"),
            Name = "USA",
            Provinces = new List<Province>
            {
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Alabama" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Alaska" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "Arizona" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = "Arkansas" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Name = "California" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), Name = "Colorado" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000007"), Name = "Connecticut" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000008"), Name = "Delaware" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000009"), Name = "Florida" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000010"), Name = "Georgia" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000011"), Name = "Hawaii" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000012"), Name = "Idaho" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000013"), Name = "Illinois" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000014"), Name = "Indiana" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000015"), Name = "Iowa" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000016"), Name = "Kansas" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000017"), Name = "Kentucky" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000018"), Name = "Louisiana" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000019"), Name = "Maine" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000020"), Name = "Maryland" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000021"), Name = "Massachusetts" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000022"), Name = "Michigan" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000023"), Name = "Minnesota" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000024"), Name = "Mississippi" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000025"), Name = "Missouri" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000026"), Name = "Montana" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000027"), Name = "Nebraska" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000028"), Name = "Nevada" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000029"), Name = "New Hampshire" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000030"), Name = "New Jersey" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000031"), Name = "New Mexico" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000032"), Name = "New York" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000033"), Name = "North Carolina" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000034"), Name = "North Dakota" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000035"), Name = "Ohio" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000036"), Name = "Oklahoma" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000037"), Name = "Oregon" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000038"), Name = "Pennsylvania" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000039"), Name = "Rhode Island" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000040"), Name = "South Carolina" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000041"), Name = "South Dakota" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000042"), Name = "Tennessee" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000043"), Name = "Texas" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000044"), Name = "Utah" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000045"), Name = "Vermont" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000046"), Name = "Virginia" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000047"), Name = "Washington" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000048"), Name = "West Virginia" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000049"), Name = "Wisconsin" },
                new Province { Id = Guid.Parse("00000000-0000-0000-0000-000000000050"), Name = "Wyoming" }
            }
        },

        new Country
        {
            Id = Guid.Parse("16d5b6d8-c48b-4902-b840-4e6a8970a9d1"),
            Name = "Canada",
            Provinces = new List<Province>
            {
                new Province { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Alberta" },
                new Province { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "British Columbia" },
                new Province { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Name = "Manitoba" },
                new Province { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Name = "New Brunswick" },
                new Province
                    { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Name = "Newfoundland and Labrador" },
                new Province { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Name = "Nova Scotia" },
                new Province { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Name = "Ontario" },
                new Province { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Name = "Prince Edward Island" },
                new Province { Id = Guid.Parse("10000000-0000-0000-0000-000000000009"), Name = "Quebec" },
                new Province { Id = Guid.Parse("10000000-0000-0000-0000-000000000010"), Name = "Saskatchewan" }
            }
        },

        new Country
        {
            Id = Guid.Parse("d3ad604d-4385-4b5f-a687-7d7ed4c0d8eb"),
            Name = "Australia",
            Provinces = new List<Province>
            {
                new Province { Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), Name = "New South Wales" },
                new Province { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Name = "Victoria" },
                new Province { Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), Name = "Queensland" },
                new Province { Id = Guid.Parse("20000000-0000-0000-0000-000000000004"), Name = "Western Australia" },
                new Province { Id = Guid.Parse("20000000-0000-0000-0000-000000000005"), Name = "South Australia" },
                new Province { Id = Guid.Parse("20000000-0000-0000-0000-000000000006"), Name = "Tasmania" },
                new Province { Id = Guid.Parse("20000000-0000-0000-0000-000000000007"), Name = "Northern Territory" },
                new Province
                    { Id = Guid.Parse("20000000-0000-0000-0000-000000000008"), Name = "Australian Capital Territory" }
            }
        }
    ];

    /// <summary>
    /// Admin
    /// </summary>
    public static User Admin => new User()
    {
        Login = "admin@admin.com",
        Country = Countries.First(),
        Province = Countries.First().Provinces.First(),
        PasswordHash = "admin123"
    };
}