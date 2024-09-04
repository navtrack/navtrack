// using System.Collections.Generic;
// using MongoDB.Bson.Serialization.Attributes;
//
// namespace Navtrack.DataAccess.Model.Devices.Messages;
//
// public class CanElement
// {
//     public CanElement()
//     {
//         // Data = new Dictionary<string, string>();
//     }
//     
//     // [BsonElement("d")]
//     // public Dictionary<string, string>? Data { get; set; }
//     //
//     // public byte? CNGStatus { get; set; }
//     // public ushort? DoorStatus { get; set; }
//     // public uint? ProgramNumber { get; set; }
//     // public uint? ControlStateFlags { get; set; }
//     // public ulong? AgriculturalMachineryFlags { get; set; }
//     // public ulong? SecurityStateFlags { get; set; }
//     // public ulong? SecurityStateFlagsP4 { get; set; }
//     
//     /// <summary>
//     /// Diagnostic trouble codes count.
//     /// </summary>
//     public byte DtcCount { get; set; }
//
//     /// <summary>
//     /// Calculated engine load value (%).
//     /// </summary>
//     public byte EngineLoad { get; set; }
// }