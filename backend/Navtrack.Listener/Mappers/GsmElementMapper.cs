// using Navtrack.DataAccess.Model.Devices.Messages;
// using DeviceMessageDocument = Navtrack.Listener.Models.DeviceMessageDocument;
//
// namespace Navtrack.Listener.Mappers;
//
// public static class GsmElementMapper
// {
//     public static GsmElement? Map(DeviceMessageDocument source)
//     {
//         CellGlobalIdentityElement? cgi = CellGlobalIdentityElementMapper.Map(source);
//         
//         if (source.GsmSignal != null || cgi != null)
//         {
//             return new GsmElement
//             {
//                 Signal = source.GsmSignal,
//                 CellGlobalIdentity = CellGlobalIdentityElementMapper.Map(source)
//             };
//         }
//
//         return null;
//     }
// }