using FakeStoreAPI.Host.DTOs.External;

namespace FakeStoreAPI.Host.DTOs.Internal
{
    public class UserDto
    {
        public Address? Address { get; set; }
        public long Id { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public Name? Name { get; set; }
        public string? Phone { get; set; }
    }

    public class Address
    {
        public string? City { get; set; }
        public string? Street { get; set; }
        public int Number { get; set; }
        public string? Zipcode { get; set; }
    }

    public class Name
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
