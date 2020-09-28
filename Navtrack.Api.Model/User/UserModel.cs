using Navtrack.Api.Model.Custom;
using Navtrack.Api.Model.General;

namespace Navtrack.Api.Model.User
{
    public class UserModel : IModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public MeasurementSystem MeasurementSystem { get; set; }
    }
}