using Microsoft.EntityFrameworkCore.Migrations;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Model.Custom;

namespace Navtrack.DataAccess.Migrations
{
    public partial class AddDeviceTypeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            DesignTimeDbContextFactory dbContextFactory = new DesignTimeDbContextFactory();

            using NavtrackContext context = dbContextFactory.CreateDbContext(null);

            context.DeviceTypes.Add(new DeviceType
                {Brand = "Teltonika", Model = "FM3200", ProtocolId = (int)Protocol.Teltonika});
            context.DeviceTypes.Add(new DeviceType
                {Brand = "Teltonika", Model = "FM4200", ProtocolId = (int)Protocol.Teltonika});

            context.DeviceTypes.Add(new DeviceType
                {Brand = "Meitrack", Model = "MVT340", ProtocolId = (int)Protocol.Meitrack});
            context.DeviceTypes.Add(new DeviceType
                {Brand = "Meitrack", Model = "MVT380", ProtocolId = (int)Protocol.Meitrack});
            context.DeviceTypes.Add(new DeviceType
                {Brand = "Meitrack", Model = "MVT600", ProtocolId = (int)Protocol.Meitrack});

            context.SaveChanges();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
